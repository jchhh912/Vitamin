using System;
using System.Collections.Generic;
using Vitamin.Moled.Enumeration;

namespace Vitamin.Moled.Settings
{
    public class AppSettings
    {
        //编辑
        public EditorChoice Editor { get; set; }
        //水印颜色
        public int[] WatermarkARGB { get; set; }
        //水印位置
        public int WatermarkSkipPixel { get; set; }
        //验证码设置
        public CaptchaSettings CaptchaSettings { get; set; }
        //帖子内容摘要 过滤
        public int PostAbstractWords { get; set; }
        //缓存过期分钟数
        public Dictionary<string, int> CacheSlidingExpirationMinutes { get; set; }
        //系统导航菜单
        public Dictionary<string, bool> SystemNavMenus { get; set; }
        //小部件
        public Dictionary<string, bool> AsideWidgets { get; set; }
        //通知
        public NotificationSettings Notification { get; set; }
        //网站地图
        public SiteMapSettings SiteMap { get; set; }
        //主题
        // public BlogTheme[] Themes { get; set; }
        //程序图标
        public ManifestIcon[] ManifestIcons { get; set; }
        //标记规范化
        public TagNormalization[] TagNormalization { get; set; }
        //应用程序设置
        public AppSettings()
        {
            // Prevent Null Reference Exception if user didn't assign config values
            CaptchaSettings = new()
            {
                ImageHeight = 36,
                ImageWidth = 100
            };
            Notification = new();
            SiteMap = new();
            ManifestIcons = Array.Empty<ManifestIcon>();
        }
    }

    public class TagNormalization
    {
        public string Source { get; set; }
        public string Target { get; set; }
    }

    public enum FeatureFlags
    {
        EnableWebApi,
        EnableAudit
    }
}
