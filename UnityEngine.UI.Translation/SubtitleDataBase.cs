using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Translation
{
	internal class SubtitleDataBase
	{
		public string Path { get; protected set; }

		public List<SubtitleLine> Value { get; protected set; }

		public SubtitleDataBase()
		{
		}

		public SubtitleDataBase(string path) : this(path, new List<SubtitleLine>())
		{
		}

		public SubtitleDataBase(string path, List<SubtitleLine> value)
		{
			this.Path = path;
			this.Value = value;
		}

		public override string ToString()
		{
			return string.Format("{0} {{ \"{1}\" }}", this.Path, this.Value);
		}
	}
}
