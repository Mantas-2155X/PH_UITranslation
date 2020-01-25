using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.UI.Translation
{
	internal class Subtitle : ISubtitle
	{
		public AudioClip Clip { get; private set; }

		public AudioSource Source { get; private set; }

		public Subtitle(IEnumerable<TextPosition> anchors, AudioSource source)
		{
			this.anchors = anchors;
			this.content = new Dictionary<TextPosition, StringBuilder>();
			this.display = new Dictionary<TextPosition, HashSet<SubtitleLine>>();
			foreach (TextPosition key in this.anchors)
			{
				this.content.Add(key, new StringBuilder(512));
				this.display.Add(key, new HashSet<SubtitleLine>());
			}
			this.Source = source;
			this.Load();
		}

		private void Load()
		{
			if (this.Source != null && this.Source.clip != null)
			{
				this.Clip = this.Source.clip;
				this.LoadSubtitles();
				this.invalid = true;
				this.loaded = true;
			}
		}

		private void LoadSubtitles()
		{
			if (this.Clip == null || string.IsNullOrEmpty(this.Clip.name))
			{
				this.subtitles = new Subtitle.LineData[0];
				return;
			}
			foreach (TextPosition key in this.anchors)
			{
				this.display[key].Clear();
				this.ClearContent(this.content[key]);
			}
			SubtitleLine[] array;
			if (SubtitleTranslator.Translate(this.Clip.name, out array))
			{
				this.subtitles = new Subtitle.LineData[array.Length];
				for (int i = 0; i < this.subtitles.Length; i++)
				{
					HashSet<SubtitleLine> anchor;
					if (this.display.TryGetValue(array[i].Position, out anchor))
					{
						this.subtitles[i] = new Subtitle.LineData(array[i], anchor);
					}
				}
				return;
			}
			this.subtitles = new Subtitle.LineData[0];
		}

		private void Unload()
		{
			this.Clip = null;
			this.Source = null;
			this.subtitles = null;
			foreach (TextPosition key in this.display.Keys)
			{
				this.display[key].Clear();
				this.ClearContent(this.content[key]);
			}
			this.invalid = false;
			this.loaded = false;
		}

		private StringBuilder ClearContent(StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.Length = 0;
			}
			return sb;
		}

		public void Reload()
		{
			this.LoadSubtitles();
		}

		public void LateUpdate()
		{
			try
			{
				if (this.Source != null && this.Clip != null && this.Source.clip == this.Clip)
				{
					int num = (this.Clip.frequency == 0) ? 44100 : this.Clip.frequency;
					float num2 = (float)this.Source.timeSamples * (1f / (float)num);
					foreach (Subtitle.LineData lineData in this.subtitles)
					{
						if (lineData != null)
						{
							if (this.Source.isPlaying && (lineData.Line.EndTime == 0f || num2 < lineData.Line.EndTime) && num2 >= lineData.Line.StartTime)
							{
								this.invalid |= lineData.Show();
							}
							else
							{
								this.invalid |= lineData.Hide();
							}
						}
					}
					if (this.invalid)
					{
						foreach (KeyValuePair<TextPosition, HashSet<SubtitleLine>> keyValuePair in this.display)
						{
							StringBuilder stringBuilder = this.ClearContent(this.content[keyValuePair.Key]);
							foreach (SubtitleLine subtitleLine in keyValuePair.Value)
							{
								if (stringBuilder.Length > 0)
								{
									stringBuilder.Append('\n');
								}
								stringBuilder.Append(subtitleLine.Text);
							}
						}
						this.invalid = false;
					}
				}
				else if (this.loaded)
				{
					this.Unload();
				}
			}
			catch (Exception ex)
			{
				IniSettings.Error("Subtitle::LateUpdate:\n" + ex.ToString());
			}
		}

		public string this[TextPosition anchor]
		{
			get
			{
				return this.content[anchor].ToString();
			}
		}

		private bool loaded;

		private bool invalid;

		private Subtitle.LineData[] subtitles;

		private IEnumerable<TextPosition> anchors;

		private Dictionary<TextPosition, StringBuilder> content;

		private Dictionary<TextPosition, HashSet<SubtitleLine>> display;

		private class LineData
		{
			public bool Visible { get; private set; }

			public SubtitleLine Line { get; private set; }

			public bool Show()
			{
				bool flag = false;
				if (!this.Visible)
				{
					flag = this.anchor.Add(this.Line);
					this.Visible = flag;
				}
				return flag;
			}

			public bool Hide()
			{
				bool flag = false;
				if (this.Visible)
				{
					flag = this.anchor.Remove(this.Line);
					this.Visible = !flag;
				}
				return flag;
			}

			public LineData(SubtitleLine line, HashSet<SubtitleLine> anchor)
			{
				this.Line = line;
				this.Visible = false;
				this.anchor = anchor;
			}

			private readonly HashSet<SubtitleLine> anchor;
		}
	}
}
