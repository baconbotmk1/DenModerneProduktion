using System.Diagnostics;
using System.Net.Http.Headers;
using Shared.Responses;

namespace DenModerneProduktion.Services
{
    public class ApiRequester
    {
        private readonly HttpClient _client;

        public ApiRequester(HttpClient client)
        {
            _client = client;
        }


        async public Task<BaseResponse> Get<T>(string url) where T : class
        {
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();

                return new AcceptedResponse<T?>(result);
            }
            else
            {
                return new ErrorResponse((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        async public Task<BaseResponse> Get(string url) => await Get<object>(url);


        async public Task<BaseResponse> Post<T>(string url, object data) where T : class
        {
            var response = await _client.PostAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                
                Debug.WriteLine(response.Content.ReadAsStream().Length);
                var result = await response.Content.ReadFromJsonAsync<T?>();

                return new AcceptedResponse<T?>(result);
            }
            else
            {
                return new ErrorResponse((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        async public Task<BaseResponse> Post(string url, object data) => await Post<object>(url, data);


        async public Task<BaseResponse> Put<T>(string url, object data) where T : class
        {
            var response = await _client.PutAsJsonAsync(url, data);

            foreach(var header in response.Content.Headers.ToList())
            {
                Debug.WriteLine(" - " + header.Key + ": " + string.Join(", ", header.Value));
            }

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return new AcceptedResponse<T?>(result);
            }
            else
            {
                return new ErrorResponse((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        async public Task<BaseResponse> Put(string url, object data) => await Put<object>(url, data);


        async public Task<BaseResponse> Delete<T>(string url) where T : class
        {
            var response = await _client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return new AcceptedResponse<T?>(result);
            }
            else
            {
                return new ErrorResponse((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        async public Task<BaseResponse> Delete(string url)
        {
            var response = await _client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Error: {response.StatusCode}");

                return new ErrorResponse((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }

            return new EmptyResponse();
        }
    }
}
