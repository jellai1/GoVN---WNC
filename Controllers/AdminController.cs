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
        public AdminController(IResponsitories responsitories)
        {
            this.responsitories = responsitories;
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
        [HttpPost]
        public IActionResult Edit(Members model)
        {
            var member = responsitories.ListMembers().FirstOrDefault(x => x.MaUser == model.MaUser);
            if (member == null)
            {
                TempData["Message"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("Index");
            }

            member.TenDN = model.TenDN;
            member.Email = model.Email;
            member.SDT = model.SDT;
            member.VaiTro = model.VaiTro;

            if (!string.IsNullOrEmpty(model.MatKhau))
                member.MatKhau = _passwordService.HashPassword(model.MatKhau);

            responsitories.Create(member); // nếu bạn có hàm Update thì nên dùng _repo.Update(member);
            TempData["Message"] = "Cập nhật thành công!";
            return RedirectToAction("Index");
        }

        // ✅ Xóa tài khoản
        [HttpPost]
        public IActionResult Delete(int id)
        {
            bool deleted = responsitories.Delete(id);
            TempData["Message"] = deleted ? "Đã xóa tài khoản thành công!" : "Xóa thất bại!";
            return RedirectToAction("Index");
        }
    }
}
