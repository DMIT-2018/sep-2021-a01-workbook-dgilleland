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

        #region Properties
        public List<Shipper> CurrentShippers { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedShipperId { get; set; }
        [BindProperty]
        public Shipper ShipperData { get; set; }

        [TempData]
        public string UserFeedback { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool HasFeedback => !string.IsNullOrWhiteSpace(UserFeedback);
        #endregion

        #region GET/POST Handlers
        public void OnGet()
        {
            CurrentShippers = _service.ListShippers();
        }

        #region POST Handlers
        public IActionResult OnPost()
        {
            // This action handler will be for getting into an "edit mode"
            UserFeedback = $"Edit mode for shipper {SelectedShipperId}.";
            return RedirectToPage(new { SelectedShipperId });
        }

        public IActionResult OnPostCancel()
        {
            UserFeedback = "Cancel operation.";
            return RedirectToPage(new { SelectedShipperId = (int?)null });
        }

        public IActionResult OnPostUpdate()
        {
            try
            {
                _service.UpdateShipper(ShipperData);
                UserFeedback = $"Updated shipper information for {ShipperData.ID} ({ShipperData.CompanyName} - {ShipperData.Phone}).";
                return RedirectToPage(new { SelectedShipperId = ShipperData.ID });
            }
            catch (Exception ex)
            {
                return HandleCRUDError(ex);
            }
        }

        public IActionResult OnPostInsert()
        {
            try
            {
                _service.AddShipper(ShipperData);
                UserFeedback = $"New shipper information added ({ShipperData.CompanyName} - {ShipperData.Phone}).";
                return RedirectToPage(new { SelectedShipperId = (int?)null });
            }
            catch (Exception ex)
            {
                return HandleCRUDError(ex);
            }
        }

        public IActionResult OnPostDelete()
        {
            try
            {
                _service.DeleteShipper(SelectedShipperId.Value);
                UserFeedback = $"Remove shipper {SelectedShipperId}.";
                return RedirectToPage(new { SelectedShipperId = (int?)null });
            }
            catch (Exception ex)
            {
                return HandleCRUDError(ex);
            }
        }
        #endregion
        #endregion

        #region Private Helper Methods
        private IActionResult HandleCRUDError(Exception ex)
        {
            // Get to the root of the problem
            ReportError(ex);

            // Populate the list of shippers
            CurrentShippers = _service.ListShippers();

            // Return this page POST processing results
            return Page();
        }

        private void ReportError(Exception ex)
        {
            Exception innermost = ex;
            while (innermost.InnerException != null)
                innermost = innermost.InnerException;
            ErrorMessage = innermost.Message;
        }
        #endregion
    }
}
