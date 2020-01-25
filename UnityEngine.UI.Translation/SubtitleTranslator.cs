using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace UnityEngine.UI.Translation
{
	internal static class SubtitleTranslator
	{
		internal static bool Initialized { get; private set; }

		internal static string GlobalSubtitleDir
		{
			get
			{
				return IniSettings.MainDir + IniSettings.LanguageDir + "Audio\\";
			}
		}

		internal static string GlobalSubtitleDirFiles
		{
			get
			{
				return "*" + ".txt";
			}
		}

		internal static string SubtitleDir
		{
			get
			{
				return IniSettings.MainDir + IniSettings.ProcessPathDir + IniSettings.LanguageDir + "Audio\\";
			}
		}

		internal static string SubtitleDirFiles
		{
			get
			{
				return "*" + ".txt";
			}
		}

		internal static string FileDir
		{
			get
			{
				return SubtitleTranslator.SubtitleDir;
			}
		}

		internal static string FileName
		{
			get
			{
				return string.Format("{0}.txt", "Subtitle");
			}
		}

		internal static string FilePath
		{
			get
			{
				return Path.Combine(SubtitleTranslator.FileDir, SubtitleTranslator.FileName);
			}
		}

		internal static string LvFileDir
		{
			get
			{
				return SubtitleTranslator.SubtitleDir;
			}
		}

		internal static string LvFileName
		{
			get
			{
				string text = Application.loadedLevelName;
				if (string.IsNullOrEmpty(text))
				{
					text = "Subtitle";
				}
				return string.Format("{0}.{1}.txt", text, Application.loadedLevel);
			}
		}

		internal static string LvFilePath
		{
			get
			{
				return Path.Combine(SubtitleTranslator.LvFileDir, SubtitleTranslator.LvFileName);
			}
		}

		static SubtitleTranslator()
		{
			SubtitleTranslator.Initialize();
		}

		internal static void Initialize()
		{
			try
			{
				if (!SubtitleTranslator.Initialized)
				{
					double totalMilliseconds = TimeSpan.FromSeconds((double)IniSettings.LogWriterTime).TotalMilliseconds;
					SubtitleTranslator.writerdata = new Dictionary<string, StringBuilder>();
					SubtitleTranslator.writertimer = new Timer(totalMilliseconds);
					SubtitleTranslator.writertimer.AutoReset = false;
					SubtitleTranslator.writertimer.Elapsed += SubtitleTranslator.WriterTimerElapsed;
					SubtitleTranslator.Load();
					SubtitleSettings.AnchorChanged += delegate(TextPosition value)
					{
						SubtitleTranslator.Load();
						AudioSourceSubtitle.Instance.Reload();
					};
					IniSettings.LanguageDirChanged += delegate(string value)
					{
						SubtitleTranslator.Load();
						AudioSourceSubtitle.Instance.Reload();
					};
					IniSettings.ProcessPathDirChanged += delegate(string value)
					{
						SubtitleTranslator.Load();
						AudioSourceSubtitle.Instance.Reload();
					};
					IniSettings.FindAudioChanged += delegate(bool value)
					{
						AudioSourceSubtitle.Instance.Reload();
					};
					SubtitleTranslator.Initialized = true;
				}
			}
			catch (Exception ex)
			{
				SubtitleTranslator.Initialized = false;
				IniSettings.Error("SubtitleTranslator::Initialize:\n" + ex.ToString());
			}
		}

		private static void WriterTimerElapsed(object sender, ElapsedEventArgs e)
		{
			object writerLock = SubtitleTranslator.WriterLock;
			lock (writerLock)
			{
				SubtitleTranslator.StopWatchSubtitleFiles();
				try
				{
					foreach (KeyValuePair<string, StringBuilder> keyValuePair in SubtitleTranslator.writerdata)
					{
						string key = keyValuePair.Key;
						string directoryName = Path.GetDirectoryName(key);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
						}
						using (StreamWriter streamWriter = new StreamWriter(key, true, Encoding.UTF8))
						{
							streamWriter.Write(keyValuePair.Value.ToString());
						}
					}
				}
				catch (Exception ex)
				{
					IniSettings.Error("SubtitleTranslator::DumpText:\n" + ex.ToString());
				}
				SubtitleTranslator.writerdata.Clear();
				SubtitleTranslator.WatchSubtitleFiles();
			}
		}

		private static void StopWatchSubtitleFiles()
		{
			if (SubtitleTranslator.gfsw != null)
			{
				SubtitleTranslator.gfsw.Dispose();
				SubtitleTranslator.gfsw = null;
			}
			if (SubtitleTranslator.sfsw != null)
			{
				SubtitleTranslator.sfsw.Dispose();
				SubtitleTranslator.sfsw = null;
			}
		}

		private static void WatchSubtitleFiles()
		{
			try
			{
				if (SubtitleTranslator.GlobalSubtitleDir != SubtitleTranslator.SubtitleDir && SubtitleTranslator.gfsw == null && Directory.Exists(SubtitleTranslator.GlobalSubtitleDir))
				{
					SubtitleTranslator.gfsw = new FileSystemWatcher(SubtitleTranslator.GlobalSubtitleDir, SubtitleTranslator.GlobalSubtitleDirFiles);
					SubtitleTranslator.gfsw.NotifyFilter = (NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite);
					SubtitleTranslator.gfsw.IncludeSubdirectories = false;
					SubtitleTranslator.gfsw.Changed += SubtitleTranslator.WatcherNotice;
					SubtitleTranslator.gfsw.Created += SubtitleTranslator.WatcherNotice;
					SubtitleTranslator.gfsw.Error += delegate(object sender, ErrorEventArgs e)
					{
						IniSettings.Error(e.GetException().ToString());
					};
					SubtitleTranslator.gfsw.EnableRaisingEvents = true;
				}
				if (SubtitleTranslator.sfsw == null && Directory.Exists(SubtitleTranslator.SubtitleDir))
				{
					SubtitleTranslator.sfsw = new FileSystemWatcher(SubtitleTranslator.SubtitleDir, SubtitleTranslator.SubtitleDirFiles);
					SubtitleTranslator.sfsw.NotifyFilter = (NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite);
					SubtitleTranslator.sfsw.IncludeSubdirectories = false;
					SubtitleTranslator.sfsw.Changed += SubtitleTranslator.WatcherNotice;
					SubtitleTranslator.sfsw.Created += SubtitleTranslator.WatcherNotice;
					SubtitleTranslator.sfsw.Error += delegate(object sender, ErrorEventArgs e)
					{
						IniSettings.Error(e.GetException().ToString());
					};
					SubtitleTranslator.sfsw.EnableRaisingEvents = true;
				}
			}
			catch (Exception ex)
			{
				IniSettings.Error("WatchSubtitleFiles:\n" + ex.ToString());
			}
		}

		private static void WatcherNotice(object sender, FileSystemEventArgs e)
		{
			object noticeLock = SubtitleTranslator.NoticeLock;
			lock (noticeLock)
			{
				if (!(SubtitleTranslator.lastraisedfile == e.FullPath) || !(DateTime.Now < SubtitleTranslator.lastraisedtime))
				{
					SubtitleTranslator.lastraisedfile = e.FullPath;
					SubtitleTranslator.lastraisedtime = DateTime.Now.AddSeconds(1.0);
					if (e.FullPath.EndsWith(".txt"))
					{
						SubtitleTranslator.LoadTranslations(e.FullPath, true);
					}
					SubtitleTranslator.WatchSubtitleFiles();
				}
			}
		}

		private static void Load()
		{
			SubtitleTranslator.StopWatchSubtitleFiles();
			SubtitleTranslator.translations.Clear();
			SubtitleTranslator.translationsLv.Clear();
			if (SubtitleTranslator.GlobalSubtitleDir != SubtitleTranslator.SubtitleDir)
			{
				SubtitleTranslator.LoadAllFromGlobalTranslationDir();
			}
			SubtitleTranslator.LoadAllFromTranslationDir();
			SubtitleTranslator.WatchSubtitleFiles();
			if (IniSettings.DebugMode || IniSettings.FindAudio)
			{
				int num = 0;
				num += SubtitleTranslator.translations.Count;
				foreach (OrderedDictionary orderedDictionary in SubtitleTranslator.translationsLv.Values)
				{
					num += orderedDictionary.Count;
				}
				IniSettings.Log(string.Format("Subtitles Loaded: {0}", num));
			}
		}

		private static void LoadTranslations(string file, bool retranslate = false)
		{
			object translationLock = SubtitleTranslator.TranslationLock;
			lock (translationLock)
			{
				try
				{
					if (!(Path.GetExtension(file).ToLower() != ".txt"))
					{
						if (!Path.GetFileName(file).StartsWith("."))
						{
							if (file.StartsWith(Environment.CurrentDirectory))
							{
								file = file.Remove(0, Environment.CurrentDirectory.Length);
								if (!file.StartsWith("\\"))
								{
									file = "\\" + file;
								}
								file = "." + file;
							}
							int fileLevel = SubtitleTranslator.GetFileLevel(file);
							bool flag = fileLevel > -1;
							OrderedDictionary orderedDictionary = null;
							if (flag)
							{
								SubtitleTranslator.translationsLv.TryGetValue(fileLevel, out orderedDictionary);
								if (orderedDictionary != null)
								{
									SubtitleTranslator.RemoveAllTranslation(orderedDictionary, file);
								}
							}
							else
							{
								SubtitleTranslator.RemoveAllTranslation(SubtitleTranslator.translations, file);
							}
							using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
							{
								bool flag2 = false;
								bool flag3 = true;
								List<SubtitleLine> list = null;
								string text = string.Empty;
								while (!streamReader.EndOfStream)
								{
									string text2 = streamReader.ReadLine();
									if (!text2.StartsWith("//") && (text.Length != 0 || text2.Length != 0))
									{
										Match match = Regex.Match(text2.TrimEnd(new char[0]), "^#sub[ ]+\"(.+?)\"(?:[ ]+(?:{\\\\a([\\d]+)})?(.*))?$", RegexOptions.IgnoreCase);
										if (match.Success)
										{
											flag2 = false;
											flag3 = true;
											SubtitleDataBase subtitleDataBase = null;
											text = match.Groups[1].Value;
											list = new List<SubtitleLine>();
											if (match.Groups[3].Success)
											{
												string text3 = match.Groups[3].Value.Trim();
												if (text3.Length > 0)
												{
													SubtitleLine item = default(SubtitleLine);
													item.Position = (match.Groups[2].Success ? item.Position.Parse(match.Groups[2].Value, SubtitleSettings.Anchor) : SubtitleSettings.Anchor);
													item.Text = text3;
													list.Add(item);
												}
											}
											if (flag)
											{
												if (orderedDictionary != null)
												{
													subtitleDataBase = (orderedDictionary[text] as SubtitleDataBase);
												}
											}
											else
											{
												subtitleDataBase = (SubtitleTranslator.translations[text] as SubtitleDataBase);
											}
											if (subtitleDataBase != null)
											{
												if (flag)
												{
													orderedDictionary.Remove(text);
												}
												else
												{
													SubtitleTranslator.translations.Remove(text);
												}
											}
											subtitleDataBase = new SubtitleDataBase(file, list);
											if (flag)
											{
												if (orderedDictionary == null)
												{
													orderedDictionary = new OrderedDictionary();
													SubtitleTranslator.translationsLv.Add(fileLevel, orderedDictionary);
												}
												orderedDictionary.Add(text, subtitleDataBase);
											}
											else
											{
												SubtitleTranslator.translations.Add(text, subtitleDataBase);
											}
										}
										else if (text.Length > 0)
										{
											if (!flag2)
											{
												if (text2.Length == 0)
												{
													continue;
												}
												Match match2 = Regex.Match(text2, "^([\\d]*\\.?[\\d]+)[ ]+-->[ ]+([\\d]*\\.?[\\d]+)$", RegexOptions.None);
												if (match2.Success)
												{
													if (!streamReader.EndOfStream)
													{
														flag2 = true;
														list.Add(new SubtitleLine
														{
															StartTime = float.Parse(match2.Groups[1].Value, CultureInfo.InvariantCulture),
															EndTime = float.Parse(match2.Groups[2].Value, CultureInfo.InvariantCulture)
														});
														continue;
													}
													continue;
												}
												else
												{
													flag2 = true;
													list.Add(default(SubtitleLine));
												}
											}
											if (flag3)
											{
												int index = list.Count - 1;
												if (text2.Length > 0)
												{
													Match match3 = Regex.Match(text2, "^(?:{\\\\a([\\d]+)})?(.*)$", RegexOptions.None);
													if (match3.Success)
													{
														string text4 = match3.Groups[2].Value.Trim();
														if (text4.Length != 0 || !streamReader.EndOfStream)
														{
															SubtitleLine value = list[index];
															value.Position = (match3.Groups[1].Success ? value.Position.Parse(match3.Groups[1].Value, SubtitleSettings.Anchor) : SubtitleSettings.Anchor);
															value.Text = text4;
															list[index] = value;
															continue;
														}
													}
												}
												flag2 = false;
												flag3 = true;
												list.RemoveAt(index);
											}
											else if (text2.Length > 0)
											{
												string text5 = text2.Trim();
												int index2 = list.Count - 1;
												SubtitleLine value2 = list[index2];
												if (text5.Length > 0)
												{
													if (value2.Text.Length > 0)
													{
														value2.Text += "\n";
													}
													value2.Text += text5;
													list[index2] = value2;
												}
												else if (streamReader.EndOfStream && value2.Text.Length == 0)
												{
													list.RemoveAt(index2);
												}
											}
											else
											{
												flag2 = false;
												flag3 = true;
												int index3 = list.Count - 1;
												if (list[index3].Text.Length == 0)
												{
													list.RemoveAt(index3);
												}
											}
										}
									}
								}
							}
							if (retranslate)
							{
								AudioSourceSubtitle.Instance.Reload();
							}
							if (IniSettings.DebugMode || IniSettings.FindAudio)
							{
								IniSettings.Log("Loaded: " + file);
							}
						}
					}
				}
				catch (Exception ex)
				{
					IniSettings.Error("LoadSubtitles:\n" + ex.ToString());
				}
			}
		}

		private static void LoadAllFromGlobalTranslationDir()
		{
			if (!Directory.Exists(SubtitleTranslator.GlobalSubtitleDir))
			{
				return;
			}
			SubtitleTranslator.LoadAllFromTranslationDir(Directory.GetFiles(SubtitleTranslator.GlobalSubtitleDir, SubtitleTranslator.GlobalSubtitleDirFiles));
		}

		private static void LoadAllFromTranslationDir()
		{
			if (!Directory.Exists(SubtitleTranslator.SubtitleDir))
			{
				return;
			}
			SubtitleTranslator.LoadAllFromTranslationDir(Directory.GetFiles(SubtitleTranslator.SubtitleDir, SubtitleTranslator.SubtitleDirFiles));
		}

		private static void LoadAllFromTranslationDir(string[] files)
		{
			if (files == null || files.Length == 0)
			{
				return;
			}
			for (int i = 0; i < files.Length; i++)
			{
				SubtitleTranslator.LoadTranslations(files[i], false);
			}
		}

		private static int GetFileLevel(string file)
		{
			file = Path.GetFileName(file);
			if (!file.StartsWith("."))
			{
				string text = null;
				if (file.EndsWith(".txt"))
				{
					text = file.Substring(0, file.Length - ".txt".Length);
				}
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = Path.GetExtension(text);
					if (text2.StartsWith("."))
					{
						text2 = text2.Remove(0, 1);
						int num;
						if (int.TryParse(text2, out num) && num > -1)
						{
							return num;
						}
					}
				}
			}
			return -1;
		}

		private static void RemoveAllTranslation(OrderedDictionary od, string fromfile)
		{
			for (int i = od.Count - 1; i >= 0; i--)
			{
				SubtitleDataBase subtitleDataBase = od[i] as SubtitleDataBase;
				if (subtitleDataBase != null && subtitleDataBase.Path == fromfile)
				{
					od.RemoveAt(i);
				}
			}
		}

		public static bool Translate(string audio, out SubtitleLine[] lines)
		{
			object translationLock = SubtitleTranslator.TranslationLock;
			bool result;
			lock (translationLock)
			{
				try
				{
					List<SubtitleLine> list = null;
					OrderedDictionary orderedDictionary;
					if (SubtitleTranslator.translationsLv.TryGetValue(Application.loadedLevel, out orderedDictionary))
					{
						SubtitleDataBase subtitleDataBase = orderedDictionary[audio] as SubtitleDataBase;
						if (subtitleDataBase != null)
						{
							list = subtitleDataBase.Value;
							goto IL_5B;
						}
					}
					SubtitleDataBase subtitleDataBase2 = SubtitleTranslator.translations[audio] as SubtitleDataBase;
					if (subtitleDataBase2 != null)
					{
						list = subtitleDataBase2.Value;
					}
					IL_5B:
					if (list == null)
					{
						list = new List<SubtitleLine>();
						if (IniSettings.FindAudio)
						{
							if (IniSettings.DumpAudioByLevel)
							{
								OrderedDictionary orderedDictionary2;
								if (!SubtitleTranslator.translationsLv.TryGetValue(Application.loadedLevel, out orderedDictionary2))
								{
									orderedDictionary2 = new OrderedDictionary();
									SubtitleTranslator.translationsLv.Add(Application.loadedLevel, orderedDictionary2);
								}
								string lvFilePath = SubtitleTranslator.LvFilePath;
								orderedDictionary2.Add(audio, new SubtitleDataBase(lvFilePath, list));
								SubtitleTranslator.DumpSubtitle(lvFilePath, audio);
							}
							else
							{
								string filePath = SubtitleTranslator.FilePath;
								SubtitleTranslator.translations.Add(audio, new SubtitleDataBase(filePath, list));
								SubtitleTranslator.DumpSubtitle(filePath, audio);
							}
						}
					}
					List<SubtitleLine> list2 = new List<SubtitleLine>();
					foreach (SubtitleLine item in list)
					{
						if (!string.IsNullOrEmpty(item.Text))
						{
							list2.Add(item);
						}
					}
					if (list2.Count == 0 && IniSettings.FindAudio)
					{
						list2.Add(new SubtitleLine
						{
							Position = SubtitleSettings.Anchor,
							Text = audio
						});
					}
					lines = list2.ToArray();
					return list2.Count > 0;
				}
				catch (Exception ex)
				{
					IniSettings.Error("TextTranslator::Translate:\n" + ex.ToString());
				}
				lines = null;
				result = false;
			}
			return result;
		}

		private static void DumpSubtitle(string path, string audio)
		{
			object writerLock = SubtitleTranslator.WriterLock;
			lock (writerLock)
			{
				if (!(Path.GetDirectoryName(path) + "\\" != SubtitleTranslator.SubtitleDir))
				{
					StringBuilder stringBuilder;
					if (!SubtitleTranslator.writerdata.TryGetValue(path, out stringBuilder))
					{
						stringBuilder = new StringBuilder();
						SubtitleTranslator.writerdata.Add(path, stringBuilder);
					}
					stringBuilder.AppendLine(string.Format("#sub \"{0}\"", audio));
					SubtitleTranslator.writertimer.Start();
				}
			}
		}

		private const string EXT = ".txt";

		private const string FILENAME = "Subtitle";

		private const string FILE = "{0}.txt";

		private const string LVFILE = "{0}.{1}.txt";

		private const string IGNORE = ".";

		private const string COMMENT = "//";

		private static readonly object TranslationLock = new object();

		private static Dictionary<int, OrderedDictionary> translationsLv = new Dictionary<int, OrderedDictionary>();

		private static OrderedDictionary translations = new OrderedDictionary();

		private static readonly object NoticeLock = new object();

		private static string lastraisedfile;

		private static DateTime lastraisedtime;

		private static FileSystemWatcher gfsw;

		private static FileSystemWatcher sfsw;

		private static readonly object WriterLock = new object();

		private static Dictionary<string, StringBuilder> writerdata;

		private static Timer writertimer;
	}
}
