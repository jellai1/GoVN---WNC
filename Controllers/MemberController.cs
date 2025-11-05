using BTL.Models;
using BTL.Models.Class;
using Microsoft.AspNetCore.Mvc;
using BTL.Models.MK;
using Microsoft.AspNetCore.Identity;

namespace BTL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IResponsitories responsitories;
        private readonly CarDbContext _context;
        public MemberController(IResponsitories responsitories, CarDbContext context)
        {
            this.responsitories = responsitories;
            _context = context;
        }
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Members members,string role)
        {
            var email = responsitories.GetMembersEmail(members.Email);
            var phone = responsitories.GetMembersSDT(members.SDT);
            if (!ModelState.IsValid)
            {
                if (email != null || phone != null)
                {
                    ViewBag.Mess = "Tài khoản này đã có";
                    return View("DangKy", members);

                }
            }
            
            try
            {
                 var service = new PasswordService();
                    var Hashpassword = service.HashPassword(members.MatKhau);
                    var member = new Members
                    {
                        TenDN = members.TenDN,
                        MatKhau = Hashpassword,
                        Email = members.Email,
                        SDT = members.SDT,
                        VaiTro = role
                    };

                    responsitories.Create(member);
                ViewBag.Mess = "Đăng ký thành công";
            }
            catch (Exception ex)
            {
                ViewBag.Mess = ex.Message;
            }
            ModelState.Clear();
            return View("DangKy");
        }
        public IActionResult Dangnhap()
        {
            return View("DangNhap");
        }
        [HttpPost]
        public IActionResult Login(Members members,string password)
        {
            
            var service = new PasswordService(); 
            var defaultPasswordHash = service.HashPassword("admin1122");
            //kiem tra admin
            // ✅ Kiểm tra admin
            if (members.SDT == "0368721805" && service.VerifyPassword(password, defaultPasswordHash))
            {
                // Lưu session
                HttpContext.Session.SetInt32("MaUser", 1); // id ảo
                HttpContext.Session.SetString("TenDN", "Admin");  // ✅ thêm dòng này
                HttpContext.Session.SetString("VaiTro", "Admin"); // ✅ thêm dòng này

                // Chuyển đến trang admin
                return RedirectToAction("Index", "Admin");
            }

            //kiem tra sodien thoai
            var member = responsitories.GetMembersSDT(members.SDT);
            if (member == null)
            {
                ViewBag.Message = "Sai tên truy cập hoặc đăng nhập";
                return View("DangNhap");
            }
            bool result = service.VerifyPassword(password, member.MatKhau);
            if (!result)
            {
                ViewBag.Message = "Sai tên truy cập hoặc đăng nhập";
                return View("DangNhap");
            }
            HttpContext.Session.SetString("SDT", member.SDT);
            HttpContext.Session.SetInt32("MaUser", member.MaUser);
            HttpContext.Session.SetString("VaiTro", member.VaiTro);
            HttpContext.Session.SetString("TenDN", member.TenDN);


            //dieu huong dựa trên vai trò 
            if (member.VaiTro == "ChuXe")
            {
                return RedirectToAction("Index", "Chuxe");
            }
            else if (member.VaiTro == "ThueXe")
            {
                return RedirectToAction("Index", "Home");
            }
            

            //Neu co loi quay lai trang dang nhap
            ViewBag.Message = "Đã có lỗi xảy ra ";
            return View("DangNhap");

        }
        //dang xuat
        public IActionResult Logout()
        {
            // Xóa tất cả session
            HttpContext.Session.Clear();
            // Chuyển về trang đăng nhập
            return RedirectToAction("DangNhap", "Member");
        }

        public IActionResult ChinhSua()
        {
            var maUser = HttpContext.Session.GetInt32("MaUser");
            if (maUser == null)
            {
                return RedirectToAction("DangNhap");
            }

            var user = _context.members.FirstOrDefault(x => x.MaUser == maUser);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // ✅ Xử lý lưu thay đổi
        [HttpPost]
        public IActionResult ChinhSua(Members model)
        {
            var maUser = HttpContext.Session.GetInt32("MaUser");
            if (maUser == null)
            {
                return RedirectToAction("DangNhap");
            }

            var user = _context.members.FirstOrDefault(x => x.MaUser == maUser);
            if (user == null)
            {
                return NotFound();
            }

            // Cập nhật các trường cho phép sửa
            user.TenDN = model.TenDN;
            user.Email = model.Email;
            user.SDT = model.SDT;

            // Nếu người dùng nhập mật khẩu mới
            if (!string.IsNullOrEmpty(model.MatKhau))
            {
                user.MatKhau = model.MatKhau;
            }

            _context.SaveChanges();

            ViewBag.Message = "Cập nhật tài khoản thành công!";
            return View(user);
        }
    }
}
