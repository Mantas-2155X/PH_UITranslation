using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Translation
{
	internal interface ISubtitleUserInterface
	{
		string FontName { get; set; }

		int FontSize { get; set; }

		Color FontColor { get; set; }

		bool Bold { get; set; }

		bool Italic { get; set; }

		float MarginLeft { get; set; }

		float MarginTop { get; set; }

		float MarginRight { get; set; }

		float MarginBottom { get; set; }

		IEnumerable<TextPosition> Anchors { get; }

		string this[TextPosition anchor]
		{
			get;
			set;
		}
	}
}
