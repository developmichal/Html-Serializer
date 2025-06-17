using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }

        public List<string> Classes = new List<string>();
        public Selector Parent { get; set; }
        public Selector child { get; set; }

        //הופכת את המחרוזת לעץ מסוג סלקטור
        public static Selector convert(string str)
        {
            var root = new Selector();
            var father = new Selector();
            var arrStr = str.Split(' ');
            foreach (var item in arrStr)
            {//מחליף כל נקודה ברווח נקודה וכל סולמית ברווח סולמית ומחלק לפי רווחים
                Selector newSelector = new Selector();
                var f = item.Replace(".", " .").Replace("#", " #").Split(" ");
                //מכניס את הנתונים
                foreach (var item2 in f)
                {
                    if (item2 != "")
                    {
                        if (item2[0].ToString().Equals("#"))
                            newSelector.Id = item2.Substring(1);
                        else if (item2[0].ToString().Equals("."))
                            newSelector.Classes.Add(item2.Substring(1));
                        else if (HtmlHelper.JsonHtml.htmlTags.Contains(item2))
                            newSelector.TagName = item2;
                    }
                }
                //מעדכן את האבא והרות בהתאם לנתונים
                if (root.TagName == null && root.Id == null && root.Classes.Count == 0)
                {
                    root = newSelector;
                    father = newSelector;
                }
                else
                {
                    father.child = newSelector;
                    newSelector.Parent = father;
                    father = newSelector;
                }
            }
            //מחזיר את הרות
            return root;

        }

    }
}
