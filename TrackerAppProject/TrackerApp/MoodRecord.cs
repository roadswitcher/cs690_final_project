using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace TrackerApp
{
    public class MoodRecord
    {
        public string Mood { get; }
        public string Trigger { get; }
        public DateTime Timestamp { get; }

        public MoodRecord(string mood, string trigger, DateTime? timestamp = null)
        {
            Mood = mood != null ? mood : throw new ArgumentNullException(nameof(mood));
            Trigger = trigger != null ? trigger : string.Empty;
            Timestamp = timestamp != null ? timestamp.Value : DateTime.UtcNow;
        }
    }
}