using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    [HtmlTargetElement("abp-timePicker")]
    public class JobsTimepickerTagHelper : TagHelper
    {
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
            var icon = "la la-clock-o";
            //if (Style == DatePickerStyle.Time) icon = "far fa-clock";

            output.TagName = "div";

            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (output == null)
                throw new ArgumentNullException(nameof(output));
            output.AddClass($"input-group timepicker");

            var sanitizedId = $"{TagBuilder.CreateSanitizedId(Id, "_")}-picker";

            output.Attributes.Add("id", sanitizedId);
            output.Attributes.Add("data-target-input", "nearest");


            var dateValye = "";
            if (!string.IsNullOrEmpty(Value))
            {
                dateValye = TimeSpan.Parse(Value).ToString();  /// string.Format("{0:hh:mm tt}", DateTime.Parse(Value));
            }

            var textBox = $"<input  name=\"{Name}\"   {(!String.IsNullOrEmpty(Required) ? "required =" + Required : "")}    value=\"{dateValye}\" id=\"{Id}\" class=\"form-control form-control-solid time-picker\"    data-target=\"#{sanitizedId}\" type=\"text\" />";
            var span =
                 $"<div class=\"input-group-append\" data-target=\"#{sanitizedId}\" data-toggle=\"datetimepicker\"> <span  class=\"input-group-text\"><i class=\"{icon}\"></i></span ></div>";

            output.Content.SetHtmlContent($"{textBox}{span}");

            base.Process(context, output);
        }
    }
}
