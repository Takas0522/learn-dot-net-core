using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DemoLib.Model
{
    public class EventsResult
    {
        public List<Event> Events { get; set; } = new List<Event>();
    }

    public class Event
    {
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public string Start { get; set; } = "";
        public string End { get; set; } = "";

    }

    public class GraphEvent
    {
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = "";

        [JsonPropertyName("body")]
        public Body? Body { get; set; }

        [JsonPropertyName("start")]
        public DateTimeSet? Start { get; set; }

        [JsonPropertyName("end")]
        public DateTimeSet? End { get; set; }

    }

    public class Body
    {
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; } = "text";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }

    public class DateTimeSet
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string TimeZone { get; set; } = "Tokyo Standard Time";
    }
}
