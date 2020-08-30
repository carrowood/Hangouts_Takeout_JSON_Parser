using Hangouts_Takeout_JSON_Parser.Entities;
using System.IO;

namespace Hangouts_Takeout_JSON_Parser.FormatExporters
{
    internal interface IFormatExporter
    {
        void Generate(ChatHistory chatConversations, DirectoryInfo outputDir);
    }
}