using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;
using System.Security;
using static System.Net.Mime.MediaTypeNames;

[assembly: AllowPartiallyTrustedCallers]

namespace ITK_Watermark
{
    public class Watermark
    {
        public string label { get; set; } 
        public string date { get { return "On " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"); } set { } } 
        public int width { get; set; }
        public int height { get; set; }
        public float fontSizeLabel { get; set; }
        public float fontSizeDate { get; set; }
        public float gap { get; set; }
        public float angle { get; set; }
        public double alphaLabel { get; set; }
        public double alphaDate { get; set; }
        public Color colorLabel { get; set; }
        public Color colorDate { get; set; }
        public FontFamily fontFamilyLabel { get { return FontFamily.GenericSansSerif; } set { } }
        public FontFamily fontFamilyDate { get { return FontFamily.GenericSansSerif; } set { } }
        public FontStyle fontStyleLabel { get { return FontStyle.Bold; } set { } }
        public FontStyle fontStyleDate { get { return FontStyle.Bold; } set { } }
        public float horizontalResolution { get { return 500; } set { } }
        public float verticalResolution { get { return 500; } set { } }
        public bool includeDate { get { return true; } set { } }

        public Watermark(int width, int height, float fontSize = 22, float gap = 100, float angle = 30, double alpha = 1.0, Color color = new Color()) {
            this.width = width;
            this.height = height;
            this.fontSizeLabel = fontSize;
            this.fontSizeDate = (int)Math.Ceiling(fontSize*2 / 3);
            this.gap = gap;
            this.angle = angle;
            this.alphaLabel = alpha;
            this.alphaDate = alpha;
            this.colorLabel = Color.FromArgb((int)(255 * alpha), color == null ? Color.White : color);
            this.colorDate = Color.FromArgb((int)(255 * alpha), color == null ? Color.White : color);
        }

        public byte[] GenerateWatermarkLayer(string text)
        {
            this.label = text;
            System.Drawing.Bitmap img = new Bitmap(width, height);
            return Generate(img);
        }
        public byte[] DrawWatermark(System.Drawing.Bitmap img, string text) {
            this.label = text;
            return Generate(img);
        }

        private byte[] Generate(System.Drawing.Bitmap img) {
            Font fontLabel = new Font(this.fontFamilyLabel, this.fontSizeLabel, this.fontStyleLabel);
            img.SetResolution(horizontalResolution, verticalResolution);
            Graphics drawing = Graphics.FromImage(img);
            SizeF textSize = drawing.MeasureString(this.label, fontLabel);
            float radius = (float)Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            float n = gap / 2;
            Brush labelBrush = new SolidBrush(this.colorLabel);

            string date = "On " + DateTime.Now.ToString("dd MMM yyyy hh:mm tt");
            Font fontDate = new Font(this.fontFamilyDate, this.fontSizeDate, fontStyleDate);
            Brush dateBrush = new SolidBrush(this.colorDate);

            //Draw here
            drawing.TranslateTransform(width / 2, height / 2);
            drawing.RotateTransform(angle);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var x = (textSize.Width + gap) * i - radius / 2;
                    var y = (textSize.Height + gap) * j - radius / 2;

                    drawing.DrawString(this.label, fontLabel, labelBrush, x, y);
                    if (includeDate)
                    {
                        drawing.DrawString(date, fontDate, dateBrush, x, textSize.Height + y);
                    }
                }
            }

            drawing.Save();
            labelBrush.Dispose();
            dateBrush.Dispose();
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