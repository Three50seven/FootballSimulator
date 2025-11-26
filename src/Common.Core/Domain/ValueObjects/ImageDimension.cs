namespace Common.Core.Domain
{
    public class ImageDimension : ValueObject<ImageDimension>
    {
        protected ImageDimension() { }

        public ImageDimension(int width, int height, bool asPercentage = false)
        {
            Height = height;
            Width = width;
            AsPercentage = asPercentage;
        }

        public int Height { get; private set; } = 100;
        public int Width { get; private set; } = 100;
        public bool AsPercentage { get; private set; } = false;

        public string HeightText => string.Concat(Height, AsPercentage ? "%" : string.Empty);
        public string WidthText => string.Concat(Width, AsPercentage ? "%" : string.Empty);

        public static ImageDimension Thumbnail => new ImageDimension(280, 187);
    }
}
