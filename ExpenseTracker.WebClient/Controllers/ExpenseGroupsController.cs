﻿using ExpenseTracker.DTO;
using ExpenseTracker.WebClient.Helpers;
using ExpenseTracker.WebClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.WebClient.Controllers
{
    public class ExpenseGroupsController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var client = ExpenseTrackerHttpClient.GetClient();

            var model = new ExpenseGroupsViewModel();

            var egsResponse = await client.GetAsync("api/expensegroupstatusses");
            if (egsResponse.IsSuccessStatusCode)
            {
                string egsContent = await egsResponse.Content.ReadAsStringAsync();
                var lstExpenseGroupStatusses = JsonConvert
                    .DeserializeObject<IEnumerable<ExpenseGroupStatus>>(egsContent);

                model.ExpenseGroupStatusses = lstExpenseGroupStatusses;
            }
            else
            {
                return Content("An error occurred.");
            }

            HttpResponseMessage response = await client.GetAsync("api/expensegroups");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var lstExpenseGroups = JsonConvert.DeserializeObject<IEnumerable<ExpenseGroup>>(content);

                model.ExpenseGroups = lstExpenseGroups;
            }
            else
            {
                return Content("An error occurred.");
            }

            return View(model);
        }

        // GET: ExpenseGroups/Details/5
        public ActionResult Details(int id)
        {
            return Content("Not implemented yet.");
        }

        // GET: ExpenseGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpenseGroups/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExpenseGroup expenseGroup)
        {
            try
            {
                HttpClient client = ExpenseTrackerHttpClient.GetClient();

                // an expensegroup is created with status "Open", for the current user
                expenseGroup.ExpenseGroupStatusId = 1;
                expenseGroup.UserId = @"https://expensetrackeridsrv3/embedded_1";

                var serializedItemToCreate = JsonConvert.SerializeObject(expenseGroup);

                HttpResponseMessage response = await client.PostAsync("api/expensegroups",
                    new StringContent(serializedItemToCreate, System.Text.Encoding.Unicode, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error occurred.");
                }
            }
            catch
            {
                return Content("An error occurred.");
            }
        }

        // GET: ExpenseGroups/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpClient client = ExpenseTrackerHttpClient.GetClient();

            HttpResponseMessage response = await client.GetAsync("api/expensegroups/" + id);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<ExpenseGroup>(content);
                return View(model);
            }

            return Content("An error occurred.");
        }

        // POST: ExpenseGroups/Edit/5   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ExpenseGroup expenseGroup)
        {
            try
            {
                HttpClient client = ExpenseTrackerHttpClient.GetClient();

                // serialize & PUT
                var serializedItemToUpdate = JsonConvert.SerializeObject(expenseGroup);

                HttpResponseMessage response = await client.PutAsync("api/expensegroups/" + id,
                    new StringContent(serializedItemToUpdate, System.Text.Encoding.Unicode, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("An error occurred");
                }
            }
            catch
            {
                return Content("An error occurred");
            }
        }

        // POST: ExpenseGroups/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
