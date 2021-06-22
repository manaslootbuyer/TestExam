using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmAspire;

namespace ExamEdrian.Services
{
    public interface IDataService
    {
        Task<bool> GetSuccess(string endpointName, CancellationToken cts, WebRequestMethod requestMethod, object data = null);
        Task<T> GetResponseAsync<T>(string endpointName, CancellationToken cts, WebRequestMethod requestMethod, object data = null) where T : class;
        event Action<object, ErrorEventArgs> OnNewtorkError;
        void RemoveTokenInClient();
    }
}