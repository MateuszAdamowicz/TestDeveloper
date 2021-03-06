﻿using System;

namespace Developer.Models.EntityModels.Interfaces
{
    public interface IFlat
    {
        string Location { get; set; }
        string Area { get; set; }
        string Storey { get; set; }
        string TechnicalCondition { get; set; }
        string Rooms { get; set; }
        string Heating { get; set; }
        string Rent { get; set; }
        string Ownership { get; set; }
        string PricePerMeter { get; set; }
        bool ToLet { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string Details { get; set; }
        string City { get; set; }
        string Price { get; set; }
        Worker Worker { get; set; }
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        bool Visible { get; set; }
        bool Deleted { get; set; }
    }
}