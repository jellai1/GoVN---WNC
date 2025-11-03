using BTL.Models.Class;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL.Models
{
    [Table("tblDatXe")]
    public class DatXe
    {
        [Key]
        public int MaDatXe { get; set; }

        [Required]
        public int MaXe { get; set; }

        [Required]
        public int MaNguoiThue { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayBatDau { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NgayKetThuc { get; set; }

        [Required]
        public float TongTien { get; set; }

        [StringLength(100)]
        public string? PhuongThucTT { get; set; }

        [StringLength(100)]
        public string? TrangThai { get; set; }

        [StringLength(100)]
        public string? TrangThaiTT { get; set; } = "Chưa thanh toán";

        [ForeignKey("MaXe")]
        public Xe Xe { get; set; }

    
        [ForeignKey("MaNguoiThue")]
        public Members NguoiThue { get; set; }
    }
}
