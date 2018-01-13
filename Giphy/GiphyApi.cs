using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Flurl;
using Flurl.Http;
using Microsoft.CSharp.RuntimeBinder;

namespace gipher.Giphy
{
    public class GiphyApi
    {
        private const string GiphyBaseApiUrl = "http://api.giphy.com";
        private readonly string _apiKey;

        public GiphyApi(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> GetImageUrlFromText(string text)
        {
            var url = "";
            try
            {
                var request = GetGiphySearchRequest(text);
                var response = await request.GetJsonAsync();
                url = response.data.images.original.url;
            }
            catch (RuntimeBinderException)
            {
                url = "Sorry, I have been unable to find any images.";
            }
            catch (Exception error)
            {
                url = error.Message;
            }

            return url;
        }

        private Url GetGiphySearchRequest(string text)
        {
            return new Url(GiphyBaseApiUrl)
                .AppendPathSegments(new List<string>() {"v1", "gifs", "translate"})
                .SetQueryParams(new {api_key = _apiKey, s = text, rating = "R"});
        }
    }
}