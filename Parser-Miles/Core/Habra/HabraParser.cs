using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace Parser.Core.Habra
{
    class HabraParser : IParser<string[,]>
    {

        public string[,] Parse(IHtmlDocument document)
        {
            var listName = new List<string>();
            var listValue = new List<string>();

            var items = document.QuerySelectorAll("div").Where(item => item.ClassName != null && (item.ClassName.Contains("detail__characteristic-value")
                                                                                              || item.ClassName.Contains("catalog-detail__characteristic-name")
                                                                                              || item.ClassName.Contains("catalog-detail__title")));

            var item_name = document.QuerySelectorAll("h1").Where(item => item.ClassName != null && item.ClassName.Contains("catalog-detail__h1"));

            //var items = document.QuerySelectorAll("img");

            //string[,] card = new string[items.Count() + 1, 2];

            listName.Add("Наименование товара");
            foreach (var item in item_name)
            {
                listValue.Add(item.TextContent);
                //MessageBox.Show(items.Count().ToString());
            }

            bool flag = false;
            string oem = "";
            foreach (var item in items) 
            {

                if (item.ClassName.Contains("catalog-detail__title") && item.TextContent.Contains("Оригинальные номера"))
                {
                    flag = true;
                }

                if (flag)
                {
                    if (item.ClassName.Contains("catalog-detail__characteristic-value"))
                        oem = oem + item.TextContent + "; ";
                }

                if (!flag)
                {
                    if (item.ClassName.Contains("catalog-detail__characteristic-name"))
                         listName.Add(item.TextContent);

                    if (item.ClassName.Contains("catalog-detail__characteristic-value"))
                        listValue.Add(item.TextContent);
                }

                 //list.Add(card);
                //list.Add(item.GetAttribute("src"));
            }

            listName.Add("OEM номера");
            listValue.Add(oem);

            string[,] card = new string[listName.Count(), 2];

            for (int i = 0; i < listName.Count(); i++)
            {
                card[i,0] = listName[i];
                card[i,1] = listValue[i];
            }

            return card;
        }

    }
}
