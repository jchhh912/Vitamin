using System;

namespace Vitamin.Configuration
{
    //基本描述
    public class GeneralSettings : VitaminSettings
    {
        //网站标题
        public string SiteTitle { get; set; }
        //标识文字
        public string LogoText { get; set; }
        //标签
        public string MetaKeyword { get; set; }
        //标签描述
        public string MetaDescription { get; set; }
        //前缀
        public string CanonicalPrefix { get; set; }
        //版权
        public string Copyright { get; set; }
        //自定义侧边栏
        public string SideBarCustomizedHtmlPitch { get; set; }
        //自定义页脚
        public string FooterCustomizedHtmlPitch { get; set; }
        //时区
        public string TimeZoneUtcOffset { get; set; }
        //时区id
        public string TimeZoneId { get; set; }
        //主题文件
        public string ThemeFileName { get; set; }
        //站点图标
        public string SiteIconBase64 { get; set; }
        //网站管理员
        public string OwnerName { get; set; }
        //说明
        public string Description { get; set; }
        //简短描述
        public string ShortDescription { get; set; }
        //评论者图片
        public string AvatarBase64 { get; set; }
        //主题
        public bool AutoDarkLightTheme { get; set; }
        // 主题 css
        public GeneralSettings()
        {
            ThemeFileName = "word-blue.css";
            SiteIconBase64 = string.Empty;
            AvatarBase64 = string.Empty;
        }
    }
}
