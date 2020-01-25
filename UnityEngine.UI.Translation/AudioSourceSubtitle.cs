using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace UnityEngine.UI.Translation
{
	public class AudioSourceSubtitle : MonoBehaviour
	{
		public static AudioSourceSubtitle Instance
		{
			get
			{
				if (AudioSourceSubtitle._Instance == null)
				{
					AudioSourceSubtitle._Instance = Object.FindObjectOfType<AudioSourceSubtitle>();
					if (AudioSourceSubtitle._Instance == null)
					{
						GameObject gameObject = new GameObject("AudioSourceSubtitle");
						AudioSourceSubtitle._Instance = gameObject.AddComponent<AudioSourceSubtitle>();
						Object.DontDestroyOnLoad(gameObject);
					}
				}
				return AudioSourceSubtitle._Instance;
			}
		}

		static AudioSourceSubtitle()
		{
			SubtitleSettings.FontNameChanged += delegate(string value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.FontName = value;
			};
			SubtitleSettings.FontSizeChanged += delegate(int value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.FontSize = value;
			};
			SubtitleSettings.FontColorChanged += delegate(Color value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.FontColor = value;
			};
			SubtitleSettings.BoldChanged += delegate(bool value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.Bold = value;
			};
			SubtitleSettings.ItalicChanged += delegate(bool value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.Italic = value;
			};
			SubtitleSettings.BorderWidthChanged += delegate(int value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.BorderWidth = value;
			};
			SubtitleSettings.ShadowOffsetChanged += delegate(int value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.ShadowOffset = value;
			};
			SubtitleSettings.MarginLeftChanged += delegate(int value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.MarginLeft = (float)value;
			};
			SubtitleSettings.MarginTopChanged += delegate(int value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.MarginTop = (float)value;
			};
			SubtitleSettings.MarginRightChanged += delegate(int value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.MarginRight = (float)value;
			};
			SubtitleSettings.MarginBottomChanged += delegate(int value)
			{
				SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.MarginBottom = (float)value;
			};
		}

		public AudioSourceSubtitle()
		{
			this.subtitles = new OrderedDictionary();
			this.content = new Dictionary<TextPosition, StringBuilder>();
		}

		public void Reload()
		{
			if (this.subtitles.Count > 0)
			{
				this.reloadsubtitles = true;
			}
		}

		public void Add(AudioSource source)
		{
			try
			{
				this.subtitles.Remove(source);
				this.subtitles.Insert(0, source, new Subtitle(SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.Anchors, source));
			}
			catch (Exception ex)
			{
				IniSettings.Error("AudioSourceSubtitle::Load:\n" + ex.ToString());
			}
		}

		private void Awake()
		{
			if (this != AudioSourceSubtitle.Instance)
			{
				Object.DestroyImmediate(this);
			}
			foreach (TextPosition key in SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.Anchors)
			{
				this.content.Add(key, new StringBuilder(512));
			}
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.FontName = SubtitleSettings.FontName;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.FontSize = SubtitleSettings.FontSize;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.FontColor = SubtitleSettings.FontColor;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.Bold = SubtitleSettings.Bold;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.Italic = SubtitleSettings.Italic;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.BorderWidth = SubtitleSettings.BorderWidth;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.ShadowOffset = SubtitleSettings.ShadowOffset;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.MarginLeft = (float)SubtitleSettings.MarginLeft;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.MarginTop = (float)SubtitleSettings.MarginTop;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.MarginRight = (float)SubtitleSettings.MarginRight;
			SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.MarginBottom = (float)SubtitleSettings.MarginBottom;
		}

		private void LateUpdate()
		{
			try
			{
				if (this.subtitles.Count != 0)
				{
					foreach (KeyValuePair<TextPosition, StringBuilder> keyValuePair in this.content)
					{
						if (keyValuePair.Value.Length > 0)
						{
							keyValuePair.Value.Length = 0;
						}
					}
					for (int i = this.subtitles.Count - 1; i >= 0; i--)
					{
						Subtitle subtitle = this.subtitles[i] as Subtitle;
						if (subtitle == null)
						{
							this.subtitles.RemoveAt(i);
						}
						else if (subtitle.Source == null)
						{
							this.subtitles.RemoveAt(i);
						}
						else
						{
							if (this.reloadsubtitles)
							{
								subtitle.Reload();
							}
							subtitle.LateUpdate();
							foreach (TextPosition textPosition in SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.Anchors)
							{
								string text = subtitle[textPosition];
								if (text.Length > 0)
								{
									if (this.content[textPosition].Length > 0)
									{
										this.content[textPosition].Append('\n');
									}
									this.content[textPosition].Append(text);
								}
							}
						}
					}
					this.reloadsubtitles = false;
					foreach (TextPosition textPosition2 in SubtitleUserInterfaceBase<SubtitleCanvas>.Instance.Anchors)
					{
						SubtitleUserInterfaceBase<SubtitleCanvas>.Instance[textPosition2] = this.content[textPosition2].ToString();
					}
				}
			}
			catch (Exception ex)
			{
				IniSettings.Error("AudioSourceSubtitle::LateUpdate:\n" + ex.ToString());
			}
		}

		private static AudioSourceSubtitle _Instance;

		private bool reloadsubtitles;

		private OrderedDictionary subtitles;

		private Dictionary<TextPosition, StringBuilder> content;
	}
}
