using Markdig;
using Markdig.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bradjolicoeur.web.Helpers
{
    public static class MarkdownHelper
    {

        private static MarkdownPipeline pipeline;

        public static string Transform(string text)
        {
            if(pipeline == null)
            {
                pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .Build();
            }

            return Markdown.ToHtml(text, pipeline);
        }

    }
}
