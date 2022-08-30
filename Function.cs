using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace YandexCloudTest
{
    class Function
    {
        private static readonly string url = "https://sudrf.ru/index.php?id=300&var=true";
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler() { MaxConnectionsPerServer = 10 });
        private static List<Subject> subjects = new List<Subject>();
        private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        private static string GenerateUrl(string subjectId)
        {
            return url + $"&act=ajax_search&court_subj={subjectId}";
        }

        private static async Task GetCourts(string subjectId)
        {
            var html = await GetHtmlContent(GenerateUrl(subjectId));
            var options = html.DocumentNode.SelectNodes("option[@value]");
            var items = options.Select(node => new Court()
            {
                Code = node.Attributes["value"].Value,
                Name = node.InnerText
            }).ToList();

            subjects.Single(f => f.Id == subjectId).ChildCourts.AddRange(items);
        }

        private static async Task<List<Subject>> GetSubjects()
        {
            var html = await GetHtmlContent(url);
            var options = html.GetElementbyId("court_subj")
                .SelectNodes("option[@value!='0']");
            var items = options.Select(node => new Subject()
            {
                Id = node.Attributes["value"].Value,
                Name = node.InnerText
            }).ToList();

            return items;
        }

        public static async Task<string> GetAll()
        {
            subjects = await GetSubjects();
            var tasks = subjects.Select(s => GetCourts(s.Id)).ToArray();

            await Task.WhenAll(tasks);

            return JsonSerializer.Serialize(subjects, serializerOptions);
        }

        private static async Task<HtmlDocument> GetHtmlContent(string url)
        {
            HtmlDocument document = new HtmlDocument();
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            document.LoadHtml(responseString);
            return document;
        }

        static async Task Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string jsonResponse = await GetAll();
            Console.WriteLine(jsonResponse);
        }
    }
}
