using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminReciptsDemo.Context;
using AdminReciptsDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminReciptsDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProviderController: ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;

        public ProviderController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Provider>>> Get()
        {
            return await _appDbContext.Providers.ToListAsync();
        }

        [HttpGet("{id}", Name = "GetProvider")]
        [Authorize]
        public async Task<ActionResult<Provider>> Get(int id)
        {
            var provider = await _appDbContext.Providers.FirstOrDefaultAsync(x => x.ProviderId == id);

            if (provider == null)
            {
                return NotFound();
            }

            return provider;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] Provider provider)
        {
            await _appDbContext.Providers.AddAsync(provider);
            await _appDbContext.SaveChangesAsync();

            return new CreatedAtRouteResult("GetProvider", new { id = provider.ProviderId }, provider);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] Provider provider)
        {
            _appDbContext.Entry(provider).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var providerId = await _appDbContext.Providers.Select(x => x.ProviderId).FirstOrDefaultAsync(x => x == id);

            if (providerId == default(int))
            {
                return NotFound();
            }

            _appDbContext.Providers.Remove(new Provider { ProviderId = providerId });
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}