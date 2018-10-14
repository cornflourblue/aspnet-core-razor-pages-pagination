using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JW;

namespace RazorPagesPagination.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<string> Items { get; set; }
        public Pager Pager { get; set; }
        public SelectList TotalItemsList { get; set; }
        public int TotalItems { get; set; }
        public SelectList PageSizeList { get; set; }
        public int PageSize { get; set; }
        public SelectList MaxPagesList { get; set; }
        public int MaxPages { get; set; }

        public void OnGet(int p = 1)
        {
            // properties for pager parameter controls
            TotalItemsList = new SelectList(new []{ 10, 150, 500, 1000, 5000, 10000, 50000, 100000, 1000000 });
            TotalItems = HttpContext.Session.GetInt32("TotalItems") ?? 150;
            PageSizeList = new SelectList(new []{ 1, 5, 10, 20, 50, 100, 200, 500, 1000 });
            PageSize = HttpContext.Session.GetInt32("PageSize") ?? 10;
            MaxPagesList = new SelectList(new []{ 1, 5, 10, 20, 50, 100, 200, 500 });
            MaxPages = HttpContext.Session.GetInt32("MaxPages") ?? 10;

            // generate list of sample items to be paged
            var dummyItems = Enumerable.Range(1, TotalItems).Select(x => "Item " + x);

            // get pagination info for the current page
            Pager = new Pager(dummyItems.Count(), p, PageSize, MaxPages);

            // assign the current page of items to the Items property
            Items = dummyItems.Skip((Pager.CurrentPage - 1) * Pager.PageSize).Take(Pager.PageSize);
        }

        public IActionResult OnPost(int totalItems, int pageSize, int maxPages)
        {
            // update pager parameters for session and redirect back to 'OnGet'
            HttpContext.Session.SetInt32("TotalItems", totalItems);
            HttpContext.Session.SetInt32("PageSize", pageSize);
            HttpContext.Session.SetInt32("MaxPages", maxPages);
            return Redirect("/");
        }
    }
}
