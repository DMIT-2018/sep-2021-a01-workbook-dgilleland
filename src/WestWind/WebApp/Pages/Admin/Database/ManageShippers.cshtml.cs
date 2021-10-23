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

        [BindProperty(SupportsGet = true)]
        public int? SelectedShipperId { get; set; }
        [BindProperty]
        public Shipper ShipperData { get; set; }

        [TempData]
        public string TempFeedback { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasFeedback => !string.IsNullOrWhiteSpace(TempFeedback);

        public void OnGet()
        {
            CurrentShippers = _service.ListShippers();
        }

        public IActionResult OnPost()
        {
            // This action handler will be for getting into an "edit mode"
            TempFeedback = $"Edit mode for shipper {SelectedShipperId}.";
            return RedirectToPage(new { SelectedShipperId });
        }

        public IActionResult OnPostCancel()
        {
            TempFeedback = "Cancel operation.";
            return RedirectToPage(new { SelectedShipperId = (int?)null });
        }

        public IActionResult OnPostUpdate()
        {
            // TODO: Wrap with try/catch
            TempFeedback = $"Updated shipper information for {ShipperData.ID} ({ShipperData.CompanyName} - {ShipperData.Phone}).";
            return RedirectToPage(new { SelectedShipperId = ShipperData.ID });
        }

        public IActionResult OnPostInsert()
        {
            // TODO: Wrap with try/catch
            TempFeedback = $"New shipper information added ({ShipperData.CompanyName} - {ShipperData.Phone}).";
            _service.AddShipper(ShipperData);
            return RedirectToPage(new { SelectedShipperId = (int?)null });
        }

        public IActionResult OnPostDelete()
        {
            try
            {
                _service.DeleteShipper(SelectedShipperId.Value);
                TempFeedback = $"Remove shipper {SelectedShipperId}.";
                return RedirectToPage(new { SelectedShipperId = (int?)null });
            }
            catch (Exception ex)
            {
                // Get to the root of the problem
                Exception innermost = ex;
                while (innermost.InnerException != null)
                    innermost = innermost.InnerException;
                ErrorMessage = innermost.Message;

                // Populate the list of shippers
                CurrentShippers = _service.ListShippers();

                // Return this page POST processing results
                return Page();
            }
        }
    }
}
