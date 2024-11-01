using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public EmployeeController(IUnitOfWork unitOfWork ,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public IActionResult Index(string SearchValue)
		{
			IEnumerable<Employee> Employees;

			if (SearchValue == null)
			{
				Employees = _unitOfWork.EmployeeRepository.GetAll();
			}
			else
			{
				Employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
			}

			var employeevm = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);

			return View(employeevm);
		}


		public IActionResult Create()
		{
			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

			return View();
		}

		[HttpPost]
		public IActionResult Create(EmployeeViewModel employeevm)
		{
			if (ModelState.IsValid)
			{
				var employee = _mapper.Map<Employee>(employeevm);

				 _unitOfWork.EmployeeRepository.Add(employee);

				var result = _unitOfWork.complete();
				if (result > 0)
					TempData["Message"] = "Employee Added Successfully";

				return RedirectToAction("Index");
			}
			return View(employeevm);
		}

		public IActionResult Details(int? id, string ViewName = "Details")
		{
			if (id == null)
				return BadRequest();

			var Employee = _unitOfWork.EmployeeRepository.GetByID(id.Value);

			if (Employee == null)
				return NotFound();

			var employeevm = _mapper.Map<EmployeeViewModel>(Employee);

			return View(ViewName, employeevm);
		}

		public IActionResult Edit(int? id)
		{
			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

			return Details(id, "Edit");
		}

		[HttpPost]
		public IActionResult Edit(EmployeeViewModel employeevm, [FromRoute] int? id)
		{
			if (employeevm.Id != id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
					var employee = _mapper.Map<Employee>(employeevm);

					_unitOfWork.EmployeeRepository.Update(employee);

					var result = _unitOfWork.complete();

					if (result > 0)
						TempData["Message"] = "Employee Updated Successfully";

					return RedirectToAction("Index");
				}

				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(employeevm);
		}
		public IActionResult Delete(int? id)
		{
			return Details(id, "Delete");
		}

		[HttpPost]
		public IActionResult Delete(EmployeeViewModel employeevm, [FromRoute] int? id)
		{
			if (employeevm.Id != id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
					var employee = _mapper.Map<Employee>(employeevm);

					_unitOfWork.EmployeeRepository.Delete(employee);

					var result = _unitOfWork.complete();

					if (result > 0)
						TempData["Message"] = "Employee Deleted Successfully";

					return RedirectToAction("Index");
				}

				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(employeevm);
		}
	}
}
