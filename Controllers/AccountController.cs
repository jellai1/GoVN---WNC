using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class AccountController : Controller
    {
        // ✅ Giả lập tạm thời: đọc role từ session
        // Thực tế bạn nên dùng Identity hoặc cookie login
        public IActionResult CheckRoleForPost()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role))
            {
                // Chưa đăng nhập
                TempData["Message"] = "Vui lòng đăng nhập để tiếp tục.";
                return RedirectToAction("Index", "Admin");
            }

            if (role == "Chuxe")
            {
                // Là chủ xe
                return RedirectToAction("Index", "Chuxe");
            }

            // Không phải chủ xe
            TempData["Message"] = "Tài khoản của bạn không có quyền đăng tin.";
            return RedirectToAction("Index", "Home");
        }
    }
}
