using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodeHighlighter.BusinessLogic.SyntaxHighlighting
{
    class RESTHighlighter : ISyntaxHighlighter
    {
        private const int ApiRequestSizeLimit = 50_000;
        private const int EscapeDataStringSizeLimit = 65_000;

        private Uri baseUri;
        private string resource;
        private string paramCode;
        Dictionary<string, string> requestParamsWithoutCode;

        public RESTHighlighter(Uri baseUri, string resource, string paramCode, Dictionary<string, string> requestParamsWithoutCode)
        {
            this.baseUri = baseUri;
            this.resource = resource;
            this.paramCode = paramCode;
            this.requestParamsWithoutCode = requestParamsWithoutCode;
        }
        public async Task<string> HighlightCodeAsync(string code)
        {

            StringBuilder sb = new StringBuilder();
            var numberOfIterations = code.Length / ApiRequestSizeLimit;

            for (var i = 0; i <= numberOfIterations; i++)
            {
                if (i < numberOfIterations)
                {
                    sb.Append(await DoHighlightCodeAsync(code.Substring(ApiRequestSizeLimit * i, ApiRequestSizeLimit)));
                }
                else
                {
                    sb.Append(await DoHighlightCodeAsync(code.Substring(ApiRequestSizeLimit * i)));
                }
            }

            return sb.ToString();
        }

        private async Task<string> DoHighlightCodeAsync(string code)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = baseUri;
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                requestParamsWithoutCode.Add(paramCode, code);

                var requestContent = new StringContent(BuildRequestBodyContent(code), Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await httpClient.PostAsync(resource, requestContent);
                var responseBody = await response.Content.ReadAsStringAsync();

                requestParamsWithoutCode.Remove(paramCode);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HighlightingFailedException(responseBody);
                }

                return responseBody;
            }
        }

        private string BuildRequestBodyContent(string code)
        {
            var sb = new StringBuilder();
            foreach (var param in requestParamsWithoutCode)
            {
                sb.Append($"{param.Key}={UriEscapeDataString(param.Value)}&");
            }
            sb.Append($"{paramCode}={UriEscapeDataString(code)}");
            return sb.ToString();
        }

        private string UriEscapeDataString(string dataString)
        {
            StringBuilder sb = new StringBuilder();
            var numberOfIterations = dataString.Length / EscapeDataStringSizeLimit;

            for (var i = 0; i <= numberOfIterations; i++)
            {
                if (i < numberOfIterations)
                {
                    sb.Append(Uri.EscapeDataString(dataString.Substring(EscapeDataStringSizeLimit * i, EscapeDataStringSizeLimit)));
                }
                else
                {
                    sb.Append(Uri.EscapeDataString(dataString.Substring(EscapeDataStringSizeLimit * i)));
                }
            }
            return sb.ToString();
        }
    }
}
