using System;

namespace Hangouts_Takeout_JSON_Parser.Entities
{
    public partial class Event : IComparable<Event>
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal DateTime DateTime
        {
            get
            {
                var miilliSeconds = long.Parse(timestamp) / 1000; //orignal is in microSeconds apparantly
                return epoch.AddMilliseconds(miilliSeconds).ToLocalTime();
            }
        }

        public int CompareTo(Event other)
        {
            DateTime eventDateTime = this.DateTime;
            if (eventDateTime < other.DateTime)
            {
                return 1;
            }
            else if (eventDateTime > other.DateTime)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}