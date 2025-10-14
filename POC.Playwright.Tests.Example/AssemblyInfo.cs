using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: log4net.Config.XmlConfigurator(Watch = true)]