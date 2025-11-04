using BTL.Models.Class;
using Microsoft.EntityFrameworkCore;
namespace BTL.Models
{
    public class CarDbContext:DbContext
    {
        public CarDbContext(DbContextOptions<CarDbContext> options) : base(options) { }
        public DbSet<Members> members { get; set; }
        public DbSet<Xe> xes { get; set; }
        public DbSet<DatXe> datXes { get; set; }
   


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Xe>().ToTable("tblXe"); //  map đúng tên bảng
        }
    }
}
