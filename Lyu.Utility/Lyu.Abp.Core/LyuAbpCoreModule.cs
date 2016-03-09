﻿using System.Reflection;
using Abp;
using Abp.Modules;

namespace Lyu.Abp.Core
{
    [DependsOn(typeof(AbpKernelModule))]
    public class LyuAbpCoreModule : AbpModule
    {

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
