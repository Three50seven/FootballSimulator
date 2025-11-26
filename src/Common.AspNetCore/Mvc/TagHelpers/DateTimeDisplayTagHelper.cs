using Microsoft.AspNetCore.Razor.TagHelpers;
using Common.Core;
using System;

namespace Common.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "date-value")]
    public class DateTimeDisplayTagHelper : TagHelper
    {
        public static string DefaultDateFormat = DateTimeFormats.ShortDateFullYear;
        public static string DefaultDateTimeFormat = DateTimeFormats.DateAndShortTime;

        [HtmlAttributeName("date-value")]
        public DateTime? Date { get; set; }

        [HtmlAttributeName("date-format")]
        public string Format { get; set; }

        [HtmlAttributeName("date-ignore-time")]
        public bool IgnoreTime { get; set; } = false;

        [HtmlAttributeNotBound]
        protected string CleanedFormat
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Format))
                    return Format;
                else
                    return Date.IsMidnight() ? DefaultDateFormat : DefaultDateTimeFormat;
            }
        }

        [HtmlAttributeNotBound]
        protected string FormattedDate => Date?.Format(CleanedFormat) ?? string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Date.HasValue())
                return;

            if (!Date.IsMidnight())
            {
                // apply attributes that will be parsed by moment.js via the datetime.js module
                output.Attributes.AddIfAbsent("data-date", Date.Format(DateTimeFormats.FullDateTime));
                output.Attributes.AddIfAbsent("data-date-format", CleanedFormat.ToJSFriendlyDateFormat());
                output.Attributes.AddIfAbsent("data-date-ignore-time", IgnoreTime.ToString().ToLower());
            }

            output.Content.SetContent(FormattedDate);
        }
    }
}
