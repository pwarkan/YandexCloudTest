using System;
using System.Collections.Generic;
using System.Text;

namespace YandexCloudTest
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Body { get; set; }

        public Response(int statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }
    }
}
