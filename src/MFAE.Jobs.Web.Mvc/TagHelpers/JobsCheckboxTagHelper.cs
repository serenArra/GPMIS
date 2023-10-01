using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-checkBox")]
    public class JobsCheckboxTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("name")]
        public string Name { get; set; }


        [HtmlAttributeName("class")]
        public string Clazz { get; set; }

        [HtmlAttributeName("value")]
        public string Value { get; set; }


        [HtmlAttributeName("checked")]
        public bool Checked { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "";
            var childContent = await output.GetChildContentAsync();
            var checkBox = $"<label class=\"checkbox checkbox-primary\"><input  class=\"{Clazz}\"  id=\"{Id}\" {(Checked == true ? "checked =\"checked\"" : "")}   value=\"{Value}\"  type =\"checkbox\" name=\"{Name}\" ><span></span>{childContent.GetContent()} </label>";
            output.Content.SetHtmlContent($"{checkBox}");
            base.Process(context, output);
        }
    }
}
