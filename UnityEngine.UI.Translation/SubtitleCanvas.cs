using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.UI.Translation
{
	internal class SubtitleCanvas : SubtitleUserInterfaceBase<SubtitleCanvas>, ISubtitleBorder, ISubtitleShadow
	{
		public int BorderWidth
		{
			get
			{
				return this._BorderWidth;
			}
			set
			{
				value = Mathf.Min(Mathf.Max(value, 0), 1);
				if (value != this._BorderWidth)
				{
					this._BorderWidth = value;
					this.OnFontBorderChanged(this, new FontBorderChangedEventArgs(value));
				}
			}
		}

		public int ShadowOffset
		{
			get
			{
				return this._ShadowOffset;
			}
			set
			{
				value = Mathf.Min(Mathf.Max(value, 0), 1);
				if (value != this._ShadowOffset)
				{
					this._ShadowOffset = value;
					this.OnFontShadowChanged(this, new FontShadowChangedEventArgs(value));
				}
			}
		}

		public override IEnumerable<TextPosition> Anchors
		{
			get
			{
				return this.anchors.Keys;
			}
		}

		public override string this[TextPosition anchor]
		{
			get
			{
				SubtitleCanvas.TextData textData;
				if (this.anchors.TryGetValue(anchor, out textData))
				{
					return textData.Text.text;
				}
				throw new ArgumentOutOfRangeException("anchor");
			}
			set
			{
				SubtitleCanvas.TextData textData;
				if (this.anchors.TryGetValue(anchor, out textData))
				{
					if (value == null)
					{
						value = string.Empty;
					}
					if (textData.Text.text != value)
					{
						textData.Text.text = value;
					}
					return;
				}
				throw new ArgumentOutOfRangeException("anchor");
			}
		}

		static SubtitleCanvas()
		{
			MethodInfo method = typeof(Font).GetMethod("CreateDynamicFontFromOSFont", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, null, new Type[]
			{
				typeof(string),
				typeof(int)
			}, null);
			if (method != null)
			{
				SubtitleCanvas.CreateDynamicFontFromOSFont = (Func<string, int, Font>)Delegate.CreateDelegate(typeof(Func<string, int, Font>), method);
			}
		}

		public SubtitleCanvas()
		{
			this._BorderWidth = 1;
			this._ShadowOffset = 1;
			this.anchors = new Dictionary<TextPosition, SubtitleCanvas.TextData>();
		}

		public void Clear()
		{
			foreach (KeyValuePair<TextPosition, SubtitleCanvas.TextData> keyValuePair in this.anchors)
			{
				keyValuePair.Value.Text.text = string.Empty;
			}
		}

		protected override void Awake()
		{
			this.canvas = base.gameObject.AddComponent<Canvas>();
			this.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this.canvas.sortingOrder = 32767;
			HashSet<TextPosition> hashSet = new HashSet<TextPosition>();
			foreach (TextPosition textPosition in (TextPosition[])Enum.GetValues(typeof(TextPosition)))
			{
				if (!hashSet.Contains(textPosition))
				{
					hashSet.Add(textPosition);
					GameObject gameObject = new GameObject("Subtitle" + textPosition.ToString());
					gameObject.transform.SetParent(base.transform, false);
					Text text = gameObject.AddComponent<Text>();
					this.SetTextFont(text, base.FontName);
					this.SetTextSize(text, base.FontSize);
					this.SetTextColor(text, base.FontColor);
					this.SetTextStyle(text, false, false);
					this.SetTextAlignment(text, textPosition);
					this.SetTextMargin(text, 0f, 0f, 0f, 0f);
					Outline border = gameObject.AddComponent<Outline>();
					this.SetTextBorder(border, this.BorderWidth);
					Shadow shadow = gameObject.AddComponent<Shadow>();
					this.SetTextShadow(shadow, this.ShadowOffset);
					this.anchors.Add(textPosition, new SubtitleCanvas.TextData(text, border, shadow));
				}
			}
		}

		private void SetTextFont(Text text, string name)
		{
			if (SubtitleCanvas.CreateDynamicFontFromOSFont != null)
			{
				text.font = SubtitleCanvas.CreateDynamicFontFromOSFont(name, text.fontSize);
				return;
			}
			text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}

		private void SetTextSize(Text text, int size)
		{
			text.fontSize = size;
		}

		private void SetTextColor(Text text, Color color)
		{
			text.color = color;
		}

		private void SetTextStyle(Text text, bool bold, bool italic)
		{
			if (bold)
			{
				if (italic)
				{
					text.fontStyle = FontStyle.BoldAndItalic;
					return;
				}
				text.fontStyle = FontStyle.Bold;
				return;
			}
			else
			{
				if (italic)
				{
					text.fontStyle = FontStyle.Italic;
					return;
				}
				text.fontStyle = FontStyle.Normal;
				return;
			}
		}

		private void SetTextAlignment(Text text, TextPosition position)
		{
			switch (position)
			{
			case TextPosition.LowerLeft:
				text.alignment = TextAnchor.LowerLeft;
				return;
			case TextPosition.LowerCenter:
				text.alignment = TextAnchor.LowerCenter;
				return;
			case TextPosition.LowerRight:
				text.alignment = TextAnchor.LowerRight;
				return;
			case TextPosition.MiddleLeft:
				text.alignment = TextAnchor.MiddleLeft;
				return;
			case TextPosition.MiddleCenter:
				text.alignment = TextAnchor.MiddleCenter;
				return;
			case TextPosition.MiddleRight:
				text.alignment = TextAnchor.MiddleRight;
				return;
			case TextPosition.UpperLeft:
				text.alignment = TextAnchor.UpperLeft;
				return;
			case TextPosition.UpperCenter:
				text.alignment = TextAnchor.UpperCenter;
				return;
			case TextPosition.UpperRight:
				text.alignment = TextAnchor.UpperRight;
				return;
			default:
				text.alignment = TextAnchor.LowerCenter;
				return;
			}
		}

		private void SetTextMargin(Text text, float left, float top, float right, float bottom)
		{
			text.rectTransform.anchorMin = Vector2.zero;
			text.rectTransform.offsetMin = new Vector2(left, bottom);
			text.rectTransform.anchorMax = Vector2.one;
			text.rectTransform.offsetMax = new Vector2(-right, -top);
		}

		private void SetTextBorder(Outline border, int distance)
		{
			border.enabled = (distance != 0);
			border.effectDistance = new Vector2((float)distance, (float)(-(float)distance));
		}

		private void SetTextShadow(Shadow shadow, int distance)
		{
			shadow.enabled = (distance != 0);
			shadow.effectDistance = new Vector2((float)distance, (float)(-(float)distance));
		}

		protected override void OnFontNameChanged(object sender, FontNameChangedEventArgs e)
		{
			this.ChangeTextSettings(delegate(TextPosition position, SubtitleCanvas.TextData data)
			{
				this.SetTextFont(data.Text, e.FontName);
			});
		}

		protected override void OnFontSizeChanged(object sender, FontSizeChangedEventArgs e)
		{
			this.ChangeTextSettings(delegate(TextPosition position, SubtitleCanvas.TextData data)
			{
				this.SetTextSize(data.Text, e.FontSize);
			});
		}

		protected override void OnFontColorChanged(object sender, FontColorChangedEventArgs e)
		{
			this.ChangeTextSettings(delegate(TextPosition position, SubtitleCanvas.TextData data)
			{
				this.SetTextColor(data.Text, e.FontColor);
			});
		}

		protected override void OnFontStyleChanged(object sender, FontStyleChangedEventArgs e)
		{
			this.ChangeTextSettings(delegate(TextPosition position, SubtitleCanvas.TextData data)
			{
				this.SetTextStyle(data.Text, e.Bold, e.Italic);
			});
		}

		protected override void OnFontMarginChanged(object sender, FontMarginChangedEventArgs e)
		{
			this.ChangeTextSettings(delegate(TextPosition position, SubtitleCanvas.TextData data)
			{
				this.SetTextMargin(data.Text, e.MarginLeft, e.MarginTop, e.MarginRight, e.MarginBottom);
			});
		}

		protected virtual void OnFontBorderChanged(object sender, FontBorderChangedEventArgs e)
		{
			this.ChangeTextSettings(delegate(TextPosition position, SubtitleCanvas.TextData data)
			{
				this.SetTextBorder(data.Border, e.BorderWidth);
			});
		}

		protected virtual void OnFontShadowChanged(object sender, FontShadowChangedEventArgs e)
		{
			this.ChangeTextSettings(delegate(TextPosition position, SubtitleCanvas.TextData data)
			{
				this.SetTextShadow(data.Shadow, e.ShadowOffset);
			});
		}

		private void ChangeTextSettings(Action<TextPosition, SubtitleCanvas.TextData> action)
		{
			if (action == null)
			{
				return;
			}
			foreach (KeyValuePair<TextPosition, SubtitleCanvas.TextData> keyValuePair in this.anchors)
			{
				action(keyValuePair.Key, keyValuePair.Value);
			}
		}

		private void LateUpdate()
		{
			this.UpdateCanvasScaleFactor();
		}

		private void UpdateCanvasScaleFactor()
		{
			float num = Screen.dpi;
			if (this.dpi == num)
			{
				return;
			}
			this.dpi = num;
			num = ((this.dpi == 0f) ? 72f : this.dpi);
			this.canvas.scaleFactor = num / 72f;
		}

		private const int DEFAULT_BORDER_WIDTH = 1;

		private float dpi;

		private int _BorderWidth;

		private int _ShadowOffset;

		private Canvas canvas;

		private Dictionary<TextPosition, SubtitleCanvas.TextData> anchors;

		private static Func<string, int, Font> CreateDynamicFontFromOSFont;

		private struct TextData
		{
			public Text Text
			{
				get
				{
					return this._Text;
				}
			}

			public Outline Border
			{
				get
				{
					return this._Border;
				}
			}

			public Shadow Shadow
			{
				get
				{
					return this._Shadow;
				}
			}

			public TextData(Text text, Outline border, Shadow shadow)
			{
				this._Text = text;
				this._Border = border;
				this._Shadow = shadow;
			}

			private Text _Text;

			private Outline _Border;

			private Shadow _Shadow;
		}
	}
}
