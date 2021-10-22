using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.BLL;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Admin
{
    public class ManageCapstoneStudentsModel : PageModel
    {
        #region Constructor and Depedencies
        private readonly CapstoneService _service;
        public ManageCapstoneStudentsModel(CapstoneService service)
        {
            _service = service;
        }
        #endregion

        public List<StudentName> AllStudents { get; set; }
        public string ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)] // for POST and GET Requests (GET through the routing parameter)
        public int? SelectedStudent { get; set; }
        [BindProperty] // for POST Requests
        public Student CurrentStudent { get; set; }
        
        [TempData]
        public string FeedbackMessage { get; set; }

        public void OnGet()
        {
            AllStudents = _service.ListCapstoneStudents();
            if(SelectedStudent.HasValue)
            {
                // Lookup and display the student information.
                CurrentStudent = _service.LookupStudent(SelectedStudent.Value);
            }
        }

        public IActionResult OnPost()
        {
            return RedirectToPage(new { SelectedStudent });
        }

        public IActionResult OnPostAdd()
        {
            try
            {
                // The response to the browser is a Post-Redirect-Get pattern
                int newId = _service.AddStudent(CurrentStudent);
                FeedbackMessage = $"Successfully added {CurrentStudent.FirstName} to the list of Capstone Students.";
                return RedirectToPage(new { SelectedStudent = newId });
            }
            catch (Exception ex)
            {
                // The response to the browser is the result of this POST processing
                Exception innermost = ex;
                while (innermost.InnerException != null)
                    innermost = innermost.InnerException;
                ErrorMessage = innermost.Message;
                AllStudents = _service.ListCapstoneStudents();
                return Page();
            }
        }

        public IActionResult OnPostUpdate()
        {
            try
            {
                _service.UpdateStudent(SelectedStudent.Value, CurrentStudent);
                FeedbackMessage = $"Successfully updated information for {CurrentStudent.FirstName}.";
                return RedirectToPage(new { SelectedStudent = SelectedStudent });
            }
            catch (Exception ex)
            {
                // The response to the browser is the result of this POST processing
                Exception innermost = ex;
                while (innermost.InnerException != null)
                    innermost = innermost.InnerException;
                ErrorMessage = innermost.Message;
                AllStudents = _service.ListCapstoneStudents();
                return Page();
            }
        }

        public IActionResult OnPostDelete()
        {
            try
            {
                _service.DeleteStudent(SelectedStudent.Value);
                FeedbackMessage = $"Successfully removed {CurrentStudent.FirstName} {CurrentStudent.LastName} ({CurrentStudent.ID}).";
                return RedirectToPage(new { SelectedStudent = (int?)null });
            }
            catch (Exception ex)
            {
                // The response to the browser is the result of this POST processing
                Exception innermost = ex;
                while (innermost.InnerException != null)
                    innermost = innermost.InnerException;
                ErrorMessage = innermost.Message;
                AllStudents = _service.ListCapstoneStudents();
                return Page();
            }
        }

        public IActionResult OnPostClear()
        {
            return RedirectToPage(new { SelectedStudent = (int?)null });
        }
    }
}
