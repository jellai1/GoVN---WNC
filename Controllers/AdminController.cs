using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using BTL.Models.Class;

namespace BTL.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IResponsitories responsitories;
        public AdminController(IResponsitories responsitories)
        {
            this.responsitories = responsitories;
        }
        public IActionResult Index()
        {
            List<Members> ds=responsitories.ListMembers();
            return View(ds);
        }
    }
}
