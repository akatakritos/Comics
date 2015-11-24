﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

using Comics.Core.Downloaders;

namespace Comics.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IComicImporter _comicImporter;

        public AdminController(IComicImporter comicImporter)
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

            _comicImporter.ImportNewComics();
            return new EmptyResult();
        }
    }
}