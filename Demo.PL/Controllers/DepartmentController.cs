using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
	[AutoValidateAntiforgeryToken]
	public class DepartmentController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			var departments = _unitOfWork.DepartmentRepository.GetAll();

			var departmentsvm = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);

			return View(departmentsvm);
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(DepartmentViewModel departmentvm)
		{
			if (ModelState.IsValid)
			{
				var department = _mapper.Map<Department>(departmentvm);

				 _unitOfWork.DepartmentRepository.Add(department);

				var result = _unitOfWork.complete();
				if (result > 0)
					TempData["Message"] = "Department Added Successfully";

				return RedirectToAction("Index");
			}
			return View(departmentvm);
		}

		public IActionResult Details(int? id, string ViewName = "Details")
		{
			if (id is null)
				return BadRequest();

			var department = _unitOfWork.DepartmentRepository.GetByID(id.Value);

			if (department is null)
				return NotFound();

			var departmentvm = _mapper.Map<DepartmentViewModel>(department);

			return View(ViewName, departmentvm);
		}

		public IActionResult Edit(int? id)
		{
			return Details(id, "Edit");
		}

		[HttpPost]
		public IActionResult Edit(DepartmentViewModel departmentvm, [FromRoute] int? id)
		{
			if (id != departmentvm.Id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
					var department = _mapper.Map<Department>(departmentvm);

					 _unitOfWork.DepartmentRepository.Update(department);

					var result = _unitOfWork.complete();

					if (result > 0)
						TempData["Message"] = "Department Updated Successfully";

					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(departmentvm);
		}


		public IActionResult Delete(int? id)
		{
			return Details(id, "Delete");
		}


		[HttpPost]
		public IActionResult Delete(DepartmentViewModel departmentvm, [FromRoute] int? id)
		{
			if (id != departmentvm.Id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
					var department = _mapper.Map<Department>(departmentvm);

					 _unitOfWork.DepartmentRepository.Delete(department);

					var result =_unitOfWork.complete();

					if (result > 0)
						TempData["Message"] = "Department Deleted Successfully";

					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(departmentvm);
		}
	}
}
