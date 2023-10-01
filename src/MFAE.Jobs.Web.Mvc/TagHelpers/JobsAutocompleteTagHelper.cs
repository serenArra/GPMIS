using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-autocomplete")]
    public class JobsAutocompleteTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName("multiple")]
        public bool Multiple { get; set; }

        [HtmlAttributeName("id")]
        public string Id { get; set; }

        [HtmlAttributeName("name")]
        public string Name { get; set; }


        [HtmlAttributeName("paramsGenerateFunction")]
        public string ParamsGenerateFunction { get; set; }

        [HtmlAttributeName("required")]
        public bool Required { get; set; }


        [HtmlAttributeName("values")]
        public string Values { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("optionValue")]
        public string OptionValue { get; set; }
        [HtmlAttributeName("optionKey")]
        public string OptionKey { get; set; }


        [HtmlAttributeName("action")]
        public string ActionName { get; set; }
        [HtmlAttributeName("controller")]
        public string Controller { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "";
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            var selectId = Id.Replace('.', '_');
            var select = $"<select id=\"{selectId}\"    style='width: 100 %'    values='{Values}'   name=\"{Name}\"  {(!String.IsNullOrEmpty(ParamsGenerateFunction) ? "paramsGenerateFunction =" + ParamsGenerateFunction : "")}   {(Multiple ? "multiple =\"multiple\"" : "")}  optionKey=\"{OptionKey}\" optionValue=\"{OptionValue}\"  controller=\"{Controller}\"   action=\"{ActionName}\"   {(Required ? "required =\"required\"" : "")}   class=\"form-control form-control-solid select2-remote\" data-allow-clear=\"true\"></select>";
            output.Content.SetHtmlContent($"{select}");

            base.Process(context, output);
        }

        public JobsAutocompleteTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }
    }
}
