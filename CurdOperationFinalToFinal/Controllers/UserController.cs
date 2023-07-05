using CurdOperationFinalToFinal.DAl;
using CurdOperationFinalToFinal.Models;
using CurdOperationFinalToFinal.Models.UserVM;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
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
		public IActionResult GetStatesByCountry(int countryId)
		{
			List<state> states = _dal.GetStatesByCountry(countryId);
			return Json(states);
		}

		[HttpGet]
		public IActionResult Create()
		{
			//List<AddressType> addressTypes = _employeeDAL.GetAddressTypes();
			List<country> countries = _dal.GetCountries();

			UserVM userVm = new UserVM
			{
				userData = new userData(),
				Address = new userAddress(),
				
		        countries = countries,
			};

			return View(userVm); ;
		}
		private gender model = new gender();

        [HttpPost]
		public IActionResult Create(UserVM data)
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
