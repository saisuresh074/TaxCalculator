using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Interfaces;

namespace TaxCalculator.Infrastructure.Credits
{
    public sealed class TaxCreditService : ITaxCreditService
    {
        public Task<decimal> GetCreditAmountAsync(string employeeId, CancellationToken ct)
            => Task.FromResult(0m);   // no credits until the real service exists
    }
}
