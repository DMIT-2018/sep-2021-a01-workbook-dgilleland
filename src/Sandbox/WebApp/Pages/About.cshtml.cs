using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.BLL;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class AboutModel : PageModel
    {
        private readonly AboutService _service;
        public AboutModel(AboutService service)
        {
            _service = service;
        }

        public DatabaseVersion DatabaseVersion { get; set; }
        public void OnGet()
        {
            DatabaseVersion = _service.GetDatabaseVersion();

        }
    }
}
