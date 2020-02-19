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
    public class ReceiptController: ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;

        public ReceiptController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Receipt>>> Get()
        {
            return await _appDbContext.Receipts.Include(x => x.Currency).Include(x => x.Provider).ToListAsync();
        }

        [HttpGet("{id}", Name = "GetReceipt")]
        [Authorize]
        public async Task<ActionResult<Receipt>> Get(int id)
        {
            var receipt = await _appDbContext.Receipts.Include(x => x.Provider).Include(x => x.Currency).FirstOrDefaultAsync(x => x.CurrencyId == id);

            if (receipt == null)
            {
                return NotFound();
            }

            return receipt;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] Receipt receipt)
        {
            await _appDbContext.Receipts.AddAsync(receipt);
            await _appDbContext.SaveChangesAsync();

            return new CreatedAtRouteResult("GetReceipt", new { id = receipt.ReceiptId }, receipt);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] Receipt receipt)
        {
            _appDbContext.Entry(receipt).State = EntityState.Modified;
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var receiptId = await _appDbContext.Receipts.Select(x => x.ReceiptId).FirstOrDefaultAsync(x => x == id);

            if (receiptId == default(int))
            {
                return NotFound();
            }

            _appDbContext.Receipts.Remove(new Receipt { ReceiptId = receiptId });
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}