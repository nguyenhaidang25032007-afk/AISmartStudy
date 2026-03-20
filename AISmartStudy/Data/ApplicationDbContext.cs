using Microsoft.EntityFrameworkCore;
using AISmartStudy.Models;

namespace AISmartStudy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
