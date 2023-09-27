using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-wizard-step-body", ParentTag = "abp-wizard-body")]
    public class JobsWizardStepBodyTagHelper : TagHelper
    {
        [HtmlAttributeName("number")]
        public int Number { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            output.TagName = "";
            var contant = $"<div class=\"flex-column {(Number == 1 ? "current" : "")}\" data-kt-stepper-element=\"content\"  id=\"step{Number}\">{childContent.GetContent()}</div>";
            output.Content.SetHtmlContent($"{contant}");
            base.Process(context, output);
        }
    }
}
