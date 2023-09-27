using Abp.Localization;
using MFAE.Jobs.Localization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MFAE.Jobs.Web.TagHelpers
{
    public class WizardContext
    {
        public IHtmlContent WizardBody { get; set; }
        public ICollection<JobsWizardStepTagHelper> _WizardSteps { get; set; } = new List<JobsWizardStepTagHelper>();
    }

    [HtmlTargetElement("abp-wizard")]
    [RestrictChildren("abp-wizard-steps", "abp-wizard-body")]
    public class JobsWizardTagHelper : TagHelper
    {
        private readonly ILocalizationManager _localizationManager;
        public JobsWizardTagHelper(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        [HtmlAttributeName("formName")]
        public string FormName { get; set; }


        [HtmlAttributeName("hasSaveDraft")]
        public bool HasSaveDraft { get; set; }

        public string Id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var saveLabel = _localizationManager.GetString(JobsConsts.LocalizationSourceName, "Save");
            var nextLabel = _localizationManager.GetString(JobsConsts.LocalizationSourceName, "Next");
            var prevLabel = _localizationManager.GetString(JobsConsts.LocalizationSourceName, "Previous");

            var wizardContext = new WizardContext();

            context.Items.Add(typeof(JobsWizardTagHelper), wizardContext);
            await output.GetChildContentAsync();



            var arrowRight = $"<!--begin::Svg Icon | path:C:\\wamp64\\www\\keenthemes\\themes\\metronic\\theme\\html\\demo7\\dist/../src/media/svg/icons\\Navigation\\Arrow-right.svg--><svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" width=\"24px\" height=\"24px\" viewBox=\"0 0 24 24\" version=\"1.1\">\n" +
         "    <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\">\n" +
         "        <polygon points=\"0 0 24 0 24 24 0 24\"/>\n" +
         "        <rect fill=\"#000000\" opacity=\"0.3\" transform=\"translate(12.000000, 12.000000) rotate(-90.000000) translate(-12.000000, -12.000000) \" x=\"11\" y=\"5\" width=\"2\" height=\"14\" rx=\"1\"/>\n" +
         "        <path d=\"M9.70710318,15.7071045 C9.31657888,16.0976288 8.68341391,16.0976288 8.29288961,15.7071045 C7.90236532,15.3165802 7.90236532,14.6834152 8.29288961,14.2928909 L14.2928896,8.29289093 C14.6714686,7.914312 15.281055,7.90106637 15.675721,8.26284357 L21.675721,13.7628436 C22.08284,14.136036 22.1103429,14.7686034 21.7371505,15.1757223 C21.3639581,15.5828413 20.7313908,15.6103443 20.3242718,15.2371519 L15.0300721,10.3841355 L9.70710318,15.7071045 Z\" fill=\"#000000\" fill-rule=\"nonzero\" transform=\"translate(14.999999, 11.999997) scale(1, -1) rotate(90.000000) translate(-14.999999, -11.999997) \"/>\n" +
         "    </g>\n" +
         "</svg><!--end::Svg Icon-->";


            var arrowLeft = $"<!--begin::Svg Icon | path:C:\\wamp64\\www\\keenthemes\\themes\\metronic\\theme\\html\\demo7\\dist/../src/media/svg/icons\\Navigation\\Arrow-left.svg--><svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" width=\"24px\" height=\"24px\" viewBox=\"0 0 24 24\" version=\"1.1\">\n" +
         "    <g stroke=\"none\" stroke-width=\"1\" fill=\"none\" fill-rule=\"evenodd\">\n" +
         "        <polygon points=\"0 0 24 0 24 24 0 24\"/>\n" +
         "        <rect fill=\"#000000\" opacity=\"0.3\" transform=\"translate(12.000000, 12.000000) scale(-1, 1) rotate(-90.000000) translate(-12.000000, -12.000000) \" x=\"11\" y=\"5\" width=\"2\" height=\"14\" rx=\"1\"/>\n" +
         "        <path d=\"M3.7071045,15.7071045 C3.3165802,16.0976288 2.68341522,16.0976288 2.29289093,15.7071045 C1.90236664,15.3165802 1.90236664,14.6834152 2.29289093,14.2928909 L8.29289093,8.29289093 C8.67146987,7.914312 9.28105631,7.90106637 9.67572234,8.26284357 L15.6757223,13.7628436 C16.0828413,14.136036 16.1103443,14.7686034 15.7371519,15.1757223 C15.3639594,15.5828413 14.7313921,15.6103443 14.3242731,15.2371519 L9.03007346,10.3841355 L3.7071045,15.7071045 Z\" fill=\"#000000\" fill-rule=\"nonzero\" transform=\"translate(9.000001, 11.999997) scale(-1, -1) rotate(90.000000) translate(-9.000001, -11.999997) \"/>\n" +
         "    </g>\n" +
         "</svg><!--end::Svg Icon-->";




            var content = $"<!--begin::Stepper-->\n" +
               "                <div class=\"stepper stepper-pills \" id=\"" + Id + "\">\n" +
               "                    <!--begin::Nav-->\n" +
               "                    <div class=\"stepper-nav flex-center flex-wrap mb-10 \">\n";


            output.Content.AppendHtml(content);

            if (wizardContext._WizardSteps != null)
            {

                var contant = "";
                int last = wizardContext._WizardSteps.Count;
                int currentStep = 1;

                foreach (var wc in wizardContext._WizardSteps)
                {
                    contant = $"<div data-kt-stepper-element=\"nav\" data-kt-stepper-action=\"step\" class=\"stepper-item mx-8 my-4 {(currentStep == 1 ? "current" : "")}\">\n" +
                        "           <div class=\"stepper-wrapper d-flex align-items-center\">\n" +
                        "               <div class=\"stepper-icon w-40px h-40px\">\n" +
                        "                   <i class=\"stepper-check fas fa-check\"></i>\n" +
                        "                   <span class=\"stepper-number\">" + currentStep + "</span>\n" +
                        "               </div>\n" +
                        "               <div class=\"stepper-label\">\n" +
                        "                   <h3 class=\"stepper-title\">" + wc.Title + "</h3>\n" +
                        "                   <div class=\"stepper-desc\">" + wc.Description + "</div>\n" +
                        "               </div>\n";
                    if (currentStep != last)
                    {
                        contant += "<span class=\"svg-icon\">";
                        if (!CultureHelper.IsRtl)
                        {

                            contant += arrowRight;
                        }
                        else
                        {
                            contant += arrowLeft;

                        }
                        contant += "</span>";
                    }

                    contant += "           </div>\n" +
                        "           <div class=\"stepper-line h-40px\"></div>\n" +
                        "       </div>";


                    currentStep++;

                    output.Content.AppendHtml(contant);
                }

            }

            output.Content.AppendHtml("<!--end::Wizard Steps-->");
            output.Content.AppendHtml("</div>");


            var formBody = $"<!--end: Wizard Nav-->\n" +
                  "            <!--begin: Wizard Body-->\n" +
                  "  <form novalidate class=\"form w-lg mx-auto card-body\" name=\"" + FormName + "\">";

            output.Content.AppendHtml(formBody);

            if (wizardContext.WizardBody != null)
            {
                output.Content.AppendHtml(wizardContext.WizardBody);
            }

            var action = $"<!--begin: Wizard Actions-->\n" +
               "                            <div class=\"d-flex flex-stack\">\n" +
                "                                <div class=\"me-2\">\n" +
                "                                    <button type=\"button\" class=\"btn btn-light  font-weight-bolder font-size-h6  my-3 mr-3\" data-kt-stepper-action=\"previous\">\n" +
             "<span class=\"svg-icon svg-icon-md ml-2\">";

            if (!CultureHelper.IsRtl)
            {
                action += arrowLeft;

            }
            else
            {
                action += arrowRight;

            }


            action += "</span>" + prevLabel + "</button>\n" +
            "                                </div>\n" +
            "                                <div>\n" +


            "  <button  type=\"button\" id=\"btnSave\" class=\"btn btn-primary save-button\" data-kt-stepper-action=\"submit\" ><i class=\"fa fa-save\"></i> <span>" + saveLabel + "</span></button>\n" +





            "                                    <button type=\"button\" class=\"btn btn-primary  font-weight-bolder font-size-h6  my-3\" data-kt-stepper-action=\"next\">\n" +
                "                                        " + nextLabel + "\n " +
             "<span class=\"svg-icon svg-icon-md ml-2\">";



            if (!CultureHelper.IsRtl)
            {
                action += arrowRight;
            }

            else
            {
                action += arrowLeft;

            }

            action += "  </span></button>\n";


            if (HasSaveDraft)
            {
                var saveDraftLabel = _localizationManager.GetString(JobsConsts.LocalizationSourceName, "SaveDraft");
                action += "  <button  type=\"button\" class=\"btn btn-light-primary save-draft-button\" data-kt-stepper-action=\"submit-draft\" ><i class=\"fa fa-save\"></i> <span>" + saveDraftLabel + "</span></button>\n";
            }

            action += " </div>\n" +
             "           </div>\n" +
            "           </div>\n" +
            "<!--end: Wizard Actions-->";

            output.Content.AppendHtml(action);

            output.Content.AppendHtml("</form>");

            output.Content.AppendHtml(" <!--end::Form-->");
            output.Content.AppendHtml("</div>");
            output.Content.AppendHtml(" <!--end::Container-->");
            output.Content.AppendHtml("</div>");
            output.Content.AppendHtml("<!--end::Wizard 6-->");

        }
    }
}
