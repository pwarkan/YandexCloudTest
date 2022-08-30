using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YandexCloudTest
{
    public class Handler
    {
        public async Task<Response> ProgramHandler(Request request)
        {
            var jsonDoc = JsonDocument.Parse(request.body);
            var root = jsonDoc.RootElement;

            var login = root.GetProperty("login").GetString();
            var password = root.GetProperty("password").GetString();
            if (login != "test" && password != "testpass")
                return new Response(403, "Доступ запрещен");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string jsonResponse = await Function.GetAll();

            return new Response(200, jsonResponse);
        }
    }
}
