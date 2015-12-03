using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

using Comics.Core.Import;

namespace Comics.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IImportProcess _comicImporter;

        public AdminController(IImportProcess comicImporter)
        {
            if (ConfigurationManager.AppSettings["AdminAuthToken"] == null)
                throw new InvalidOperationException("AdminAuthToken not set in web.config");

            _comicImporter = comicImporter;
        }

        // GET: Admin
        [HttpPost]
        public ActionResult Refresh(string authToken)
        {
            if (authToken != ConfigurationManager.AppSettings["AdminAuthToken"])
                return new HttpUnauthorizedResult();

            _comicImporter.Run();
            return new EmptyResult();
        }

        public ActionResult ThrowException(string authToken)
        {
            if (authToken != ConfigurationManager.AppSettings["AdminAuthToken"])
                return new HttpUnauthorizedResult();

            throw new Exception("Test Exception for Rollbar");
        }
    }
}