﻿using Hangouts_Takeout_JSON_Parser.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hangouts_Takeout_JSON_Parser.FormatExporters
{
    internal class HTMLExporter : FormatExporterBase, IFormatExporter
    {
        private DirectoryInfo outputTopDirectory = null;
        private string messageFromSelfTemplate = string.Empty;
        private string messageFromOtherTemplate = string.Empty;
        private string picFromSelfTemplate = string.Empty;
        private string picFromOtherTemplate = string.Empty;
        private string headerText = string.Empty;
        private string footerText = string.Empty;

        public HTMLExporter()
        {
        }

        public void Generate(ChatHistory content, DirectoryInfo outputDir)
        {
            CreateOutPutDirIfMissing(outputDir);
            LoadTemplates();
            outputTopDirectory = outputDir;

            var selfId = content.FindSelfParticapantId();

            foreach (var conv in content.conversations)
            {
                var otherParticipants = conv.FindOtherParticapants(selfId);
                var selfName = conv.FindSelfName(selfId);
                var otherParticipantNamesAndIds = conv.FindOtherParticapantNameAndIds(otherParticipants);
                var subDirNameForConv = GetDirectoryName(otherParticipantNamesAndIds, conv.conversation.conversation_id.id);
                var convDir = Directory.CreateDirectory($"{outputTopDirectory.FullName}{Path.DirectorySeparatorChar}{subDirNameForConv}");
                Log.Information($"Created subdirectory: {subDirNameForConv}");
                GenerateConversationHTML(convDir, conv, otherParticipants, otherParticipantNamesAndIds, selfId, selfName);
                CloseOutAllHtmlFiles(convDir);
            }
        }

        private void LoadTemplates()
        {

            if (string.IsNullOrEmpty(messageFromOtherTemplate))
            {
                messageFromOtherTemplate = System.IO.File.ReadAllText(
              $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}HtmlTemplates{Path.DirectorySeparatorChar}MessageFromOther.txt");
            }
            if (string.IsNullOrEmpty(messageFromSelfTemplate))
            {
                messageFromSelfTemplate = System.IO.File.ReadAllText(
              $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}HtmlTemplates{Path.DirectorySeparatorChar}MessageFromSelf.txt");
            }
            if (string.IsNullOrEmpty(picFromOtherTemplate))
            {
                picFromOtherTemplate = System.IO.File.ReadAllText(
              $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}HtmlTemplates{Path.DirectorySeparatorChar}PicFromOther.txt");
            }
            if (string.IsNullOrEmpty(picFromSelfTemplate))
            {
                picFromSelfTemplate = System.IO.File.ReadAllText(
              $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}HtmlTemplates{Path.DirectorySeparatorChar}PicFromSelf.txt");
            }
        }

        private void GenerateConversationHTML(DirectoryInfo convDir, Conversation conv, IList<Current_Participant> otherParticipants,
            Dictionary<string, string> otherParticipantNamesAndIds, Participant_Id selfId, string selfName)
        {
            var sortedChatEvents = conv.events.ToList();
            sortedChatEvents.Sort();
            sortedChatEvents.Reverse();
            Console.WriteLine("");

            foreach (var chatEvent in sortedChatEvents)
            {
                DateTime eventDateTime = chatEvent.DateTime;
                bool senderIsSelf = false;

                var senderId = chatEvent.sender_id.chat_id ?? chatEvent.sender_id.gaia_id;
                var senderName = string.Empty;
                if (selfId.chat_id == senderId || selfId.gaia_id == senderId)
                {
                    senderName = selfName;
                    senderIsSelf = true;
                }
                else
                {
                    if (senderId != null && otherParticipantNamesAndIds.ContainsKey(senderId))
                    {
                        senderName = otherParticipantNamesAndIds[senderId] ?? senderId;
                    }
                    else
                    {
                        senderName = senderId;
                    }
                }

                if (chatEvent?.chat_message?.message_content?.segment != null && (chatEvent?.chat_message?.message_content?.segment.Length > 0))
                {
                    foreach (var segment in chatEvent?.chat_message?.message_content?.segment)
                    {
                        var fileName = $"{eventDateTime.ToString("yyyy-MM")}.html";
                        var fullFilePath = $"{convDir}{Path.DirectorySeparatorChar}{fileName}";
                        if (segment?.type?.ToUpper() == "TEXT")
                        {
                            AppendToFile(fullFilePath, eventDateTime, segment.text, senderName, senderIsSelf);
                        }
                        else if (segment?.type?.ToUpper() == "LINK")
                        {
                            var html = $"<a href=\"{segment.text}\">Link</a>";
                            AppendToFile(fullFilePath, eventDateTime, html, senderName, senderIsSelf);
                        }
                        else if (segment?.type?.ToUpper() == "LINE_BREAK")
                        {
                            //TODO: segment?.type?.ToUpper() == "LINE_BREAK"
                            int i = 0;
                        }
                        else
                        {
                            int i = 0;
                        }
                    }
                }

                if (chatEvent?.chat_message?.message_content?.attachment != null && (chatEvent?.chat_message?.message_content?.attachment.Length > 0))
                {
                    foreach (var attachment in chatEvent?.chat_message?.message_content?.attachment)
                    {
                        var fileName = $"{eventDateTime.ToString("yyyy-MM")}.html";
                        var fullFilePath = $"{convDir}{Path.DirectorySeparatorChar}{fileName}";
                        if (attachment?.embed_item?.type!= null && attachment.embed_item.type.Length>0 && attachment.embed_item.type[0].ToUpper() == "PLUS_PHOTO")
                        {
                            if (attachment?.embed_item?.plus_photo!=null)
                            {
                                AppendToFile(fullFilePath, eventDateTime, attachment.embed_item.plus_photo, senderName, senderIsSelf);
                            }
                        }
                        else
                        {
                            int i = 0;
                        }
                    }
                }
            }
        }

        private static string SetTimestamp(DateTime eventDateTime, string content)
        {
            content = content.Replace("%%MESSAGETIMESTAMP%%", eventDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return content;
        }

        private void AppendToFile(string fullFilePath, DateTime eventDateTime, string text, string senderName, bool senderIsSelf)
        {
            if (!File.Exists(fullFilePath))
            {
                CreateNewFile(fullFilePath);
            }

            var content = string.Empty;
            content = SetSender(senderName, senderIsSelf);
            content = SetTimestamp(eventDateTime, content);
            content = content.Replace("%%MESSAGE%%", text);
            using (StreamWriter sw = File.AppendText(fullFilePath))
            {
                sw.WriteLine(content);
            }
            Console.Write(".");
        }

        private void AppendToFile(string fullFilePath, DateTime eventDateTime, Plus_Photo plusPhoto, string senderName, bool senderIsSelf)
        {
            if (!File.Exists(fullFilePath))
            {
                CreateNewFile(fullFilePath);
            }

            var content = string.Empty;

            if (senderIsSelf)
            {
                content = picFromSelfTemplate.Replace("%%SENDERNAME%%", string.Empty);
                content = content.Replace("%%SELFSENDERNAME%%", senderName);
            }
            else
            {
                content = picFromOtherTemplate.Replace("%%SELFSENDERNAME%%", string.Empty);
                content = content.Replace("%%SENDERNAME%%", senderName);
            }
            content = SetTimestamp(eventDateTime, content);


            StringBuilder html = new StringBuilder();
            html.Append($"{plusPhoto?.media_type}: <br>");
            html.Append($"<a href=\"..\\{plusPhoto.FindFileName()}\" >Local File</a> &nbsp; &nbsp;");
            html.Append($"<a href=\"{plusPhoto.url}\" >Online URL</a> &nbsp; &nbsp;");
            html.Append($"<a href=\"{plusPhoto.original_content_url}\" >Original URL</a>");

            content = content.Replace("%%PIC%%", html.ToString());
            using (StreamWriter sw = File.AppendText(fullFilePath))
            {
                sw.WriteLine(content);
            }
            Console.Write(".");
        }

        private string SetSender(string senderName, bool senderIsSelf)
        {
            string content;
            if (senderIsSelf)
            {
                content = messageFromSelfTemplate.Replace("%%SENDERNAME%%", string.Empty);
                content = content.Replace("%%SELFSENDERNAME%%", senderName);
            }
            else
            {
                content = messageFromOtherTemplate.Replace("%%SELFSENDERNAME%%", string.Empty);
                content = content.Replace("%%SENDERNAME%%", senderName);
            }

            return content;
        }

        private void CloseOutHtmlFile(FileInfo file, string footerText = null)
        {
            if (footerText == null)
            {
                footerText = System.IO.File.ReadAllText(
                    $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}HtmlTemplates{Path.DirectorySeparatorChar}footer.txt");
            }
            using (StreamWriter sw = File.AppendText(file.FullName))
            {
                sw.WriteLine(footerText);
            }
        }

        private void CloseOutAllHtmlFiles(DirectoryInfo outputTopDirectory)
        {
            foreach (var file in outputTopDirectory.GetFiles("*.html"))
            {
                CloseOutHtmlFile(file);
            }
            foreach (var subDir in outputTopDirectory.GetDirectories())
            {
                CloseOutAllHtmlFiles(subDir);
            }
        }

        private void CreateNewFile(string fullFilePath)
        {
            if (string.IsNullOrWhiteSpace(headerText))
            {
                headerText = System.IO.File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}HtmlTemplates{Path.DirectorySeparatorChar}header.txt");
                headerText = headerText.Replace("%%HEADERTEXT%%", $"All timestamps are {DateTime.Now.ToLocalTime().ToString("\"GMT\"zzz")}");
            }
            System.IO.File.WriteAllText(fullFilePath, headerText);

            Console.WriteLine("");
            Log.Information($"Created new file {fullFilePath}");
        }
    }
}