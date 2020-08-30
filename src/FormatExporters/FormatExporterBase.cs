using Serilog;
using System.Collections.Generic;
using System.IO;

namespace Hangouts_Takeout_JSON_Parser.FormatExporters
{
    internal abstract class FormatExporterBase
    {
        protected internal void CreateOutPutDirIfMissing(DirectoryInfo outputDir)
        {
            if (!outputDir.Exists)
            {
                Directory.CreateDirectory(outputDir.FullName);
                Log.Information($"Created output directory: {outputDir.FullName}");
            }
            else
            {
                Log.Information($"Using existing output directory: {outputDir.FullName}");
            }
        }

        protected internal string GetDirectoryName(Dictionary<string, string> otherParticipantNamesAndIds, string chatId)
        {
            var dirName = string.Empty;
            if (otherParticipantNamesAndIds.Count == 1)
            {
                var enumerator = otherParticipantNamesAndIds.GetEnumerator();
                enumerator.MoveNext();
                string name = enumerator.Current.Value;
                string id = enumerator.Current.Key;
                dirName = $"{name} {id}";
            }
            else
            {
                dirName = $"Group Chat {chatId}";
            }

            return dirName.ToString();
        }
    }
}