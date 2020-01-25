using System;

namespace UnityEngine.UI.Translation
{
	internal class FontShadowChangedEventArgs : EventArgs
	{
		public int ShadowOffset
		{
			get
			{
				return this._ShadowOffset;
			}
		}

		public FontShadowChangedEventArgs(int shadowOffset)
		{
			this._ShadowOffset = shadowOffset;
		}

		private readonly int _ShadowOffset;
	}
}
