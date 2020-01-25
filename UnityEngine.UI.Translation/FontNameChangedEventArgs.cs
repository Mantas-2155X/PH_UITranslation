using System;

namespace UnityEngine.UI.Translation
{
	internal class FontNameChangedEventArgs : EventArgs
	{
		public string FontName
		{
			get
			{
				return this._FontName;
			}
		}

		public FontNameChangedEventArgs(string name)
		{
			this._FontName = name;
		}

		private readonly string _FontName;
	}
}
