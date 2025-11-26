using Common.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Common.Core
{
    [Obsolete("Default implmentation is now outdated. Use HttpClient instance directly for sending web requests.")]
    public interface IRequestService
    {
        T RequestData<T>(RequestInfo requestInfo) where T : class;
        Task<T> RequestDataAsync<T>(RequestInfo requestInfo) where T : class;
        string Request(RequestInfo requestInfo);
        Task<string> RequestAsync(RequestInfo requestInfo);
    }
}
