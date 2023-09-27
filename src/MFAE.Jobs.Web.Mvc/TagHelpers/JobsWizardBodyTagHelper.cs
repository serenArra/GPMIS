using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [RestrictChildren("abp-wizard-step-body")]
    [HtmlTargetElement("abp-wizard-body", ParentTag = "abp-wizard")]
    public class JobsWizardBodyTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var cardContext = (WizardContext)context.Items[typeof(JobsWizardTagHelper)];
            cardContext.WizardBody = childContent;
            output.SuppressOutput();
        }
    }
}
