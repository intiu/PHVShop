﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PTHNVShop.Service;
using PTHNVShop.Web.Infrastructure.Core;

namespace PTHNVShop.Web.Api
{
    [RoutePrefix("api/home")]
    //[Authorize]
    public class HomeController : ApiControllerBase
    {
        IErrorService _errorService;
        public HomeController(IErrorService errorService) : base(errorService)
        {
            this._errorService = errorService;
        }

        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "Hello, PTHNV Member. ";
        }
    }
}
