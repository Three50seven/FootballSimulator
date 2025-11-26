using Common.Core;
using Common.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.AspNetCore
{
    public class ImageResizeSettings : ValueObject<ImageResizeSettings>
    {
        public ImageResizeSettings(
            int width, 
            int height, 
            bool asPercentage = false, 
            ImageResizeFormatOption format = ImageResizeFormatOption.None) 
            : this(new ImageDimension(width, height, asPercentage), format)
        {

        }

        public ImageResizeSettings(
            ImageDimension dimensions, 
            ImageResizeFormatOption format = ImageResizeFormatOption.None)
        {
            Dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
            Format = format;

            if (dimensions.Width > 0)
                (QueryValues as Dictionary<string, string>).Add("width", dimensions.WidthText);

            if (dimensions.Height > 0)
                (QueryValues as Dictionary<string, string>).Add("height", dimensions.HeightText);

            if (Format != ImageResizeFormatOption.None)
                (QueryValues as Dictionary<string, string>).Add("rmode", Format.AsFriendlyName().ToLower());
        }

        public ImageDimension Dimensions { get; }
        public ImageResizeFormatOption Format { get; }
        public IReadOnlyDictionary<string, string> QueryValues { get; } = new Dictionary<string, string>();

        public virtual string ToQueryString()
        {
            if (QueryValues == null || !QueryValues.Any())
                return string.Empty;

            return string.Join("&", QueryValues.Select(q => $"{q.Key}={q.Value}"));
        }

        public override string ToString()
        {
            return ToQueryString();
        }

        public static ImageResizeSettings Thumbnail => new(ImageDimension.Thumbnail, ImageResizeFormatOption.Pad);
    }
}
