﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GiphyDotNet.Interfaces;
using GiphyDotNet.Model.Parameters;
using GiphyDotNet.Model.Results;
using GiphyDotNet.Tools;
using Newtonsoft.Json;

namespace GiphyDotNet.Manager
{
    public class Giphy
    {
        private readonly IWebManager _webManager = new WebManager();
        private readonly string _authKey;

        private const string BaseUrl = "http://api.giphy.com/";
        private const string BaseGif = "v1/gifs";
        private const string BaseSticker = "v1/stickers";

        /// <summary>
        /// Initialize Giphy Manager.
        /// </summary>
        /// <param name="authKey">Key used for authentication. By default set to the public beta key.</param>
        public Giphy(string authKey = "dc6zaTOxFJmzC")
        {
            _authKey = authKey;
        }

        /// <summary>
        /// Search for GIFs.
        /// </summary>
        /// <param name="searchParameter">Required: Used to query the search engine.</param>
        /// <returns>A GifSearch Result object.</returns>
        public async Task<GiphySearchResult> GifSearch(SearchParameter searchParameter)
        {
            if (string.IsNullOrEmpty(searchParameter.Query))
            {
                throw new FormatException("Must set query in order to search.");
            }

            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            nvc.Add("q", searchParameter.Query);
            nvc.Add("limit", searchParameter.Limit.ToString());
            nvc.Add("offset", searchParameter.Offset.ToString());
            if(searchParameter.Rating != Rating.None)
                nvc.Add("rating", searchParameter.Rating.ToFriendlyString());
            if(!string.IsNullOrEmpty(searchParameter.Format))
                nvc.Add("fmt", searchParameter.Format);

            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseGif}/search{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get GIFs: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphySearchResult>(result.ResultJson);
        }

        /// <summary>
        /// Search for Stickers.
        /// </summary>
        /// <param name="searchParameter">Required: Used to query the search engine.</param>
        /// <returns>A Search Result object.</returns>
        public async Task<GiphySearchResult> StickerSearch(SearchParameter searchParameter)
        {
            if (string.IsNullOrEmpty(searchParameter.Query))
            {
                throw new FormatException("Must set query in order to search.");
            }

            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            nvc.Add("q", searchParameter.Query);
            nvc.Add("limit", searchParameter.Limit.ToString());
            nvc.Add("offset", searchParameter.Offset.ToString());
            if (searchParameter.Rating != Rating.None)
                nvc.Add("rating", searchParameter.Rating.ToFriendlyString());
            if (!string.IsNullOrEmpty(searchParameter.Format))
                nvc.Add("fmt", searchParameter.Format);

            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseSticker}/search{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get Sticker: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphySearchResult>(result.ResultJson);
        }

        public async Task<GiphyIdResult> GetGifById(string id)
        {
            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseGif}/{id}{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get GIF: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphyIdResult>(result.ResultJson);
        }

        public async Task<GiphyIdsResult> GetGifsByIds(string[] ids)
        {
            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            nvc.Add("ids", string.Join(",", ids));

            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseGif}{UriExtensions.ToQueryString(nvc, false)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get GIFs: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphyIdsResult>(result.ResultJson);
        }

        public async Task<GiphyIdResult> TranslateIntoGif(TranslateParameter translateParameter)
        {
            if (string.IsNullOrEmpty(translateParameter.Phrase))
            {
                throw new FormatException("Must set phrase or term in order to translate.");
            }

            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            nvc.Add("s", translateParameter.Phrase);
            if (translateParameter.Rating != Rating.None)
                nvc.Add("rating", translateParameter.Rating.ToFriendlyString());
            if (!string.IsNullOrEmpty(translateParameter.Format))
                nvc.Add("fmt", translateParameter.Format);
            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseGif}/translate{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get GIFs: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphyIdResult>(result.ResultJson);
        }

        public async Task<GiphyIdResult> TranslateIntoSticker(TranslateParameter translateParameter)
        {
            if (string.IsNullOrEmpty(translateParameter.Phrase))
            {
                throw new FormatException("Must set phrase or term in order to translate.");
            }

            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            nvc.Add("s", translateParameter.Phrase);
            if (translateParameter.Rating != Rating.None)
                nvc.Add("rating", translateParameter.Rating.ToFriendlyString());
            if (!string.IsNullOrEmpty(translateParameter.Format))
                nvc.Add("fmt", translateParameter.Format);
            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseSticker}/translate{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get Sticker: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphyIdResult>(result.ResultJson);
        }

        public async Task<GiphyRandomResult> RandomGif(RandomParameter randomParameter)
        {
            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            if (randomParameter.Rating != Rating.None)
                nvc.Add("rating", randomParameter.Rating.ToFriendlyString());
            if (!string.IsNullOrEmpty(randomParameter.Format))
                nvc.Add("fmt", randomParameter.Format);
            if (!string.IsNullOrEmpty(randomParameter.Tag))
                nvc.Add("tag", randomParameter.Tag);
            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseGif}/random{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get GIF: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphyRandomResult>(result.ResultJson);
        }

        public async Task<GiphyRandomResult> RandomSticker(RandomParameter randomParameter)
        {
            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            if (randomParameter.Rating != Rating.None)
                nvc.Add("rating", randomParameter.Rating.ToFriendlyString());
            if (!string.IsNullOrEmpty(randomParameter.Format))
                nvc.Add("fmt", randomParameter.Format);
            if (!string.IsNullOrEmpty(randomParameter.Tag))
                nvc.Add("tag", randomParameter.Tag);
            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseSticker}/random{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get Sticker: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphyRandomResult>(result.ResultJson);
        }

        public async Task<GiphySearchResult> TrendingGifs(TrendingParameter trendingParameter)
        {
            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            nvc.Add("limit", trendingParameter.Limit.ToString());
            if (trendingParameter.Rating != Rating.None)
                nvc.Add("rating", trendingParameter.Rating.ToFriendlyString());
            if (!string.IsNullOrEmpty(trendingParameter.Format))
                nvc.Add("fmt", trendingParameter.Format);
            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseGif}/trending{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get GIF: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphySearchResult>(result.ResultJson);
        }

        public async Task<GiphySearchResult> TrendingStickers(TrendingParameter trendingParameter)
        {
            var nvc = new NameValueCollection();
            nvc.Add("api_key", _authKey);
            nvc.Add("limit", trendingParameter.Limit.ToString());
            if (trendingParameter.Rating != Rating.None)
                nvc.Add("rating", trendingParameter.Rating.ToFriendlyString());
            if (!string.IsNullOrEmpty(trendingParameter.Format))
                nvc.Add("fmt", trendingParameter.Format);
            var result =
                await _webManager.GetData(new Uri($"{BaseUrl}{BaseSticker}/trending{UriExtensions.ToQueryString(nvc)}"));
            if (!result.IsSuccess)
            {
                throw new WebException($"Failed to get Sticker: {result.ResultJson}");
            }

            return JsonConvert.DeserializeObject<GiphySearchResult>(result.ResultJson);
        }
    }
}
