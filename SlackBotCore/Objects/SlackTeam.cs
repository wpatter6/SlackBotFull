﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SlackBotCore.Objects
{
    public class SlackTeam
    {
        [JsonIgnore]
        private SlackBotApi api;

        [JsonProperty("id")]
        public string Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("domain")]
        public string Domain;

        [JsonIgnore]
        public List<SlackChannel> Channels = new List<SlackChannel>();

        [JsonIgnore]
        public List<SlackUser> Users = new List<SlackUser>();

        public SlackTeam(SlackBotApi api)
        {
            this.api = api;
        }

        public SlackChannel GetChannel(string id)
        {
            return Channels.FirstOrDefault(x => x.Id == id);
        }

        public SlackUser GetUser(string id)
        {
            return Users.FirstOrDefault(x => x.Id == id);
        }

        public static SlackTeam FromData(dynamic data, SlackBotApi api)
        {
            var team = new SlackTeam(api)
            {
                Id = data.team.Value<string>("id"),
                Name = data.team.Value<string>("name"),
                Domain = data.team.Value<string>("domain")
            };

            foreach(var u in data.users)
            {
                if (!u.Value<bool>("deleted"))
                    team.Users.Add(new SlackUser()
                    {
                        Id = u.Value<string>("id"),
                        Name = u.Value<string>("name")
                    });
            }

            foreach(var c in data.channels)
            {
                if (c == null) continue;
                var channel = new SlackChannel(api)
                {
                    Id = c.Value<string>("id"),
                    Name = c.Value<string>("name"),
                    Purpose = c["purpose"]?.Value<string>("value"),
                    Topic = c["topic"]?.Value<string>("value"),
                    Team = team
                };

                if(c["members"] != null)
                    foreach(var u in c.members)
                    {
                        var user = team.Users.FirstOrDefault(x => x.Id == u.ToString());
                        if(user != null) channel.Members.Add(user);
                    }

                team.Channels.Add(channel);
            }
            
            return team;
        }
    }
}