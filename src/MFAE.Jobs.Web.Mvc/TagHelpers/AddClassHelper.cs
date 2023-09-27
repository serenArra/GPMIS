using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;

namespace MFAE.Jobs.Web.TagHelpers
{
    public static class AddClassHelper
    {
        /// <summary>
        /// Adds <paramref name="newClass"/> to <paramref name="output"/> or appends it.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="newClass"></param>
        public static void AddClass(this TagHelperOutput output, string newClass)
        {
            var classAttribute = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttribute == null)
            {
                classAttribute = new TagHelperAttribute("class", newClass);
                output.Attributes.Add(classAttribute);
            }
            else if (classAttribute.Value == null || classAttribute.Value.ToString().IndexOf(newClass, StringComparison.Ordinal) < 0)
            {
                output.Attributes.SetAttribute("class", classAttribute.Value == null
                    ? newClass
                    : $"{newClass} {classAttribute.Value}");
            }
        }
    }
}
