using bradjolicoeur.Core.Models.ContentType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.ViewModels
{
    public class HomeViewModel
    {
        public ContentPage ContentPage { get; set; }
        public IEnumerable<BlogArticle> RecentArticles {get;set;}

    }
}
