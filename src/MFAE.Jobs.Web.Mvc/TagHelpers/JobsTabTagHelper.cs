using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    public class TabContext
    {
        public IHtmlContent TabBody { get; set; }
        public ICollection<JobsTabItemTagHelper> TabItems { get; set; } = new List<JobsTabItemTagHelper>();
    }

    [HtmlTargetElement("abp-tab")]
    [RestrictChildren("abp-tab-items", "abp-tab-body")]
    public class JobsTabTagHelper : TagHelper
    {
        public string Id { get; set; }

        [HtmlAttributeName("formName")]
        public string FormName { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            var tabContext = new TabContext();

            context.Items.Add(typeof(JobsTabTagHelper), tabContext);
            await output.GetChildContentAsync();


            var content = $"<div class=\"card card-custom\">\n" +
         "                    <!--begin::Card header-->\n" +
         "                    <div class=\"card-header card-header-tabs-line nav-tabs-line-3x\">\n" +
         "                        <!--begin::Toolbar-->\n" +
         "                        <div class=\"card-toolbar\">\n" +
         "                            <ul class=\"nav nav-tabs nav-line-tabs mb-5 fs-6 nav-bold\">";

            output.Content.AppendHtml(content);

            if (tabContext.TabItems != null)
            {

                int currentItem = 1;

                foreach (var ti in tabContext.TabItems)
                {
                    content = $"<li class=\"nav-item mr-3\"> <a id=\"{ti.Id}\" class=\"nav-link  {(currentItem == 1 ? "active" : "")}\" data-toggle=\"tab\" data-bs-toggle=\"tab\" href=\"#kt_user_edit_tab_{currentItem}\">{ti.Output} <span class=\"nav - text font - size - lg\">{ti.Title}</span></a> </li>";
                    output.Content.AppendHtml(content);
                    currentItem++;
                }
            }
            content = $"</ul></div></div>";

            content += $"<div class=\"card-body px-0\">\n" +
    "  <form novalidate class=\"form-validation\" name=\"" + FormName + "\">" +
            "                            <div class=\"tab-content\">";

            output.Content.AppendHtml(content);
            if (tabContext.TabBody != null)
            {
                output.Content.AppendHtml(tabContext.TabBody);
            }

            output.Content.AppendHtml(" </div>  </form>  </div>  </div>");
        }
    }
}
