using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
namespace BTL.Models.Class
{
    [Table("tblMembers")]
    public class Members
    {
        [Key]
        public int MaUser {  get; set; }
        [Required(ErrorMessage ="KHông được để trống")]
        [StringLength(50,ErrorMessage ="Hãy nhập lại tên", MinimumLength = 3)]
        public string TenDN { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$",ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ và 1 số, tối thiểu 6 ký tự")]
        public string MatKhau {  get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Không đúng định dạng Email")]
        public string  Email { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Không đúng định dạng phone")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng 0")]
        public string SDT {  get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string VaiTro {  get; set; }
    }    
}
