using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(MvcDbContext dbContext) : base(dbContext)
		{

		}

		public IQueryable<Employee> GetEmployeesByAddress(string address)

		=> _dbContext.Employees.Where(E => E.Address == address);

		public IQueryable<Employee> GetEmployeesByName(string searchvalue)
		{
			_dbContext.Employees.Include(E => E.Department).ToList();

			return _dbContext.Employees.Where(E => E.Name.ToLower().Contains(searchvalue.ToLower()));
		}
	}
}
