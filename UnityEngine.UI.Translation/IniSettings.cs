using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace UnityEngine.UI.Translation
{
	internal static class IniSettings
	{
		internal static event Action<IniFile> LoadSettings;

		internal static event Action<bool> DebugModeChanged;

		internal static bool DebugMode
		{
			get
			{
				return IniSettings.debugmode;
			}
			private set
			{
				if (value != IniSettings.debugmode)
				{
					IniSettings.debugmode = value;
					if (IniSettings.DebugModeChanged != null && IniSettings.initialized)
					{
						IniSettings.DebugModeChanged(value);
					}
				}
			}
		}

		internal static event Action<string> LanguageChanged;

		internal static event Action<string> LanguageDirChanged;

		internal static string Language
		{
			get
			{
				if (IniSettings.language == null)
				{
					IniSettings.language = string.Empty;
				}
				return IniSettings.language;
			}
			private set
			{
				char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
				if (value == null)
				{
					value = string.Empty;
				}
				else if (value != string.Empty)
				{
					value = value.Trim();
					if (value != string.Empty)
					{
						if (value.Length > 5)
						{
							value = value.Substring(0, 5);
						}
						if (value.IndexOfAny(invalidFileNameChars) != -1)
						{
							value = string.Empty;
						}
					}
				}
				if (value != IniSettings.language)
				{
					IniSettings.language = value;
					if (IniSettings.LanguageChanged != null && IniSettings.initialized)
					{
						IniSettings.LanguageChanged(value);
					}
					IniSettings.languagedir = value;
					if (!string.IsNullOrEmpty(value))
					{
						IniSettings.languagedir += "\\";
					}
					if (IniSettings.LanguageDirChanged != null && IniSettings.initialized)
					{
						IniSettings.LanguageDirChanged(value);
					}
				}
			}
		}

		internal static string LanguageDir
		{
			get
			{
				if (IniSettings.languagedir == null)
				{
					if (string.IsNullOrEmpty(IniSettings.Language))
					{
						IniSettings.languagedir = string.Empty;
					}
					else
					{
						IniSettings.languagedir = IniSettings.Language + "\\";
					}
				}
				return IniSettings.languagedir;
			}
		}

		internal static event Action<bool> FindImageChanged;

		internal static bool FindImage
		{
			get
			{
				return IniSettings.findimage;
			}
			private set
			{
				if (value != IniSettings.findimage)
				{
					IniSettings.findimage = value;
					if (IniSettings.FindImageChanged != null && IniSettings.initialized)
					{
						IniSettings.FindImageChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> FindAudioChanged;

		internal static bool FindAudio
		{
			get
			{
				return IniSettings.findaudio;
			}
			private set
			{
				if (value != IniSettings.findaudio)
				{
					IniSettings.findaudio = value;
					if (IniSettings.FindAudioChanged != null && IniSettings.initialized)
					{
						IniSettings.FindAudioChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> DumpAudioByLevelChanged;

		internal static bool DumpAudioByLevel
		{
			get
			{
				return IniSettings.dumpaudiobylevel;
			}
			private set
			{
				if (value != IniSettings.dumpaudiobylevel)
				{
					IniSettings.dumpaudiobylevel = value;
					if (IniSettings.DumpAudioByLevelChanged != null && IniSettings.initialized)
					{
						IniSettings.DumpAudioByLevelChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> FindTextChanged;

		internal static bool FindText
		{
			get
			{
				return IniSettings.findtext;
			}
			private set
			{
				if (value != IniSettings.findtext)
				{
					IniSettings.findtext = value;
					if (IniSettings.FindTextChanged != null && IniSettings.initialized)
					{
						IniSettings.FindTextChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> DumpTextByLevelChanged;

		internal static bool DumpTextByLevel
		{
			get
			{
				return IniSettings.dumptextbylevel;
			}
			private set
			{
				if (value != IniSettings.dumptextbylevel)
				{
					IniSettings.dumptextbylevel = value;
					if (IniSettings.DumpTextByLevelChanged != null && IniSettings.initialized)
					{
						IniSettings.DumpTextByLevelChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> UseRegExChanged;

		internal static bool UseRegEx
		{
			get
			{
				return IniSettings.useregex;
			}
			private set
			{
				if (value != IniSettings.useregex)
				{
					IniSettings.useregex = value;
					if (IniSettings.UseRegExChanged != null && IniSettings.initialized)
					{
						IniSettings.UseRegExChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> UseTextPredictionChanged;

		internal static bool UseTextPrediction
		{
			get
			{
				return IniSettings.usetextprediction;
			}
			private set
			{
				if (value != IniSettings.usetextprediction)
				{
					IniSettings.usetextprediction = value;
					if (IniSettings.UseTextPredictionChanged != null && IniSettings.initialized)
					{
						IniSettings.UseTextPredictionChanged(value);
					}
				}
			}
		}

		internal static event Action<bool> UseCopy2ClipboardChanged;

		internal static bool UseCopy2Clipboard
		{
			get
			{
				return IniSettings.usecopy2clipboard;
			}
			private set
			{
				if (value != IniSettings.usecopy2clipboard)
				{
					IniSettings.usecopy2clipboard = value;
					if (IniSettings.UseCopy2ClipboardChanged != null && IniSettings.initialized)
					{
						IniSettings.UseCopy2ClipboardChanged(value);
					}
				}
			}
		}

		internal static event Action<int> Copy2ClipboardTimeChanged;

		internal static int Copy2ClipboardTime
		{
			get
			{
				return IniSettings.copy2clipboardtime;
			}
			private set
			{
				if (value != IniSettings.copy2clipboardtime)
				{
					IniSettings.copy2clipboardtime = value;
					if (IniSettings.Copy2ClipboardTimeChanged != null && IniSettings.initialized)
					{
						IniSettings.Copy2ClipboardTimeChanged(value);
					}
				}
			}
		}

		internal static event Action<string> ProcessPathChanged;

		internal static event Action<string> ProcessPathDirChanged;

		internal static string ProcessPath
		{
			get
			{
				if (IniSettings.processpath == null)
				{
					IniSettings.ProcessPath = IniSettings.processname;
				}
				return IniSettings.processpath;
			}
			private set
			{
				char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
				if (value == null)
				{
					value = IniSettings.processname;
				}
				else if (value != string.Empty)
				{
					value = value.Trim();
					if (value != string.Empty && value.IndexOfAny(invalidFileNameChars) != -1)
					{
						value = IniSettings.processname;
					}
				}
				if (value != IniSettings.processpath)
				{
					IniSettings.processpath = value;
					if (IniSettings.ProcessPathChanged != null && IniSettings.initialized)
					{
						IniSettings.ProcessPathChanged(value);
					}
					IniSettings.processpathdir = value;
					if (!string.IsNullOrEmpty(value))
					{
						IniSettings.processpathdir += "\\";
					}
					if (IniSettings.ProcessPathDirChanged != null && IniSettings.initialized)
					{
						IniSettings.ProcessPathDirChanged(value);
					}
				}
			}
		}

		internal static string ProcessPathDir
		{
			get
			{
				if (IniSettings.processpathdir == null)
				{
					if (string.IsNullOrEmpty(IniSettings.ProcessPath))
					{
						IniSettings.processpathdir = string.Empty;
					}
					else
					{
						IniSettings.processpathdir = IniSettings.ProcessPath + "\\";
					}
				}
				return IniSettings.processpathdir;
			}
		}

		internal static string ProcessName
		{
			get
			{
				return IniSettings.processname;
			}
		}

		internal static string ProcessFile
		{
			get
			{
				return IniSettings.processfile;
			}
		}

		internal static string PluginDir
		{
			get
			{
				return ".\\Plugins\\";
			}
		}

		internal static string MainDir
		{
			get
			{
				return IniSettings.PluginDir + "UITranslation\\";
			}
		}

		internal static string LogFileDir
		{
			get
			{
				return IniSettings.MainDir;
			}
		}

		internal static string LogFileName
		{
			get
			{
				return "Translation.log";
			}
		}

		internal static string LogFilePath
		{
			get
			{
				return IniSettings.LogFileDir + IniSettings.LogFileName;
			}
		}

		internal static string SettingsFileDir
		{
			get
			{
				return IniSettings.MainDir;
			}
		}

		internal static string SettingsFileName
		{
			get
			{
				return "Translation.ini";
			}
		}

		internal static string SettingsFilePath
		{
			get
			{
				return IniSettings.SettingsFileDir + IniSettings.SettingsFileName;
			}
		}

		internal static int LogWriterTime
		{
			get
			{
				return IniSettings.writetime;
			}
			set
			{
				if (value < 1)
				{
					value = 1;
				}
				IniSettings.writetime = value;
			}
		}

		static IniSettings()
		{
			IniSettings.processname = Process.GetCurrentProcess().ProcessName;
			IniSettings.processfile = IniSettings.processname + ".exe";
			IniSettings.PROCESSPATHKEY = IniSettings.processname + "_Folder";
			IniSettings.sb = new StringBuilder();
			IniSettings.timer = new Timer(TimeSpan.FromSeconds((double)IniSettings.LogWriterTime).TotalMilliseconds);
			IniSettings.timer.AutoReset = false;
			IniSettings.timer.Elapsed += IniSettings.timer_Elapsed;
			try
			{
				if (File.Exists(IniSettings.LogFilePath))
				{
					File.Delete(IniSettings.LogFilePath);
				}
			}
			catch (Exception ex)
			{
				IniSettings.Error("IniSettings:\n" + ex.ToString());
			}
			IniSettings.Load();
			IniSettings.WatchTextFiles();
		}

		private static void WatchTextFiles()
		{
			try
			{
				if (IniSettings.iniw == null && Directory.Exists(IniSettings.SettingsFileDir))
				{
					IniSettings.iniw = new FileSystemWatcher(IniSettings.SettingsFileDir, IniSettings.SettingsFileName);
					IniSettings.iniw.NotifyFilter = (NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite);
					IniSettings.iniw.IncludeSubdirectories = false;
					IniSettings.iniw.Changed += IniSettings.WatcherNotice;
					IniSettings.iniw.Created += IniSettings.WatcherNotice;
					IniSettings.iniw.Error += delegate(object sender, ErrorEventArgs e)
					{
						IniSettings.Error(e.GetException().ToString());
					};
					IniSettings.iniw.EnableRaisingEvents = true;
				}
			}
			catch (Exception ex)
			{
				IniSettings.Error("WatchTextFiles:\n" + ex.ToString());
			}
		}

		private static void WatcherNotice(object sender, FileSystemEventArgs e)
		{
			if (IniSettings.lastraisedfile == e.FullPath && DateTime.Now < IniSettings.lastraisedtime)
			{
				return;
			}
			IniSettings.lastraisedfile = e.FullPath;
			IniSettings.lastraisedtime = DateTime.Now.AddSeconds(1.0);
			IniSettings.Load();
		}

		internal static IniFile GetINIFile()
		{
			return new IniFile(IniSettings.SettingsFilePath);
		}

		internal static void Load()
		{
			object loadLock = IniSettings.LoadLock;
			lock (loadLock)
			{
				try
				{
					if (IniSettings.iniw != null)
					{
						IniSettings.iniw.Dispose();
						IniSettings.iniw = null;
					}
					if (!Directory.Exists(IniSettings.SettingsFileDir))
					{
						Directory.CreateDirectory(IniSettings.SettingsFileDir);
					}
					IniFile inifile = IniSettings.GetINIFile();
					string key = "bDebugMode";
					string value = inifile.GetValue("Translation", key, null);
					bool flag;
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = false;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.DebugMode = flag;
					key = "sLanguage";
					value = inifile.GetValue("Translation", key, null);
					IniSettings.Language = value;
					if (value != IniSettings.Language)
					{
						inifile.WriteValue("Translation", key, IniSettings.Language);
					}
					key = "bFindImage";
					value = inifile.GetValue("Translation", key, null);
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = false;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.FindImage = flag;
					key = "bFindAudio";
					value = inifile.GetValue("Translation", key, null);
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = false;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.FindAudio = flag;
					key = "bDumpAudioByLevel";
					value = inifile.GetValue("Translation", key, null);
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = true;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.DumpAudioByLevel = flag;
					key = "bFindText";
					value = inifile.GetValue("Translation", key, null);
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = false;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.FindText = flag;
					key = "bDumpTextByLevel";
					value = inifile.GetValue("Translation", key, null);
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = true;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.DumpTextByLevel = flag;
					key = "bUseRegEx";
					value = inifile.GetValue("Translation", key, null);
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = true;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.UseRegEx = flag;
					key = "bUseTextPrediction";
					value = inifile.GetValue("Translation", key, null);
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = true;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.UseTextPrediction = flag;
					key = "bUseCopy2Clipboard";
					value = inifile.GetValue("Translation", key, null);
					if (value == null || !bool.TryParse(value, out flag))
					{
						flag = false;
						inifile.WriteValue("Translation", key, flag);
					}
					IniSettings.UseCopy2Clipboard = flag;
					key = "iCopy2ClipboardTime(ms)";
					value = inifile.GetValue("Translation", key, null);
					int num;
					if (value == null || !int.TryParse(value, out num))
					{
						num = 250;
						inifile.WriteValue("Translation", key, num);
					}
					IniSettings.Copy2ClipboardTime = num;
					key = IniSettings.PROCESSPATHKEY;
					value = inifile.GetValue("Translation", key, null);
					IniSettings.ProcessPath = value;
					if (value != IniSettings.ProcessPath)
					{
						inifile.WriteValue("Translation", key, IniSettings.ProcessPath);
					}
					IniSettings.initialized = true;
					try
					{
						Action<IniFile> loadSettings = IniSettings.LoadSettings;
						if (loadSettings != null)
						{
							loadSettings(inifile);
						}
					}
					catch (Exception ex)
					{
						IniSettings.Error("LoadSettings:\n" + ex.ToString());
					}
					IniSettings.WatchTextFiles();
				}
				catch (Exception ex2)
				{
					IniSettings.Error("LoadSettings:\n" + ex2.ToString());
				}
			}
		}

		private static void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			object logLock = IniSettings.LogLock;
			lock (logLock)
			{
				try
				{
					if (!Directory.Exists(IniSettings.LogFileDir))
					{
						Directory.CreateDirectory(IniSettings.LogFileDir);
					}
					using (StreamWriter streamWriter = new StreamWriter(IniSettings.LogFilePath, true, Encoding.UTF8))
					{
						streamWriter.Write(IniSettings.sb.ToString());
						IniSettings.sb.Length = 0;
					}
				}
				catch
				{
				}
			}
		}

		internal static void Log(object obj = null)
		{
			object logLock = IniSettings.LogLock;
			lock (logLock)
			{
				if (obj == null)
				{
					obj = "null";
				}
				IniSettings.sb.AppendLine(obj.ToString());
				IniSettings.timer.Start();
			}
		}

		internal static void Error(object obj = null)
		{
			if (IniSettings.DebugMode)
			{
				IniSettings.Log(obj);
			}
		}

		internal const string DIR1 = ".\\Plugins\\";

		internal const string DIR2 = "UITranslation\\";

		internal const string DIR3 = "Text\\";

		internal const string DIR4 = "Image\\";

		internal const string DIR5 = "Audio\\";

		private const string INI = "Translation.ini";

		private const string LOG = "Translation.log";

		internal const string SECTION = "Translation";

		private const string DEBUGMODEKEY = "bDebugMode";

		private const string FINDTEXTKEY = "bFindText";

		private const string DUMPTEXTBYLEVELKEY = "bDumpTextByLevel";

		private const string FINDAUDIOKEY = "bFindAudio";

		private const string DUMPAUDIOBYLEVELKEY = "bDumpAudioByLevel";

		private const string FINDIMAGEKEY = "bFindImage";

		private const string LANGUAGEKEY = "sLanguage";

		private const string USEREGEXKEY = "bUseRegEx";

		private const string USETEXTPREDICTIONKEY = "bUseTextPrediction";

		private const string USECOPY2CLIPBOARDKEY = "bUseCopy2Clipboard";

		private const string COPY2CLIPBOARDTIMEKEY = "iCopy2ClipboardTime(ms)";

		private static bool debugmode;

		private static string language;

		private static string languagedir;

		private static bool findimage;

		private static bool findaudio;

		private static bool dumpaudiobylevel;

		private static bool findtext;

		private static bool dumptextbylevel;

		private static bool useregex;

		private static bool usetextprediction;

		private static bool usecopy2clipboard;

		private static int copy2clipboardtime;

		private static string processpath;

		private static string processpathdir;

		private static string PROCESSPATHKEY;

		private static string processname;

		private static string processfile;

		private static bool initialized = false;

		private static string lastraisedfile;

		private static DateTime lastraisedtime;

		private static FileSystemWatcher iniw;

		private static StringBuilder sb;

		private static Timer timer;

		private static int writetime = 3;

		private static readonly object LoadLock = new object();

		private static readonly object LogLock = new object();

		private class NativeMethods
		{
			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern bool AttachConsole(int dwProcessId);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern bool AllocConsole();

			internal const int ATTACH_PARENT_PROCESS = -1;
		}
	}
}
