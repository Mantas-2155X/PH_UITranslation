using System;
using System.Globalization;

namespace UnityEngine.UI.Translation
{
	internal static class SubtitleSettings
	{
		public static string ToHex(Color32 color)
		{
			return "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}

		public static Color32 ToColor(string hex)
		{
			if (string.IsNullOrEmpty(hex) || !hex.StartsWith("#") || hex.Length != 7)
			{
				return Color.white;
			}
			byte r = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
			return new Color32(r, g, b, byte.MaxValue);
		}
		
		internal static event Action<bool> EnabledChanged;

		internal static bool Enabled
		{
			get
			{
				return SubtitleSettings._Enabled;
			}
			private set
			{
				if (value != SubtitleSettings._Enabled)
				{
					SubtitleSettings._Enabled = value;
					Action<bool> enabledChanged = SubtitleSettings.EnabledChanged;
					if (enabledChanged != null && SubtitleSettings.initialized)
					{
						enabledChanged(value);
					}
				}
			}
		}

		internal static event Action<TextPosition> AnchorChanged;

		internal static TextPosition Anchor
		{
			get
			{
				return SubtitleSettings._Anchor;
			}
			private set
			{
				if (value != SubtitleSettings._Anchor)
				{
					SubtitleSettings._Anchor = value;
					Action<TextPosition> anchorChanged = SubtitleSettings.AnchorChanged;
					if (anchorChanged != null && SubtitleSettings.initialized)
					{
						anchorChanged(value);
					}
				}
			}
		}

		internal static event Action<string> FontNameChanged;

		internal static string FontName
		{
			get
			{
				if (string.IsNullOrEmpty(SubtitleSettings._FontName))
				{
					SubtitleSettings._FontName = "Arial";
				}
				return SubtitleSettings._FontName;
			}
			private set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = "Arial";
				}
				if (value != SubtitleSettings._FontName)
				{
					SubtitleSettings._FontName = value;
					Action<string> fontNameChanged = SubtitleSettings.FontNameChanged;
					if (fontNameChanged != null && SubtitleSettings.initialized)
					{
						fontNameChanged(value);
					}
				}
			}
		}

		internal static event Action<int> FontSizeChanged;

		internal static int FontSize
		{
			get
			{
				return SubtitleSettings._FontSize;
			}
			private set
			{
				if (value != SubtitleSettings._FontSize)
				{
					SubtitleSettings._FontSize = value;
					Action<int> fontSizeChanged = SubtitleSettings.FontSizeChanged;
					if (fontSizeChanged != null && SubtitleSettings.initialized)
					{
						fontSizeChanged(value);
					}
				}
			}
		}

		internal static event Action<Color> FontColorChanged;

		internal static Color FontColor
		{
			get
			{
				return SubtitleSettings._FontColor;
			}
			private set
			{
				if (value != SubtitleSettings._FontColor)
				{
					SubtitleSettings._FontColor = value;
					Action<Color> fontColorChanged = SubtitleSettings.FontColorChanged;
					if (fontColorChanged != null && SubtitleSettings.initialized)
					{
						fontColorChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> BoldChanged;

		internal static bool Bold
		{
			get
			{
				return SubtitleSettings._Bold;
			}
			private set
			{
				if (value != SubtitleSettings._Bold)
				{
					SubtitleSettings._Bold = value;
					Action<bool> boldChanged = SubtitleSettings.BoldChanged;
					if (boldChanged != null && SubtitleSettings.initialized)
					{
						boldChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> ItalicChanged;

		internal static bool Italic
		{
			get
			{
				return SubtitleSettings._Italic;
			}
			private set
			{
				if (value != SubtitleSettings._Italic)
				{
					SubtitleSettings._Italic = value;
					Action<bool> italicChanged = SubtitleSettings.ItalicChanged;
					if (italicChanged != null && SubtitleSettings.initialized)
					{
						italicChanged(value);
					}
				}
			}
		}

		internal static event Action<int> BorderWidthChanged;

		internal static int BorderWidth
		{
			get
			{
				return SubtitleSettings._BorderWidth;
			}
			private set
			{
				if (value != SubtitleSettings._BorderWidth)
				{
					SubtitleSettings._BorderWidth = value;
					Action<int> borderWidthChanged = SubtitleSettings.BorderWidthChanged;
					if (borderWidthChanged != null && SubtitleSettings.initialized)
					{
						borderWidthChanged(value);
					}
				}
			}
		}

		internal static event Action<int> ShadowOffsetChanged;

		internal static int ShadowOffset
		{
			get
			{
				return SubtitleSettings._ShadowOffset;
			}
			private set
			{
				if (value != SubtitleSettings._ShadowOffset)
				{
					SubtitleSettings._ShadowOffset = value;
					Action<int> shadowOffsetChanged = SubtitleSettings.ShadowOffsetChanged;
					if (shadowOffsetChanged != null && SubtitleSettings.initialized)
					{
						shadowOffsetChanged(value);
					}
				}
			}
		}

		internal static event Action<int> MarginLeftChanged;

		internal static int MarginLeft
		{
			get
			{
				return SubtitleSettings._MarginLeft;
			}
			private set
			{
				if (value != SubtitleSettings._MarginLeft)
				{
					SubtitleSettings._MarginLeft = value;
					Action<int> marginLeftChanged = SubtitleSettings.MarginLeftChanged;
					if (marginLeftChanged != null && SubtitleSettings.initialized)
					{
						marginLeftChanged(value);
					}
				}
			}
		}

		internal static event Action<int> MarginTopChanged;

		internal static int MarginTop
		{
			get
			{
				return SubtitleSettings._MarginTop;
			}
			private set
			{
				if (value != SubtitleSettings._MarginTop)
				{
					SubtitleSettings._MarginTop = value;
					Action<int> marginTopChanged = SubtitleSettings.MarginTopChanged;
					if (marginTopChanged != null && SubtitleSettings.initialized)
					{
						marginTopChanged(value);
					}
				}
			}
		}

		internal static event Action<int> MarginRightChanged;

		internal static int MarginRight
		{
			get
			{
				return SubtitleSettings._MarginRight;
			}
			private set
			{
				if (value != SubtitleSettings._MarginRight)
				{
					SubtitleSettings._MarginRight = value;
					Action<int> marginRightChanged = SubtitleSettings.MarginRightChanged;
					if (marginRightChanged != null && SubtitleSettings.initialized)
					{
						marginRightChanged(value);
					}
				}
			}
		}

		internal static event Action<int> MarginBottomChanged;

		internal static int MarginBottom
		{
			get
			{
				return SubtitleSettings._MarginBottom;
			}
			private set
			{
				if (value != SubtitleSettings._MarginBottom)
				{
					SubtitleSettings._MarginBottom = value;
					Action<int> marginBottomChanged = SubtitleSettings.MarginBottomChanged;
					if (marginBottomChanged != null && SubtitleSettings.initialized)
					{
						marginBottomChanged(value);
					}
				}
			}
		}

		static SubtitleSettings()
		{
			IniSettings.LoadSettings += SubtitleSettings.Load;
			IniSettings.Load();
		}

		private static void Load(IniFile ini)
		{
			string key = "bEnabled";
			string value = ini.GetValue("Subtitles", key, null);
			bool flag;
			if (value == null || !bool.TryParse(value, out flag))
			{
				flag = true;
				ini.WriteValue("Subtitles", key, flag);
			}
			SubtitleSettings.Enabled = flag;
			key = "iAnchor";
			value = ini.GetValue("Subtitles", key, null);
			int num;
			if (value == null || !int.TryParse(value, out num))
			{
				num = 2;
				ini.WriteValue("Subtitles", key, num);
			}
			SubtitleSettings.Anchor = SubtitleSettings.Anchor.Parse(num.ToString(), SubtitleSettings.Anchor);
			key = "sFontName";
			value = ini.GetValue("Subtitles", key, null);
			SubtitleSettings.FontName = value;
			if (value != SubtitleSettings.FontName)
			{
				ini.WriteValue("Subtitles", key, SubtitleSettings.FontName);
			}
			key = "iFontSize";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !int.TryParse(value, out num))
			{
				num = 16;
				ini.WriteValue("Subtitles", key, num);
			}
			SubtitleSettings.FontSize = num;
			key = "sFontColor";
			value = ini.GetValue("Subtitles", key, null);
			SubtitleSettings.FontColor = ToColor(value);
			string text = ToHex(SubtitleSettings.FontColor);
			if (value != text)
			{
				ini.WriteValue("Subtitles", key, text);
			}
			key = "bBold";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !bool.TryParse(value, out flag))
			{
				flag = true;
				ini.WriteValue("Subtitles", key, flag);
			}
			SubtitleSettings.Bold = flag;
			key = "bItalic";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !bool.TryParse(value, out flag))
			{
				flag = false;
				ini.WriteValue("Subtitles", key, flag);
			}
			SubtitleSettings.Italic = flag;
			key = "iBorderWidth";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !int.TryParse(value, out num))
			{
				num = 2;
				ini.WriteValue("Subtitles", key, num);
			}
			SubtitleSettings.BorderWidth = num;
			key = "iShadowOffset";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !int.TryParse(value, out num))
			{
				num = 3;
				ini.WriteValue("Subtitles", key, num);
			}
			SubtitleSettings.ShadowOffset = num;
			key = "iMarginLeft";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !int.TryParse(value, out num))
			{
				num = 20;
				ini.WriteValue("Subtitles", key, num);
			}
			SubtitleSettings.MarginLeft = num;
			key = "iMarginTop";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !int.TryParse(value, out num))
			{
				num = 20;
				ini.WriteValue("Subtitles", key, num);
			}
			SubtitleSettings.MarginTop = num;
			key = "iMarginRight";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !int.TryParse(value, out num))
			{
				num = 20;
				ini.WriteValue("Subtitles", key, num);
			}
			SubtitleSettings.MarginRight = num;
			key = "iMarginBottom";
			value = ini.GetValue("Subtitles", key, null);
			if (value == null || !int.TryParse(value, out num))
			{
				num = 20;
				ini.WriteValue("Subtitles", key, num);
			}
			SubtitleSettings.MarginBottom = num;
			SubtitleSettings.initialized = true;
		}

		private const string SECTION = "Subtitles";

		private const string ENABLEDKEY = "bEnabled";

		private const string ANCHORKEY = "iAnchor";

		private const string FONTNAMEKEY = "sFontName";

		private const string FONTSIZEKEY = "iFontSize";

		private const string FONTCOLORKEY = "sFontColor";

		private const string BOLDKEY = "bBold";

		private const string ITALICKEY = "bItalic";

		private const string BORDERWIDTHKEY = "iBorderWidth";

		private const string SHADOWOFFSETKEY = "iShadowOffset";

		private const string MARGINLEFTKEY = "iMarginLeft";

		private const string MARGINTOPKEY = "iMarginTop";

		private const string MARGINRIGHTKEY = "iMarginRight";

		private const string MARGINBOTTOMKEY = "iMarginBottom";

		private static bool _Enabled;

		private static TextPosition _Anchor;

		private static string _FontName;

		private static int _FontSize;

		private static Color _FontColor;

		private static bool _Bold;

		private static bool _Italic;

		private static int _BorderWidth;

		private static int _ShadowOffset;

		private static int _MarginLeft;

		private static int _MarginTop;

		private static int _MarginRight;

		private static int _MarginBottom;

		private static bool initialized;
	}
}
