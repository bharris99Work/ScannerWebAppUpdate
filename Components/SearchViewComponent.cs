using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ScannerWebAppUpdate.Components
{
    public class SearchViewComponent: ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }


    }
}
