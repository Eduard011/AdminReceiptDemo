using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminReciptsDemo.Models
{
    public class Receipt
    {
        public int ReceiptId { get; set; }
        public int ProviderId { get; set; }
        public float Amount { get; set; }
        public int CurrencyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
        public Provider Provider { get; set; }
        public Currency Currency { get; set; }
    }
}