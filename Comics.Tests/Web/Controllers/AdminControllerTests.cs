using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

using AutoMoq;

using Comics.Core.Import;
using Comics.Web.Controllers;

using Moq;

using NFluent;

using Xunit;

namespace Comics.Tests.Web.Controllers
{
    public class AdminControllerTests
    {
        [Fact]
        public void ItHasRudimentarySecurity()
        {
            //Requires appsetting: AdminAuthToken
            var mocker = new AutoMoqer();

            Check.That(ConfigurationManager.AppSettings["AdminAuthToken"]).IsNotEmpty();
            Check.That(ConfigurationManager.AppSettings["AdminAuthToken"]).IsNotNull();

            var result = mocker.Create<AdminController>().Refresh(authToken: "foobar");

            Check.That(ConfigurationManager.AppSettings["AdminAuthToken"]).IsNotEmpty();
            Check.That(ConfigurationManager.AppSettings["AdminAuthToken"]).IsNotNull();
            Check.That(ConfigurationManager.AppSettings["AdminAuthToken"]).IsNotEqualTo("foobar");
            Check.That(result).IsInstanceOf<HttpUnauthorizedResult>();
        }

        [Fact]
        public void WhenAuthorizedReturnsEmptyResult()
        {
            var mocker = new AutoMoqer();

            var result = mocker.Create<AdminController>().Refresh(authToken: ConfigurationManager.AppSettings["AdminAuthToken"]);

            Check.That(result).IsInstanceOf<EmptyResult>();
        }

        [Fact]
        public void ItRunsTheImports()
        {
            var mocker = new AutoMoqer();

             mocker.Create<AdminController>().Refresh(authToken: ConfigurationManager.AppSettings["AdminAuthToken"]);

            mocker.GetMock<IImportProcess>()
                .Verify(m => m.Run(), Times.Once);
        }
    }
}
