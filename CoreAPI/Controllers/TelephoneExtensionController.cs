using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreAPI.Controllers
{
    [Route("api/v1/[controller]")]
    public class TelephoneExtensionController : ControllerBase
    {
        private readonly TelephoneExtensionContext _telephoneExtensionContext;
        //private readonly ICatalogIntegrationEventService
        public TelephoneExtensionController(TelephoneExtensionContext context)
        {
            _telephoneExtensionContext = context ?? throw new ArgumentNullException(nameof(context));
            ((DbContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/[controller]/extensions
        [HttpGet]
        [Route("extensions")]
        [ProducesResponseType(typeof(List<TelephoneExtensionItem>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> TelephoneExtensions()
        {
            var items = await _telephoneExtensionContext.TelephoneItems.ToListAsync();

            return Ok(items);
        }

        //POST api/v1/[controller]/items
        [Route("extensions")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateProduct([FromBody]TelephoneExtensionItem telephoneExtension)
        {
            var item = new TelephoneExtensionItem
            {
                BeginDate = telephoneExtension.BeginDate,
                EndDate = telephoneExtension.EndDate,
                RecordId = telephoneExtension.RecordId,
                ParentId = telephoneExtension.ParentId,
                Number = telephoneExtension.Number,
                UserChanger = telephoneExtension.UserChanger
            };
            _telephoneExtensionContext.TelephoneItems.Add(item);

            await _telephoneExtensionContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = item.RecordId }, null);
        }

        [HttpGet]
        [Route("extensions/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(TelephoneExtensionItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _telephoneExtensionContext.TelephoneItems.SingleOrDefaultAsync(ci => ci.RecordId == id);

            //var baseUri = _settings.PicBaseUrl;
            //var azureStorageEnabled = _settings.AzureStorageEnabled;
            //item.FillProductUrl(baseUri, azureStorageEnabled: azureStorageEnabled);

            if (item != null)
            {
                return Ok(item);
            }

            return NotFound();
        }

    }
}
