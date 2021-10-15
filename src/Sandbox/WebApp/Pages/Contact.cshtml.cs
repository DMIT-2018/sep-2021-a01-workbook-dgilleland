using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO; // The namespace for everything File I/O

namespace WebApp.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        public ContactModel(IWebHostEnvironment env)
        {
            _environment = env;
        }

        [BindProperty] // This attribute allows our Razor Page to "assign" the form item to this property
        public IFormFile ApplicantResume { get; set; }
        public string ApplicationPath
        { get { return _environment.ContentRootPath; } }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Save this file to my application (somewhere) - Confidential
            string folderPath = Path.Combine(ApplicationPath, "Confidential");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // File name, I'll use the same name that was given by the user
            // CAUTION - It's kinda dangerous to trust the file name given by the browser
            string fileName = Path.Combine(folderPath, ApplicantResume.FileName);
            if (!System.IO.File.Exists(fileName)) 
            {
                using(var stream = System.IO.File.Create(fileName))
                {
                    ApplicantResume.CopyTo(stream);
                }
            }
        }
    }
}
