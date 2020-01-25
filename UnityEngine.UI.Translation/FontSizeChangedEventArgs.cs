using System;

namespace UnityEngine.UI.Translation
{
	internal class FontSizeChangedEventArgs : EventArgs
	{
		public int FontSize
		{
			get
			{
				return this._FontSize;
			}
		}

		public FontSizeChangedEventArgs(int size)
		{
			this._FontSize = size;
		}

		private readonly int _FontSize;
	}
}
