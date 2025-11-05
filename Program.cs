using BTL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BTL.Models.Class;
using BTL.Models.MK;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<CarDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbconnect")).EnableSensitiveDataLogging().LogTo(Console.WriteLine, LogLevel.Information));  // th�m d�ng n�y
          

builder.Services.AddScoped<IResponsitories,DbResponsitories>();
//add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromSeconds(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    }

);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
//su dung session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarDbContext>();

    // Kiểm tra xem admin có chưa
    if (!context.members.Any(m => m.VaiTro == "Admin"))
    {
        var service = new PasswordService();
        var admin = new Members
        {
            TenDN = "Admin",
            Email = "admin@govn.vn",
            SDT = "0368721805",
            VaiTro = "Admin",
            MatKhau = service.HashPassword("admin1122")
        };
        context.members.Add(admin);
        context.SaveChanges();
        Console.WriteLine("✅ Đã tạo tài khoản Admin mặc định!");
    }
}
