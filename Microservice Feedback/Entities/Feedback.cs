using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Microservice_Feedback.Entities
{
    [Table("Feedback")]
    public partial class Feedback
    {
        [Key]
        public Guid FeedbackId { get; set; }
        public Guid FeedbackCategoryId { get; set; }
        public Guid ObjectStoreCheckId { get; set; }
        [Required]
        [StringLength(255)]
        public string Text { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public bool Resolved { get; set; }
        [StringLength(255)]
        public string Img { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [ForeignKey(nameof(FeedbackCategoryId))]
        [InverseProperty("Feedbacks")]
        public FeedbackCategory FeedbackCategory { get; set; }
    }
}
