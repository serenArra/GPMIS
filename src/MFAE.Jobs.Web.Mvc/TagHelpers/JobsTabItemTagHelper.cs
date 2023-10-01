using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-tab-item", ParentTag = "abp-tab-items")]
    public class JobsTabItemTagHelper : TagHelper
    {
        [HtmlAttributeName("Id")]
        public string Id { get; set; }

        [HtmlAttributeName("description")]
        public string Description { get; set; }

        [HtmlAttributeName("title")]
        public string Title { get; set; }


        [HtmlAttributeNotBound]
        public string Output { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "";
            var wizardContext = (TabContext)context.Items[typeof(JobsTabTagHelper)];
            var childContent = await output.GetChildContentAsync();

            wizardContext.TabItems.Add(new JobsTabItemTagHelper
            {
                Id = Id,
                Description = Description,
                Title = Title,
                Output = childContent.GetContent()
            });
        }
    }
}
