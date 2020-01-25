using System;

namespace UnityEngine.UI.Translation
{
	internal class FontStyleChangedEventArgs : EventArgs
	{
		public bool Bold
		{
			get
			{
				return this._Bold;
			}
		}

		public bool Italic
		{
			get
			{
				return this._Italic;
			}
		}

		public FontStyleChangedEventArgs(bool bold, bool italic)
		{
			this._Bold = bold;
			this._Italic = italic;
		}

		private readonly bool _Bold;

		private readonly bool _Italic;
	}
}
