using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvvmAspire;
using MvvmAspire.Helpers;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ExamEdrian.Services
{
    public class DataService : IDataService
    {
        private const string AuthorizationKey = "Authorization";
        private HttpClient client;

        public event Action<object, ErrorEventArgs> OnNewtorkError;

        public async Task<bool> GetSuccess(string endpointName, CancellationToken cts, WebRequestMethod requestMethod, object data = null)
        {
            var response = await GetResponse<Response>(endpointName, requestMethod, cts, data);
            return response != null;
        }

        public async Task<T> GetResponseAsync<T>(string endpointName, CancellationToken cts, WebRequestMethod requestMethod, object data = null) where T : class
        {
            try
            {
                var response = await GetResponse<T>(endpointName, requestMethod, cts, data);
                return response?.Data;
            }
            catch (ArgumentNullException aEx)
            {
                OnNewtorkError?.Invoke(this, new ErrorEventArgs(ErrorType.BadRequest));
            }
            catch (InvalidOperationException iEx)
            {
                OnNewtorkError?.Invoke(this, new ErrorEventArgs(ErrorType.BadRequest));
            }
            catch (TaskCanceledException)
            {
                OnNewtorkError?.Invoke(this, new ErrorEventArgs(ErrorType.Timeout));
            }
            catch (HttpRequestException hEx)
            {
                var errorType = GetErrorType(hEx);
                OnNewtorkError?.Invoke(this, new ErrorEventArgs(errorType));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString(), "Custom Error");
                //throw;
            }
            return null;
        }

        public ErrorType GetErrorType(HttpRequestException ex)
        {
            var errorType = ErrorType.Unkown;

            if (!int.TryParse(ex.Message, out var statusCode)) return errorType;

            if (statusCode == (int)HttpStatusCode.Unauthorized)
                errorType = ErrorType.Unauthorized;
            else if (statusCode >= (int)HttpStatusCode.BadRequest && statusCode < (int)HttpStatusCode.InternalServerError)
                errorType = ErrorType.BadRequest;
            else if (statusCode >= (int)HttpStatusCode.InternalServerError)
                errorType = ErrorType.ServerError;

            return errorType;
        }

        private async Task<Response<T>> GetResponse<T>(string endpointName, WebRequestMethod requestMethod, CancellationToken cts, object data)
        {
            var result = new Response<T>();

            using (var response = await GetResponse(endpointName, requestMethod, cts, data))
            {
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (responseString == "[]" && !typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
                {
                    result.Data = default(T);
                }
                else
                {
                    result.Data = JsonConvert.DeserializeObject<T>(responseString, new NoNullableIntConverter());
                }

                responseString = null;
                return result;
            }
        }

        private async Task<HttpResponseMessage> GetResponse(string endpointName, WebRequestMethod requestMethod, CancellationToken cts, object data)
        {
            client = await GetClient($"{App.WebUrl}/{endpointName}");
            switch (requestMethod)
            {
                case WebRequestMethod.DELETE: return await client.DeleteAsync(string.Empty, cts);
                case WebRequestMethod.GET: return await client.GetAsync(string.Empty, cts).ConfigureAwait(true);
                case WebRequestMethod.POST: return await client.PostAsync(string.Empty, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"), cts);
                case WebRequestMethod.PUT: return await client.PutAsync(string.Empty, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"), cts);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private async Task<HttpClient> GetClient(string url)
        {
            var httpHandler = new HttpClientHandler();

            var httpClient = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri(url)
            };

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            //httpClient.DefaultRequestHeaders.Add(AuthorizationKey, $"Bearer {await GetToken()}");
            //await CheckTokenAndAddIfNeeded(httpClient);

            return httpClient;
        }

        public void RemoveTokenInClient()
        {
            if (client.DefaultRequestHeaders.Any(c => c.Key.Equals(AuthorizationKey)))
            {
                client.DefaultRequestHeaders.Remove(AuthorizationKey);
            }
        }



        private class Response
        {
        }

        private class Response<T> : Response
        {
            public T Data { get; set; }
        }
    }
}