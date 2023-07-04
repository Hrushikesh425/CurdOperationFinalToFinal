using CurdOperationFinalToFinal.DAl;
using CurdOperationFinalToFinal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CurdOperationFinalToFinal.Controllers
{
	public class UserController : Controller
	{
		private readonly UserDAl _dal;
        public UserController(UserDAl dal)
        {
            _dal = dal;
        }
        public IActionResult Index()
        {
            List<userData> employees = new List<userData>();
            try
            {
                employees = _dal.GetAll();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(employees);
        }

        [HttpGet]
		public IActionResult Create()
		{
			return View();
		}
        private gender model = new gender();

        [HttpPost]
		public IActionResult Create(userData data)
		{
			if(!ModelState.IsValid)
			{
				TempData["errorMessage"] = "model data is not valid ";
			}
			bool result = _dal.Insert(data);
			if(!result)
			{
				TempData["errorMessage"] = "unable to save the data";
				return View();
			}
			TempData["successMessage"] = "Employe Details Saved";
			return RedirectToAction("Index");
		}
	}
}
