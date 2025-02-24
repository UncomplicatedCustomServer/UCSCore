using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UncomplicatedCustomServerCore.API.Features.WebSocket;

namespace UncomplicatedCustomServerCore.API.Features.Console
{
    internal class LogEntry
    {
        [JsonIgnore]
        public static IReadOnlyList<LogEntry> List => HiddenList;

        [JsonIgnore]
        internal static readonly List<LogEntry> HiddenList = [];

        [JsonProperty("time")]
        public long Time { get; }

        [JsonProperty("display_time")]
        public string DisplayTime => DateTimeOffset.FromUnixTimeSeconds(Time).ToString("yyyy-MM-dd HH:mm:ss.fff zzz");

        [JsonIgnore]
        public long PreciseTime { get; }

        [JsonProperty("content")]
        public string Content { get; }

        [JsonProperty("color")]
        public ConsoleColor Color { get; }

        public LogEntry(long time, long preciseTime, string content, ConsoleColor color)
        {
            Time = time;
            PreciseTime = preciseTime;
            Content = content;
            Color = color;

            HiddenList.Add(this);

            if (ConsoleSocketService.Authorized.Count > 0)
                ConsoleSocketService.Broadcast(this);
        }

        public LogEntry(DateTimeOffset time, string content, ConsoleColor color) : this(time.ToUnixTimeSeconds(), time.ToUnixTimeMilliseconds(), content, color)
        { }
    }
}
