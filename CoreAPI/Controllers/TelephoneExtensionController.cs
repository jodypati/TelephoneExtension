using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreAPI.DataProvider;
using CoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TelephoneextensionController : Controller
    {
        private ITelephoneExtensionDataProvider extensionDataProvider;

        public string Protected()
        {
            return "Only if you have a valid token!";
        }

        public TelephoneextensionController(ITelephoneExtensionDataProvider e)
        {
            this.extensionDataProvider = e;
        }

        [HttpGet]
        public async Task<IEnumerable<TelephoneExtension>> Get()
        {
            return await this.extensionDataProvider.GetExtensions();
        }

        [HttpGet("{RecordId:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(TelephoneExtension), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRecordById(int RecordId)
        {
            if (RecordId <= 0)
            {
                return BadRequest();
            }
            var extension = await this.extensionDataProvider.GetExtension(RecordId);
            if (extension != null)
            {
                return Ok(extension);
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody]TelephoneExtension telephoneExtension)
        {
            var id = await this.extensionDataProvider.AddExtension(telephoneExtension);
            return CreatedAtAction(nameof(GetRecordById), new { RecordId = id }, null);
        }
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> UpdateTelephoneExtension([FromBody]TelephoneExtension telephoneExtension)
        {
            await this.extensionDataProvider.UpdateExtension(telephoneExtension);
            return CreatedAtAction(nameof(GetRecordById), new { RecordId = telephoneExtension.RecordId }, null);
        }

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await this.extensionDataProvider.DeleteExtension(id);
            return NoContent();
        }
    }
}
