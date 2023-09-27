using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-tab-item-body", ParentTag = "abp-tab-body")]
    public class JobsTabItemBodyTagHelper : TagHelper
    {
        [HtmlAttributeName("number")]
        public int Number { get; set; }

        [HtmlAttributeName("class")]
        public string Clazz { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            var childContent = await output.GetChildContentAsync();

            output.TagName = "";
            var contant = $"<div class=\"tab-pane fade {(Number == 1 ? "show active" : "")}  {Clazz} \" id=\"kt_user_edit_tab_{Number}\" role=\"tabpanel\">{childContent.GetContent()} </div>";

            output.Content.SetHtmlContent($"{contant}");
            base.Process(context, output);
        }
    }
}
