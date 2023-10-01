using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [RestrictChildren("abp-tab-item-body")]
    [HtmlTargetElement("abp-tab-body", ParentTag = "abp-tab")]
    public class JobsTabBodyTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var cardContext = (TabContext)context.Items[typeof(JobsTabTagHelper)];
            cardContext.TabBody = childContent;
            output.SuppressOutput();

        }
    }
}
