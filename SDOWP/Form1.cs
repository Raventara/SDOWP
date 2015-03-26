using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDOWP
{
	public partial class Form1 : Form
	{
		private Dictionary<string, ScreenSetting> ScreenSettings = new Dictionary<string, ScreenSetting>();
		private List<ScreenSetting> ScreenSettings_OTHER = new List<ScreenSetting>();

		private string StaticFileName = string.Empty;

		private int LastSDOIndex = 0;

		//Filter kernel for mild smoothing of downloaded SDO images
		private int[,] kernel = new int[3, 3] { { 5, 4, 5 }, { 4, 100, 4 }, { 5, 4, 5 } };

		private const string outputWallpaperFile = @"C:\Users\Public\Pictures\SDOWallpaper.bmp";

		private string fSelectedScreen = string.Empty;

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

		private const uint SPI_SETDESKWALLPAPER = 20;
		private const uint SPIF_UPDATEINIFILE = 0x01;
		private const uint SPIF_SENDWININICHANGE = 0x02;

		private const string baseURL = "http://sdo.gsfc.nasa.gov/assets/img/latest/";

		private Image SDOPreviewImage;

		public Form1()
		{
			InitializeComponent();
		}

		private async Task<Bitmap> GetRemoteImage(string pUrl)
		{
			Bitmap RemoteImage;
			using (var client = new WebClient())
			{
				var data = await client.DownloadDataTaskAsync(new Uri(pUrl));

				using (MemoryStream mem = new MemoryStream(data))
				{
					RemoteImage = new Bitmap(Image.FromStream(mem));
				}
			}

			return RemoteImage;
		}

		private async void SetToRemoteImage(ToggleImageButton pTarget, string pUrl)
		{
			var sizemode = pTarget.SizeMode;
			var backcolour = pTarget.BackColor;
			pTarget.BackColor = Color.White;
			pTarget.SizeMode = PictureBoxSizeMode.CenterImage;
			pTarget.Image = Image.FromFile("wait.gif");
			this.Cursor = Cursors.AppStarting;
			Bitmap HTML = await GetRemoteImage(pUrl);
			this.Cursor = Cursors.Arrow;
			pTarget.SetImage(HTML);
			pTarget.BackColor = backcolour;
			pTarget.SizeMode = sizemode;

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			LoadSDOThumbnails();
			LoadDisplays();


			//string path = string.Empty;
			//GetFancySDOWP();

			DrawDisplays();

			//var images = new Dictionary<string, Image>
			//{
			//    //{ @"\\.\DISPLAY1", Image.FromFile(@"C:\Users\russell.bruechert\Pictures\Jupiter.png") },
			//    { @"\\.\DISPLAY1", Image.FromFile(Application.CommonAppDataPath + @"\SDO.jpg") },
			//    //{ @"\\.\DISPLAY3", Image.FromFile(@"C:\Users\russell.bruechert\Pictures\Jupiter.png") },
			//};

			//CreateFullWallperImageForAllMonitors(images);

		}

		private void LoadSDOThumbnails()
		{
			string[] urls = new string[] {"latest_170_0094.jpg", "latest_170_0131.jpg", "latest_170_0171.jpg", "latest_170_0193.jpg", "latest_170_0211.jpg", "latest_170_0304.jpg"
									,"latest_170_0335.jpg"}; //, "latest_170_1600.jpg", "latest_170_1700.jpg", "latest_170_4500.jpg", "latest_170_HMIIC.jpg", "latest_170_HMIIF.jpg" };

			int x = 0;
			foreach (PictureBox control in tpSDO.Controls.Cast<Control>().OrderBy(c => c.Name).Where(c => c.GetType() == typeof(ToggleImageButton)))
			{
				SetToRemoteImage((ToggleImageButton)control, baseURL + urls[x]);
				x++;
			}
		}

		/// <summary>
		/// Generates the image from the supplied list of images as either a slide show or a sliced together image
		/// </summary>
		/// <param name="pImages">The list of images to generate the output image from</param>
		/// <returns></returns>
		private Bitmap GenerateSDOImage(List<Image> pImages)
		{
			int segmentCount = pImages.Count;


			int w = pImages[0].Width;
			int h = pImages[0].Height;
			float segmentWidth = w / segmentCount;

			Random rnd = new Random();

			Bitmap result = new Bitmap(w, h);

			if (cbRandom.Checked)
				pImages = pImages.OrderBy(c => rnd.Next()).ToList();

			if (rbSliced.Checked)
			{   // Sliced images
				using (Graphics g = Graphics.FromImage(result))
				{
					GraphicsUnit units = GraphicsUnit.Pixel;
					g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

					float offset = 0;
					foreach (var i in pImages)
					{
						g.DrawImage(i, new RectangleF(offset - 1, 0, segmentWidth + 1, w), new RectangleF(offset - 1, 0, segmentWidth + 1, h), units);
						offset += segmentWidth;
					}
				}
			}
			else
			{   //Slide Show
				using (Graphics g = Graphics.FromImage(result))
				{
					g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

					int rndIndex = rnd.Next(pImages.Count);

					g.DrawImage(pImages[cbRandom.Checked ? rndIndex : LastSDOIndex], new RectangleF(0, 0, w, h));
					LastSDOIndex = (LastSDOIndex + 1) % pImages.Count;
				}
			}

			return result;
		}

		public void LoadDisplays()
		{
			foreach (var screen in Screen.AllScreens)
			{
				cboScreens.Items.Add(screen.DeviceName);
				ScreenSettings.Add(screen.DeviceName, new ScreenSetting() { ScreenDeviceName = screen.DeviceName });
			}
			cboScreens.SelectedIndex = 0;
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

				int screenNo = 1;

				foreach (var screen in Screen.AllScreens)
				{
					float ratioX = (float)((float)i.Width / (float)SystemInformation.VirtualScreen.Width);
					float ratioY = (float)((float)i.Height / (float)SystemInformation.VirtualScreen.Height);

					float ratio = Math.Min(ratioX, ratioY) * 0.999f;

					float ScreenX = (float)((screen.WorkingArea.X - minX) * ratio);
					if (ScreenX > 0) { ScreenX++; }
					float ScreenY = (float)((screen.WorkingArea.Y - minY) * ratio);
					if (ScreenY > 0) { ScreenY++; }
					float ScreenW = (float)(screen.WorkingArea.Width * ratio);
					float ScreenH = (float)(screen.WorkingArea.Height * ratio);

					float SDOsize = Math.Min(ScreenW, ScreenH);

					if (SDOPreviewImage != null && screenNo == 1)
					{
						g.FillRectangle(Brushes.Black, ScreenX, ScreenY, ScreenW, ScreenH);
						g.DrawImage(SDOPreviewImage, new RectangleF((ScreenW / 2) - (SDOsize / 2), (ScreenH / 2) - (SDOsize / 2), SDOsize, SDOsize));
						g.DrawRectangle(Pens.GreenYellow, ScreenX, ScreenY, ScreenW, ScreenH);
					}
					else
					{
						g.DrawImage(new Bitmap("Display.png"), ScreenX, ScreenY, ScreenW, ScreenH);

					}

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

		private void GetFancySDOWP(List<bool> pSelections)
		{
			List<Image> SDOImages = new List<Image>();
			IFilter filter = new Convolution(kernel);

			string[] urls = new string[] {"latest_2048_0094.jpg", "latest_2048_0131.jpg", "latest_2048_0171.jpg", "latest_2048_0193.jpg", "latest_2048_0211.jpg", "latest_2048_0304.jpg"
									,"latest_2048_0335.jpg"}; //, "latest_2048_1600.jpg", "latest_2048_1700.jpg", "latest_2048_4500.jpg", "latest_2048_HMIIC.jpg", "latest_2048_HMIIF.jpg" };
			int i = 0;
			foreach (string url in urls)
			{
				if (pSelections[i])
				{
					using (WebClient webClient = new WebClient())
					{

						byte[] data = webClient.DownloadData(baseURL + url);

						using (MemoryStream mem = new MemoryStream(data))
						{
							Image ti = new Bitmap(Image.FromStream(mem));
							SDOImages.Add(ti);
						}
					}
				}

				i++;
			}

			Bitmap SDO = GenerateSDOImage(SDOImages);


			//Random rnd = new Random();
			//urls = urls.OrderBy(x => rnd.Next()).ToArray();

			//int partwidth = 2048 / urls.Count();

			//Bitmap SDO = new Bitmap(2048, 2048);

			//using (Graphics g = Graphics.FromImage(SDO))
			//{
			//	GraphicsUnit units = GraphicsUnit.Pixel;
			//	g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			//	using (WebClient webClient = new WebClient())
			//	{
			//		int offset = 0;
			//		for (int i = 0; i < urls.Count(); i++)
			//		{



			//			string URL = urls[i];
			//			byte[] data = webClient.DownloadData(baseURL + URL);

			//			using (MemoryStream mem = new MemoryStream(data))
			//			{
			//				Image ti = new Bitmap(Image.FromStream(mem));
			//				g.DrawImage(ti, new Rectangle(offset - 1, 0, partwidth + 1, SDO.Height), new Rectangle(offset - 1, 0, partwidth + 1, SDO.Height), units);
			//			}
			//			offset += partwidth;
			//		}
			//	}
			//}


			SDO = filter.Apply(SDO);

			this.BackgroundImage = SDO;

			if (File.Exists("SDO.png"))
				File.Delete("SDO.png");

			SDO.Save("SDO.png"); //Application.CommonAppDataPath + @"\SDO.jpg");

			////System.Diagnostics.Process.Start("explorer.exe", Application.CommonAppDataPath);

			//this.BackgroundImage = new Bitmap(SDO);
		}

		private static void CreateFullWallperImageForAllMonitors(Dictionary<string, Image> imageFiles)
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

					virtualScreenBitmap.Save(outputWallpaperFile, ImageFormat.Bmp);
				}
			}

			Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WallpaperStyle", 6);
			Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "TileWallpaper", 0);
			SystemParametersInfo(SPI_SETDESKWALLPAPER, 0u, outputWallpaperFile, SPIF_UPDATEINIFILE);
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

		private void btnSDOPreview_Click(object sender, EventArgs e)
		{
			List<Image> SDOPreviewImages = new List<Image>();

			foreach (var item in tpSDO.Controls.Cast<Control>().OrderBy(c => c.Name).Where(c => c.GetType() == typeof(ToggleImageButton) && ((ToggleImageButton)c).Selected == true).ToList())
			{
				SDOPreviewImages.Add(((ToggleImageButton)item).Image);
			}

			SDOPreviewImage = GenerateSDOImage(SDOPreviewImages);
			DrawDisplays();
		}

		private bool FirstLoad = true;
		private void cbScreens_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!FirstLoad)
				SaveSettings();
			else
				LoadSettings();

			FirstLoad = false;

			LoadScreen(cboScreens.SelectedItem.ToString());
			fSelectedScreen = cboScreens.SelectedItem.ToString();
			LoadScreenSettings();
		}

		private void LoadScreen(string p)
		{
			if (p != string.Empty)
			{
				lbScreenInfo.Items.Clear();
				if (Screen.AllScreens.First(c => c.DeviceName == p).Primary)
					lbScreenInfo.Items.Add("Primary Monitor");
				else
					lbScreenInfo.Items.Add("Secondary Monitor");

				lbScreenInfo.Items.Add("Resolution: " + Screen.AllScreens.First(c => c.DeviceName == p).WorkingArea.Width.ToString() + " X " + Screen.AllScreens.First(c => c.DeviceName == p).WorkingArea.Height.ToString());
			}
		}

		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void SaveSettings()
		{
			ScreenSetting SC = ScreenSettings[fSelectedScreen]; // Settings.AllScreenSettings.Settings.Find(x => x.ScreenDeviceName == LastScreenName);

			if (SC == null)
				SC = new ScreenSetting();

			SC.ScreenDeviceName = cboScreens.SelectedItem.ToString();

			SDOSetting SDOs = new SDOSetting();

			SDOs.Random = cbRandom.Checked;
			SDOs.SDOSelections = GetSDOImageSelections();
			SDOs.Sliced = rbSliced.Checked;
			SDOs.SlideShow = rbSlideShow.Checked;

			SC.SDOSettings = SDOs;

			SC.StaticWallpaperFilename = txtStaticFilename.Text;

			if (cbUseSDO.Checked)
				SC.ScreenWallpaperType = WallPaperType.SDOWallpaper;
			else
				SC.ScreenWallpaperType = WallPaperType.StaticWallpaper;

			ScreenSettings[fSelectedScreen] = SC;

			Settings.AllScreenSettings.Settings = ScreenSettings;
			Settings.SaveSettings(".settings");
		}

		private List<bool> GetSDOImageSelections()
		{
			List<bool> results = new List<bool>();
			foreach (PictureBox control in tpSDO.Controls.Cast<Control>().OrderBy(c => c.Name).Where(c => c.GetType() == typeof(ToggleImageButton)))
			{
				if (((ToggleImageButton)control).Selected)
					results.Add(true);
				else
					results.Add(false);

			}
			return results;
		}

		private void btnLoadSettings_Click(object sender, EventArgs e)
		{
			LoadSettings();
			LoadScreenSettings();
		}

		private void LoadSettings()
		{
			Settings.LoadSettings(".settings");

			if (Settings.AllScreenSettings.Settings.Count() > 0)
			{
				foreach (var ScreenSetting in Settings.AllScreenSettings.Settings)
				{
					if (ScreenSettings.ContainsKey(ScreenSetting.Key))
						ScreenSettings[ScreenSetting.Key] = ScreenSetting.Value;
					else
					{	//TODO: SAVE SCREENSETTINGS FOR SCREENS THAT DO NOT EXISTS THESE SHOULD BE LOADED IF THE SCREEN COMES BACK ONLINE??
						ScreenSettings_OTHER.Add(ScreenSetting.Value);
					}
				}

			}
		}

		private void LoadScreenSettings()
		{
			ScreenSetting S = ScreenSettings[fSelectedScreen];

			if (S.SDOSettings != null)
			{
				cbRandom.Checked = S.SDOSettings.Random;

				if (S.ScreenWallpaperType == WallPaperType.SDOWallpaper)
					cbUseSDO.Checked = true;
				else
					cbUseStatic.Checked = true;

				rbSliced.Checked = S.SDOSettings.Sliced;
				rbSlideShow.Checked = S.SDOSettings.SlideShow;

				txtStaticFilename.Text = S.StaticWallpaperFilename;

				if (File.Exists(S.StaticWallpaperFilename))
					pbStaticPreview.Image = Image.FromFile(S.StaticWallpaperFilename);

				int i = 0;
				foreach (PictureBox control in tpSDO.Controls.Cast<Control>().OrderBy(c => c.Name).Where(c => c.GetType() == typeof(ToggleImageButton)))
				{
					((ToggleImageButton)control).Selected = S.SDOSettings.SDOSelections[i];
					i++;
				}
			}

		}

		private void cbUseStatic_CheckedChanged(object sender, EventArgs e)
		{
			cbUseSDO.Checked = !cbUseStatic.Checked;
		}

		private void cbUseSDO_CheckedChanged(object sender, EventArgs e)
		{
			cbUseStatic.Checked = !cbUseSDO.Checked;
		}

		private void txtStaticFilename_Click(object sender, EventArgs e)
		{
			OpenFileDialog OFD = new OpenFileDialog();
			OFD.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
			OFD.Multiselect = false;
			if (OFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				StaticFileName = OFD.FileName;
				txtStaticFilename.Text = StaticFileName;
				pbStaticPreview.Image = Image.FromFile(OFD.FileName);
			}
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			SaveSettings();
			var images = new Dictionary<string, Image>();

			foreach (var item in ScreenSettings)
			{
				//ScreenSetting S = Settings.AllScreenSettings.Settings.Find(x => x.ScreenDeviceName == item.ToString());
				if (item.Value.ScreenWallpaperType == WallPaperType.StaticWallpaper)
					images.Add(item.Key, Image.FromFile(item.Value.StaticWallpaperFilename));
				else
				{
					GetFancySDOWP(item.Value.SDOSettings.SDOSelections);
					images.Add(item.Key, Image.FromFile("SDO.png"));
				}
			}

			CreateFullWallperImageForAllMonitors(images);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{

			if (ScreenSettings.ContainsKey("\\\\.\\DISPLAY1"))
			{
				ScreenSetting S1 = ScreenSettings["\\\\.\\DISPLAY1"];
				textBox1.Text = @"\\\\.\\DISPLAY1\r\n" + S1.ScreenWallpaperType.ToString();
			}
			else textBox1.Text = "";

			if (ScreenSettings.ContainsKey("\\\\.\\DISPLAY2"))
			{
				ScreenSetting S2 = ScreenSettings["\\\\.\\DISPLAY2"];
				textBox2.Text = @"\\\\.\\DISPLAY2\r\n" + S2.ScreenWallpaperType.ToString();
			}
			else textBox2.Text = "";
		}



	}
}