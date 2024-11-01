using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly MvcDbContext _dbContext;

		public IDepartmentRepository DepartmentRepository { get; }

		public IEmployeeRepository EmployeeRepository { get; }

		public UnitOfWork(MvcDbContext dbContext)
		{
			_dbContext = dbContext;
			DepartmentRepository = new DepartmentRepository(dbContext);
			EmployeeRepository = new EmployeeRepository(dbContext);
		}

		public int complete()
		{
			return _dbContext.SaveChanges();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}
	}
}
