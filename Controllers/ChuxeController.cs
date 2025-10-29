using BTL.Models;
using BTL.Models.Class;
using Microsoft.AspNetCore.Mvc;

namespace BTL.Controllers
{
    public class ChuxeController : BaseController
    {
        private readonly IResponsitories responsitories;
        public ChuxeController(IResponsitories responsitories) { this.responsitories = responsitories; }
        public IActionResult Index(Xe xes)
        {

            var maChuXe = HttpContext.Session.GetInt32("MaUser");
            List<Xe> ds=new List<Xe>();
             if (maChuXe == null)
            {
                return View("DangNhap","Member");
            }
            else if (maChuXe == 1)
            {
                ds = responsitories.GetAll();
            }
            else
            {
                ds = responsitories.ListXe(maChuXe.Value);
            } 
            return View("Index", ds);
        }
        public IActionResult Them(int? id)
        {
            if (id == null) return View(new Xe()); // Thêm mới
            var xe = responsitories.GetId(id.Value); // Sửa
            return View(xe);
        }
        public IActionResult CreateXe(Xe xes,IFormFile AnhXe)
        {
            //them moi
            if (xes.MaXe == 0)
            {
                var bienso=responsitories.GetBienso(xes.BienSo);
                if (bienso != null)
                {
                    ViewBag.Mes = "Xe đã được đăng ký";
                    return View("Them",xes);
                }
                // Lấy mã chủ xe từ session
                var maChuXe = HttpContext.Session.GetInt32("MaUser");
                Console.WriteLine($"Session MaUser hiện tại: {maChuXe}");

                if (maChuXe == null)
                {
                    ViewBag.Mes = "Vui lòng đăng nhập trước khi thêm xe!";
                    return RedirectToAction("DangNhap", "Member");
                }
                //kiem tra anh
                if (AnhXe == null || AnhXe.Length == 0)
                {
                    ViewBag.Mes = "Vui lòng chọn ảnh hợp lệ.";
                    return View("Them",xes);
                }
                try
                {
                    // Thư mục tương đối
                    string folder = "Anh";
                    // Tên file gốc
                    string fileName = Path.GetFileName(AnhXe.FileName);
                    // Đường dẫn tuyệt đối đến thư mục lưu ảnh
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

                    // Nếu thư mục chưa tồn tại -> tạo mới
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Đường dẫn tuyệt đối lưu file
                    string filePath = Path.Combine(uploadPath, fileName);

                    // Lưu file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        AnhXe.CopyTo(stream);
                    }
                    //luu
                    string Anh = "/" + folder + "/" + fileName; // đường dẫn tương đối để hiển thi
                    xes.AnhXe = Anh;
                    xes.MaChuXe = maChuXe.Value;
                    responsitories.CreateCar(xes); // Lưu DB

                    ViewBag.Mes = "Thêm xe thành công!";
                }
                catch (Exception ex)
                {
                    ViewBag.Mes = "Lỗi khi thêm xe: " + ex.Message;
                    return View("Them",xes);
                }
            }
            //cap nhat
            else
            {
                var xeCu = responsitories.GetId(xes.MaXe);
                if (xeCu == null)
                {
                    ViewBag.Mes = "Xe không tồn tại!";
                    return View("Them", xes);
                }
                xeCu.TenXe = xes.TenXe;
                xeCu.LoaiXe = xes.LoaiXe;
                xeCu.BienSo = xes.BienSo;
                xeCu.GiaThueNgay = xes.GiaThueNgay;
                xeCu.MoTa = xes.MoTa;
                xeCu.TrangThai = xes.TrangThai;
                // Nếu người dùng upload ảnh mới, cập nhật; nếu không giữ ảnh cũ
                if (AnhXe != null && AnhXe.Length > 0)
                {
                    string folder = "Anh";
                    string fileName = Path.GetFileName(AnhXe.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        AnhXe.CopyTo(stream);
                    }
                    xeCu.AnhXe = "/" + folder + "/" + fileName;
                }
                //cap nhat
                responsitories.Update(xeCu);
                ViewBag.Mes = "Cập nhật xe thành công!";
            }
            return View("Them", xes);
        }

        public IActionResult Delete(int id)
        {
            bool deleted = responsitories.Delete(id);
            return Json(new { Success = deleted });
        }
        
        
    }
    
}
