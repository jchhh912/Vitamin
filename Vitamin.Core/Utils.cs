
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Vitamin.Core
{

    public class Utils
    {
        //获取程序版本信息 
        public static string AppVersion =>
           Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        public static string TryGetFullOSVersion() 
        {
            //获取平台
            var osVer = Environment.OSVersion;
            //判断是否运行环境是否在Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    //var currentVersion=Registry.

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
