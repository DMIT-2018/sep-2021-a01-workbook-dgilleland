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
            Person.FirstName = null;
            ModelState.Clear();
        }

        public void OnPostFullClear()
        {
            CopyData(nameof(OnPostFullClear));
            Person = null;
            ModelState.Clear();
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
