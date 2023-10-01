using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [RestrictChildren("abp-tab-item")]
    [HtmlTargetElement("abp-tab-items", ParentTag = "abp-tab")]
    public class JobsTabItemsTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            await output.GetChildContentAsync();
        }
    }
}
