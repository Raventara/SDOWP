using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using System.IO;
namespace SDOWP
{
    public enum WallPaperType
    {
        SDOWallpaper,
        StaticWallpaper
    };

	public static class Settings
	{
        public static ScreenSettings AllScreenSettings { get { return _AllScreenSettings; } set { _AllScreenSettings = value; } }
        private static ScreenSettings _AllScreenSettings = new ScreenSettings();

        public static bool SaveSettings(string pFilename)
        {
            if (_AllScreenSettings != null)
            {
                // Do save
                using (TextWriter w = new StreamWriter(pFilename))
                {
                    JsonSerializer.SerializeToWriter(_AllScreenSettings, w);
                    return true;
                }
            }
         
            return false;
        }

        public static bool LoadSettings(string pFilename)
        {
			using (TextReader r = new StreamReader(pFilename))
			{
				_AllScreenSettings = JsonSerializer.DeserializeFromReader<ScreenSettings>(r);
				return false;
			}
        }
	}

    public class ScreenSettings
    {
        public List<ScreenSetting> Settings { get; set; }
        public ScreenSettings() { Settings = new List<ScreenSetting>(); }
    }

	public class ScreenSetting
	{
		public string ScreenDeviceName { get; set; }
        public WallPaperType ScreenWallpaperType { get; set; }
        public string StaticWallpaperFilename { get; set; }
		public SDOSetting SDOSettings { get; set; }
	}

	public class SDOSetting
	{
		public List<bool> SDOSelections { get; set; }
		public bool Sliced { get; set; }
		public bool SlideShow { get; set; }
		public bool Random { get; set; }
	}
}
