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
            var count = context.SaveChanges();
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
            var x = GetId(id);
            if (x == null)
            {
                return false;
            }
            context.xes.Remove(x);
            context.SaveChanges();
            return true;

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
