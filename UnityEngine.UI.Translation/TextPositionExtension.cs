using System;

namespace UnityEngine.UI.Translation
{
	internal static class TextPositionExtension
	{
		public static TextPosition Parse(this TextPosition tp, string value, TextPosition defaultValue = TextPosition.LowerCenter)
		{
			TextPosition textPosition = (TextPosition)0;
			try
			{
				textPosition = (TextPosition)Enum.Parse(typeof(TextPosition), value, true);
			}
			catch
			{
			}
			if (!Enum.IsDefined(typeof(TextPosition), textPosition))
			{
				textPosition = defaultValue;
			}
			return textPosition;
		}
	}
}
