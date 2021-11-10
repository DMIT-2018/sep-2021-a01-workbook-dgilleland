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
    public class AdHocModel : PageModel
    {
        #region Constructor and Depedencies
        private readonly CapstoneService _service;

        public AdHocModel(CapstoneService service)
        {
            _service = service;
        }
        #endregion

        #region Properties
        public List<CapstoneClient> Companies { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? ClientId { get; set; }
        [BindProperty]
        public CapstoneClient Client { get; set; }
        #endregion

        #region Request Handlers
        public void OnGet()
        {
            Companies = _service.ListClients();
            if (ClientId.HasValue)
                Client = _service.LookupClient(ClientId.Value);
        }

        public void OnPostAdd()
        {
            Companies = _service.ListClients();
            // TODO: something
            // pretend I have updated....
            Client.Id = 345;
        }
        #endregion
    }
}
