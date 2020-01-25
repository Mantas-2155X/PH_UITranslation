using System;

namespace UnityEngine.UI.Translation
{
	internal class FontColorChangedEventArgs : EventArgs
	{
		public Color FontColor
		{
			get
			{
				return this._FontColor;
			}
		}

		public FontColorChangedEventArgs(Color color)
		{
			this._FontColor = color;
		}

		private readonly Color _FontColor;
	}
}
