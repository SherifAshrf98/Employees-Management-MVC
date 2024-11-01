using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
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
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		protected MvcDbContext _dbContext;
		public GenericRepository(MvcDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public void Add(T item)
		{
			_dbContext.Add(item);
		}
		public void Update(T item)
		{
			_dbContext.Update(item);
		}
		public void Delete(T item)
		{
			_dbContext.Remove(item);
		}

		public IEnumerable<T> GetAll()
		{
			if (typeof(T) == typeof(Employee))
			{
				return (IEnumerable<T>)_dbContext.Employees.Include(E => E.Department).ToList();
			}
			return _dbContext.Set<T>().ToList();
		}

		public T GetByID(int id)
		=> _dbContext.Set<T>().Find(id);
	}
}

