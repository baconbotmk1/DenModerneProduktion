using System.Diagnostics;

namespace DenModerneProduktion.Services
{
    public class ApiRequester
    {
        private readonly HttpClient _client;

        public ApiRequester(HttpClient client)
        {
            _client = client;
        }


        async public Task<RequesterResponse<T>> Get<T>(string url) where T : class
        {
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();

                return new RequesterResponse<T>(result);
            }
            else
            {
                return new RequesterResponse<T>(null, (int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        async public Task<object?> Get(string url) => await Get<object>(url);


        async public Task<RequesterResponse<T>> Post<T>(string url, object data) where T : class
        {
            var response = await _client.PostAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();

                return new RequesterResponse<T>(result);
            }
            else
            {
                return new RequesterResponse<T>(null, (int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        async public Task<object?> Post(string url, object data) => await Post<object>(url, data);


        async public Task<RequesterResponse<T>> Put<T>(string url, object data) where T : class
        {
            var response = await _client.PutAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return new RequesterResponse<T>(result);
            }
            else
            {
                return new RequesterResponse<T>(null, (int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        async public Task<object?> Put(string url, object data) => await Put<object>(url, data);


        async public Task<RequesterResponse<T>> Delete<T>(string url) where T : class
        {
            var response = await _client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return new RequesterResponse<T>(result);
            }
            else
            {
                return new RequesterResponse<T>(null, (int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
        }
        async public Task Delete(string url)
        {
            var response = await _client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }

    public class RequesterResponse<T> where T : class
    {
        public RequesterResponse() {}
        public RequesterResponse(T? data = null, int status = 200, string message = "")
        {
            Data = data;
            StatusCode = status;
            Message = message;
        }

        public T? Data { get; set; } = null;
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = "";
    }
}
