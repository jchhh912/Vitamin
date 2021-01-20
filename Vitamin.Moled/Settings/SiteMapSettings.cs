using System.Collections.Generic;

namespace Vitamin.Moled.Settings
{
    public class SiteMapSettings
    {
        //Url集合名称空间
        public string UrlSetNamespace { get; set; }
        //更改频率
        public Dictionary<string, string> ChangeFreq { get; set; }
    }
}
