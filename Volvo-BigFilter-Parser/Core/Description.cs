using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserVolvo
{
    internal class Description
    {
        static public string imgLinkSec = "http://fd26.narod.ru/vinhelp.jpg";
        static public string imgLinkMain = "";

        static public string description = "Уважаемые клиенты! " +
            "\r\nВаш автомобиль состоит из миллионов деталей, каждую из которых мы можем вам поставить, но к сожалению их все невозможно заранее загрузить на ОЗОН. " +
            "Делайте ваши запросы с указанием VIN вашего автомобиля через кнопку \"Спросить продавца о товаре\", и мы обязательно сделаем возможность в короткие сроки " +
            "что бы у вас была заказать и получить любую запчасть.\r\n\r\nНа данный момент количество предложений наших поставщиков почти 75 000 000";

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
