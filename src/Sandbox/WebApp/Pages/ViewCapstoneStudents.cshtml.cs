using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.BLL;
using Backend.Models.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers;

namespace WebApp.Pages
{
    public class ViewCapstoneStudentsModel : PageModel
    {
        #region Constructor and Dependencies
        private readonly CapstoneService _service;
        public ViewCapstoneStudentsModel(CapstoneService service)
        {
            _service = service;
        }
        #endregion

        public IEnumerable<StudentAssignment> StudentAssignments { get; set; }

        private const int PAGE_SIZE = 8;
        public Paginator Paging { get; set; }
        public int TotalCount { get; set; }

        public void OnGet(int? currentPage)
        {
            int pageNumber = currentPage.HasValue ? currentPage.Value : 1;
            // call your service to get the data & the total count
            PageState current = new(pageNumber, PAGE_SIZE);
            int total;
            StudentAssignments = _service.ListStudentAssignments(pageNumber, PAGE_SIZE, out total);
            TotalCount = total;
            Paging = new(TotalCount, current);
        }
    }
}
