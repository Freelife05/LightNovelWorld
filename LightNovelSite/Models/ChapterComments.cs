﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LightNovelSite.Models
{
    public class ChapterComments
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public int ChapterId { get; set; }
        [Required]
        public string Content { get; set; }

        //public int? ParentId { get; set; } // For replies
        //public ChapterComments Parent { get; set; } // Navigation property for parent comment
        //public ICollection<ChapterComments> Replies { get; set; } // Collection of child comments
    }
    //public class MyDbContext : DbContext
    //{
    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<ChapterComments>()
    //          .HasOne(c => c.Parent)
    //          .WithMany(c => c.Replies)
    //          .HasForeignKey(c => c.ParentId)
    //          .OnDelete(DeleteBehavior.ClientSetNull); // Optional: Set behavior on parent deletion
    //    }
    //}
}
