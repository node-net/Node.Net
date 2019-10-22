using System;
using System.Diagnostics;

namespace Node.Net
{
	/// <summary>
	/// LogLevel enumeration
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Debug
		/// </summary>
		Debug,

		/// <summary>
		/// Information
		/// </summary>
		Info,

		/// <summary>
		/// Warning
		/// </summary>
		Warn,

		/// <summary>
		/// Error
		/// </summary>
		Error,

		/// <summary>
		/// Fatal
		/// </summary>
		Fatal
	};

	/// <summary>
	/// Log
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// The LogFunction to be used by the Log class
		/// </summary>
		public static Action<Type, LogLevel, string> LogFunction { get; set; }

		public static int StackFrameIndex { get; set; } = 2;

		/// <summary>
		/// Log Info
		/// </summary>
		/// <param name="message"></param>
		public static void Info(string message) => _Log(LogLevel.Info, message, StackFrameIndex);

		/// <summary>
		/// Log Error
		/// </summary>
		/// <param name="message"></param>
		public static void Error(string message) => _Log(LogLevel.Error, message, StackFrameIndex);

		/// <summary>
		/// Log Warning
		/// </summary>
		/// <param name="message"></param>
		public static void Warn(string message) => _Log(LogLevel.Warn, message, StackFrameIndex);

		/// <summary>
		/// Log Fatal
		/// </summary>
		/// <param name="message"></param>
		public static void Fatal(string message) => _Log(LogLevel.Fatal, message, StackFrameIndex);

		/// <summary>
		/// Log Debug
		/// </summary>
		/// <param name="message"></param>
		public static void Debug(string message) => _Log(LogLevel.Debug, message, StackFrameIndex);

		private static void _Log(LogLevel level, string message, int stackFrameIndex = 2)
		{
			if (LogFunction != null)
			{
				var stackTrace = new StackTrace();
				var stackFrame = stackTrace.GetFrame(stackFrameIndex);
				var type = stackFrame.GetMethod().DeclaringType;

				LogFunction(type, level, message);
			}
		}
	}
}