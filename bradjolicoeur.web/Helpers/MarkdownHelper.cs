using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Helpers
{
    public static class MarkdownHelper
    {
        public static string Transform(string text)
        {
            return Markdown.ToHtml(text);
        }
    }
}
