using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace WebApp.Pages
{
    public class UnderstandingModelBindingModel : PageModel
    {
        public void OnGet()
        {
            IsGet = true;
        }

        [BindProperty]
        public Person Person { get; set; }

        public string PostHandler { get; set; }
        public bool IsGet { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime GraduationDate { get; set; }

        public void OnPostNormal()
        {
            CopyData(nameof(OnPostNormal));
        }

        public void OnPostPartialClear()
        {
            CopyData(nameof(OnPostPartialClear));
            Person.FirstName = null; // Clear just part of the person information
            ModelState.Clear(); // Without this, the change to Person.FirstName will not be applied when the page is rendered
        }

        public void OnPostFullClear()
        {
            CopyData(nameof(OnPostFullClear));
            Person = null; // Clear out the whole property object
            ModelState.Clear(); // Without this, the change to Person will not be applied when the page is rendered
        }

        private void CopyData(string handlerName)
        {
            PostHandler = handlerName;
            FirstName = Person.FirstName;
            LastName = Person.LastName;
            GraduationDate = Person.GraduationDate;
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime GraduationDate { get; set; }
    }
}
