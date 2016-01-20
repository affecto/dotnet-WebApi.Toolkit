using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using Affecto.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.WebApi.Toolkit.Tests
{
    [TestClass]
    public class RequestLoggingFilterTests
    {
        private ILoggerFactory loggerFactory;
        private ICorrelationLogger logger;
        private ICorrelation correlation;
        private RequestLoggingFilter sut;
        private HttpActionContext context;

        [TestInitialize]
        public void Setup()
        {
            context = new HttpActionContext(new HttpControllerContext(new HttpRequestContext(), new HttpRequestMessage(), new HttpControllerDescriptor(), Substitute.For<IHttpController>()),
                new ReflectedHttpActionDescriptor());
            loggerFactory = Substitute.For<ILoggerFactory>();
            logger = Substitute.For<ICorrelationLogger>();
            loggerFactory.CreateCorrelationLogger(Arg.Any<RequestLoggingFilter>()).Returns(logger);
            correlation = Substitute.For<ICorrelation>();
            sut = new RequestLoggingFilter(loggerFactory, correlation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullContextThrowsError()
        {
            sut.OnActionExecuting(null);
        }

        [TestMethod]
        public void ArgumentsAreLogged()
        {
            context.ActionArguments.Add("Key1", "Value1");
            context.ActionArguments.Add("Key2", "Value2");
            sut.OnActionExecuting(context);
            logger.Received().LogVerbose(correlation, "Request received: {0}({1})", null, "Key1: Value1, Key2: Value2");
        }

        [TestMethod]
        public void ListArgumentsAreLogged()
        {
            var listAsArgument = new List<string>();
            listAsArgument.Add("Value1");
            listAsArgument.Add("Value2");

            context.ActionArguments.Add("Key1", listAsArgument);
            sut.OnActionExecuting(context);

            logger.Received().LogVerbose(correlation, "Request received: {0}({1})", null, string.Format("Key1 contains items:{0}Value1{0}Value2{0}", Environment.NewLine));
        }
        
        [TestMethod]
        public void NullCorrelationIsLogged()
        {
            sut = new RequestLoggingFilter(loggerFactory);
            context.ActionArguments.Add("Key1", "Value1");
            context.ActionArguments.Add("Key2", "Value2");
            sut.OnActionExecuting(context);
            logger.Received().LogVerbose(null, "Request received: {0}({1})", null, "Key1: Value1, Key2: Value2");
        }

    }
}