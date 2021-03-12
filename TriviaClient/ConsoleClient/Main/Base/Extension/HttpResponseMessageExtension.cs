using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main.Extension
{
    public static class HttpResponseMessageExtension
    {
        public static void EnsureSuccessResponse(this HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null)
            {
                throw new ApplicationException("response is null");
            }

            if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                string errorMessage = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                throw new ApplicationException($"500 error, error info: {errorMessage}");
            }

            httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}