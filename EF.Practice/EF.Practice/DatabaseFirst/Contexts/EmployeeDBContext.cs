#nullable disable

namespace EF.Practice.DatabaseFirst.Contexts
{
    using EF.Practice.DatabaseFirst.Entities;
    using Microsoft.EntityFrameworkCore;

    public partial class EmployeeDBContext : DbContext
    {
        public EmployeeDBContext()
        {
        }

        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<EmployeeDepartmentShift> EmployeeDepartmentShifts { get; set; }

        public virtual DbSet<EmployeePayHistory> EmployeePayHistories { get; set; }

        public virtual DbSet<JobCandidate> JobCandidates { get; set; }

        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<SalesPerson> SalesPeople { get; set; }

        public virtual DbSet<Shift> Shifts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=EmployeeDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department", "HumanResources");

                entity.HasIndex(e => e.Name, "IX_Department_Name")
                    .IsUnique()
                    .HasFilter("([Name] IS NOT NULL)");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.BusinessEntityId);

                entity.ToTable("Employee", "HumanResources");

                entity.HasIndex(e => e.LoginId, "IX_Employee_LoginID")
                    .IsUnique();

                entity.HasIndex(e => e.NationalIdnumber, "IX_Employee_NationalIDNumber")
                    .IsUnique();

                entity.HasIndex(e => e.PersonId, "IX_Employee_PersonId");

                entity.Property(e => e.LoginId).HasColumnName("LoginID");

                entity.Property(e => e.NationalIdnumber).HasColumnName("NationalIDNumber");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PersonId);
            });

            modelBuilder.Entity<EmployeeDepartmentShift>(entity =>
            {
                entity.HasKey(e => new { e.BusinessEntityId, e.DepartmentId, e.ShiftId, e.StartDate });

                entity.ToTable("EmployeeDepartmentShift", "HumanResources");

                entity.HasIndex(e => e.DepartmentId, "IX_EmployeeDepartmentShift_DepartmentId");

                entity.HasIndex(e => e.ShiftId, "IX_EmployeeDepartmentShift_ShiftId");

                entity.HasOne(d => d.BusinessEntity)
                    .WithMany(p => p.EmployeeDepartmentShifts)
                    .HasForeignKey(d => d.BusinessEntityId);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.EmployeeDepartmentShifts)
                    .HasForeignKey(d => d.DepartmentId);

                entity.HasOne(d => d.Shift)
                    .WithMany(p => p.EmployeeDepartmentShifts)
                    .HasForeignKey(d => d.ShiftId);
            });

            modelBuilder.Entity<EmployeePayHistory>(entity =>
            {
                entity.ToTable("EmployeePayHistory", "HumanResources");

                entity.HasIndex(e => e.BusinessEntityId, "IX_EmployeePayHistory_BusinessEntityId");

                entity.HasOne(d => d.BusinessEntity)
                    .WithMany(p => p.EmployeePayHistories)
                    .HasForeignKey(d => d.BusinessEntityId);
            });

            modelBuilder.Entity<JobCandidate>(entity =>
            {
                entity.ToTable("JobCandidates", "HumanResources");

                entity.HasIndex(e => e.BusinessEntityId, "IX_JobCandidates_BusinessEntityId");

                entity.HasOne(d => d.BusinessEntity)
                    .WithMany(p => p.JobCandidates)
                    .HasForeignKey(d => d.BusinessEntityId);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "Person");
            });

            modelBuilder.Entity<SalesPerson>(entity =>
            {
                entity.ToTable("SalesPerson", "Sales");

                entity.HasIndex(e => e.BusinessEntityId, "IX_SalesPerson_BusinessEntityId");

                entity.HasOne(d => d.BusinessEntity)
                    .WithMany(p => p.SalesPeople)
                    .HasForeignKey(d => d.BusinessEntityId);
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift", "HumanResources");

                entity.HasIndex(e => e.EndTime, "IX_Shift_EndTime")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "IX_Shift_Name")
                    .IsUnique()
                    .HasFilter("([Name] IS NOT NULL)");

                entity.HasIndex(e => e.StartTime, "IX_Shift_StartTime")
                    .IsUnique();
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
