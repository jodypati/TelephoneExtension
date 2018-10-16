using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreAPI.DataProvider;
using CoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Controllers
{
    [Route("api/[controller]")]
    public class TelephoneextensionController : Controller
    {
        private ITelephoneExtensionDataProvider extensionDataProvider;

        public TelephoneextensionController(ITelephoneExtensionDataProvider e)
        {
            this.extensionDataProvider = e;
        }

        [HttpGet]
        public async Task<IEnumerable<TelephoneExtension>> Get()
        {
            return await this.extensionDataProvider.GetExtensions();
        }

        [HttpGet("{RecordId}")]
        public async Task<TelephoneExtension> Get(int RecordId)
        {
            return await this.extensionDataProvider.GetExtension(RecordId);
        }

        [HttpPost]
        public async Task Post([FromBody]TelephoneExtension telephoneExtension)
        {
            await this.extensionDataProvider.AddExtension(telephoneExtension);
        }

    }
}
