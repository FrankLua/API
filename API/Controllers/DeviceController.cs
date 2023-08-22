using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using API.Entity.SecrurityClass;
using API.Entity.APIResponce;
using API.Entity.Models;
using System.Text;

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
        public BaseResponse<List<Device>> GetDevicebyId([FromQuery(Name = "id")] int id)
        {            
            return Device.GetDevicebyId(id);
        }

        // GET api/<DeviceController>/5
        [HttpGet,BasicAuthorization]
        [Route("devicebyuserid")]
        public BaseResponse<List<int>> GetDevicebyUserId([FromQuery(Name = "user_id")] int id)
        {
            var login = Decoder(Request.Headers["Authorization"].ToString());
            return Device.GetDeviceIdbyUserId(id);
        }

        // POST api/<DeviceController>
        [HttpPost]
        public ActionResult<Device> Post([FromBody] Device device)
        {
            return device;
        }

        // PUT api/<DeviceController>/5
        [HttpPatch("{id}")]
        public ActionResult Put(int id, [FromBody] Device device)
        {
            var student = Device.GetbyID(id);
            if (student == null)
            {
                return NotFound($"Student with id = {id} not found");
            }
            Device.Update(id, device);
            return NoContent();
        }

        // DELETE api/<DeviceController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var student = Device.GetbyID(id);
            if (student == null)
            {
                return NotFound($"Student with id = {id} not found");
            }
            Device.Delete(id);
            return Ok($"Device with id = {id} deleted");
        }
        public static string Decoder (string httpget)
        {
            
            var authBase64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(httpget.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)));
            var authSplit = authBase64Decoded.Split(new[] { ':' }, 2);
            var clientId = authSplit[0];
            return clientId;

        }
    }
}
