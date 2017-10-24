using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DoutrinaAgil.Service.Converter;

namespace DoutrinaAgil.Service.Api
{
    public class ApiClient
    {
        private const string URL = @"https://lapuinka.pythonanywhere.com/DoutrinaAgil/";
        private const string TOKEN = @"DAWbiMVyDhNOhBOgs7vbFMhEIUrLSQ6o2FZea=";

        public async Task<string> GetApiBooks(string query)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                var formContent = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("token", TOKEN),
                        new KeyValuePair<string, string>("query", query)
                    });

                try
                {
                    var response = await client.PostAsync("api/find", formContent);
                    return BookConverter.OrganizeBooksContent(response);
                }
                catch (Exception e)
                {
                    //TODO - fazer log 
                    return string.Empty;
                }
            }
        }

        public async Task<string> GetTotalDoctrines()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                var formContent = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("token", TOKEN),
                        new KeyValuePair<string, string>("query", string.Empty)
                    });

                try
                {
                    var response = await client.PostAsync("total/count", formContent);
                    return response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception e)
                {
                    //TODO - fazer log 
                    return string.Empty;
                }
            }
        }
    }
}
