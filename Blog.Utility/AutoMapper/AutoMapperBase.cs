using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Utility
{
    public abstract class AutoMapperBase : Profile
    {
        //public IMappingExpression<TSource, TDestination> CreateMapWithIgnore<TSource, TDestination>(List<string> igrnoredProperties = null)
        //{
        //    var expression = this.CreateMap<TSource, TDestination>();
        //    const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
        //    var sourceType = typeof(TSource);
        //    var destinationProperties = typeof(TDestination).GetProperties(flags);
        //    foreach (var property in destinationProperties)
        //    {
        //        if (igrnoredProperties != null && igrnoredProperties.Contains(property.Name))
        //        {
        //            expression.ForMember(property.Name, opt => opt.Ignore());
        //            continue;
        //        }
        //        if (sourceType.GetProperty(property.Name, flags) == null)
        //        {
        //            expression.ForMember(property.Name, opt => opt.Ignore());
        //        }
        //    }
        //    return expression;
        //}
    }
}
