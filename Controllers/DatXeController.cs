using BTL.Models;
using BTL.Models.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BTL.Controllers
{
    public class DatXeController : Controller
    {
        private readonly IResponsitories _repo;
        private readonly CarDbContext _context;

        public DatXeController(CarDbContext context, IResponsitories repo)
        {
            _context = context;
            _repo = repo;
        }

       
        public IActionResult Index(int id)
        {
            var xe = _repo.GetId(id);
            if (xe == null) return NotFound();

            ViewBag.Xe = xe;
            return View();
        }

        // Danh sách xe khách đã đặt
        public IActionResult Index1()
        {
            var maNguoiThue = HttpContext.Session.GetInt32("MaUser");
            if (maNguoiThue == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập để xem danh sách thuê!";
                return RedirectToAction("DangNhap", "Member");
            }

            var danhSach = _context.datXes
                .Include(d => d.Xe)
                .Where(d => d.MaNguoiThue == maNguoiThue)
                .Select(d => new YeuCauDatXeViewModel
                {
                    MaDatXe = d.MaDatXe,
                    MaXe = d.MaXe,
                    TenXe = d.Xe.TenXe,
                    NgayBatDau = d.NgayBatDau,
                    NgayKetThuc = d.NgayKetThuc,
                    TongTien = d.TongTien,
                    TrangThai = d.TrangThai,
                    TrangThaiTT = d.TrangThaiTT
                }).ToList();

            return View(danhSach);
        }

  
        [HttpPost]
        public IActionResult DatXe(int MaXe, DateTime NgayBatDau, DateTime NgayKetThuc, string PhuongThucTT)
        {
            var maNguoiThue = HttpContext.Session.GetInt32("MaUser");
            if (maNguoiThue == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập để đặt xe!";
                return RedirectToAction("DangNhap", "Member");
            }

            var xe = _repo.GetId(MaXe);
            if (xe == null) return NotFound();

            bool daDat = _context.datXes.Any(x => x.MaXe == MaXe &&
                        (x.TrangThai == "Đang chờ xác nhận" || x.TrangThai == "Đã xác nhận") &&
                        NgayBatDau <= x.NgayKetThuc && NgayKetThuc >= x.NgayBatDau);

            if (daDat)
            {
                TempData["Message"] = "Xe đã được đặt trong khoảng thời gian này.";
                return RedirectToAction("Index", new { id = MaXe });
            }

            double soNgay = (NgayKetThuc - NgayBatDau).TotalDays + 1;
            float tongTien = (float)(xe.GiaThueNgay * soNgay);

            var datXe = new DatXe
            {
                MaXe = MaXe,
                MaNguoiThue = maNguoiThue.Value,
                NgayBatDau = NgayBatDau,
                NgayKetThuc = NgayKetThuc,
                TongTien = tongTien,
                PhuongThucTT = PhuongThucTT,
                TrangThai = "Đang chờ xác nhận",
                TrangThaiTT = "Chưa thanh toán"
            };

            _context.datXes.Add(datXe);
            _context.SaveChanges();

            TempData["Message"] = "Yêu cầu thuê xe đã gửi. Trạng thái: Đang chờ xác nhận.";
            return RedirectToAction("Index1");
        }

    
        public IActionResult ThanhToan(int maDatXe)
        {
            var maNguoiThue = HttpContext.Session.GetInt32("MaUser");
            if (maNguoiThue == null)
                return RedirectToAction("DangNhap", "Member");

            var datXe = _context.datXes
                .Include(d => d.Xe)
                .FirstOrDefault(d => d.MaDatXe == maDatXe && d.MaNguoiThue == maNguoiThue);

            if (datXe == null)
                return NotFound();

            var model = new YeuCauDatXeViewModel
            {
                MaDatXe = datXe.MaDatXe,
                MaXe = datXe.MaXe,
                TenXe = datXe.Xe.TenXe,
                TongTien = datXe.TongTien
            };

            return View(model);
        }

  
        [HttpPost]
        public IActionResult ThanhToanPost(int MaDatXe, string HinhThuc)
        {
            var datXe = _context.datXes.FirstOrDefault(d => d.MaDatXe == MaDatXe);
            if (datXe != null && datXe.TrangThai == "Đã xác nhận")
            {
                datXe.TrangThaiTT = "Đã thanh toán";
                datXe.PhuongThucTT = HinhThuc;
                _context.SaveChanges();
            }

            TempData["Mes"] = "Thanh toán thành công!";
            return RedirectToAction("Index1");
        }
    }
}
