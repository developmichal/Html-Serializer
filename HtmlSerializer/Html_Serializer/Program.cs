
using Html_Serializer;
using System.Text.RegularExpressions;
               
// htmlשליפת ה
var html = await Load("http://localhost:4200/");
//מנקה רווחים
var cleanHtml = new Regex("[\\r\\t\\n]").Replace(html, "");
//מוחק שורות ריקות
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s != string.Empty && s.Replace(" ", "").Length != 0);

//הגדרת משתנים
var root = new HtmlElement();
var father = new HtmlElement();
string str = "";
//לולאה שעוברת עלHTML  
foreach (var line in htmlLines)
{
    //מחלקת לפי רווח
    int i1 = -1;
    if (line.IndexOf(' ') != -1)
        i1 = line.IndexOf(' ');
    if (i1 != -1)
    {
        str = line.Substring(0, i1);
    }
    else { str = line; }
    //מאתחלת מאפיין חדש
    var newHtmlElement = new HtmlElement();
    //HTML בודקת אם היא מהתגיות של 
    if (HtmlHelper.JsonHtml.htmlTags.Contains(str) || HtmlHelper.JsonHtml.HtmlVoidTags.Contains(str))
    {
        //מכניסה את הנתונים למאפיין החדש
        newHtmlElement.Name = str;
        //וניתוחו והכנסתו לתןך המאפיין החדש למקומות הנכונים attribute שליפת ה
        var attribute = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
        foreach (Match match in attribute)
        {
            Group key = match.Groups[1];
            Group valu = match.Groups[2];
            if (key.ToString().Equals("class"))
            {
                string[] word = valu.ToString().Split(" ");
                newHtmlElement.Classes = word.ToList();
            }
            else if (key.ToString().Equals("id"))
            {
                newHtmlElement.Id = valu.ToString();
            }
            newHtmlElement.Attributes.Add(match);
        }
        //בדיקה אם הרות ריק ואם כן להתחל אותו
        if (root.Name == null)
        {
            root = newHtmlElement;
            father = newHtmlElement;
        }

        else
        {
            newHtmlElement.Parent = father;
            father.Children.Add(newHtmlElement);
            if (HtmlHelper.JsonHtml.htmlTags.Contains(str) && !HtmlHelper.JsonHtml.HtmlVoidTags.Contains(str))
                father = newHtmlElement;
        }


    }
    //בדיקה אם נסגרה תגית
    else if (str[0].ToString() == "/" && !str.Equals("/html"))
    {
        father = father.Parent;
    }
    //ואם זה אף אחד מהתנאים הקודמים זה טקסט
    else
    {
        if(father!=null)
        father.InnerHtml = line;
    }
}
//הפעלת הפונקציה שמחזירה את כל צאצאי האלמנט ברשימה שטוחה
var l = root.Descendants();
//הפעלת הפונקציה שמחזירה את האבות של האלמנט
var l1 = HtmlElement.Ancestors(root.Children[0].Children[0]);
//הפעלת הרקורסיה לחיפוש תגיות לפי סלקטור
var list1 = new HashSet<HtmlElement>();
Selector b=Selector.convert("div .form-control");
var l5 = HtmlElement.Search(root, b,list1);
l5.ToList().ForEach(x => Console.WriteLine(x.Name));
Console.ReadLine();


//של האתר המסויים html שליפת קוד ה
async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}


