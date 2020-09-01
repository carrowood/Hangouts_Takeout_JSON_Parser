﻿using System;

/*
  ____          _   _       _     _____    _ _ _
 |  _ \  ___   | \ | | ___ | |_  | ____|__| (_) |_
 | | | |/ _ \  |  \| |/ _ \| __| |  _| / _` | | __|
 | |_| | (_) | | |\  | (_) | |_  | |__| (_| | | |_
 |____/ \___/  |_| \_|\___/ \__| |_____\__,_|_|\__|

    Manual changes to this file may be lost.
    
    This file was generated by
    "Edit->Paste Special->Paste JSON as classes"
    in VS 2019 and then the "Code-Maid" 
    extension was ran.

    Add functionality to classes by using a partial
    class under the Entities namespace
 * */

namespace Hangouts_Takeout_JSON_Parser.Entities
{
    public partial class ChatHistory //RootObject
    {
        public Conversation[] conversations { get; set; }
    }

    public partial class Conversation
    {
        public Conversation1 conversation { get; set; }
        public Event[] events { get; set; }
    }

    public partial class Conversation1
    {
        public Conversation_Id conversation_id { get; set; }
        public Conversation2 conversation { get; set; }
    }

    public partial class Conversation_Id
    {
        public string id { get; set; }
    }

    public partial class Conversation2
    {
        public Id id { get; set; }
        public string type { get; set; }
        public Self_Conversation_State self_conversation_state { get; set; }
        public Read_State[] read_state { get; set; }
        public bool has_active_hangout { get; set; }
        public string otr_status { get; set; }
        public string otr_toggle { get; set; }
        public Current_Participant[] current_participant { get; set; }
        public Participant_Data[] participant_data { get; set; }
        public bool fork_on_external_invite { get; set; }
        public string[] network_type { get; set; }
        public string force_history_state { get; set; }
        public string group_link_sharing_status { get; set; }
    }

    public partial class Id
    {
        public string id { get; set; }
    }

    public partial class Self_Conversation_State
    {
        public Self_Read_State self_read_state { get; set; }
        public string status { get; set; }
        public string notification_level { get; set; }
        public string[] view { get; set; }
        public Inviter_Id inviter_id { get; set; }
        public string invite_timestamp { get; set; }
        public string sort_timestamp { get; set; }
        public string active_timestamp { get; set; }
        public Delivery_Medium_Option[] delivery_medium_option { get; set; }
        public bool is_guest { get; set; }
    }

    public partial class Self_Read_State
    {
        public Participant_Id participant_id { get; set; }
        public string latest_read_timestamp { get; set; }
    }

    public partial class Participant_Id
    {
        public string gaia_id { get; set; }
        public string chat_id { get; set; }
    }

    public partial class Inviter_Id
    {
        public string gaia_id { get; set; }
        public string chat_id { get; set; }
    }

    public partial class Delivery_Medium_Option
    {
        public Delivery_Medium delivery_medium { get; set; }
        public bool current_default { get; set; }
    }

    public partial class Delivery_Medium
    {
        public string medium_type { get; set; }
    }

    public partial class Read_State
    {
        public Participant_Id1 participant_id { get; set; }
        public string latest_read_timestamp { get; set; }
    }

    public partial class Participant_Id1
    {
        public string gaia_id { get; set; }
        public string chat_id { get; set; }
    }

    public partial class Current_Participant
    {
        public string gaia_id { get; set; }
        public string chat_id { get; set; }
    }

    public partial class Participant_Data
    {
        public Id1 id { get; set; }
        public string fallback_name { get; set; }
        public string invitation_status { get; set; }
        public string participant_type { get; set; }
        public string new_invitation_status { get; set; }
        public bool in_different_customer_as_requester { get; set; }
        public string domain_id { get; set; }
    }

    public partial class Id1
    {
        public string gaia_id { get; set; }
        public string chat_id { get; set; }
    }

    public partial class Event : IComparable<Event>
    {
        public Conversation_Id1 conversation_id { get; set; }
        public Sender_Id sender_id { get; set; }
        public string timestamp { get; set; }
        public Self_Event_State self_event_state { get; set; }
        public Chat_Message chat_message { get; set; }
        public string event_id { get; set; }
        public bool advances_sort_timestamp { get; set; }
        public string event_otr { get; set; }
        public Delivery_Medium1 delivery_medium { get; set; }
        public string event_type { get; set; }
        public string event_version { get; set; }
    }

    public partial class Conversation_Id1
    {
        public string id { get; set; }
    }

    public partial class Sender_Id
    {
        public string gaia_id { get; set; }
        public string chat_id { get; set; }
    }

    public partial class Self_Event_State
    {
        public User_Id user_id { get; set; }
        public string notification_level { get; set; }
        public string client_generated_id { get; set; }
    }

    public partial class User_Id
    {
        public string gaia_id { get; set; }
        public string chat_id { get; set; }
    }

    public partial class Chat_Message
    {
        public Message_Content message_content { get; set; }
    }

    public partial class Message_Content
    {
        public Segment[] segment { get; set; }
        public Attachment[] attachment { get; set; }
    }

    public partial class Segment
    {
        public string type { get; set; }
        public string text { get; set; }
        public Formatting formatting { get; set; }
    }

    public partial class Formatting
    {
        public bool bold { get; set; }
        public bool italics { get; set; }
        public bool strikethrough { get; set; }
        public bool underline { get; set; }
    }

    public partial class Attachment
    {
        public Embed_Item embed_item { get; set; }
        public string id { get; set; }
    }

    public partial class Embed_Item
    {
        public string[] type { get; set; }
        public Plus_Photo plus_photo { get; set; }
    }

    public partial class Plus_Photo
    {
        public Thumbnail thumbnail { get; set; }
        public string owner_obfuscated_id { get; set; }
        public string album_id { get; set; }
        public string photo_id { get; set; }
        public string url { get; set; }
        public string original_content_url { get; set; }
        public string media_type { get; set; }
        public string[] stream_id { get; set; }
    }

    public partial class Thumbnail
    {
        public string url { get; set; }
        public string image_url { get; set; }
        public int width_px { get; set; }
        public int height_px { get; set; }
    }

    public partial class Delivery_Medium1
    {
        public string medium_type { get; set; }
    }
}