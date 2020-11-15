using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.ViewModels
{
    public class TagsViewModel
    {
        public IEnumerable<Tag> Tags { get; }
        public TagsViewModel(IEnumerable<string> tags)
        {
            var list = new List<Tag>();
            foreach(var item in tags)
            {
                list.Add(new Tag(item, item.Replace(" ","-").ToLower()));
            }

            Tags = list;
        }

        public class Tag
        {
            public string Name { get; }
            public string Value { get; }

            public Tag(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }
    }
}
