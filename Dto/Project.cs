﻿using System;

namespace AutoCloner.VsOnline.Dto
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string State { get; set; }
        public string Visibility { get; set; }
        public int Revision { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
