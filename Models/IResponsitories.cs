using BTL.Models.Class;

namespace BTL.Models
{
    public interface IResponsitories
    {
        public void Create(Members members);
        public Members GetMembersEmail(string email);
        public Members GetMembersSDT(string sdt);
        public List<Members> ListMembers();
        public void CreateCar(Xe xes);
        public List<Xe> ListXe(int? id);
        public List<Xe> GetAll();
        public Xe GetBienso(string bienso);
        public Xe GetId(int id);
        public Xe GetIDchu(int? id);
        public bool Delete(int id); 
        public Xe Update(Xe x);

    }
}
