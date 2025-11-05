using BTL.Models;
using Microsoft.AspNetCore.Mvc;
using BTL.Models.Class;
using BTL.Models.MK;

namespace BTL.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IResponsitories responsitories;
        private readonly PasswordService _passwordService;
        private readonly CarDbContext context;
        public AdminController(IResponsitories responsitories, CarDbContext context)
        {
            this.responsitories = responsitories;
            this.context = context;
            _passwordService = new PasswordService();
        }
        public IActionResult Index()
        {
            List<Members> ds = responsitories.ListMembers();
            return View(ds);
        }
        
        [HttpPost]
        public IActionResult Create(Members member)
        {
            if (string.IsNullOrEmpty(member.MatKhau))
                member.MatKhau = "123456"; // mật khẩu mặc định

            member.MatKhau = _passwordService.HashPassword(member.MatKhau);
            responsitories.Create(member);
            return RedirectToAction("Index");
        }

        // ✅ Sửa tài khoản
         public IActionResult Edit(int id)
        {
            if (context == null)
            {
                throw new Exception("CarDbContext chưa được inject — kiểm tra lại constructor hoặc Program.cs!");
            }

            var member = context.members.FirstOrDefault(x => x.MaUser == id);
            if (member == null)
                return NotFound();

            return View(member);
        }
        // 💾 Xử lý cập nhật
        [HttpPost]
        public IActionResult Edit(Members model)
        {
            try
            {
                var existing = context.members.FirstOrDefault(m => m.MaUser == model.MaUser);
                if (existing == null)
                {
                    TempData["Message"] = "Không tìm thấy tài khoản.";
                    return RedirectToAction("Index");
                }

                existing.TenDN = model.TenDN;
                existing.Email = model.Email;
                existing.SDT = model.SDT;
                existing.VaiTro = model.VaiTro;

                // Nếu người admin nhập mật khẩu mới
                if (!string.IsNullOrEmpty(model.MatKhau))
                {
                    var service = new BTL.Models.MK.PasswordService();
                    existing.MatKhau = service.HashPassword(model.MatKhau);
                }

                context.SaveChanges();
                TempData["Message"] = "✅ Cập nhật tài khoản thành công!";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "❌ Lỗi khi cập nhật: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // ✅ Xóa tài khoản
        public IActionResult Delete(int id)
        {
            bool deleted = responsitories.Delete(id);

            if (deleted)
                TempData["Message"] = "✅ Xóa tài khoản thành công!";
            else
                TempData["Message"] = "❌ Không thể xóa tài khoản. Vui lòng thử lại.";

            return RedirectToAction("Index");
        }

    }
}
