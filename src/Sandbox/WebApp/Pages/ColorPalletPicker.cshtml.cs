using Backend.BLL;
using Backend.Models.Colors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Pages
{
    public class ColorPalletPickerModel : PageModel
    {
        private readonly AboutService _service;

        public ColorPalletPickerModel(AboutService service)
        {
            _service = service;
        }

        [BindProperty]
        public List<NamedColor> AvailableColors { get; set; }
        [BindProperty]
        public List<NamedColor> ColorPallete { get; set; } = new();
        [BindProperty]
        public string SelectedColor {get;set;}

        public void OnGet()
        {
            AvailableColors = _service.ListHTMLColors();
        }

        public void OnPostAddItem()
        {
            var found = AvailableColors.SingleOrDefault(x => x.Name == SelectedColor);
            if (found != null)
            {
                AvailableColors.Remove(found);
                ColorPallete.Add(found);
            }
        }

        public void OnPostRemoveItem()
        {
            var found = ColorPallete.SingleOrDefault(x => x.Name == SelectedColor);
            if (found != null)
            {
                AvailableColors.Add(found);
                ColorPallete.Remove(found);
            }
        }
    }
}
