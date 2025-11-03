using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BTL.Models.Class
{
    [Table("tblXe")]
    public class Xe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaXe { get; set; }
        public int MaChuXe {  get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string TenXe {  get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string LoaiXe {  get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [RegularExpression(@"^\d{2}[A-Z]{1}-\d{3,5}(\.\d{2})?$", ErrorMessage = "Biển số xe không hợp lệ (VD: 30A-12345 hoặc 30A-123.45)")]
        public string BienSo { get; set; }
        [Range(1,float.MaxValue,ErrorMessage ="Giá phải lớn hơn 0")]
          public float GiaThueNgay {  get; set; }
          public string MoTa {  get; set; }
          public string TrangThai {  get; set; }
          public string AnhXe {  get; set; }

 

    }
}
