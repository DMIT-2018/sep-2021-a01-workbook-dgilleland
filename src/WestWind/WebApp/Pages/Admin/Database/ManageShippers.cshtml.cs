using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WestWind.App.BLL;
using WestWind.App.Models.CRUD;

namespace WebApp.Pages.Admin.Database
{
    // ![](../../Design/ManageShippers.png)
    public class ManageShippersModel : PageModel
    {
        #region Constructor and Depedencies
        private readonly ShippingService _service;
        public ManageShippersModel(ShippingService service)
        {
            _service = service;
        }
        #endregion

        public List<Shipper> CurrentShippers { get; set; }

        public void OnGet()
        {
            CurrentShippers = _service.ListShippers();
        }
    }
}
