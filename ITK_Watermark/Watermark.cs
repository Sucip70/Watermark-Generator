using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace ITK_Watermark
{
    public class Watermark
    {

        public byte[] GenerateWatermarkLayer(String text, int width, int height, float fontSize = 22, float gap = 100, float angle = 30, double alpha = 0.2)
        {
            System.Drawing.Image img = new Bitmap(width, height);
            return Generate(img, text, width, height, fontSize, gap, angle, alpha);
        }
        public byte[] DrawWatermark(System.Drawing.Image img, String text, float fontSize = 22, float gap = 100, float angle = 30, double alpha = 0.2) {
            return Generate(img, text, img.Width, img.Height, fontSize, gap, angle, alpha);
        }

        private byte[] Generate(System.Drawing.Image img, String text, int width, int height, float fontSize = 22, float gap = 100, float angle = 30, double alpha = 0.2) {
            Color textColor = Color.FromArgb((int)(255*alpha), 255, 255, 255);
            Color backColor = Color.FromArgb(0, 255, 255, 255);
            Font font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);
            Graphics drawing = Graphics.FromImage(img);
            SizeF textSize = drawing.MeasureString(text, font);
            float radius = (float)Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            float n = gap / 2;
            String date = "On " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
            Font fontDate = new Font(FontFamily.GenericSansSerif, fontSize / 2);

            //drawing.Clear(backColor);

            Brush textBrush = new SolidBrush(textColor);
            //Draw here
            drawing.TranslateTransform(width / 2, height / 2);
            drawing.RotateTransform(angle);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    drawing.DrawString(text, font, textBrush, (textSize.Width + gap) * i - radius / 2, (textSize.Height + gap) * j - radius / 2);
                    drawing.DrawString(date, fontDate, textBrush, (textSize.Width + gap) * i - radius / 2, textSize.Height + (textSize.Height + gap) * j - radius / 2);
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