using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExamEdrian.Commands;
using ExamEdrian.Models;
using MvvmAspire;

namespace ExamEdrian.Services
{
    public class SearchService : ISearchService
    {
        readonly IDataService _dataService;
        public SearchService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<CustomerDTO>> GetCustomers(string parkCode, string arriving, CancellationToken cts = default)
        {
            return await _dataService.GetResponseAsync<List<CustomerDTO>>($"api/NPS/Customers?parkCode={parkCode}&arriving={arriving}", cts, WebRequestMethod.GET);
        }

        public async Task<bool> SaveEmailAsync(ReserveCommand command, CancellationToken cts = default)
        {
            var result = await _dataService.GetResponseAsync<string>("api/NPS/Response", cts, WebRequestMethod.POST, command);
            return true;
        }
    }
}
