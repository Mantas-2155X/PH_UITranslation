using System;

namespace UnityEngine.UI.Translation
{
	internal class FontBorderChangedEventArgs : EventArgs
	{
		public int BorderWidth
		{
			get
			{
				return this._BorderWidth;
			}
		}

		public FontBorderChangedEventArgs(int borderWidth)
		{
			this._BorderWidth = borderWidth;
		}

		private readonly int _BorderWidth;
	}
}
