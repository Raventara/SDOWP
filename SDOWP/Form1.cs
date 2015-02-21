using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDOW_P
{
    public partial class Form1 : Form
    {
        private const string outputBackgroundFile = @"C:\Users\Public\Pictures\SDOBackground.bmp";

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        private const uint SPI_SETDESKWALLPAPER = 20;
        private const uint SPIF_UPDATEINIFILE = 0x01;
        private const uint SPIF_SENDWININICHANGE = 0x02;

        private const string baseURL = "http://sdo.gsfc.nasa.gov/assets/img/latest/";
        private List<Bitmap> SDOIcons = new List<Bitmap>();
        private List<Bitmap> SDOIconsGrey = new List<Bitmap>();
        private List<string> SDONames = new List<string>();
        private List<bool> SDOSelections = new List<bool>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            LoadSDOThumbnails();

            string path = string.Empty;
            //GetFancySDOWP();

            DrawDisplays();

            var images = new Dictionary<string, Image>
			{
				//{ @"\\.\DISPLAY1", Image.FromFile(@"C:\Users\russell.bruechert\Pictures\Jupiter.png") },
				{ @"\\.\DISPLAY1", Image.FromFile(Application.CommonAppDataPath + @"\SDO.jpg") },
				//{ @"\\.\DISPLAY3", Image.FromFile(@"C:\Users\russell.bruechert\Pictures\Jupiter.png") },
			};

            CreateBackgroundImage(images);

        }

        private void LoadSDOThumbnails()
        {
            string[] urls = new string[] {"latest_170_0094.jpg", "latest_170_0131.jpg", "latest_170_0171.jpg", "latest_170_0193.jpg", "latest_170_0211.jpg", "latest_170_0304.jpg"
									,"latest_170_0335.jpg"}; //, "latest_170_1600.jpg", "latest_170_1700.jpg", "latest_170_4500.jpg", "latest_170_HMIIC.jpg", "latest_170_HMIIF.jpg" };

            foreach (var url in urls)
            {

                using (WebClient webClient = new WebClient())
                {
                    byte[] data = webClient.DownloadData(baseURL + url);

                    using (MemoryStream mem = new MemoryStream(data))
                    {
                        Bitmap SDO = new Bitmap(Image.FromStream(mem));
                        SDOIcons.Add(SDO);
                        SDOIconsGrey.Add(MakeGrayscale3(SDO));
                    }
                }
            }

            int x = 0;
            foreach (Control control in tpSDO.Controls.Cast<Control>()
                                                    .OrderBy(c => c.Name).Where(c => c.GetType() == typeof(PictureBox)))
            {
                control.BackgroundImage = SDOIcons[x];
                SDONames.Add(control.Name);
                SDOSelections.Add(true);
                x++;
            }
        }

        /// <summary>
        /// http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            //      ColorMatrix colorMatrix = new ColorMatrix(
            //         new float[][] 
            //{
            //   new float[] {.3f, .3f, .3f, 0, 0},
            //   new float[] {.59f, .59f, .59f, 0, 0},
            //   new float[] {.11f, .11f, .11f, 0, 0},
            //   new float[] {0, 0, 0, 1, 0},
            //   new float[] {0, 0, 0, 0, 1}
            //});
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
                       {
                        new float[] {0.4f, 0, 0, 0, 0},
                        new float[] {0, 0.4f, 0, 0, 0},
                        new float[] {0, 0, 0.4f, 0, 0},
                        new float[] {0, 0, 0, 0.7f, 0},
                        new float[] {0, 0, 0, 0, 1}
                       });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        private Bitmap SDOPreviewImage()
        {
            int segmentCount = 0;
            List<int> indices = new List<int>();

            for (int i = 0; i < SDOSelections.Count; i++)
            {
                if (SDOSelections[i] == true)
                {
                    indices.Add(i);
                    segmentCount++;
                }
            }

            float segmentWidth = 170f / segmentCount;

            Bitmap SDOPreview = new Bitmap(170, 170);

            using (Graphics g = Graphics.FromImage(SDOPreview))
            {
                GraphicsUnit units = GraphicsUnit.Pixel;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                float offset = 0;
                for (int i = 0; i < indices.Count(); i++)
                {
                    g.DrawImage(SDOIcons[indices[i]], new RectangleF(offset - 1, 0, segmentWidth + 1, 170), new RectangleF(offset - 1, 0, segmentWidth + 1, 170), units);
                    offset += segmentWidth;
                }

            }
            return SDOPreview;
        }

        private void DrawDisplays()
        {
            Image i = new Bitmap(pbScreensPreview.Width, pbScreensPreview.Height);

            using (Graphics g = Graphics.FromImage(i))
            {
                int minX = 0;
                int minY = 0;

                foreach (var screen in Screen.AllScreens)
                {
                    if (screen.WorkingArea.X < minX)
                        minX = screen.WorkingArea.X;
                    if (screen.WorkingArea.Y < minY)
                        minY = screen.WorkingArea.Y;
                }

                listBox1.Items.Add("minX:" + minX.ToString());
                listBox1.Items.Add("minY:" + minY.ToString());

                int screenNo = 1;

                foreach (var screen in Screen.AllScreens)
                {
                    // For each screen, add the screen properties to a list box.
                    listBox1.Items.Add("Device Name: " + screen.DeviceName);
                    listBox1.Items.Add("Bounds: " + screen.Bounds.ToString());
                    listBox1.Items.Add("Type: " + screen.GetType().ToString());
                    listBox1.Items.Add("Working Area: " + screen.WorkingArea.ToString());
                    listBox1.Items.Add("Primary Screen: " + screen.Primary.ToString());

                    decimal ratioX = (decimal)((decimal)i.Width / (decimal)SystemInformation.VirtualScreen.Width);
                    decimal ratioY = (decimal)((decimal)i.Height / (decimal)SystemInformation.VirtualScreen.Height);

                    decimal ratio = Math.Min(ratioX, ratioY);

                    float ScreenX = (float)((screen.WorkingArea.X - minX) * ratio);
                    float ScreenY = (float)((screen.WorkingArea.Y - minY) * ratio);
                    float ScreenW = (float)(screen.WorkingArea.Width * ratio);
                    float ScreenH = (float)(screen.WorkingArea.Height * ratio);

                    g.DrawImage(new Bitmap("Display.png"), ScreenX, ScreenY, ScreenW, ScreenH);

                    Font fNo = new Font(FontFamily.GenericSansSerif, (float)(200 * ratio), FontStyle.Bold);
                    SizeF NumberSize = g.MeasureString(screenNo.ToString(), fNo);

                    g.DrawString(screenNo.ToString(), fNo, Brushes.White, ScreenX + (ScreenW / 2) - (NumberSize.Width / 2), ScreenY + (ScreenH / 2) - (NumberSize.Height / 2));

                    screenNo++;
                }

            }

            pbScreensPreview.Image = i;

        }


        private void GetSDOWP()
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData("http://sdo.gsfc.nasa.gov/assets/img/latest/latest_2048_0304.jpg");

                using (MemoryStream mem = new MemoryStream(data))
                {
                    Bitmap SDO = new Bitmap(Image.FromStream(mem));
                    SDO.Save(Application.CommonAppDataPath + @"\SDO.jpg");
                }
            }
        }

        private void GetFancySDOWP()
        {
            string[] urls = new string[] {"latest_2048_0094.jpg", "latest_2048_0131.jpg", "latest_2048_0171.jpg", "latest_2048_0193.jpg", "latest_2048_0211.jpg", "latest_2048_0304.jpg"
									,"latest_2048_0335.jpg"}; //, "latest_2048_1600.jpg", "latest_2048_1700.jpg", "latest_2048_4500.jpg", "latest_2048_HMIIC.jpg", "latest_2048_HMIIF.jpg" };

            Random rnd = new Random();
            urls = urls.OrderBy(x => rnd.Next()).ToArray();

            int partwidth = 2048 / urls.Count();

            Bitmap SDO = new Bitmap(2048, 2048);

            using (Graphics g = Graphics.FromImage(SDO))
            {
                GraphicsUnit units = GraphicsUnit.Pixel;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                using (WebClient webClient = new WebClient())
                {
                    int offset = 0;
                    for (int i = 0; i < urls.Count(); i++)
                    {



                        string URL = urls[i];
                        byte[] data = webClient.DownloadData(baseURL + URL);

                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            Image ti = new Bitmap(Image.FromStream(mem));
                            g.DrawImage(ti, new Rectangle(offset - 1, 0, partwidth + 1, SDO.Height), new Rectangle(offset - 1, 0, partwidth + 1, SDO.Height), units);
                        }
                        offset += partwidth;
                    }
                }
            }

            SDO.Save(Application.CommonAppDataPath + @"\SDO.jpg");

            this.BackgroundImage = new Bitmap(SDO);
        }

        private static void CreateBackgroundImage(Dictionary<string, Image> imageFiles)
        {

            using (var virtualScreenBitmap = new Bitmap((int)SystemInformation.VirtualScreen.Width, (int)SystemInformation.VirtualScreen.Height))
            {
                using (var virtualScreenGraphic = Graphics.FromImage(virtualScreenBitmap))
                {
                    foreach (var screen in System.Windows.Forms.Screen.AllScreens)
                    {
                        var image = (imageFiles.ContainsKey(screen.DeviceName)) ? imageFiles[screen.DeviceName] : null;

                        var monitorDimensions = screen.Bounds;
                        var width = monitorDimensions.Width;
                        var monitorBitmap = new Bitmap(width, monitorDimensions.Height);
                        var fromImage = Graphics.FromImage(monitorBitmap);
                        fromImage.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                        fromImage.FillRectangle(SystemBrushes.Desktop, 0, 0, monitorBitmap.Width, monitorBitmap.Height);

                        if (image != null)
                            DrawImageCentered(fromImage, image, new Rectangle(0, 0, monitorBitmap.Width, monitorBitmap.Height));

                        Rectangle rectangle;
                        if (monitorDimensions.Top == 0 && monitorDimensions.Left == 0)
                        {
                            rectangle = monitorDimensions;
                        }
                        else
                        {
                            if ((monitorDimensions.Left < 0 && monitorDimensions.Width > -monitorDimensions.Left) ||
                                (monitorDimensions.Top < 0 && monitorDimensions.Height > -monitorDimensions.Top))
                            {
                                var isMain = (monitorDimensions.Top < 0 && monitorDimensions.Bottom > 0);

                                var left = (monitorDimensions.Left < 0)
                                            ? (int)SystemInformation.VirtualScreen.Width + monitorDimensions.Left
                                            : monitorDimensions.Left;

                                var top = (monitorDimensions.Top < 0)
                                            ? (int)SystemInformation.VirtualScreen.Height + monitorDimensions.Top
                                            : monitorDimensions.Top;

                                Rectangle srcRect;
                                if (isMain)
                                {
                                    rectangle = new Rectangle(left, 0, monitorDimensions.Width, monitorDimensions.Bottom);
                                    srcRect = new Rectangle(0, -monitorDimensions.Top, monitorDimensions.Width, monitorDimensions.Height + monitorDimensions.Top);
                                }
                                else
                                {
                                    rectangle = new Rectangle(0, top, monitorDimensions.Right, monitorDimensions.Height);
                                    srcRect = new Rectangle(-monitorDimensions.Left, 0, monitorDimensions.Width + monitorDimensions.Left,
                                                        monitorDimensions.Height);
                                }

                                virtualScreenGraphic.DrawImage(monitorBitmap, rectangle, srcRect, GraphicsUnit.Pixel);
                                rectangle = new Rectangle(left, top, monitorDimensions.Width, monitorDimensions.Height);
                            }
                            else
                            {
                                var left = (monitorDimensions.Left < 0)

                                            ? (int)SystemInformation.VirtualScreen.Height + monitorDimensions.Left
                                            : monitorDimensions.Left;
                                var top = (monitorDimensions.Top < 0)
                                            ? (int)SystemInformation.VirtualScreen.Width + monitorDimensions.Top
                                            : monitorDimensions.Top;
                                rectangle = new Rectangle(left, top, monitorDimensions.Width, monitorDimensions.Height);
                            }
                        }
                        virtualScreenGraphic.DrawImage(monitorBitmap, rectangle);
                    }

                    virtualScreenBitmap.Save(outputBackgroundFile, ImageFormat.Bmp);
                }
            }

            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallpaperStyle", 6);
            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "TileWallpaper", 0);
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0u, outputBackgroundFile, SPIF_UPDATEINIFILE);
        }


        private static void DrawImageCentered(Graphics g, Image img, Rectangle monitorRect)
        {
            float heightRatio = (float)monitorRect.Height / (float)img.Height;
            float widthRatio = (float)monitorRect.Width / (float)img.Width;
            int height = monitorRect.Height;
            int width = monitorRect.Width;
            int x = 0;
            int y = 0;

            if (heightRatio > 1f && widthRatio > 1f)
            {
                height = img.Height;
                width = img.Width;
                x = (int)((float)(monitorRect.Width - width) / 2f);
                y = (int)((float)(monitorRect.Height - height) / 2f);
            }
            else
            {
                if (heightRatio < widthRatio)
                {
                    width = (int)((float)img.Width * heightRatio);
                    height = (int)((float)img.Height * heightRatio);
                    x = (int)((float)(monitorRect.Width - width) / 2f);
                }
                else
                {
                    width = (int)((float)img.Width * widthRatio);
                    height = (int)((float)img.Height * widthRatio);
                    y = (int)((float)(monitorRect.Height - height) / 2f);
                }
            }

            Rectangle rect = new Rectangle(x, y, width, height);
            g.DrawImage(img, rect);
        }

        private void pbStatic_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.ShowDialog();

            pbStatic.Image = Bitmap.FromFile(ofd.FileName);
        }

        private void pbSDO1_Click(object sender, EventArgs e)
        {
            PictureBox s = (PictureBox)sender;
            int index = SDONames.IndexOf(s.Name);
            if (SDOSelections[index] == true)
            {
                s.BackgroundImage = SDOIconsGrey[index];
                SDOSelections[index] = false;
            }
            else
            {
                s.BackgroundImage = SDOIcons[index];
                SDOSelections[index] = true;
            }
        }

        private void btnSDOPreview_Click(object sender, EventArgs e)
        {
            pbScreensPreview.Image = SDOPreviewImage();
        }
    }
}