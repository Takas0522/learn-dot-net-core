using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MsGraphSkillsSample
{
    public class ToDoData
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Reminder")]
        public string Reminder { get; set; }

        [JsonPropertyName("Due")]
        public DateTime Due { get; set; }

        [JsonPropertyName("IsCompleted")]
        public bool IsCompleted { get; set; }
    }
}
