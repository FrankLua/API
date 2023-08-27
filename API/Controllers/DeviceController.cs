using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using API.Entity.SecrurityClass;
using System.Text;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.Services.ForAPI;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService Device;

        public DeviceController(IDeviceService _device)
        {
            Device = _device;
        }

        // GET: api/<DeviceController>
        [HttpGet, BasicAuthorization]
        [Route("")]
        public BaseResponse<DeviceResponce> GetDevicebyId([FromQuery(Name = "id")] int id)
        {
            Loger.WriterLogMethod("GetDevice", "Called");

            return Device.GetDevice(id);
        }
        [HttpGet]
        [Route("helloy")]
        public ActionResult helloy()
        {
            return Ok("Helloy, I'am work!");
        }

    }
}
