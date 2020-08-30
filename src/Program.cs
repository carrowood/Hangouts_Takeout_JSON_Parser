using Hangouts_Takeout_JSON_Parser.Entities;
using Hangouts_Takeout_JSON_Parser.FormatExporters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Hangouts_Takeout_JSON_Parser
{
    internal class Program
    {
        private static string jsonPath = string.Empty;

        private static int Main(string[] args)
        {
            int retValue = -1;

            try
            {
                SetupStaticLogger();
                ChatHistory chatConversations = null;
                var convCounter = 0;


                retValue = ParseArgs(args);
                if (retValue!=0)
                {
                    return retValue;
                }

                
                Log.Information($"Attempting to read {jsonPath}. This may take a few minutes.");
                var regex = new Regex(@"^ROOT.conversations\[\d+\].conversation$");

                using (FileStream s = File.Open(jsonPath, FileMode.Open))
                using (StreamReader sr = new StreamReader(s))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    reader.SupportMultipleContent = true;

                    var serializer = new JsonSerializer();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            convCounter++;
                            chatConversations = serializer.Deserialize<ChatHistory>(reader);

                            Log.Information($"Found {chatConversations?.conversations?.Length} conversations to process");
                        }
                    }
                }

                DirectoryInfo outputDir = new DirectoryInfo($".{Path.DirectorySeparatorChar}Output");
                IFormatExporter exporter = new HTMLExporter();
                exporter.Generate(chatConversations, outputDir);
                return retValue;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An unhandled exception has occurred. Details follow.");
                return -1;
            }
            finally
            {
                Log.CloseAndFlush();
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
        }

        private static int ParseArgs(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please specify the full filename and path of the json file you want to parse");
                System.Console.WriteLine("Usage: Hangouts-Takeout-JSON-Parser.exe \"C:\\Temp\\My Dir\\Hangouts.json\"");
                return 1;
            }else if (args.Length == 1)
            {
                jsonPath = args[0];
                return 0;
            }
            else
            {
                System.Console.WriteLine("Too many arguments");
                System.Console.WriteLine("Usage: Hangouts-Takeout-JSON-Parser.exe \"C:\\Temp\\My Dir\\Hangouts.json\"");
                return 1;
            }
        }

        private static void SetupStaticLogger()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}