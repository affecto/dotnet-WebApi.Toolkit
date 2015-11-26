using System;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.WebApi.Toolkit.Tests
{
    [TestClass]
    public class RequestExceptionFilterTests
    {
        private RequestExceptionFilter testClass;
        private HttpActionExecutedContext context;

        [TestInitialize]
        public void Setup()
        {
            testClass = new RequestExceptionFilter();
            context = new HttpActionExecutedContext(new HttpActionContext(), new ArgumentException());
            testClass.OnException(context);
        }

        [TestMethod]
        public void DefaultImplementationThrowsInternalServerError()
        {
            Assert.AreEqual(HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        [TestMethod]
        public void DefaultImplementationSetsReasonPhrase()
        {
            Assert.AreEqual("Unknown error occured.", context.Response.ReasonPhrase);
        }
    }
}