using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.BLL;
using Backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class AboutModel : PageModel
    {
        private readonly AboutService _service;
        private readonly IWebHostEnvironment _environment;
        public AboutModel(AboutService service, IWebHostEnvironment env)
        {
            _service = service;
            _environment = env;
        }

        public DatabaseVersion DatabaseVersion { get; set; }
        public DatabaseInfo DatabaseInfo { get; set; }
        public string WebRootPath
        { get { return _environment.WebRootPath; } }
        public string ApplicationPath
        { get { return _environment.ContentRootPath; } }

        public void OnGet()
        {
            DatabaseVersion = _service.GetDatabaseVersion();
            DatabaseInfo = _service.GetDatabaseInformation();
        }
    }
}
