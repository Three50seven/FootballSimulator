using Common.Core.Domain;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Common.Core.Services
{
    [Obsolete("Use HttpClient instance directly for sending web requests.")]
    public class SystemNetWebRequestService : IRequestService
    {
        private readonly ISerializer _serializer;

        public SystemNetWebRequestService(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public string Request(RequestInfo requestInfo)
        {
            if (requestInfo == null)
                throw new ArgumentNullException(nameof(requestInfo));

            HttpWebRequest request = requestInfo.ToHttpWebRequest();
            ApplyRequestData(request, requestInfo.Data);
            string responseFromServer = null;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                        reader.Close();
                    }
                }

                response.Close();
            }

            return responseFromServer;
        }

        public async Task<string> RequestAsync(RequestInfo requestInfo)
        {
            if (requestInfo == null)
                throw new ArgumentNullException(nameof(requestInfo));

            HttpWebRequest request = requestInfo.ToHttpWebRequest();
            await ApplyRequestDataAsync(request, requestInfo.Data);
            string responseFromServer = null;

            using (var response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                        reader.Close();
                    }
                }

                response.Close();
            }

            return responseFromServer;
        }

        public T RequestData<T>(RequestInfo requestInfo) where T : class
        {
            if (requestInfo == null)
                throw new ArgumentNullException(nameof(requestInfo));

            HttpWebRequest request = requestInfo.ToHttpWebRequest();
            ApplyRequestData(request, requestInfo.Data);

            string responseFromServer = null;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseFromServer = reader.ReadToEnd();
                        reader.Close();
                    }
                }

                response.Close();
            }

            return FormatResponse<T>(responseFromServer);
        }

        public async Task<T> RequestDataAsync<T>(RequestInfo requestInfo) where T : class
        {
            if (requestInfo == null)
                throw new ArgumentNullException(nameof(requestInfo));

            HttpWebRequest request = requestInfo.ToHttpWebRequest();
            await ApplyRequestDataAsync(request, requestInfo.Data);

            string responseFromServer = null;

            using (var response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        responseFromServer = await reader.ReadToEndAsync();
                        reader.Close();
                    }
                }

                response.Close();
            }

            return FormatResponse<T>(responseFromServer);
        }

        private void ApplyRequestData(HttpWebRequest request, object data)
        {
            if (data != null)
            {
                string serializedData = SerializeData(data);
                var stream = request.GetRequestStream();

                using (var streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(serializedData);
                    streamWriter.Flush();
                }
            }
        }

        private async Task ApplyRequestDataAsync(HttpWebRequest request, object data)
        {
            if (data != null)
            {
                string serializedData = SerializeData(data);
                var stream = await request.GetRequestStreamAsync();

                using (var streamWriter = new StreamWriter(stream))
                {
                    await streamWriter.WriteAsync(serializedData);
                    streamWriter.Flush();
                }
            }
        }

        private string SerializeData(object data)
        {
            if (data is string)
                return data as string;
            else
                return _serializer.Serialize(data);
        }

        private T FormatResponse<T>(string reponse) where T : class
        {
            if (string.IsNullOrWhiteSpace(reponse))
                return default(T);

            return _serializer.Deserialize<T>(reponse);
        }
    }
}
