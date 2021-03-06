﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SlackBotCore.Objects
{
    public class SlackFile : SlackBaseApiObject
    {
        public readonly string Id;
        public readonly SlackUser User;

        public SlackFile(SlackBotApi api, string id, SlackUser user)
        {
            SetApi(api);
            Id = id;
            User = user;
        }

        public async Task<SlackResponse> AddReactionAsync(string emojiName)
        {
            return await Api.AddReactionAsync(this, emojiName);
        }

        public async Task<SlackResponse> RemoveReactionAsync(string emojiName)
        {
            return await Api.RemoveReactionAsync(this, emojiName);
        }

        public async Task<SlackResponse> AddStarAsync()
        {
            return await Api.AddStarAsync(this);
        }

        public async Task<SlackResponse> RemoveStarAsync()
        {
            return await Api.RemoveStarAsync(this);
        }
    }
}
