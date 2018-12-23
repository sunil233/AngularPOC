using System.Data.Entity;
using POC.Models;

namespace POC.Repository.Implementation
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("name=TimesheetDBEntities")
        {
            Database.SetInitializer<DatabaseContext>(null);
        }

        public DbSet<Registration> Registration { get; set; }
        public DbSet<Roles> Role { get; set; }
        public DbSet<ProjectMaster> ProjectMaster { get; set; }
        public DbSet<TimeSheetMaster> TimeSheetMaster { get; set; }
        public DbSet<TimeSheetDetails> TimeSheetDetails { get; set; }
        public DbSet<ExpenseModel> ExpenseModel { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<TimeSheetAuditTB> TimeSheetAuditTB { get; set; }
        public DbSet<ExpenseAuditTB> ExpenseAuditTB { get; set; }
        public DbSet<AuditTB> AuditTB { get; set; }
        public DbSet<DescriptionTB> DescriptionTB { get; set; }
        public DbSet<AssignedRoles> AssignedRoles { get; set; }
        public DbSet<DepartmentTB> Departments { get; set; }
        public DbSet<TaskTB> Tasks { get; set; }
        public DbSet<JobsTB> Jobs { get; set; }
        public DbSet<AssignedProjects> AssignedProjects { get; set; }
        public DbSet<NotificationsTB> NotificationsTBs { get; set; }
        public DbSet<DocumentTypes> DocumentTypes { get; set; }
    }
}
