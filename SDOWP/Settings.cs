using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDOW_P
{
	[Serializable]
	class Settings
	{
		public List<ScreenSetting> ScreenSettings { get; set; }
	}

	[Serializable]
	public class ScreenSetting
	{
		public string ScreenDeviceName { get; set; }
		public SDOSetting SDOSettings { get; set; }
	}

	[Serializable]
	public class SDOSetting
	{
		public List<bool> SDOSelections { get; set; }
		public bool Sliced { get; set; }
		public bool SlideShow { get; set; }
		public bool Random { get; set; }
	}
}
