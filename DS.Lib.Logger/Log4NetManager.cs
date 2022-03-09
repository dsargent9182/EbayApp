using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace DS.Lib.Logger
{
	public  class Log4NetManager : ILoggerManager
	{
		private enum LevelEnum { Fatal, Error, Warn, Info, Debug, Trace, Ignite, SlowSQL, SlowMessageClient, SlowMessageConsumer }
		private const int SKIP_FRAMES = 2;
		private readonly ILog _logger = LogManager.GetLogger(typeof(Log4NetManager));
		public Log4NetManager(string configFileName)
		{
			try
			{
				XmlDocument log4netConfig = new XmlDocument();

				using (var fs = File.OpenRead(configFileName))
				{
					log4netConfig.Load(fs);

					var repo = LogManager.CreateRepository(
							Assembly.GetEntryAssembly(),
							typeof(log4net.Repository.Hierarchy.Hierarchy));

					XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

					// The first log to be written 
					_logger.Info("Log System Initialized");
				}
			}
			catch (Exception ex)
			{
				_logger.Error("Error", ex);
			}
		}
		public int CurrentProcessID
		{
			get
			{
				return Process.GetCurrentProcess().Id;
			}
		}

		public void LogInfo(string message)
		{
			ThreadContext.Properties["ContextLocation"] = GetContextLocation();
			_logger.Info(message);
			ThreadContext.Properties["ContextLocation"] = string.Empty;
		}

		private void LogError(object message)
		{
			ThreadContext.Properties["ContextLocation"] = GetContextLocation();
			_logger.Error(message);
			ThreadContext.Properties["ContextLocation"] = string.Empty;
		}
		public void LogError(string message, Exception ex)
		{
			ThreadContext.Properties["ContextLocation"] = GetContextLocation();
			_logger.Error(message, ex);
			ThreadContext.Properties["ContextLocation"] = string.Empty;
		}
		private string GetContextLocation(int stackFramOffset = 0)
		{
			try
			{
				var stackTrace = new StackTrace(SKIP_FRAMES + stackFramOffset, true);
				var method = stackTrace.GetFrame(0).GetMethod();

				var className = method.DeclaringType.Name;
				int extraFrames = 0;
				string extraText = "";
				while (string.Equals(className, "LoggerManager", StringComparison.OrdinalIgnoreCase))
				{
					//if (!Debugger.IsAttached) { Debugger.Launch(); }

					extraFrames++;
					try
					{
						stackTrace = new StackTrace(SKIP_FRAMES + extraFrames, true);
						method = stackTrace.GetFrame(0).GetMethod();
						className = method.DeclaringType.Name;
					}
					catch (Exception ex)
					{
						extraText = "ERROR: " + ex.Message;
						break;
					}
				}
				var assemblyName = method.DeclaringType.Assembly.GetName().Name;
				var methodName = method.Name;
				var loggerName = string.Format("{0}.{1}.{2}.{3}.{4}", Environment.MachineName, assemblyName, CurrentProcessID, className, methodName);
				return loggerName;
			}
			catch (Exception exc)
			{
				System.Diagnostics.Debug.WriteLine(exc.ToString());
			}
			return string.Empty;
		}

		public List<Tuple<string, string>> GetParameters<T>(T instance)
		{
			List<Tuple<string, string>> tuplePrams = new List<Tuple<string, string>>();
			if (null != instance)
			{
				var prop = instance.GetType().GetProperties();
				foreach (PropertyInfo pi in prop)
				{
					foreach (CustomAttributeData c in pi.CustomAttributes)
					{
						if (string.Equals(c.AttributeType.Name, "LogParameter", StringComparison.OrdinalIgnoreCase))
						{
							if (pi.PropertyType == typeof(Dictionary<string, string>))
							{
								var dict = (Dictionary<string, string>)instance.GetType().GetProperty(pi.Name).GetValue(instance, null);

								tuplePrams.Add(new Tuple<string, string>(pi.Name, "Dictionary<string,string>"));
								foreach (KeyValuePair<string, string> kvp in dict ?? new Dictionary<string, string>())
								{
									string val = kvp.Value ?? string.Empty;
									if (val.Length > 1024) val = val.Substring(0, 1024);
									tuplePrams.Add(new Tuple<string, string>($"\t{pi.Name}->{kvp.Key}", val));
								}
							}
							else
							{
								string value = $"{instance.GetType().GetProperty(pi.Name).GetValue(instance, null)}";
								if (value.Length > 1024) value = value.Substring(0, 1024);
								tuplePrams.Add(new Tuple<string, string>(pi.Name ?? string.Empty, value));
							}

						}
					}
				}
				FieldInfo[] fields = instance.GetType().GetFields();
				foreach (FieldInfo fi in fields)
				{
					foreach (CustomAttributeData c in fi.CustomAttributes)
					{
						if (string.Equals(c.AttributeType.Name, "LogParameter", StringComparison.OrdinalIgnoreCase))
						{
							if (fi.FieldType == typeof(Dictionary<string, string>))
							{
								var dict = (Dictionary<string, string>)instance.GetType().GetField(fi.Name).GetValue(instance);

								tuplePrams.Add(new Tuple<string, string>(fi.Name, "Dictionary<string,string>"));

								foreach (KeyValuePair<string, string> kvp in dict ?? new Dictionary<string, string>())
								{
									string val = kvp.Value ?? string.Empty;
									if (val.Length > 1024) val = val.Substring(0, 1024);
									tuplePrams.Add(new Tuple<string, string>($"\t{fi.Name}->{kvp.Key}", val));
								}
							}
							else
							{
								string value = $"{instance.GetType().GetField(fi.Name).GetValue(instance)}";
								if (value.Length > 1024) value = value.Substring(0, 1024);
								tuplePrams.Add(new Tuple<string, string>(fi.Name ?? string.Empty, value));
							}
						}
					}
				}
				return tuplePrams;
			}

			return null;
		}

		public void LogError(string message)
		{
			_logger.Error(message);
		}
	}
}
