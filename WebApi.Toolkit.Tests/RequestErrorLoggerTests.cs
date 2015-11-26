using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Affecto.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.WebApi.Toolkit.Tests
{
    [TestClass]
    public class RequestErrorLoggerTests
    {
        private ILoggerFactory loggerFactory;
        private ICorrelationLogger logger;
        private ICorrelation correlation;
        private RequestErrorLogger sut;

        [TestInitialize]
        public void Setup()
        {
            loggerFactory = Substitute.For<ILoggerFactory>();
            logger = Substitute.For<ICorrelationLogger>();
            loggerFactory.CreateCorrelationLogger(Arg.Any<RequestErrorLogger>()).Returns(logger);
            correlation = Substitute.For<ICorrelation>();

            sut = new RequestErrorLogger(loggerFactory, request => correlation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullLoggerThrowsError()
        {
            sut = new RequestErrorLogger(null, request => correlation);
        }

        [TestMethod]
        public void ErrorIsLogged()
        {
            var exception = new ArgumentException("Test");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.server.domain");
            var httpRouteData = new HttpRouteData(new HttpRoute());
            var httpControllerContext = new HttpControllerContext(new HttpConfiguration(), httpRouteData, httpRequestMessage);
            var actionContext = new HttpActionContext(httpControllerContext, new ReflectedHttpActionDescriptor());
            actionContext.ActionArguments.Add("Key1", "Value1");
            actionContext.ActionArguments.Add("Key2", "Value2");

            var exceptionContext = new ExceptionContext(exception, new ExceptionContextCatchBlock("TestBlock", false, false), actionContext);
            var loggerContext = new ExceptionLoggerContext(exceptionContext);

            sut.Log(loggerContext);
            logger.Received().LogError(correlation, exception, "Unhandled exception from request: {0}({1})", httpRequestMessage, "Key1: Value1, Key2: Value2");
        }

        [TestMethod]
        public void CorrelationCanBeNull()
        {
            var exception = new ArgumentException("Test");
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://www.server.domain");
            var httpRouteData = new HttpRouteData(new HttpRoute());
            var httpControllerContext = new HttpControllerContext(new HttpConfiguration(), httpRouteData, httpRequestMessage);
            var actionContext = new HttpActionContext(httpControllerContext, new ReflectedHttpActionDescriptor());
            actionContext.ActionArguments.Add("Key1", "Value1");
            actionContext.ActionArguments.Add("Key2", "Value2");

            var exceptionContext = new ExceptionContext(exception, new ExceptionContextCatchBlock("TestBlock", false, false), actionContext);
            var loggerContext = new ExceptionLoggerContext(exceptionContext);

            sut = new RequestErrorLogger(loggerFactory, request => null);
            sut.Log(loggerContext);
            logger.Received().LogError(null, exception, "Unhandled exception from request: {0}({1})", httpRequestMessage, "Key1: Value1, Key2: Value2");
        }
    }
}