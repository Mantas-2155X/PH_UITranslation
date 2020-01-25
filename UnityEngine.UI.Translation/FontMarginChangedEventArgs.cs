using System;

namespace UnityEngine.UI.Translation
{
	internal class FontMarginChangedEventArgs : EventArgs
	{
		public float MarginLeft
		{
			get
			{
				return this._MarginLeft;
			}
		}

		public float MarginTop
		{
			get
			{
				return this._MarginTop;
			}
		}

		public float MarginRight
		{
			get
			{
				return this._MarginRight;
			}
		}

		public float MarginBottom
		{
			get
			{
				return this._MarginBottom;
			}
		}

		public FontMarginChangedEventArgs(float left, float top, float right, float bottom)
		{
			this._MarginLeft = left;
			this._MarginTop = top;
			this._MarginRight = right;
			this._MarginBottom = bottom;
		}

		private readonly float _MarginLeft;

		private readonly float _MarginTop;

		private readonly float _MarginRight;

		private readonly float _MarginBottom;
	}
}
