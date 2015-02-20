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

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			string path = string.Empty;
			//GetFancySDOWP();

			DrawDisplays();
			
			var images = new Dictionary<string, Image>
			{
				{ @"\\.\DISPLAY1", Image.FromFile(@"C:\Users\russell.bruechert\Pictures\Jupiter.png") },
				{ @"\\.\DISPLAY2", Image.FromFile(Application.CommonAppDataPath + @"\SDO.jpg") },
				//{ @"\\.\DISPLAY3", Image.FromFile(@"C:\Users\russell.bruechert\Pictures\Jupiter.png") },
			};

			CreateBackgroundImage(images);

		}

		private void DrawDisplays()
		{
			Image i = new Bitmap(pictureBox1.Width, pictureBox1.Height);

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

				foreach (var screen in Screen.AllScreens)
				{
					// For each screen, add the screen properties to a list box.
					listBox1.Items.Add("Device Name: " + screen.DeviceName);
					listBox1.Items.Add("Bounds: " + screen.Bounds.ToString());
					listBox1.Items.Add("Type: " + screen.GetType().ToString());
					listBox1.Items.Add("Working Area: " + screen.WorkingArea.ToString());
					listBox1.Items.Add("Primary Screen: " + screen.Primary.ToString());
					decimal ratioX = (decimal)((decimal)i.Width / (decimal)SystemInformation.VirtualScreen.Width) * (decimal)0.9;
					decimal ratioY = (decimal)((decimal)i.Height / (decimal)SystemInformation.VirtualScreen.Height) * (decimal)0.9;
					
					decimal ratio = Math.Min(ratioX, ratioY);

					g.DrawImage(new Bitmap("Display.png"), (float)((screen.WorkingArea.X - minX) * ratio), (float)((screen.WorkingArea.Y - minY) * ratio), (float)(screen.WorkingArea.Width * ratio), (float)(screen.WorkingArea.Height * ratio));
					g.DrawString(screen.DeviceName, SystemFonts.IconTitleFont, Brushes.White, (float)((screen.WorkingArea.X - minX) * ratio), (float)((screen.WorkingArea.Y - minY) * ratio));
					//g.DrawRectangle(Pens.Black, (float)(screen.WorkingArea.X * ratio), (float)(screen.WorkingArea.Y * ratio), (float)(screen.WorkingArea.Width * ratio), (float)(screen.WorkingArea.Height * ratio));
				}

			}

			pictureBox1.Image = i;

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
			string baseURL = "http://sdo.gsfc.nasa.gov/assets/img/latest/";
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
							g.DrawImage(ti, new Rectangle(offset-1, 0, partwidth+1, SDO.Height), new Rectangle(offset-1, 0, partwidth+1, SDO.Height), units);
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
	}
}
