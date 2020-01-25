using System;

namespace UnityEngine.UI.Translation
{
	internal struct SubtitleLine
	{
		public TextPosition Position { get; set; }

		public float StartTime { get; set; }

		public float EndTime { get; set; }

		public string Text { get; set; }
	}
}
