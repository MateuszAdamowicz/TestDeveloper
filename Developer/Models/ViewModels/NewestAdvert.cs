﻿using System;
using Developer.Models.EntityModels;

namespace Developer.Models.ViewModels
{
    public class NewestAdvert
    {
        public string Number { get; set; }
        public string City { get; set; }
        public string Price { get; set; }
        public AdType AdType { get; set; }
        public DateTime CreatedAt { get; set; }
        public ShowListPhoto Picture { get; set; }
    }
}