using BTL.Models.Class;

using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace BTL.Models
{
  
    public class DbResponsitories:IResponsitories
    {
        private readonly CarDbContext context;
        public DbResponsitories(CarDbContext context) { this.context = context; }

        public void Create(Members members)
        {
             context.members.Add(members);
             context.SaveChanges();
        }

        public void CreateDatXe(DatXe datXe)
        {
            context.datXes.Add(datXe);
            context.SaveChanges();
        }


     

        public bool IsXeDaDuocDat(int maXe, int maNguoiThue)
        {
            return context.datXes.Any(d =>
                d.MaXe == maXe &&
                d.MaNguoiThue == maNguoiThue &&
                (d.TrangThai == "Đang chờ xác nhận" || d.TrangThai == "Đã xác nhận"));
        }





        public IEnumerable<DatXe> GetAllDatXe()
        {
            return context.datXes.ToList();
        }

        public List<Members> ListMembers()
        {
            return context.members.ToList();
        }
        public Members GetMembersEmail(string email) {
            return context.members.FirstOrDefault(m=> m.Email==email);
        }
        public Members GetMembersSDT(string sdt)
        {
            return context.members.FirstOrDefault(m => m.SDT == sdt);
        }
        //-------------------------------xe----------------------------------------------
        public void CreateCar(Xe xes)
        {
            context.xes.Add(xes);
            context.SaveChanges();
        }
        public List<Xe> ListXe(int? id)
        {
            return context.xes.Where(x => x.MaChuXe == id).ToList();
        }
        public List<Xe> GetAll()
        {
            return context.xes.ToList();
        }
        public Xe GetId(int id)
        {
            return context.xes.FirstOrDefault(m => m.MaXe == id);
        }
        public Xe GetIDchu(int? id)
        {
            return context.xes.FirstOrDefault(m => m.MaChuXe == id);
        }
        public bool Delete(int id)
        {
            try
            {
                // 🔹 Xóa các đơn thuê do người này tạo (nếu là khách thuê xe)
                var datXeNguoiThue = context.datXes.Where(d => d.MaNguoiThue == id).ToList();
                context.datXes.RemoveRange(datXeNguoiThue);

                // 🔹 Lấy các xe do người này đăng (nếu là chủ xe)
                var xeCuaChu = context.xes.Where(x => x.MaChuXe == id).ToList();

                // 🔹 Với mỗi xe, xóa các đơn thuê liên quan trước
                foreach (var xe in xeCuaChu)
                {
                    var datXeLienQuan = context.datXes.Where(d => d.MaXe == xe.MaXe).ToList();
                    context.datXes.RemoveRange(datXeLienQuan);
                }

                // 🔹 Sau đó mới xóa các xe của họ
                context.xes.RemoveRange(xeCuaChu);

                // 🔹 Cuối cùng, xóa tài khoản thành viên
                var member = context.members.FirstOrDefault(m => m.MaUser == id);
                if (member != null)
                {
                    context.members.Remove(member);
                }

                // 🔹 Lưu thay đổi
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi xóa tài khoản: " + ex.Message);
                return false;
            }
        }

        public Xe Update(Xe x)
        {
            var xes = GetId(x.MaXe);
            if (xes==null)
            {
                return null;
            }
            context.xes.Update(xes);
            context.SaveChanges();
            return xes ;
        }
        public Xe GetBienso(string bienso)
        {
            return context.xes.FirstOrDefault(m => m.BienSo == bienso);
        }
    }
}
