using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-card-toolbar", ParentTag = "abp-card-header")]
    public class JobsCardToolbarTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            var childContent = await output.GetChildContentAsync();
            var contant = $"<div class='card-toolbar'>{childContent.GetContent()} </div>";
            output.Content.SetHtmlContent($"{contant}");
            base.Process(context, output);
        }
    }
}
