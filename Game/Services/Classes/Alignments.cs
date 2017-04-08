namespace OpenGL_Practice.Services.Classes
{
    public static class Alignments
    {
        public static IAlignment TopLeft = new Alignment(Enums.WidthAlignment.Left, Enums.HeightAlignment.Top);
        public static IAlignment TrueLeft = new Alignment(Enums.WidthAlignment.Left, Enums.HeightAlignment.Center);
        public static IAlignment BottomLeft = new Alignment(Enums.WidthAlignment.Left, Enums.HeightAlignment.Bottom);

        public static IAlignment TrueTop = new Alignment(Enums.WidthAlignment.Center, Enums.HeightAlignment.Top);
        public static IAlignment TrueCenter = new Alignment(Enums.WidthAlignment.Center, Enums.HeightAlignment.Center);
        public static IAlignment TrueBottom = new Alignment(Enums.WidthAlignment.Center, Enums.HeightAlignment.Bottom);

        public static IAlignment TopRight = new Alignment(Enums.WidthAlignment.Right, Enums.HeightAlignment.Top);
        public static IAlignment TrueRight = new Alignment(Enums.WidthAlignment.Right, Enums.HeightAlignment.Center);
        public static IAlignment BottomRight = new Alignment(Enums.WidthAlignment.Right, Enums.HeightAlignment.Bottom);
    }

    public class Alignment : IAlignment
    {
        public Enums.WidthAlignment WidthAlignment { get; set; }
        public Enums.HeightAlignment HeightAlignment { get; set; }

        public Alignment(Enums.WidthAlignment widthAlignment, Enums.HeightAlignment heightAlignment)
        {
            WidthAlignment = widthAlignment;
            HeightAlignment = heightAlignment;
        }
    }

    public interface IAlignment
    {
        Enums.WidthAlignment WidthAlignment { get; set; }
        Enums.HeightAlignment HeightAlignment { get; set; }
    }
}
