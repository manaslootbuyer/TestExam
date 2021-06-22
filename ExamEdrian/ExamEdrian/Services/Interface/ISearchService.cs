using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExamEdrian.Commands;
using ExamEdrian.Models;

namespace ExamEdrian.Services
{
    public interface ISearchService
    {
        Task<bool> SaveEmailAsync(ReserveCommand command, CancellationToken cts = default);
        Task<List<CustomerDTO>> GetCustomers(string parkCode,string arriving, CancellationToken cts = default);
    }
}
