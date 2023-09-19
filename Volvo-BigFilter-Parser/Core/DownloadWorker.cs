using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParserVolvo.Core
{
    internal class DownloadWorker
    {
        public void Download(string urlAll, string dir, string sufix)
        {
            string[] urlMas = urlAll.Split(';');

            int counter = 0;
            foreach (var url in urlMas)
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Uri uri = new Uri(url);

                    string fileName = $"{sufix}_{++counter}.jpg";

                    WebClient webClient = new WebClient();

                    webClient.DownloadFileAsync(uri, @$"{dir}\{fileName}");
                }
            }
        }
    }
}
