using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Affecto.WebApi.Toolkit.Tests
{
    [TestClass]
    public class RequireHttpsAttributeTests
    {
        private RequireHttpsAttribute sut;
        private HttpActionContext context;

        [TestInitialize]
        public void Setup()
        {
            context = new HttpActionContext(new HttpControllerContext(new HttpRequestContext(), new HttpRequestMessage(HttpMethod.Get, new Uri("https://www.server.domain")), new HttpControllerDescriptor(), Substitute.For<IHttpController>()),
                new ReflectedHttpActionDescriptor());
            sut = new RequireHttpsAttribute();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FailsForNullParameter()
        {
            sut.OnAuthorization(null);
        }

        [TestMethod]
        public void HttpAddressIsForbidden()
        {
            context.Request.RequestUri = new Uri("http://www.server.domain");
            sut.OnAuthorization(context);
            Assert.AreEqual(HttpStatusCode.Forbidden, context.Response.StatusCode);
        }

        [TestMethod]
        public void HttpsAddressIsAllowed()
        {
            sut.OnAuthorization(context);
            Assert.IsNull(context.Response);
        }
    }
}