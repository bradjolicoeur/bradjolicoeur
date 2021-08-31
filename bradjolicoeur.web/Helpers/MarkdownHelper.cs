using Markdig;

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
