using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Product.Web.UI.ViewModels;

namespace Product.Web.UI.Pages
{
    public class ProductModel : PageModel
    {
        private readonly ILogger<ProductModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private IConfidentialClientApplication _clientApp;

        public List<Products> listOfProducts;
        public string errorMessage;

        public ProductModel(ILogger<ProductModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient();
            listOfProducts = new List<Products>();
        }

        ///// <summary>
        /////     This is OnGet() method without authorization
        ///// </summary>
        ///// <returns></returns>
        //public async Task OnGet()
        //{
        //    try
        //    {
        //        var productAPIUrl = _configuration.GetValue<string>("ProductAPIUrl");
        //        var response = await _httpClient.GetAsync("productAPIUrl");

        //        if (response.StatusCode.Equals(200))
        //        {
        //            var result = await response.Content.ReadAsStringAsync();

        //            listOfProducts = JsonConvert.DeserializeObject<List<Products>>(result);
        //        }
        //        else
        //        {                                        
        //            throw new Exception(response.ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = ex.Message.ToString();
        //    }
        //}


        /// <summary>
        ///     This is OnGet() method with authorization access token.
        /// </summary>
        /// <returns></returns>
        public async Task OnGet()
        {
            try
            {                
                var productAPIUrl = _configuration.GetValue<string>("ProductAPIUrl");
                var accessToken = await GetAccessToken();

                if (!String.IsNullOrEmpty(accessToken))
                {
                    var defaultRequestHeaders = _httpClient.DefaultRequestHeaders;
                    defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                    var response = await _httpClient.GetAsync(productAPIUrl);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        listOfProducts = JsonConvert.DeserializeObject<List<Products>>(result);
                    }
                    else
                    {
                        throw new Exception(response.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.ToString();
            }
        }

        /// <summary>
        /// 
        ///     Method to fetch token from latest oAuth2 token flow.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAccessToken()
        {
            var clientId = _configuration.GetValue<string>("AzureAD:ClientId");
            var clientSecret = _configuration.GetValue<string>("AzureAD:ClientSecret");
            var tenantId = _configuration.GetValue<string>("AzureAD:TenantId");
            var apiResourceId = _configuration.GetValue<string>("AzureAD:APIResourceId");
            var authorityUrl = $"{_configuration.GetValue<string>("AzureAD:InstanceId")}{tenantId}/oauth2/v2.0/token";

            _clientApp = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authorityUrl))
                .Build();

            string[] scopes = new string[] { apiResourceId };

            try
            {
                AuthenticationResult result = await _clientApp.AcquireTokenForClient(scopes).ExecuteAsync();
                return result.AccessToken;
            }
            catch (MsalClientException ex)
            {
                throw ex;
            }

        }
    }
}