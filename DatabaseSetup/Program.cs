using CommandLine;
using DbUp;
using System;
using System.Linq;
using System.Reflection;

namespace DatabaseSetup
{
    public class Program
    {
        private static Options _options;

        static int Main(string[] args)
        {
            bool hasErrors = false;
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                       _options = o;
                   })
                   .WithNotParsed<Options> (o =>
                   {
                       hasErrors = o.Count() > 0;
                   });

            if (hasErrors)
            {
                return -1;
            }

            if (_options.CreateNewIfNotPresent)
            {
                EnsureDatabase.For.MySqlDatabase(_options.ConnectionString);
            }

            var upgrader =
                        DeployChanges.To
                            .MySqlDatabase(_options.ConnectionString)
                            .WithScriptsEmbeddedInAssembly(Assembly.LoadFrom(_options.AssemblyName))
                            .LogToConsole()
                            .WithTransaction()                            
                            .Build();

                    var result = upgrader.PerformUpgrade();

                    if (!result.Successful)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(result.Error);
                        Console.ResetColor();
        #if DEBUG
                        Console.ReadLine();
        #endif
                        return -1;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Success!");
                    Console.ResetColor();
                    return 0;
        }
    }

    public class Options
    {
        [Option('c', "ConnectionString", Required = true, HelpText = "ConnectionString for the database to be upgraded.")]
        public string ConnectionString { get; set; }

        [Option("CreateIfNotPresent", Default = true, HelpText = "Create new database if not present.")]
        public bool CreateNewIfNotPresent { get; set; }

        [Option('a', "assembly", Required = true, HelpText = "Assembly name containing all the scripts.")]
        public string AssemblyName { get; set; }
    }
}
