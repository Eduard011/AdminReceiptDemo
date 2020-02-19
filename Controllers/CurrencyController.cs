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
    public class CurrencyController: ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;

        public CurrencyController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Currency>>> Get()
        {
            return await _appDbContext.MultiCurrency.ToListAsync();
        }

        [HttpGet("{id}", Name="GetCurrency")]
        [Authorize]
        public async Task<ActionResult<Currency>> Get(int id)
        {
            var currency = await _appDbContext.MultiCurrency.FirstOrDefaultAsync(x => x.CurrencyId == id);

            if (currency == null)
            {
                return NotFound();
            }

            return currency;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] Currency currency)
        {
            await _appDbContext.MultiCurrency.AddAsync(currency);
            await _appDbContext.SaveChangesAsync();

            return new CreatedAtRouteResult("GetCurrency", new { id = currency.CurrencyId }, currency);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] Currency currency)
        {
            _appDbContext.Entry(currency).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Currency>> Delete(int id)
        {
            var currencyId = await _appDbContext.MultiCurrency.Select(x => x.CurrencyId).FirstOrDefaultAsync(x => x == id);

            if (currencyId == default(int))
            {
                return NotFound();
            }

            _appDbContext.MultiCurrency.Remove(new Currency { CurrencyId = currencyId });
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}