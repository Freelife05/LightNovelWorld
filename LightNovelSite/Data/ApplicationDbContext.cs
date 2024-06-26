﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LightNovelSite.Models;

namespace LightNovelSite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LightNovelSite.Models.Novel>? Novels { get; set; }
        public DbSet<LightNovelSite.Models.Chapter>? Chapter { get; set; }
        public DbSet<LightNovelSite.Models.NamesToLinks>? NamesToLinks { get; set; }
        public DbSet<LightNovelSite.Models.ChapterComments>? Comments { get; set; }



        
    }
}