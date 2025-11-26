using Common.Core;

namespace Common.AspNetCore
{
    public class ImageModel
    {
        protected ImageModel() { }

        public ImageModel(
            string source, 
            ImageResizeSettings resizeSettings = null)
        {
            Source = source.SetEmptyToNull();
            ResizeSettings = resizeSettings;
        }

        public string Source { get; protected set; }

        public virtual string FullSource
        {
            get
            {
                if (ResizeSettings == null)
                    return Source;
                return $"{Source}?{ResizeSettings.ToQueryString()}";
            }
        }

        public ImageResizeSettings ResizeSettings { get; protected set; }
    }
}
