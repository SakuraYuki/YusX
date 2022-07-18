﻿using YusX.Entity.Domain;

namespace VOL.Builder.Services
{
    public partial class Sys_TableInfoService : ServiceBase<Sys_TableInfo, ISys_TableInfoRepository>, ISys_TableInfoService, IDependency
    {
        public Sys_TableInfoService(ISys_TableInfoRepository repository) : base(repository)
        {
            Init(repository);
        }
        public static ISys_TableInfoService Instance
        {
            get { return AutofacContainerModule.GetService<ISys_TableInfoService>(); }
        }
    }
}

