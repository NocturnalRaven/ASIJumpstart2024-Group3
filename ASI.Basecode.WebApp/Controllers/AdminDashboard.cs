using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AdminDashboard : ControllerBase<AdminDashboard>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="localizer"></param>
        /// <param name="mapper"></param>

        public AdminDashboard(
           IHttpContextAccessor httpContextAccessor,
                             ILoggerFactory loggerFactory,
                             IConfiguration configuration,
                             IMapper mapper = null) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {

        }


        #region GET Methods

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin-dashboard/index")]
        public IActionResult Admindashboard()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin-dashboard/users")]
        public IActionResult AdminUserDashboard()
        {
            return View();
        }

        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin-dashboard/rooms")]
        public IActionResult Rooms()
        {
            return View();
        }


        [HttpGet]
        //[Authorize(Policy = "AdminOnly")]
        [Route("/admin-dashboard/analytics")]
        public IActionResult AdminAnalytics()
        {

            return View();
        }

        #endregion


    }
}
