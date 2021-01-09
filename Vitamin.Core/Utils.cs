
using Microsoft.Win32;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Vitamin.Core
{

    public class Utils
    {
        /// <summary>
        ///  获取程序版本信息 
        /// </summary>
        public static string AppVersion =>
           Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        /// <summary>
        /// 尝试获取电脑信息
        /// </summary>
        /// <returns></returns>
        public static string TryGetFullOSVersion() 
        {
            //获取平台
            var osVer = Environment.OSVersion;
            //判断是否运行环境是否在Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    var currentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                    var name = currentVersion.GetValue("ProductName", "Microsoft Windows NT");
                    var ubr = currentVersion.GetValue("UBR", string.Empty).ToString();
                    if (!string.IsNullOrWhiteSpace(ubr))
                    {
                        return $"{name} {osVer.Version.Major}.{osVer.Version.Minor}.{osVer.Version.Build}.{ubr}";
                    }
                }
                catch 
                {

                    return osVer.VersionString;
                }
            }
            return osVer.VersionString;
        }
    }
}
