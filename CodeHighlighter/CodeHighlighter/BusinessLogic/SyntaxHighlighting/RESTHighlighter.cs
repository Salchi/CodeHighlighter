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
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = baseUri;
                requestParamsWithoutCode.Add(paramCode, code);

                var requestContent = new FormUrlEncodedContent(requestParamsWithoutCode);
                var response = await httpClient.PostAsync(resource, requestContent);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HighlightingFailedException(responseBody);
                }

                return responseBody;
            }
        }
    }
}
