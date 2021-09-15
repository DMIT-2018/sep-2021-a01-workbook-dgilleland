using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // This property will receive the value(s) from the form when a POST request is generated
        [BindProperty]
        public Translation PopularPhrase { get; set; }

        public void OnGet()
        {
            // This runs when the request method is a GET
        }

        public void OnPost()
        {
            // This runs when the request method is a POST
        }
    }
}
