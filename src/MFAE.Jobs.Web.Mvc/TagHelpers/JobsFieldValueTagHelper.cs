using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-field-value")]
    public class JobsFieldValueTagHelper : TagHelper
    {
        [HtmlAttributeName("label")]
        public string Label { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            var childContent = await output.GetChildContentAsync();
            var contant = $"  <h6 class='font-weight-bolder pt-6'>{Label}</h6> {childContent.GetContent()}  <div class='separator  separator-solid py-3'></div>";
            output.Content.SetHtmlContent($"{contant}");
            base.Process(context, output);
        }
    }
}
