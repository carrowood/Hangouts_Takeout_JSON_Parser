namespace Hangouts_Takeout_JSON_Parser.Entities
{
    public partial class ChatHistory
    {
        internal Participant_Id FindSelfParticapantId()
        {
            foreach (var conv in this.conversations)
            {
                Participant_Id selfId = conv?.conversation?.conversation?.self_conversation_state?.self_read_state?.participant_id;
                if (selfId.chat_id != null || selfId.gaia_id != null)
                {
                    return selfId;
                }
            }

            return null;
        }
    }
}