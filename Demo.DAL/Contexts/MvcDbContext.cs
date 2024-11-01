using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Contexts
{
	public class MvcDbContext : DbContext
	{
		public MvcDbContext(DbContextOptions options) : base(options)
		{

		}
		public DbSet<Department> Departments { get; set; }
		public DbSet<Employee> Employees { get; set; }
	}
}
