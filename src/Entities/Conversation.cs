using System.Collections.Generic;
using System.Linq;

namespace Hangouts_Takeout_JSON_Parser.Entities
{
    public partial class Conversation
    {
        private static Dictionary<string, string> SelfNamesFound = new Dictionary<string, string>();

        public IList<Current_Participant> FindOtherParticapants(Participant_Id selfId)
        {
            var participants = this.conversation?.conversation?.current_participant.ToList();

            var participantsOtherThanSelf = participants.Where(
                p => (p.chat_id != null && p.chat_id != selfId.chat_id) || (p.gaia_id != null && p.gaia_id != selfId.gaia_id))
                .ToList();

            return participantsOtherThanSelf;
        }

        internal string FindSelfName(Participant_Id selfId)
        {
            if (SelfNamesFound.ContainsKey(selfId.chat_id))
            {
                return SelfNamesFound[selfId.chat_id];
            }
            else if (SelfNamesFound.ContainsKey(selfId.gaia_id))
            {
                return SelfNamesFound[selfId.gaia_id];
            }

            var participants = this.conversation?.conversation?.current_participant.ToList();
            var selfName = string.Empty;

            var self = participants.FirstOrDefault(
                p => (p.chat_id != null && p.chat_id == selfId.chat_id) || (p.gaia_id != null && p.gaia_id == selfId.gaia_id));

            foreach (var participantData in conversation.conversation.participant_data)
            {
                if (self.chat_id != null && self.chat_id == participantData.id.chat_id)
                {
                    selfName = participantData.fallback_name;
                    SelfNamesFound.Add(self.chat_id, selfName);
                }
                else if (self.gaia_id != null && self.gaia_id == participantData.id.gaia_id)
                {
                    selfName = participantData.fallback_name;
                    SelfNamesFound.Add(self.gaia_id, selfName);
                }
            }

            return selfName;
        }

        public Dictionary<string, string> FindOtherParticapantNameAndIds(IList<Current_Participant> participantsOtherThanSelf)
        {
            var otherParticipantNames = new Dictionary<string, string>();
            foreach (var p in participantsOtherThanSelf)
            {
                foreach (var participantData in conversation.conversation.participant_data)
                {
                    if ((p.chat_id != null && p.chat_id == participantData.id.chat_id) || (p.gaia_id != null && p.gaia_id == participantData.id.gaia_id))
                    {
                        otherParticipantNames.Add($"{p.chat_id ?? p.gaia_id}", participantData.fallback_name);
                    }
                }
            }
            return otherParticipantNames;
        }

        public IList<string> FindOtherParticapantNames(IList<Current_Participant> participantsOtherThanSelf)
        {
            var otherParticipantNames = new List<string>();
            foreach (var p in participantsOtherThanSelf)
            {
                foreach (var participantData in conversation.conversation.participant_data)
                {
                    if ((p.chat_id != null && p.chat_id == participantData.id.chat_id) || (p.gaia_id != null && p.gaia_id == participantData.id.gaia_id))
                    {
                        otherParticipantNames.Add(participantData.fallback_name);
                    }
                }
            }
            return otherParticipantNames;
        }
    }
}