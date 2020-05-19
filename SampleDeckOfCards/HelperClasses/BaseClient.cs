using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using SampleDeckOfCards.Model;

namespace SampleDeckOfCards.HelperClasses
{
    public class BaseClient
    {
        readonly HttpClient client;

        public async Task<BaseResponse> GetCallAsync(string baseAddress, string endpoint)
        {
            BaseResponse baseresponse = new BaseResponse();
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-us"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "81.0.4044.138"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                string url = Uri.EscapeUriString(baseAddress + endpoint);
                HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);
                baseresponse.ResponseMessage = await response.Content.ReadAsStringAsync();
                baseresponse.StatusCode = (int)response.StatusCode;

                return baseresponse;
            }
            catch (Exception ex)
            {
                baseresponse.StatusCode = 0;
                baseresponse.ResponseMessage = (ex.Message ?? ex.InnerException.ToString());
            }
            return baseresponse;
        }

        public async Task<BaseResponse> PostCallAsync(string baseAddress, string endpoint, string content)
        {
            BaseResponse baseresponse = new BaseResponse();
            try
            {

                client.DefaultRequestHeaders.Accept.Clear();
                //We are gettign 403 Forbidden error . We need authentication details to make successfull post call. 
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-us"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "81.0.4044.138"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string url = Uri.EscapeUriString(baseAddress + endpoint);
                HttpContent httpContent = new StringContent(content, Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(url, httpContent);
                baseresponse.ResponseMessage = await response.Content.ReadAsStringAsync();
                baseresponse.StatusCode = (int)response.StatusCode;
            }
            catch (Exception ex)
            {

                baseresponse.StatusCode = 0;
                baseresponse.ResponseMessage = (ex.Message ?? ex.InnerException.ToString());
            }

            return baseresponse;
        }

        public BaseClient()
        {
            client = new HttpClient();
        }
    }
}
