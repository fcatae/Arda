﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using Arda.Kanban.Models.Repositories;
using Arda.Kanban.ViewModels;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Arda.Kanban.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }


        [HttpPost]
        [Route("add")]
        public HttpResponseMessage AddUser()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(HttpContext.Request.Body);
            string requestFromPost = reader.ReadToEnd();
            var user = JsonConvert.DeserializeObject<UserKanbanViewModel>(requestFromPost);

            if (user != null)
            {
                var res = _repository.AddNewUser(user);
                if (res)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            }
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage DeleteUser([FromQuery]string userID)
        {
            if (userID != null)
            {
                var res = _repository.DeleteUserByID(userID);
                if (res)
                {
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            }
        }

        [HttpGet]
        [Route("list")]
        public IEnumerable<UserKanbanViewModel> List()
        {
            var users = _repository.GetAllUsers();

            return users;
        }
    }
}