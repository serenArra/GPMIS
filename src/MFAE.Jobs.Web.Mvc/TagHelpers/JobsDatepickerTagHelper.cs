using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-datePicker")]
    public class JobsDatepickerTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _generator;

        [HtmlAttributeName("format")]
        public string Format { get; set; }
        [HtmlAttributeName("required")]
        public string Required { get; set; }


        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        
        [HtmlAttributeName("id")]
        public string Id { get; set; }


        [HtmlAttributeName("name")]
        public string Name { get; set; }


        [HtmlAttributeName("value")]
        public string Value { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var icon = "far fa-calendar";
            //if (Style == DatePickerStyle.Time) icon = "far fa-clock";

            output.TagName = "div";

            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            

            output.AddClass($"input-group ");

            var sanitizedId = $"{TagBuilder.CreateSanitizedId(Id, "_")}-picker";

            output.Attributes.Add("id", sanitizedId);
            output.Attributes.Add("data-target-input", "nearest");

            
            var textBox = $"<input  name=\"{Name}\"  value=\"{Value}\"   {(!String.IsNullOrEmpty(Required) ? "required =" + Required : "")}  id=\"{Id}\" class=\"form-control form-control-solid date-picker\" data-date-format=\"{Format}\"  data-target=\"#{sanitizedId}\" type=\"datetime\" />";


            var span =
                // ReSharper disable once StringLiteralTypo
                $"<div class=\"input-group-append\" data-target=\"#{sanitizedId}\" data-toggle=\"datetimepicker\"> <div class=\"input-group-text\"><i class=\"{icon}\"></i></div></div>";
           
            output.Content.SetHtmlContent($"{textBox}{span}");

            base.Process(context, output);
        }

        public JobsDatepickerTagHelper(IHtmlGenerator generator)
        {
            _generator = generator;
        }
    }
}
