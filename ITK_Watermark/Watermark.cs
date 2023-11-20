using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;
using System.Security;

[assembly: AllowPartiallyTrustedCallers]

namespace ITK_Watermark
{
    public class Watermark
    {
        public byte[] GenerateWatermarkLayer(string text, int width, int height, float fontSize = 22, float gap = 100, float angle = 30, double alpha = 0.1, Color color = new Color(), bool shadow = false)
        {
            color = Color.FromArgb((int)(255*alpha), color==null?Color.White:color);
            System.Drawing.Image img = new Bitmap(width, height);
            return Generate(img, text, width, height, fontSize, gap, angle, color);
        }
        public byte[] DrawWatermark(System.Drawing.Image img, string text, float fontSize = 22, float gap = 100, float angle = 30, double alpha = 0.1, Color color = new Color(), bool shadow = false) {
            color = Color.FromArgb((int)(255 * alpha), color == null ? Color.White : color);
            
            return Generate(img, text, img.Width, img.Height, fontSize, gap, angle, color);
        }

        private byte[] Generate(System.Drawing.Image img, string text, int width, int height, float fontSize, float gap, float angle, Color textColor) {
            Font font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);
            Graphics drawing = Graphics.FromImage(img);
            SizeF textSize = drawing.MeasureString(text, font);
            float radius = (float)Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            float n = gap / 2;
            string date = "On " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
            Font fontDate = new Font(FontFamily.GenericSansSerif, fontSize / 2, FontStyle.Bold);
            Brush textBrush = new SolidBrush(textColor);

            //Draw here
            drawing.TranslateTransform(width / 2, height / 2);
            drawing.RotateTransform(angle);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var x = (textSize.Width + gap) * i - radius / 2;
                    var y = (textSize.Height + gap) * j - radius / 2;

                    drawing.DrawString(text, font, textBrush, x, y);
                    drawing.DrawString(date, fontDate, textBrush, x, textSize.Height + y);
                }
            }

            drawing.Save();
            textBrush.Dispose();
            drawing.Dispose();

            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public void SaveToPath(byte[] byteArray, string fileName, string path)
        {
            string imagePath = path + "/" + fileName;
            using (var ms = new MemoryStream(byteArray))
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                img.Save(imagePath, ImageFormat.Png);
            }
        }
    }
}