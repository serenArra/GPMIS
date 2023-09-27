using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-card-body", ParentTag = "abp-card")]
    public class JobsCardBodyTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var cardContext = (CardContext)context.Items[typeof(JobsCardTagHelper)];
            cardContext.Body = childContent;
            output.SuppressOutput();
        }
    }
}
