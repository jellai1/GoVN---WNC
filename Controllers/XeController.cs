using Microsoft.AspNetCore.Mvc;
using BTL.Models;
using System.Linq;

namespace BTL.Controllers
{
    public class XeController : Controller
    {
        private readonly CarDbContext _context;

        public XeController(CarDbContext context)
        {
            _context = context;
        }

        // 👉 Trang tìm kiếm
        public IActionResult Search(string keyword, string loaiXe, int? minPrice, int? maxPrice)
        {
            var query = _context.xes.AsQueryable();

            // Lọc theo tên hoặc mô tả
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.TenXe.Contains(keyword) || x.MoTa.Contains(keyword));

            // Lọc theo loại xe
            if (!string.IsNullOrEmpty(loaiXe))
                query = query.Where(x => x.LoaiXe == loaiXe);

            // Lọc theo giá thuê
            if (minPrice.HasValue)
                query = query.Where(x => x.GiaThueNgay >= minPrice);
            if (maxPrice.HasValue)
                query = query.Where(x => x.GiaThueNgay <= maxPrice);

            var result = query.ToList();

            // Truyền giá trị lọc để giữ lại trên form
            ViewBag.Keyword = keyword;
            ViewBag.LoaiXe = loaiXe;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(result);
        }
    }
}
