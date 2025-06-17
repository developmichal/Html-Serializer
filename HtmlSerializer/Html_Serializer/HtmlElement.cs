using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Match> Attributes = new List<Match>();
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }

        public List<HtmlElement> Children = new List<HtmlElement>();
        
        //פונקציה שמחזירה רשימה של כל הצאצאים של האובייקט שעליו היא מופעלת 
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Any())
            {
                foreach (var item in queue.Peek().Children)
                {
                    queue.Enqueue(item);
                }
                var d= queue.Dequeue();
                if(d!=this)
                yield return d;
            }
        }

        //מחזיר רשימה של כל האבות מהאובייקט ומעלה
        public static IEnumerable<HtmlElement> Ancestors(HtmlElement element)
        {
            var father = element;
            while (father != null)
            {
                yield return father;
                father = father.Parent;
            }
        }

        //פונקציית חיפוש לפי סלקטור ואובייקט
        public static HashSet<HtmlElement> Search(HtmlElement element, Selector selector, HashSet<HtmlElement> l2)
        {
            //שליפה של רשימת הצאצאים -השתמשות בפונקציה הנ"ל
            var list5 = element.Descendants();
            //עוברים על הרשימה
            foreach (var item in list5)
            {
                Boolean fleg = false;
                //בודקים האם הסלקטור תואם לאלמנט הנוכחי
                if ((selector.Id != null && item.Id == selector.Id))
                {
                    fleg = true;
                }
                if (selector.TagName != null && item.Name == selector.TagName)
                {
                    fleg = true;
                }

                if (item.Classes != null)
                {
                    foreach (var i in selector.Classes)
                    {
                        if (item.Classes.Contains(i))
                        {
                            fleg = true;
                        }
                        else
                        {
                            fleg = false;
                            break;
                        }
                    }
                }
                if (fleg)
                {
                    //אם הגעת לסוף תוסיף לרשימה
                    if (selector.child == null)
                        l2.Add(item);
                    //אם לא תפעיל את הפונקציה שוב עם האלמנט הנוכחי ומקדמים את הסלקטור
                    else
                        Search(item, selector.child, l2);
                }

            }
            return l2;
        }



    }
}
