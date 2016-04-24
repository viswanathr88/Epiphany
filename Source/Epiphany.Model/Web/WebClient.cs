﻿using Epiphany.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Epiphany.Web
{
    /// <summary>
    /// Represents the WebClient implementation
    /// </summary>
    public sealed class WebClient : IWebClient
    {
        private readonly IAuthenticatorFactory authFactory;
        /// <summary>
        /// Create an instance of WebClient
        /// </summary>
        /// <param name="authFactory"></param>
        public WebClient(IAuthenticatorFactory authFactory)
        {
            if (authFactory == null)
            {
                throw new ArgumentNullException(nameof(authFactory));
            }

            this.authFactory = authFactory;
        }
        /// <summary>
        /// Execute a given request asynchronously
        /// </summary>
        /// <param name="request">request to execute</param>
        /// <returns></returns>
        public async Task<WebResponse> ExecuteAsync(WebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(request.Url))
            {
                throw new ArgumentNullException(nameof(request.Url));
            }

            if (request.Authenticate)
            {
                // We should authenticate if the request says it should be
                IAuthenticator authenticator = this.authFactory.CreateOAuthAuthenticator();
                authenticator.Authenticate(request);
            }

            System.Net.HttpWebRequest httpWebRequest = System.Net.WebRequest.CreateHttp(request.Url);

            // Translate Method
            httpWebRequest.Method = request.Method.ToString().ToUpperInvariant();
            
            // Copy over the headers
            foreach (var header in request.Headers)
            {
                httpWebRequest.Headers[header.Key] = header.Value;
            }

            WebResponse response = null;
            try
            {
                var httpWebResponse = (System.Net.HttpWebResponse)await httpWebRequest.GetResponseAsync();
                if (httpWebResponse != null)
                {
                    string content = string.Empty;
                    using (StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        content = await reader.ReadToEndAsync();
                        response = new WebResponse(httpWebResponse.StatusCode, content);
                    }
                }
                else
                {
                    Logger.LogError("GetResponseAsync returned null");
                    response = new WebResponse(System.Net.HttpStatusCode.BadRequest, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return response;
        }
    }
}