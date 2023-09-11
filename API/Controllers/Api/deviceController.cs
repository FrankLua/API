﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using API.Entity.SecrurityClass;
using System.Text;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.DAL.Entity.SupportClass;
using MongoDB.Bson;
using Amazon.Runtime.Internal;
using API.Services.ForAPI.Int;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDevice_Service Device;

        public DeviceController(IDevice_Service _device)
        {
            Device = _device;
        }

        // GET: api/<DeviceController>
        [HttpGet, BasicAuthorization]
        [Route("")]
        public async Task<BaseResponse<DeviceResponce>> GetDevicebyId([FromQuery(Name = "id")] string id)
        {
            await Loger.BeginMethod(Request);
            var responce = await Device.GetDevice(id);
            await Loger.FinishMethod(Request,"Successful",responce.ToJson());
            return responce;
        }
        [HttpGet]
        [Route("helloy")]
        public ActionResult helloy()
        {
            return Ok("Helloy, I'am work!");
        }

    }
}