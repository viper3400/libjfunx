# libjfunx
This is a collection of some helper classes and methods I'm using in my projects. This library is and will be work in progress.
It's recommended to use only the library members which are documentend here:


## Get library (NuGet)

Install-Package ch.jaxx.libjfunx.dll

## Logging
### Example
```chsarp
using libjfunx.logging;

class Program
{
	static void Main(string[] args)
	{         
		Logger.SetLogger(new ReflectingFileLogger("D:/Log.log",LogEintragTyp.Debug));
		Logger.Log(LogEintragTyp.Erfolg, "Application started);
		DoSomething();
	}

	static void DoSomething()
	{
		// do something in here and create a log entry.
		// ...
		Logger.Log(LogEintragTyp.Erfolg, "Yeah, you did it!");
	}
}
```


```chsharp
// You have to activate the logger somewhere in your project. 
Logger.SetLogger(new ReflectingFileLogger(String Logfile));
 
// You can use the static Logger.Log method from everywhere. 
Logger.Log(LogEintragTyp.Status, "Applikation gestartet.");

// Log application exit 
// > Create an event for application exit in your main thread
Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
 
// Handle this event and log applications end
static void Application_ApplicationExit(object sender, EventArgs e)
{
    Logger.Log(LogEintragTyp.Status, "Applikation beendet.");
}
```
 