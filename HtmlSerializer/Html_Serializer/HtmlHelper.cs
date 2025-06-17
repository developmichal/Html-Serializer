

using System.Text.Json;
namespace Html_Serializer
{
    public class HtmlHelper
    {
        private static readonly HtmlHelper _JsonHtml = new HtmlHelper();
        public string[] htmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        public static HtmlHelper JsonHtml=> _JsonHtml;

        private HtmlHelper()
        {
          //שליפת הג'סון והמרתם
            this.htmlTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("JSON/HtmlTags.json"));
            this.HtmlVoidTags = JsonSerializer.Deserialize<string[]>(File.ReadAllText("JSON/HtmlVoidTags.json"));
        }
    }

}
