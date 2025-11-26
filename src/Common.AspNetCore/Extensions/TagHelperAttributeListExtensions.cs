using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Common.AspNetCore
{
    public static class TagHelperAttributeListExtensions
    {
        /// <summary>
        /// Appends CSS class <paramref name="cssClass"/> to the "class" tag attribute.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="cssClass"></param>
        public static void AddCssClass(this TagHelperAttributeList attributes, string cssClass)
        {
            if (attributes == null || string.IsNullOrWhiteSpace(cssClass))
                return;

            var existingClassAttr = attributes.FirstOrDefault(a => a.Name == "class");
            if (existingClassAttr != null)
                attributes.SetAttribute("class", $"{existingClassAttr.Value} {cssClass}");
            else
                attributes.SetAttribute("class", cssClass);
        }

        /// <summary>
        /// Adds attribute if not already applied to list of attributes.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void AddIfAbsent(this TagHelperAttributeList attributes, string name, string value)
        {
            if (attributes == null || string.IsNullOrWhiteSpace(name))
                return;

            if (attributes.Any(a => a.Name == name))
                return;

            attributes.SetAttribute(name, value);
        }


        /// <summary>
        /// Removes attribute with name <paramref name="name"/>.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="name"></param>
        public static void Remove(this TagHelperAttributeList attributes, string name)
        {
            if (attributes == null || string.IsNullOrWhiteSpace(name))
                return;

            attributes.Remove(new TagHelperAttribute(name));
        }
    }
}
