using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-card-header", ParentTag = "abp-card")]
    public class JobsCardHeaderTagHelper : TagHelper
    {
        public string Title { get; set; }
        public string Icon { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var cardContext = (CardContext)context.Items[typeof(JobsCardTagHelper)];

            var content = $@"<div class='card-header'>
                        <div class='card-title'>
                         {(Icon != "" ? "<div class='symbol symbol-primary mr-3'><span class='symbol-label icon-lg'>  <i class='" + Icon + " text-white'></i>  </span></div>" : "")}   
                            <h3 class='card-label'>{Title}</h3>
                        </div>
                            {childContent.GetContent()}
                       </div>
                       ";
            childContent.SetHtmlContent(content);
            cardContext.Header = childContent;
            output.SuppressOutput();
        }
    }
}
