// programmed by Adrian Magdina in 2013
// in this file is the implementation of class for linear interpolation of color.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CAExplorerNamespace
{
    //in this class is the implementation of linear interpolation for colors (RGBA color)
    public static class ColorInterpolation
    {
        public static Color InterpolateColor(Color color1In, Color color2In, int currentStateIn, int minStateIn, int maxStateIn)
        {
            Color aColor = new Color();

            aColor.R = (byte)(color1In.R + (color2In.R - color1In.R) * (currentStateIn - minStateIn) / (maxStateIn - minStateIn));
            aColor.G = (byte)(color1In.G + (color2In.G - color1In.G) * (currentStateIn - minStateIn) / (maxStateIn - minStateIn));
            aColor.B = (byte)(color1In.B + (color2In.B - color1In.B) * (currentStateIn - minStateIn) / (maxStateIn - minStateIn));
            aColor.A = (byte)(color1In.A + (color2In.A - color1In.A) * (currentStateIn - minStateIn) / (maxStateIn - minStateIn));

            return aColor;
        }
    }
}
