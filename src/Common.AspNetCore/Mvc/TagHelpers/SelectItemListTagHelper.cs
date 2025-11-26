using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("select-list")]
    public class SelectItemListTagHelper : TagHelper
    {
        public static string DefaultSelectionText = "-- Select --";

        private bool _allowMultiple;
        private ICollection<string> _currentValues;

        private readonly IHtmlGenerator _htmlGenerator;

        public SelectItemListTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public string Name { get; set; }

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        [HtmlAttributeName("list")]
        public IEnumerable<SelectItem> Selections { get; set; }

        [HtmlAttributeName("multiple")]
        public bool Multiple { get; set; }

        [HtmlAttributeName("include-default")]
        public bool IncludeDefault { get; set; }

        [HtmlAttributeName("default-text")]
        public string DefaultText { get; set; }

        [HtmlAttributeNotBound]
        protected virtual string DefaultOptionLabel
        {
            get
            {
                return IncludeDefault
                    ? (string.IsNullOrWhiteSpace(DefaultText) ? DefaultSelectionText : DefaultText)
                    : null;
            }
        }

        [HtmlAttributeNotBound]
        protected IDictionary<string, object> HtmlAttributes
        {
            get
            {
                if (For != null &&
                    string.IsNullOrEmpty(For.Name) &&
                    string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix) &&
                    !string.IsNullOrEmpty(Name))
                {
                    return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "name", Name },
                    };
                }

                return null;
            }
        }

        public override void Init(TagHelperContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            // NOTE: code snippets here taken from SelectTagHelper
            // Ref: https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.TagHelpers/SelectTagHelper.cs

            if (For == null)
            {
                // Informs contained elements that they're running within a targeted <select/> element.
                context.Items[typeof(SelectItemListTagHelper)] = null;
                return;
            }

            // Note null or empty For.Name is allowed because TemplateInfo.HtmlFieldPrefix may be sufficient.
            // IHtmlGenerator will enforce name requirements.
            if (For.Metadata == null)
                throw new InvalidOperationException("No model metadata found.");

            // Base allowMultiple on the instance or declared type of the expression to avoid a
            // "SelectExpressionNotEnumerable" InvalidOperationException during generation.
            // Metadata.IsEnumerableType is similar but does not take runtime type into account.
            var realModelType = For.ModelExplorer.ModelType;
            _allowMultiple = typeof(string) != realModelType && typeof(IEnumerable).IsAssignableFrom(realModelType) && Multiple;
            _currentValues = _htmlGenerator.GetCurrentValues(ViewContext, For.ModelExplorer, For.Name, _allowMultiple);

            // Whether or not (not being highly unlikely) we generate anything, could update contained <option/>
            // elements. Provide selected values for <option/> tag helpers.
            context.Items[typeof(SelectItemListTagHelper)] = _currentValues;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Guard.IsNotNull(context, nameof(context));
            Guard.IsNotNull(output, nameof(output));

            output.TagName = "select";

            if (Name != null)
                output.CopyHtmlAttribute(nameof(Name), context);

            IEnumerable<SelectListItem> items = Selections?.Select(s => new SelectListItem(s.Name, s.Value, s.Selected)) ?? Enumerable.Empty<SelectListItem>();

            if (For == null)
            {
                var options = _htmlGenerator.GenerateGroupsAndOptions(optionLabel: DefaultOptionLabel, selectList: items);

                if (!string.IsNullOrWhiteSpace(Name))
                    output.Attributes.AddIfAbsent("name", Name);

                if (_allowMultiple)
                    output.Attributes.AddIfAbsent("multiple", "multiple");

                output.PostContent.AppendHtml(options);
                return;
            }

            var tagBuilder = _htmlGenerator.GenerateSelect(
                ViewContext,
                For.ModelExplorer,
                optionLabel: DefaultOptionLabel,
                expression: For.Name,
                selectList: items,
                currentValues: _currentValues,
                allowMultiple: _allowMultiple,
                htmlAttributes: HtmlAttributes);

            if (tagBuilder != null)
            {
                output.MergeAttributes(tagBuilder);

                if (tagBuilder.HasInnerHtml)
                {
                    output.PostContent.AppendHtml(tagBuilder.InnerHtml);
                }
            }
        }
    }
}
