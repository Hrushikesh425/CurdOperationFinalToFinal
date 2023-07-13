﻿using CurdOperationFinalToFinal.DAl;
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
            userData employee = new userData();
            return View(employee);


        }
		private gender model = new gender();

        [HttpPost]
		public IActionResult Create(userData data, List<userAddress> Address)
		{
			//if(!ModelState.IsValid)
			//{
			//	TempData["errorMessage"] = "model data is not valid ";
			//}
			bool result = _dal.Insert(data,Address);
			if(!result)
			{
				TempData["errorMessage"] = "unable to save the data";
				return View();
			}
			TempData["successMessage"] = "Employe Details Saved";
			return RedirectToAction("Index");
		}

        [HttpGet]
        public IActionResult Edit(int id)
        {
            userData employee = new userData();
            try
            {
                employee = _dal.GetById(id);
                List<country> countries = _dal.GetCountries();
                foreach (var item in employee.AddressList)
                {
                    item.countries = countries;
                }

                if (employee.id == null)
                {
                    TempData["errorMessage"] = $"User details not found with Id: {id}";
                    return RedirectToAction("Index");
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }


         [HttpPost]
        public IActionResult Edit(userData employee, List<userAddress> AddressList)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    TempData["errorMessage: "] = "Model Data is invalid";
                //    return View();
                //}
                bool result = _dal.UpdateAll(employee, AddressList);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to update the data";
                    return View();
                }
                TempData["successMessage"] = "Employee Details upadated";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["errorMessage: "] = ex.Message;
                return View();
            }

        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
           
            try
            {

                userData userViewModel = _dal.GetById(id);

                if (userViewModel.id == 0)
                {
                    TempData["errorMessage"] = $"User details not found with Id: {id}";
                    return RedirectToAction("Index");
                }
                return View(userViewModel);
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DelteConfirmed(userData data)
        {
            try
            {
                bool result = _dal.Delete(data.id);
                

                if (!result)
                {
                    TempData["errorMessage"] = "Unable to update the data";
                    return View();
                }
                TempData["sucessMessage"] = "Employee details Updated";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }


        public IActionResult AddAddressRow()
        {
            //List<AddressType> addressTypes = _employeeDAL.GetAddressTypes();
            List<country> countries = _dal.GetCountries();

            userAddress employeeVM = new userAddress
            {
                
                
                countries = countries,
            };

            return PartialView("_AddressTableRow", employeeVM);
        }






    }
}
