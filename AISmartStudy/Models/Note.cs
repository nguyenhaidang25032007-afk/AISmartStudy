using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AISmartStudy.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public string ReferenceLinks { get; set; }

        [ForeignKey(nameof(StudyPlan))]
        public int StudyPlanId { get; set; }

        public StudyPlan StudyPlan { get; set; }
    }
}
