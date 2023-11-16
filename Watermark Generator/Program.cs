using ITK_Watermark;
using System.Drawing;

var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

//Image img = Image.FromFile(path+"/8-EeyshzMeQHgfHNi.png");
Bitmap image = new Bitmap(path+"/8-EeyshzMeQHgfHNi.png");

var wm = new Watermark();

byte[] b = wm.GenerateWatermarkLayer("Created by Ahmad Sucipto",1280, 720,alpha:0.1);
wm.SaveToPath(b, "Dummy.png", path);

byte[] b2 = wm.DrawWatermark(image, "Created by Ahmad Sucipto", alpha:0.1);
wm.SaveToPath(b2, "Dummy2.png", path);
