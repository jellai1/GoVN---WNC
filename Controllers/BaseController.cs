using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class BaseController : Controller
    {
        //chi khi dang nhap moi cho phep truy cap cac trang
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var userId = HttpContext.Session.GetInt32("MaUser"); // session lưu ID user khi đăng nhập
            if (userId == null)
            {
                // Chuyển hướng về trang đăng nhập
                context.Result = RedirectToAction("DangNhap", "Member");
            }
            base.OnActionExecuting(context);
        }
    }
}
