using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [RestrictChildren("abp-wizard-step")]
    [HtmlTargetElement("abp-wizard-steps", ParentTag = "abp-wizard")]
    public class JobsWizardStepsTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.SuppressOutput();
            await output.GetChildContentAsync();
        }
    }
}
