using System.Diagnostics;
using System.Net.Http.Headers;
using Shared.Responses;

namespace DenModerneProduktion.Services
{
    public class ApiRequester
    {
        private readonly HttpClient _client;

        private readonly System.Text.Json.JsonSerializerOptions _jsonOptions;

        public ApiRequester(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("api");

            _jsonOptions = new System.Text.Json.JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            };
        }


        async public Task<BaseResponse> Get<T>(string url) where T : class
        {
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                if (response.Content.ReadAsStream().Length == 0)
                {
                    return new EmptyResponse(((int)response.StatusCode));
                }

                var result = await response.Content.ReadFromJsonAsync<T>();

                return new AcceptedResponse<T?>(result, ((int)response.StatusCode));
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
                if (response.Content.ReadAsStream().Length == 0)
                {
                    return new EmptyResponse(((int)response.StatusCode));
                }

                var result = await response.Content.ReadFromJsonAsync<T?>();

                return new AcceptedResponse<T?>(result, ((int)response.StatusCode));
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

            if (response.IsSuccessStatusCode)
            {
                if (response.Content.ReadAsStream().Length == 0)
                {
                    return new EmptyResponse(((int)response.StatusCode));
                }

                var result = await response.Content.ReadFromJsonAsync<T>();
                return new AcceptedResponse<T?>(result, ((int)response.StatusCode));
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
                if (response.Content.ReadAsStream().Length == 0)
                {
                    return new EmptyResponse(((int)response.StatusCode));
                }

                var result = await response.Content.ReadFromJsonAsync<T>();
                return new AcceptedResponse<T?>(result, ((int)response.StatusCode));
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
