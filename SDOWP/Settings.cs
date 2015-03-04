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
		[Serializable]
		public List<ScreenSetting> ScreenSettings { get; set; }
	}

	[Serializable]
	public class ScreenSetting
	{
		[Serializable]
		public string ScreenDeviceName { get; set; }
		[Serializable]
		public SDOSetting SDOSettings { get; set; }
	}

	[Serializable]
	public class SDOSetting
	{
		[Serializable]
		public List<bool> SDOSelections { get; set; }
		[Serializable]
		public bool Sliced { get; set; }
		[Serializable]
		public bool SlideShow { get; set; }
		[Serializable]
		public bool Random { get; set; }
	}
}
