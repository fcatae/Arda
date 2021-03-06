﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Arda.Common.Utils;
using System.Net.Http;
using Arda.Main.Utils;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Net.Http.Headers;
using Arda.Main.ViewModels;
using Newtonsoft.Json;

using ArdaSDK.Kanban;

namespace Arda.Main.Controllers
{
    [Authorize]
    public class UserFormController : Controller
    {
        [HttpGet]
        public IActionResult New(string template)
        {
            var user = this.GetCurrentUser();

            ViewBag.Title = "Title";

            switch (template.ToLower())
            {
                case "event":
                    ViewBag.Title = "Event Name";
                    ViewBag.ReportName = "_sampleEventReport";
                    break;
                case "customer":
                    ViewBag.Title = "Report Title";
                    ViewBag.ReportName = "_sampleCustomerReport";
                    break;
            }

            UsageTelemetry.Track(user, ArdaUsage.Userform_Index);

            return View();
        }        

        [HttpPost]
        public async Task<IActionResult> Submit([FromForm] string workloadTitle, [FromForm] string workloadDate, [FromForm] string workloadDescription)
        {
            var user = this.GetCurrentUser();

            UsageTelemetry.Track(user, ArdaUsage.Userform_Submit);

            await CreateWorkload(workloadTitle, DateTime.Parse(workloadDate), workloadDescription);

            return RedirectToAction("Index", "Dashboard");
        }

        async Task CreateWorkload(string title, DateTime date, string description)
        {
            var uniqueName = this.GetCurrentUser();

            var workload = new WorkloadViewModel2()
            {
                WBTitle = title,
                WBDescription = description,
                WBStartDate = date,
                WBEndDate = date,

                WBID = Guid.NewGuid(),
                WBCreatedDate = DateTime.Now,
                WBCreatedBy = uniqueName,
                WBUsers = new string[] { uniqueName },

                WBIsWorkload = true, // dont forget to set to true
                WBStatus = 2,

                WBActivity = Guid.Empty,
                WBComplexity = 0,
                WBExpertise = 0
            };            
                        
            var response = await Util.ConnectToRemoteService(HttpMethod.Post, Util.KanbanURL + "api/workload/add", uniqueName, "", workload);

            // Util.KanbanClient.WorkspaceFolders.Create(uniqueName, "workload");

            //var client = new ArdaSDK.Kanban.KanbanClient();
            //client.WorkspaceFolders.Create("abc");
            //var workspace = new ArdaSDK.Kanban.WorkspaceFolders(client);

            // workspace.
                ///.KanbanClient();
            
        }
    }
}
