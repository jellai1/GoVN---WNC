using BTL.Models;
using BTL.Models.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace BTL.Controllers
{
    public class ChuxeController : Controller
    {
        private readonly IResponsitories _repo;
        private readonly CarDbContext _context;


    public ChuxeController(IResponsitories repo, CarDbContext context)
        {
            _repo = repo;
            _context = context;
        }

      
        public IActionResult Index()
        {
            var maChuXe = HttpContext.Session.GetInt32("MaUser");
            if (maChuXe == null)
            {
                return RedirectToAction("DangNhap", "Member");
            }

            List<Xe> dsXe;
            if (maChuXe == 1) // admin
                dsXe = _repo.GetAll();
            else
                dsXe = _repo.ListXe(maChuXe.Value);

            return View(dsXe);
        }


        public IActionResult Them(int? id)
        {
            if (id == null) return View(new Xe()); // Thêm mới
            var xe = _repo.GetId(id.Value); // Sửa
            return View(xe);
        }

        [HttpPost]
        public IActionResult CreateXe(Xe xes, IFormFile AnhXe)
        {
            var maChuXe = HttpContext.Session.GetInt32("MaUser");
            if (maChuXe == null) return RedirectToAction("DangNhap", "Member");

            if (xes.MaXe == 0) // Thêm mới
            {
                var bienso = _repo.GetBienso(xes.BienSo);
                if (bienso != null)
                {
                    ViewBag.Mes = "Xe đã được đăng ký";
                    return View("Them", xes);
                }

                if (AnhXe == null || AnhXe.Length == 0)
                {
                    ViewBag.Mes = "Vui lòng chọn ảnh hợp lệ.";
                    return View("Them", xes);
                }

                try
                {
                    string folder = "Anh";
                    string fileName = Path.GetFileName(AnhXe.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);

                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        AnhXe.CopyTo(stream);
                    }

                    xes.AnhXe = "/" + folder + "/" + fileName;
                    xes.MaChuXe = maChuXe.Value;
                    _repo.CreateCar(xes);

                    ViewBag.Mes = "Thêm xe thành công!";
                }
                catch (Exception ex)
                {
                    ViewBag.Mes = "Lỗi khi thêm xe: " + ex.Message;
                    return View("Them", xes);
                }
            }
            else 
            {
                var xeCu = _repo.GetId(xes.MaXe);
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

                if (AnhXe != null && AnhXe.Length > 0)
                {
                    string folder = "Anh";
                    string fileName = Path.GetFileName(AnhXe.FileName);
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        AnhXe.CopyTo(stream);
                    }
                    xeCu.AnhXe = "/" + folder + "/" + fileName;
                }

                _repo.Update(xeCu);
                ViewBag.Mes = "Cập nhật xe thành công!";
            }

            return View("Them", xes);
        }

        public IActionResult Delete(int id)
        {
            bool deleted = _repo.Delete(id);
            return Json(new { Success = deleted });
        }

   
        public IActionResult YeuCauDatXe()
        {
            var maChuXe = HttpContext.Session.GetInt32("MaUser");
            if (maChuXe == null) return RedirectToAction("DangNhap", "Member");

            var dsYeuCau = (from d in _context.datXes
                            join x in _context.xes on d.MaXe equals x.MaXe
                            join m in _context.members on d.MaNguoiThue equals m.MaUser
                            where x.MaChuXe == maChuXe.Value
                            orderby d.MaDatXe descending
                            select new YeuCauDatXeViewModel
                            {
                                MaDatXe = d.MaDatXe,
                                TenXe = x.TenXe,
                                TenNguoiThue = m.TenDN,
                                Email = m.Email,
                                SDT = m.SDT,
                                NgayBatDau = d.NgayBatDau,
                                NgayKetThuc = d.NgayKetThuc,
                                TongTien = d.TongTien,
                                TrangThai = d.TrangThai,
                                TrangThaiTT = d.TrangThaiTT
                            }).ToList();

            return View(dsYeuCau);
        }

        // Cập nhật trạng thái xác nhận
        [HttpPost]
        public IActionResult CapNhatTrangThai(int maDatXe, string trangThai)
        {
            var datXe = _context.datXes.FirstOrDefault(d => d.MaDatXe == maDatXe);
            if (datXe != null)
            {
                datXe.TrangThai = trangThai;
                _context.SaveChanges();
            }

            return RedirectToAction("YeuCauDatXe");
        }

       
        [HttpPost]
        public IActionResult ThanhToan(int maDatXe)
        {
            var datXe = _context.datXes.FirstOrDefault(d => d.MaDatXe == maDatXe);
            if (datXe != null && datXe.TrangThai == "Đã xác nhận")
            {
                datXe.TrangThaiTT = "Đã thanh toán";
                _context.SaveChanges();
            }
            return RedirectToAction("YeuCauDatXe");
        }
    }  

}


