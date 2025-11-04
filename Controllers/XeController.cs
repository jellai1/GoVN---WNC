using Microsoft.AspNetCore.Mvc;
using BTL.Models;
using System.Linq;
using BTL.Models.Class;

namespace BTL.Controllers
{
    public class XeController : Controller
    {
        private readonly CarDbContext _context;


    public XeController(CarDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Search(string keyword, string loaiXe, int? minPrice, int? maxPrice)
        {
            var query = _context.xes.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.TenXe.Contains(keyword) || x.MoTa.Contains(keyword));

            if (!string.IsNullOrEmpty(loaiXe))
                query = query.Where(x => x.LoaiXe == loaiXe);

            if (minPrice.HasValue)
                query = query.Where(x => x.GiaThueNgay >= minPrice);
            if (maxPrice.HasValue)
                query = query.Where(x => x.GiaThueNgay <= maxPrice);

            var result = query.ToList();

            var repo = new DbResponsitories(_context);
            var tatCaDatXe = repo.GetAllDatXe()
                                 .Where(d => d.TrangThai == "Đang chờ xác nhận" || d.TrangThai == "Đã xác nhận")
                                 .ToList();

            var daDatXe = new Dictionary<int, string>();

            foreach (var xe in result)
            {
                var datXeXe = tatCaDatXe
                                .Where(d => d.MaXe == xe.MaXe)
                                .OrderByDescending(d => d.TrangThai == "Đã xác nhận" ? 2 : 1)
                                .FirstOrDefault();

                if (datXeXe != null)
                {
                    daDatXe[xe.MaXe] = datXeXe.TrangThai;
                }
                else
                {
                    daDatXe[xe.MaXe] = "Chưa đặt";
                }
            }

            ViewBag.DaDatXe = daDatXe;
            ViewBag.Keyword = keyword;
            ViewBag.LoaiXe = loaiXe;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(result);
        }

        public IActionResult DatXe(int id)
        {
            var xe = _context.xes.FirstOrDefault(x => x.MaXe == id);
            if (xe == null)
                return NotFound();

            return View(xe);
        }
    }


}
