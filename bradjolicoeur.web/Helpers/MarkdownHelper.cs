using HeyRed.MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Helpers
{
    public static class MarkdownHelper
    {
        private static Markdown Markdown = new Markdown();

        public static string Transform(string text)
        {
            return Markdown.Transform(text);
        }
    }
}
