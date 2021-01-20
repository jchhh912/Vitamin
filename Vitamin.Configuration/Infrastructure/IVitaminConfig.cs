using System;
using System.Collections.Generic;
using System.Text;

namespace Vitamin.Configuration.Infrastructure
{
    public interface IVitaminConfig
    {

        //常规设置
        GeneralSettings GeneralSettings { get; set; }
    }
}
