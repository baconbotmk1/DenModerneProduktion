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


        async public Task<T?> Get<T>(string url)
        {
            var response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            else
            {
                Debug.WriteLine($"Error: {response.StatusCode}");
                return default;
            }
        }
        async public Task<object?> Get(string url) => await Get<object>(url);


        async public Task<T?> Post<T>(string url, object data)
        {
            var response = await _client.PostAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            else
            {
                Debug.WriteLine($"Error: {response.StatusCode}");
                return default;
            }
        }
        async public Task<object?> Post(string url, object data) => await Post<object>(url, data);


        async public Task<T?> Put<T>(string url, object data)
        {
            var response = await _client.PutAsJsonAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            else
            {
                Debug.WriteLine($"Error: {response.StatusCode}");
                return default;
            }
        }
        async public Task<object?> Put(string url, object data) => await Put<object>(url, data);


        async public Task<T?> Delete<T>(string url)
        {
            var response = await _client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return result;
            }
            else
            {
                Debug.WriteLine($"Error: {response.StatusCode}");
                return default;
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
}
