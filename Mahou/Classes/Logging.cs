// Created by BladeMight in 05.12.2016-18:55
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

namespace Mahou
{
	public static class Logging {
		public static string logdir = Path.Combine(MahouUI.nPath, "Logs");
		public static string log = Path.Combine(logdir, DateTime.Today.ToString("yyyy.MM.dd") + ".txt");
		static object locky = new Object(); // To prevent `file in use` error in multi-threads
		static BlockingCollection<string> _logMessages = new BlockingCollection<string>();
		/// <summary>
		/// Write message to log.
		/// </summary>
		/// <param name="logmsg">Message that will be written.</param>
		/// <param name="msgtype">Type of message: 0(or nothing) = Info, 1 = Error, 2 = Warning.</param>
		public static void Log(string logmsg, int msgtype = 0) {			
			if (!MahouUI.LoggingEnabled) return;
			if (!File.Exists(Configs.filePath)) 
				MMain.MyConfs = new Configs();	
			if (!Directory.Exists(Path.Combine(MahouUI.nPath, "Logs")))
				Directory.CreateDirectory(logdir);
			var messagetype = "I";
			var msgtime = DateTime.Now.ToString("hh:mm:ss.fff");
			switch (msgtype) {
				case 1:
					messagetype = "E";
					break;
				case 2:
					messagetype = "W";
					break;
			}
			var tologmsg = msgtime + " [" + messagetype + "]: " + logmsg + "\r\n";
			_logMessages.Add(tologmsg);
			}
		public static void UpdateLog() {
			lock (locky) {
				foreach (var msg in _logMessages.GetConsumingEnumerable()) {
					#if VSCDEBUG
						Console.Write(msg);
					#elif DEBUG 
						Debug.Write(msg);
					#else
						File.AppendAllText(log, msg);
					#endif
				}
			}
		}
	}
}
