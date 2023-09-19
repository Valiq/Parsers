using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParserStellox.Core
{
    internal class DownloadWorker
    {
        public void Download(string urlAll, string dir, string sufix)
        {
            string[] urlMas = urlAll.Split(';');

            foreach (var url in urlMas)
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Uri uri = new Uri(url);

                    string fileName = $"{sufix}=SLX.jpg";

                    WebClient webClient = new WebClient();

                    webClient.DownloadFileAsync(uri, @$"{dir}\{fileName}");
                }
            }
        }
    }
}
