using System;
using System.Collections.Generic;
using System.Linq;
using KenticoCloud.Delivery;

namespace bradjolicoeur.Core.Models.ContentType
{
    public class CustomTypeProvider : ICodeFirstTypeProvider
    {
        private static readonly Dictionary<Type, string> _codenames = new Dictionary<Type, string>
        {
            {typeof(UrlRedirect), "url_redirect"}
        };

        public Type GetType(string contentType)
        {
            return _codenames.Keys.FirstOrDefault(type => GetCodename(type).Equals(contentType));
        }

        public string GetCodename(Type contentType)
        {
            return _codenames.TryGetValue(contentType, out var codename) ? codename : null;
        }
    }
}