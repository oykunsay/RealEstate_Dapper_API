using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace RealEstate_Dapper_UI.ViewComponents.HomePage
{
    public class _DefaultServicesComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
