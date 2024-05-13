using EdManagementSystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace EdManagementSystem.DataAccess.Data
{
    public partial class EdSystemDbContext : DbContext
    {
        public EdSystemDbContext()
        {
        }

        public EdSystemDbContext(DbContextOptions<EdSystemDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Squad> Squads { get; set; }

        public virtual DbSet<Schedule> Schedules { get; set; }

        public virtual DbSet<SocialMedium> SocialMedia { get; set; }

        public virtual DbSet<SquadStudent> SquadStudents { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<Teacher> Teachers { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<TechSupport> TechSupports { get; set; }

        public virtual DbSet<Material> Materials { get; set; }

        public virtual DbSet<Models.File> Files { get; set; }

        public virtual DbSet<Homework> Homeworks { get; set; }

        public virtual DbSet<HomeworkFile> HomeworkFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.Property(e => e.WeekDate).IsFixedLength();

                entity.HasOne(d => d.File).WithMany(p => p.Attendances).HasConstraintName("Attendance_ibfk_1");

                entity.HasOne(d => d.Squad).WithMany(p => p.Attendances).HasConstraintName("Attendance_ibfk_2");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId).HasName("PRIMARY");

                entity.HasOne(d => d.CourseTutorNavigation).WithMany(p => p.Courses).HasConstraintName("Course_ibfk_1");
            });

            modelBuilder.Entity<Models.File>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.HasOne(d => d.Course).WithMany(p => p.Homeworks).HasConstraintName("Homework_ibfk_1");

                entity.HasOne(d => d.Squad).WithMany(p => p.Homeworks)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Homework_ibfk_2");
            });

            modelBuilder.Entity<HomeworkFile>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.HasOne(d => d.File).WithMany(p => p.HomeworkFiles)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Homework_File_ibfk_1");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.Materials).HasConstraintName("Material_ibfk_1");

                entity.HasOne(d => d.IdFileNavigation).WithMany(p => p.Materials).HasConstraintName("Material_ibfk_3");

                entity.HasOne(d => d.IdSquadNavigation).WithMany(p => p.Materials)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Material_ibfk_2");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.HasOne(d => d.Squad).WithMany(p => p.Schedules).HasConstraintName("Schedule_ibfk_1");

                entity.HasOne(d => d.Teacher).WithMany(p => p.Schedules).HasConstraintName("Schedule_ibfk_2");
            });

            modelBuilder.Entity<SocialMedium>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.HasOne(d => d.IdTeacherNavigation).WithMany(p => p.SocialMedia).HasConstraintName("SocialMedia_ibfk_1");
            });

            modelBuilder.Entity<Squad>(entity =>
            {
                entity.HasKey(e => e.SquadId).HasName("PRIMARY");

                entity.HasOne(d => d.IdCourseNavigation).WithMany(p => p.Squads).HasConstraintName("Squad_ibfk_1");
            });

            modelBuilder.Entity<SquadStudent>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.HasOne(d => d.IdSquadNavigation).WithMany(p => p.SquadStudents).HasConstraintName("SquadStudent_ibfk_1");

                entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.SquadStudents).HasConstraintName("SquadStudent_ibfk_2");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId).HasName("PRIMARY");

                entity.Property(e => e.Rate).HasDefaultValueSql("'4'");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.TeacherId).HasName("PRIMARY");

                entity.Property(e => e.TeacherId).ValueGeneratedNever();
                entity.Property(e => e.PhoneNumber).IsFixedLength();
                entity.Property(e => e.Rate).HasDefaultValueSql("'4'");

                entity.HasOne(d => d.TeacherNavigation).WithOne(p => p.Teacher).HasConstraintName("Teacher_ibfk_1");
            });

            modelBuilder.Entity<TechSupport>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.Property(e => e.Status).HasDefaultValueSql("'в обработке'");

                entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TechSupports)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TechSupport_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PRIMARY");

                entity.Property(e => e.UserPassword).IsFixedLength();
                entity.Property(e => e.UserRole).HasDefaultValueSql("'teacher'");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}