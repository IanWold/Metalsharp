using System;
using Xunit;

namespace Metalsharp.Tests;

public class LogTests
{
	private static Action<string> GetLogAction(MetalsharpProject project, LogLevel level) => level switch
	{
		LogLevel.Debug => m => project.LogDebug(m),
		LogLevel.Info => m => project.LogInfo(m),
		LogLevel.Error => m => project.LogError(m),
		_ => m => project.LogFatal(m),
	};

	private static void TestOnLog(LogLevel projectLevel, LogLevel messageLevel, Action<bool> assert)
	{
		var wasOnLogInvoked = false;
		var project = new MetalsharpProject(new MetalsharpConfiguration()
		{
			Verbosity = projectLevel
		});

		project.OnLog += (_, _) => wasOnLogInvoked = true;

		GetLogAction(project, messageLevel)("Test");

		assert(wasOnLogInvoked);
	}

	private static void TestOnAnyLog(LogLevel projectLevel, LogLevel messageLevel)
	{
		var wasOnAnyLogInvoked = false;
		var project = new MetalsharpProject(new MetalsharpConfiguration()
		{
			Verbosity = projectLevel
		});

		project.OnAnyLog += (_, _) => wasOnAnyLogInvoked = true;

		GetLogAction(project, messageLevel)("Test");

		Assert.True(wasOnAnyLogInvoked);
	}

	#region Debug | OnLog

	[Fact]
	public void LoggerAtLevelDebugInvokesOnLogForMessagesAtLevelDebug() =>
		TestOnLog(LogLevel.Debug, LogLevel.Debug, Assert.True);

	[Fact]
	public void LoggerAtLevelDebugInvokesOnLogForMessagesAtLevelInfo() =>
		TestOnLog(LogLevel.Debug, LogLevel.Info, Assert.True);

	[Fact]
	public void LoggerAtLevelDebugInvokesOnLogForMessagesAtLevelError() =>
		TestOnLog(LogLevel.Debug, LogLevel.Error, Assert.True);

	[Fact]
	public void LoggerAtLevelDebugInvokesOnLogForMessagesAtLevelFatal() =>
		TestOnLog(LogLevel.Debug, LogLevel.Fatal, Assert.True);

	#endregion

	#region Debug | OnAnyLog

	[Fact]
	public void LoggerAtLevelDebugInvokesOnAnyLogForMessagesAtLevelDebug() =>
		TestOnAnyLog(LogLevel.Debug, LogLevel.Debug);

	[Fact]
	public void LoggerAtLevelDebugInvokesOnAnyLogForMessagesAtLevelInfo() =>
		TestOnAnyLog(LogLevel.Debug, LogLevel.Info);

	[Fact]
	public void LoggerAtLevelDebugInvokesOnAnyLogForMessagesAtLevelError() =>
		TestOnAnyLog(LogLevel.Debug, LogLevel.Error);

	[Fact]
	public void LoggerAtLevelDebugInvokesOnAnyLogForMessagesAtLevelFatal() =>
		TestOnAnyLog(LogLevel.Debug, LogLevel.Fatal);

	#endregion

	#region Info | OnLog

	[Fact]
	public void LoggerAtLevelInfoDoesNotInvokeOnLogForMessagesAtLevelDebug() =>
		TestOnLog(LogLevel.Info, LogLevel.Debug, Assert.False);

	[Fact]
	public void LoggerAtLevelInfoInvokesOnLogForMessagesAtLevelInfo() =>
		TestOnLog(LogLevel.Info, LogLevel.Info, Assert.True);

	[Fact]
	public void LoggerAtLevelInfoInvokesOnLogForMessagesAtLevelError() =>
		TestOnLog(LogLevel.Info, LogLevel.Error, Assert.True);

	[Fact]
	public void LoggerAtLevelInfoInvokesOnLogForMessagesAtLevelFatal() =>
		TestOnLog(LogLevel.Info, LogLevel.Fatal, Assert.True);

	#endregion

	#region Info | OnAnyLog

	[Fact]
	public void LoggerAtLevelInfoInvokesOnAnyLogForMessagesAtLevelDebug() =>
		TestOnAnyLog(LogLevel.Info, LogLevel.Debug);

	[Fact]
	public void LoggerAtLevelInfoInvokesOnAnyLogForMessagesAtLevelInfo() =>
		TestOnAnyLog(LogLevel.Info, LogLevel.Info);

	[Fact]
	public void LoggerAtLevelInfoInvokesOnAnyLogForMessagesAtLevelError() =>
		TestOnAnyLog(LogLevel.Info, LogLevel.Error);

	[Fact]
	public void LoggerAtLevelInfoInvokesOnAnyLogForMessagesAtLevelFatal() =>
		TestOnAnyLog(LogLevel.Info, LogLevel.Fatal);

	#endregion

	#region Error | OnLog

	[Fact]
	public void LoggerAtLevelErrorDoesNotInvokeOnLogForMessagesAtLevelDebug() =>
		TestOnLog(LogLevel.Error, LogLevel.Debug, Assert.False);

	[Fact]
	public void LoggerAtLevelErrorDoesNotInvokeOnLogForMessagesAtLevelInfo() =>
		TestOnLog(LogLevel.Error, LogLevel.Info, Assert.False);

	[Fact]
	public void LoggerAtLevelErrorInvokesOnLogForMessagesAtLevelError() =>
		TestOnLog(LogLevel.Error, LogLevel.Error, Assert.True);

	[Fact]
	public void LoggerAtLevelErrorInvokesOnLogForMessagesAtLevelFatal() =>
		TestOnLog(LogLevel.Error, LogLevel.Fatal, Assert.True);

	#endregion

	#region Error | OnAnyLog

	[Fact]
	public void LoggerAtLevelErrorInvokesOnAnyLogForMessagesAtLevelDebug() =>
		TestOnAnyLog(LogLevel.Error, LogLevel.Debug);

	[Fact]
	public void LoggerAtLevelErrorInvokesOnAnyLogForMessagesAtLevelInfo() =>
		TestOnAnyLog(LogLevel.Error, LogLevel.Info);

	[Fact]
	public void LoggerAtLevelErrorInvokesOnAnyLogForMessagesAtLevelError() =>
		TestOnAnyLog(LogLevel.Error, LogLevel.Error);

	[Fact]
	public void LoggerAtLevelErrorInvokesOnAnyLogForMessagesAtLevelFatal() =>
		TestOnAnyLog(LogLevel.Error, LogLevel.Fatal);

	#endregion

	#region Fatal | OnLog

	[Fact]
	public void LoggerAtLevelFatalDoesNotInvokeOnLogForMessagesAtLevelDebug() =>
		TestOnLog(LogLevel.Fatal, LogLevel.Debug, Assert.False);

	[Fact]
	public void LoggerAtLevelFatalDoesNotInvokeOnLogForMessagesAtLevelInfo() =>
		TestOnLog(LogLevel.Fatal, LogLevel.Info, Assert.False);

	[Fact]
	public void LoggerAtLevelFatalDoesNotInvokeOnLogForMessagesAtLevelError() =>
		TestOnLog(LogLevel.Fatal, LogLevel.Error, Assert.False);

	[Fact]
	public void LoggerAtLevelFatalInvokesOnLogForMessagesAtLevelFatal() =>
		TestOnLog(LogLevel.Fatal, LogLevel.Fatal, Assert.True);

	#endregion

	#region Fatal | OnAnyLog

	[Fact]
	public void LoggerAtLevelFatalInvokesOnAnyLogForMessagesAtLevelDebug() =>
		TestOnAnyLog(LogLevel.Fatal, LogLevel.Debug);

	[Fact]
	public void LoggerAtLevelFatalInvokesOnAnyLogForMessagesAtLevelInfo() =>
		TestOnAnyLog(LogLevel.Fatal, LogLevel.Info);

	[Fact]
	public void LoggerAtLevelFatalInvokesOnAnyLogForMessagesAtLevelError() =>
		TestOnAnyLog(LogLevel.Fatal, LogLevel.Error);

	[Fact]
	public void LoggerAtLevelFatalInvokesOnAnyLogForMessagesAtLevelFatal() =>
		TestOnAnyLog(LogLevel.Fatal, LogLevel.Fatal);

	#endregion

	#region None | OnLog

	[Fact]
	public void LoggerAtLevelNoneDoesNotInvokeOnLogForMessagesAtLevelDebug() =>
		TestOnLog(LogLevel.None, LogLevel.Debug, Assert.False);

	[Fact]
	public void LoggerAtLevelNoneDoesNotInvokeOnLogForMessagesAtLevelInfo() =>
		TestOnLog(LogLevel.None, LogLevel.Info, Assert.False);

	[Fact]
	public void LoggerAtLevelNoneDoesNotInvokeOnLogForMessagesAtLevelError() =>
		TestOnLog(LogLevel.None, LogLevel.Error, Assert.False);

	[Fact]
	public void LoggerAtLevelNoneDoesNotInvokeOnLogForMessagesAtLevelFatal() =>
		TestOnLog(LogLevel.None, LogLevel.Fatal, Assert.False);

	#endregion

	#region None | OnAnyLog

	[Fact]
	public void LoggerAtLevelNoneInvokesOnAnyLogForMessagesAtLevelDebug() =>
		TestOnAnyLog(LogLevel.None, LogLevel.Debug);

	[Fact]
	public void LoggerAtLevelNoneInvokesOnAnyLogForMessagesAtLevelInfo() =>
		TestOnAnyLog(LogLevel.None, LogLevel.Info);

	[Fact]
	public void LoggerAtLevelNoneInvokesOnAnyLogForMessagesAtLevelError() =>
		TestOnAnyLog(LogLevel.None, LogLevel.Error);

	[Fact]
	public void LoggerAtLevelNoneInvokesOnAnyLogForMessagesAtLevelFatal() =>
		TestOnAnyLog(LogLevel.None, LogLevel.Fatal);

	#endregion
}
