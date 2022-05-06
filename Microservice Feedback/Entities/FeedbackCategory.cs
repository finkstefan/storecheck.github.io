using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Microservice_Feedback.Entities
{
    [Table("FeedbackCategory")]
    public partial class FeedbackCategory
    {
        public FeedbackCategory()
        {
            Feedbacks = new HashSet<Feedback>();
        }

        [Key]
        public Guid FeedbackCategoryId { get; set; }
        [StringLength(50)]
        public string FeedbackCategoryName { get; set; }

        [InverseProperty(nameof(Feedback.FeedbackCategory))]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
