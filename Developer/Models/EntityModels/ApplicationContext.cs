﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Developer.Models.EntityModels.Interfaces;

namespace Developer.Models.EntityModels
{
    public class ApplicationContext: DbContext, IApplicationContext
    {

        public IDbSet<Worker> Workers { get; set; }
        public IDbSet<Flat> Flats { get; set; }
        public IDbSet<House> Houses { get; set; }
        public IDbSet<Land> Lands { get; set; }
        public IDbSet<Photo> Photos { get; set; }
        public IDbSet<Offer> Offers { get; set; }
        public IDbSet<Mail> Mails { get; set; }
        public IDbSet<Statistics> Statisticses { get; set; } 
        

        public ApplicationContext() : base()
        {
            
        }

        

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}