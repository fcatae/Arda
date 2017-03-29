using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arda.Common.Utils;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Microsoft.Net.Http.Headers;
using System.Net;
using Arda.Main.ViewModels;

namespace Arda.Main.Controllers
{
    [Authorize]
    public class WorkloadController : Controller
    {
        [HttpGet]
        public async Task<JsonResult> ListBacklogsByUser([FromQuery] string User)
        {
            var loggedUser = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var workloads = new List<string>();

            string filtered_user = (User == null || User == "") ? loggedUser : User;

            var existentWorkloads = await Util.ConnectToRemoteService<List<WorkloadsByUserViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/workload/listworkloadbyuser", filtered_user, "");

            var dados = existentWorkloads.Where(x => x._WorkloadIsWorkload == false)
                         .Select(x => new {
                             id = x._WorkloadID,
                             title = x._WorkloadTitle,
                             start = x._WorkloadStartDate.ToString("dd/MM/yyyy"),
                             end = x._WorkloadEndDate.ToString("dd/MM/yyyy"),
                             hours = x._WorkloadHours,
                             attachments = x._WorkloadAttachments,
                             tag = x._WorkloadExpertise,
                             status = x._WorkloadStatus,
                             users = x._WorkloadUsers,
                             textual = x._WorkloadTitle + " (Started in " + x._WorkloadStartDate.ToString("dd/MM/yyyy") + " and Ending in " + x._WorkloadEndDate.ToString("dd/MM/yyyy") + ", with  " + x._WorkloadHours + " hours spent on this."
                         })
                         .Distinct()
                         .ToList();

            return Json(dados);
        }

        [HttpGet]
        public async Task<JsonResult> ListWorkloadsByUser([FromQuery] string User)
        {
            var loggedUser = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var workloads = new List<string>();

            string filtered_user = (User == null || User == "") ? loggedUser : User;

            var existentWorkloads = await Util.ConnectToRemoteService<List<WorkloadsByUserViewModel>>(HttpMethod.Get, Util.KanbanURL + "api/workload/listworkloadbyuser", filtered_user, "");

            var dados = existentWorkloads.Where(x => x._WorkloadIsWorkload == true)
                         .Select(x => new {
                             id = x._WorkloadID,
                             title = x._WorkloadTitle,
                             start = x._WorkloadStartDate.ToString("dd/MM/yyyy"),
                             end = x._WorkloadEndDate.ToString("dd/MM/yyyy"),
                             hours = x._WorkloadHours,
                             attachments = x._WorkloadAttachments,
                             tag = x._WorkloadExpertise,
                             status = x._WorkloadStatus,
                             users = x._WorkloadUsers,
                             textual = x._WorkloadTitle + " (Started in " + x._WorkloadStartDate.ToString("dd/MM/yyyy") + " and Ending in " + x._WorkloadEndDate.ToString("dd/MM/yyyy") + ", with  " + x._WorkloadHours + " hours spent on this."
                         })
                         .Distinct()
                         .ToList();

            return Json(dados);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Add(ICollection<IFormFile> WBFiles, WorkloadViewModel workload)
        {
            //Owner:
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            //Complete WB fields:
            workload.WBCreatedBy = uniqueName;
            workload.WBCreatedDate = DateTime.Now;
            //Iterate over files:
            if (WBFiles.Count > 0)
            {
                List<Tuple<Guid, string, string>> fileList = await UploadNewFiles(WBFiles);
                //Adds the file lists to the workload object:
                workload.WBFilesList = fileList;
            }
            var response = await Util.ConnectToRemoteService(HttpMethod.Post, Util.KanbanURL + "api/workload/add", uniqueName, "", workload);

            UsageTelemetry.Track(uniqueName, ArdaUsage.Workload_Add);

            if (response.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPut]
        public async Task<HttpResponseMessage> Update(ICollection<IFormFile> WBFiles, List<string> oldFiles, WorkloadViewModel workload)
        {
            //Owner:
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            //Update WB fields:
            //workload.WBCreatedBy = uniqueName;
            //workload.WBCreatedDate = DateTime.Now;
            //Iterate over files:
            var fileList = new List<Tuple<Guid, string, string>>();
            if (WBFiles.Count > 0)
            {
                fileList = await UploadNewFiles(WBFiles);
            }
            if (oldFiles != null)
            {
                for (int i = 0; i < oldFiles.Count; i++)
                {
                    var split = oldFiles[i].Split('&');
                    var f = new Tuple<Guid, string, string>(new Guid(split[0]), split[1], split[2]);
                    fileList.Add(f);
                }
            }
            //Adds the file lists to the workload object:
            workload.WBFilesList = fileList;

            var response = await Util.ConnectToRemoteService(HttpMethod.Put, Util.KanbanURL + "api/workload/edit", uniqueName, "", workload);

            if (response.IsSuccessStatusCode)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetWorkload(Guid workloadID)
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            var workload = await Util.ConnectToRemoteService<WorkloadViewModel>(HttpMethod.Get, Util.KanbanURL + "api/workload/details?=" + workloadID, uniqueName, "");
            return Json(workload);
        }

        [HttpPut]
        public async Task<HttpResponseMessage> UpdateStatus([FromQuery]string Id, [FromQuery]int Status)
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            //System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            //string requestFromPost = reader.ReadToEnd();

            await Util.ConnectToRemoteService<string>(HttpMethod.Put, Util.KanbanURL + "api/workload/updatestatus?id=" + Id + "&status=" + Status, uniqueName, "");
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid workloadID)
        {
            var uniqueName = HttpContext.User.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            await Util.ConnectToRemoteService(HttpMethod.Delete, Util.KanbanURL + "api/workload/delete?=" + workloadID, uniqueName, "");
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public JsonResult GetGuid()
        {
            return Json(Util.GenerateNewGuid());
        }

        private static async Task<List<Tuple<Guid, string, string>>> UploadNewFiles(ICollection<IFormFile> WBFiles)
        {
            var fileList = new List<Tuple<Guid, string, string>>();
            var Configuration = new ConfigurationBuilder().AddJsonFile("secrets.json").Build();
            var connectionString = Configuration["Storage:AzureBLOB:ConnectionString"];
            var containerName = Configuration["Storage:AzureBLOB:ContainerName"];
            // Retrieve storage account information from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            // Create a blob client for interacting with the blob service.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Create a container for organizing blobs within the storage account.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            foreach (var file in WBFiles)
            {
                if (file.Length > 0)
                {
                    //Get file properties:
                    var filePath = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileStream = file.OpenReadStream();
                    var fileName = new FileInfo(filePath).Name;
                    var fileExt = new FileInfo(filePath).Extension;
                    var fileID = Util.GenerateNewGuid();
                    var fileNameUpload = string.Concat(fileID, fileExt);
                    //Upload the file:
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileNameUpload);
                    await blockBlob.UploadFromStreamAsync(fileStream);
                    //Retrieve the url:
                    string fileURL = blockBlob.Uri.ToString();
                    //GUID, URL and Name:
                    fileList.Add(Tuple.Create(fileID, fileURL, fileName));
                }
            }

            return fileList;
        }
    }
}
