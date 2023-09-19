using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserStellox.Core
{
    internal class Description
    {
        static public string imgLinkSec = "http://fd26.narod.ru/vinhelp.jpg";
        static public string imgLinkMain = "";

        static public string description = "Уважаемые клиенты! Если есть дополнительные запросы, то свяжитесь с нами с указанием идентификационного номера вашего автомобиля, и мы обязательно вам поможем в подборе автозапчастей.";

        static public string richFirst = @"
       {
        ""content"": [
        {
            ""widgetName"": ""raTable"",
            ""title"": 
            {
                ""content"": [""Характеристики""],
                ""size"": ""size4"",
                ""align"": ""left"",
                ""color"": ""color1""
            },
                ""table"": 
	            {
                ""head"": 
	             [
                    {""text"": [""Тип""],""contentAlign"": ""left""},
	                {""text"": [""Значение""],""contentAlign"": ""left""}
                 ],
                ""body"": 
	            [";

        static public string rischSecond = @"
            ]
            }
        },
        {
            ""widgetName"": ""raTable"",
            ""title"": 
            {
                ""content"": [""OEM-номера""],
                ""size"": ""size4"",
                ""align"": ""left"",
                ""color"": ""color1""
            },
            ""table"": {
             ""head"": 
	        [
                {""text"": [""Название""],""contentAlign"": ""left""},
                {""text"": [""Номер""],""contentAlign"": ""left""}
            ],
            ""body"": 
	        [";

        static public string rischThird = @"
                    ]
                }
              }
            ],
            ""version"": 0.3
        }";
    }
}
