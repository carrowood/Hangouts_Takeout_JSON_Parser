using System;
using System.Collections.Generic;
using System.Text;

namespace Hangouts_Takeout_JSON_Parser.Entities
{
    public partial class Plus_Photo
    {
        public string FindFileName()
        {
            var uri = new Uri(this?.url);
            return uri.Segments[uri.Segments.Length-1];

        }
    }
}
