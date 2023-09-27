using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    public class CardContext
    {
        public IHtmlContent Toolbar { get; set; }
        public IHtmlContent Body { get; set; }
        public IHtmlContent Header { get; set; }
    }

    [HtmlTargetElement("abp-card")]
    [RestrictChildren("abp-card-body", "abp-card-header")]
    public class JobsCardTagHelper : TagHelper
    {
        public string Id { get; set; }
        public bool Collapse { get; set; }
        public string Style { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            var cardContext = new CardContext();

            context.Items.Add(typeof(JobsCardTagHelper), cardContext);
            await output.GetChildContentAsync();
            var content = $@"<div class='card card-custom   {(!string.IsNullOrEmpty("Style") ? $"{Style}" : "gutter-b ")}   {(Collapse ? "card-collapsed" : "")}' data-card='true' id='{Id}'  >";
            //<div class='card-header'>
            //    <div class='card-title'>
            //     {  (Icon != "" ? "<span class='card-icon'>  <i class='" + Icon + " text -primary'></i>  </span>" : "") }   
            //        <h3 class='card-label'>{Title}</h3>
            //    </div>
            //   ";
            output.Content.AppendHtml(content);

            if (cardContext.Header != null)
            {
                /// output.Content.AppendHtml(" <div class='card-toolbar'>");
                output.Content.AppendHtml(cardContext.Header);
                ///  output.Content.AppendHtml("</div>");
            }


            output.Content.AppendHtml(" <div class='card-body'  >");
            if (cardContext.Body != null)
            {

                output.Content.AppendHtml(cardContext.Body);

            }
            output.Content.AppendHtml("</div>");
            output.Content.AppendHtml("</div>");

        }
    }
}
