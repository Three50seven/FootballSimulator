using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("hidden-serialized-model", Attributes = "model", TagStructure = TagStructure.WithoutEndTag)]
    public class HiddenSerializedModelTagHelper : TagHelper
    {
        private readonly ISerializer _serializer;

        public HiddenSerializedModelTagHelper(ISerializer serializer)
        {
            _serializer = serializer;
        }

        [HtmlAttributeName("model")]
        public object Model { get; set; }

        [HtmlAttributeName("name")]
        public string Name { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.Attributes.SetAttribute("type", "hidden");
            output.Attributes.SetAttribute("name", string.IsNullOrWhiteSpace(Name) ? "SerializedModel" : Name);
            output.Attributes.SetAttribute("id", output.Attributes["name"].Value);
            output.TagMode = TagMode.SelfClosing;

            string serializedValue = string.Empty;

            if (Model != null)
                serializedValue = _serializer.Serialize(Model);

            output.Attributes.SetAttribute("value", serializedValue);
        }
    }
}
