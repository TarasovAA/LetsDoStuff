﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LetsDoStuff.WebApi.Services.DTO
{
    public class ParticipationResponseForUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public ActivityCreatorResponse Creator { get; set; }

        public List<string> Tags { get; set; }

        public bool AcceptAsParticipant { get; set; }

        public ParticipationResponseForUser()
        {
            Tags = new List<string>();
        }
    }
}
