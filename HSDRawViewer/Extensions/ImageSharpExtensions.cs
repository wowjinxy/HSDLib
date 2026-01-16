using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using HSDRawViewer.GUI.Dialog;

namespace HSDRawViewer.Extensions
{
    public static class ImageSharpExtensions
    {
        public static void RemapChannels(this Image<Bgra32> image, ChannelType color, ChannelType alpha)
        {
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                {
                    image[x, y] = ApplyChannels(image[x, y], color, alpha);
                }
        }

        private static Bgra32 ApplyChannels(Bgra32 input, ChannelType color, ChannelType alpha)
        {
            return new Bgra32(
                GetChannel(input, ChannelType.RED, color),
                GetChannel(input, ChannelType.GREEN, color),
                GetChannel(input, ChannelType.BLUE, color),
                GetChannel(input, ChannelType.ALPHA, alpha));
        }

        private static byte GetChannel(Bgra32 input, ChannelType inputChannel, ChannelType channel)
        {
            byte r = input.R;
            byte g = input.G;
            byte b = input.B;
            byte a = input.A;

            switch (channel)
            {
                case ChannelType.ALPHA:
                    return a;
                case ChannelType.RED:
                    return r;
                case ChannelType.GREEN:
                    return g;
                case ChannelType.BLUE:
                    return b;
                default:
                    return GetChannel(input, inputChannel, inputChannel);
            }
        }
    }
}
