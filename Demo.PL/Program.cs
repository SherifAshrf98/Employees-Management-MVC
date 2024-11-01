using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); // Add this line

builder.Services.AddDbContext<MvcDbContext>(DbContextOptionsBuilder =>
{
	DbContextOptionsBuilder.UseSqlServer(ConnectionString);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 

var assemblies = AppDomain.CurrentDomain.GetAssemblies()
					 .Where(assembly => assembly.FullName.StartsWith("Demo.PL"))
					 .ToArray();

builder.Services.AddAutoMapper(assemblies);

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

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
