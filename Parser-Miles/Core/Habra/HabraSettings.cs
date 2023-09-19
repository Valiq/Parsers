
namespace Parser.Core.Habra
{
    class HabraSettings : IParserSettings
    {
       /* public HabraSettings(int start, int end)
        {
            StartPoint = start;
            EndPoint = end;
        } */

        public string BaseUrl { get; set; } = "https://www.miles-auto.com/product";

        public string Prefix { get; set; } = "?artnr={CurrentId}";

        public int StartPoint { get; set; }

        public int EndPoint { get; set; }
    }
}
