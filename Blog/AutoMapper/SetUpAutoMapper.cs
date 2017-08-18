using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.AutoMapper
{
    public static class SetUpAutoMapper
    {
        public static void RegisterAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new CommonMapperProfile());
            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}