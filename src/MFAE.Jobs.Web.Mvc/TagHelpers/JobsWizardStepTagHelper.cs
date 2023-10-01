using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-wizard-step", ParentTag = "abp-wizard-steps")]
    public class JobsWizardStepTagHelper : TagHelper
    {
        [HtmlAttributeName("description")]
        public string Description { get; set; }

        [HtmlAttributeName("title")]
        public string Title { get; set; }

        [HtmlAttributeNotBound]
        public string Output { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            var wizardContext = (WizardContext)context.Items[typeof(JobsWizardTagHelper)];

            wizardContext._WizardSteps.Add(new JobsWizardStepTagHelper
            {
                Description = Description,
                Title = Title,
                Output = Output
            });

        }
    }
}
