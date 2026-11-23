using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicPerformanceRecord> AcademicPerformanceRecords { get; set; }

    public virtual DbSet<AttestationCommission> AttestationCommissions { get; set; }

    public virtual DbSet<AttestationCommissionMember> AttestationCommissionMembers { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }

    public virtual DbSet<FgosDiscipline> FgosDisciplines { get; set; }

    public virtual DbSet<FgosDisciplineMapping> FgosDisciplineMappings { get; set; }

    public virtual DbSet<ImportBatch> ImportBatches { get; set; }

    public virtual DbSet<License> Licenses { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentDisciplineGrade> StudentDisciplineGrades { get; set; }

    public virtual DbSet<StudentLicense> StudentLicenses { get; set; }

    public virtual DbSet<StudentLicenseHistory> StudentLicenseHistories { get; set; }

    public virtual DbSet<StudyGroup> StudyGroups { get; set; }

    public virtual DbSet<StudyPlan> StudyPlans { get; set; }

    public virtual DbSet<StudyPlanDiscipline> StudyPlanDisciplines { get; set; }

    public virtual DbSet<StudyProgram> StudyPrograms { get; set; }

    public virtual DbSet<Suborganization> Suborganizations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicPerformanceRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("academic_performance_records_pkey");

            entity.ToTable("academic_performance_records");

            entity.HasIndex(e => new { e.ImportBatchId, e.SourceRowNumber }, "ap_batch_row_key").IsUnique();

            entity.HasIndex(e => new { e.DisciplineId, e.SemesterId }, "ap_discipline_semester_idx");

            entity.HasIndex(e => new { e.StudentId, e.SemesterId }, "ap_student_semester_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllDisciplineTeachers).HasColumnName("allDisciplineTeachers");
            entity.Property(e => e.AvailableControlPoints).HasColumnName("availableControlPoints");
            entity.Property(e => e.CurrentDisciplinePercent).HasColumnName("currentDisciplinePercent");
            entity.Property(e => e.CurrentPointsFivePointScale).HasColumnName("currentPointsFivePointScale");
            entity.Property(e => e.CurrentStudentAveragePercent).HasColumnName("currentStudentAveragePercent");
            entity.Property(e => e.DisciplineId).HasColumnName("disciplineId");
            entity.Property(e => e.FinalPoints).HasColumnName("finalPoints");
            entity.Property(e => e.FinalPointsFivePointScale).HasColumnName("finalPointsFivePointScale");
            entity.Property(e => e.ImportBatchId).HasColumnName("importBatchId");
            entity.Property(e => e.MaxAvailableControlPoints).HasColumnName("maxAvailableControlPoints");
            entity.Property(e => e.MaxDisciplinePoints).HasColumnName("maxDisciplinePoints");
            entity.Property(e => e.MaxPointsByTopics).HasColumnName("maxPointsByTopics");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.RetakePoints).HasColumnName("retakePoints");
            entity.Property(e => e.RetakePointsNew).HasColumnName("retakePointsNew");
            entity.Property(e => e.SemesterId).HasColumnName("semesterId");
            entity.Property(e => e.SourceRowNumber).HasColumnName("sourceRowNumber");
            entity.Property(e => e.StudentId).HasColumnName("studentId");
            entity.Property(e => e.StudyGroupId).HasColumnName("studyGroupId");
            entity.Property(e => e.SuborganizationId).HasColumnName("suborganizationId");
            entity.Property(e => e.TaskAndTestPoints).HasColumnName("taskAndTestPoints");
            entity.Property(e => e.TeachersFromWorkload).HasColumnName("teachersFromWorkload");
            entity.Property(e => e.TeachersWhoGraded).HasColumnName("teachersWhoGraded");
        });

        modelBuilder.Entity<AttestationCommission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attestation_commissions_pkey");

            entity.ToTable("attestation_commissions");

            entity.HasIndex(e => new { e.StudyGroupId, e.AcademicYear, e.SemesterNumber }, "att_comm_group_term_idx");

            entity.HasIndex(e => new { e.StudyProgramId, e.AcademicYear, e.SemesterNumber }, "att_comm_prog_term_idx");

            entity.HasIndex(e => e.DisciplineId, "attestation_commissions_disciplineId_idx_e8c0205f");

            entity.HasIndex(e => e.StudyGroupId, "attestation_commissions_studyGroupId_idx_b7300630");

            entity.HasIndex(e => e.StudyProgramId, "attestation_commissions_studyProgramId_idx_4b5996c5");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicYear).HasColumnName("academicYear");
            entity.Property(e => e.DisciplineId).HasColumnName("disciplineId");
            entity.Property(e => e.SemesterLabel).HasColumnName("semesterLabel");
            entity.Property(e => e.SemesterNumber).HasColumnName("semesterNumber");
            entity.Property(e => e.StudyGroupId).HasColumnName("studyGroupId");
            entity.Property(e => e.StudyProgramId).HasColumnName("studyProgramId");

            entity.HasOne(d => d.Discipline).WithMany(p => p.AttestationCommissions)
                .HasForeignKey(d => d.DisciplineId)
                .HasConstraintName("attestation_commissions_disciplineId_fkey");

            entity.HasOne(d => d.StudyGroup).WithMany(p => p.AttestationCommissions)
                .HasForeignKey(d => d.StudyGroupId)
                .HasConstraintName("attestation_commissions_studyGroupId_fkey");

            entity.HasOne(d => d.StudyProgram).WithMany(p => p.AttestationCommissions)
                .HasForeignKey(d => d.StudyProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("attestation_commissions_studyProgramId_fkey");
        });

        modelBuilder.Entity<AttestationCommissionMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attestation_commission_members_pkey");

            entity.ToTable("attestation_commission_members");

            entity.HasIndex(e => new { e.CommissionId, e.SortOrder }, "att_comm_member_order_idx");

            entity.HasIndex(e => e.CommissionId, "attestation_commission_members_commissionId_idx_5ca7b813");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CommissionId).HasColumnName("commissionId");
            entity.Property(e => e.FullName).HasColumnName("fullName");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue(0)
                .HasColumnName("sortOrder");

            entity.HasOne(d => d.Commission).WithMany(p => p.AttestationCommissionMembers)
                .HasForeignKey(d => d.CommissionId)
                .HasConstraintName("attestation_commission_members_commissionId_fkey");
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("disciplines_pkey");

            entity.ToTable("disciplines");

            entity.HasIndex(e => new { e.SuborganizationId, e.Code, e.Name }, "disciplines_suborganizationId_code_name_key").IsUnique();

            entity.HasIndex(e => e.SuborganizationId, "disciplines_suborganizationId_idx_0edb9417");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SuborganizationId).HasColumnName("suborganizationId");

            entity.HasOne(d => d.Suborganization).WithMany(p => p.Disciplines)
                .HasForeignKey(d => d.SuborganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("disciplines_suborganizationId_fkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName).HasColumnName("fullName");
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_positions_pkey");

            entity.ToTable("employee_positions");

            entity.HasIndex(e => new { e.EmployeeId, e.PositionId }, "employee_position_key").IsUnique();

            entity.HasIndex(e => e.EmployeeId, "employee_positions_employeeId_idx_087dd4a6");

            entity.HasIndex(e => e.PositionId, "employee_positions_positionId_idx_f0417710");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employeeId");
            entity.Property(e => e.PositionId).HasColumnName("positionId");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeePositions)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("employee_positions_employeeId_fkey");

            entity.HasOne(d => d.Position).WithMany(p => p.EmployeePositions)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_positions_positionId_fkey");
        });

        modelBuilder.Entity<FgosDiscipline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fgos_disciplines_pkey");

            entity.ToTable("fgos_disciplines");

            entity.HasIndex(e => new { e.StudyProgramId, e.Code, e.Name }, "fgos_disciplines_studyProgramId_code_name_key").IsUnique();

            entity.HasIndex(e => e.StudyProgramId, "fgos_disciplines_studyProgramId_idx_4b5996c5");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.StudyProgramId).HasColumnName("studyProgramId");

            entity.HasOne(d => d.StudyProgram).WithMany(p => p.FgosDisciplines)
                .HasForeignKey(d => d.StudyProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fgos_disciplines_studyProgramId_fkey");
        });

        modelBuilder.Entity<FgosDisciplineMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fgos_discipline_mappings_pkey");

            entity.ToTable("fgos_discipline_mappings");

            entity.HasIndex(e => new { e.DisciplineId, e.FgosDisciplineId }, "fgos_discipline_mappings_disciplineId_fgosDisciplineId_key").IsUnique();

            entity.HasIndex(e => e.DisciplineId, "fgos_discipline_mappings_disciplineId_idx_e8c0205f");

            entity.HasIndex(e => e.FgosDisciplineId, "fgos_discipline_mappings_fgosDisciplineId_idx_94293b20");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DisciplineId).HasColumnName("disciplineId");
            entity.Property(e => e.FgosDisciplineId).HasColumnName("fgosDisciplineId");

            entity.HasOne(d => d.Discipline).WithMany(p => p.FgosDisciplineMappings)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fgos_discipline_mappings_disciplineId_fkey");

            entity.HasOne(d => d.FgosDiscipline).WithMany(p => p.FgosDisciplineMappings)
                .HasForeignKey(d => d.FgosDisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fgos_discipline_mappings_fgosDisciplineId_fkey");
        });

        modelBuilder.Entity<ImportBatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("import_batches_pkey");

            entity.ToTable("import_batches");

            entity.HasIndex(e => new { e.Semester, e.ImportedAt }, "import_batches_semester_importedAt_idx_87a6ccc8");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImportedAt)
                .HasPrecision(3)
                .HasDefaultValueSql("now()")
                .HasColumnName("importedAt");
            entity.Property(e => e.RowCount)
                .HasDefaultValue(0)
                .HasColumnName("rowCount");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.SourceFileName).HasColumnName("sourceFileName");
        });

        modelBuilder.Entity<License>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("licenses_pkey");

            entity.ToTable("licenses");

            entity.HasIndex(e => e.Name, "licenses_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organizations_pkey");

            entity.ToTable("organizations");

            entity.HasIndex(e => e.Name, "organizations_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("positions_pkey");

            entity.ToTable("positions");

            entity.HasIndex(e => e.Title, "positions_title_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("students_pkey");

            entity.ToTable("students");

            entity.HasIndex(e => e.Email, "students_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdmissionBase).HasColumnName("admissionBase");
            entity.Property(e => e.AdmissionYear).HasColumnName("admissionYear");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.Patronymic).HasColumnName("patronymic");
        });

        modelBuilder.Entity<StudentDisciplineGrade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_discipline_grades_pkey");

            entity.ToTable("student_discipline_grades");

            entity.HasIndex(e => e.DisciplineId, "student_discipline_grades_disciplineId_idx_e8c0205f");

            entity.HasIndex(e => e.EmployeeId, "student_discipline_grades_employeeId_idx_087dd4a6");

            entity.HasIndex(e => e.StudentId, "student_discipline_grades_studentId_idx_bf255322");

            entity.HasIndex(e => new { e.DisciplineId, e.AdmissionYear, e.SemesterNumber }, "student_grade_discipline_term_idx");

            entity.HasIndex(e => e.AcademicPerformanceId, "student_grade_performance_idx");

            entity.HasIndex(e => new { e.StudentId, e.AdmissionYear, e.SemesterNumber }, "student_grade_student_term_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicPerformanceId).HasColumnName("academicPerformanceId");
            entity.Property(e => e.AdmissionYear).HasColumnName("admissionYear");
            entity.Property(e => e.Date)
                .HasPrecision(3)
                .HasColumnName("date");
            entity.Property(e => e.DisciplineId).HasColumnName("disciplineId");
            entity.Property(e => e.EmployeeId).HasColumnName("employeeId");
            entity.Property(e => e.Grade).HasColumnName("grade");
            entity.Property(e => e.SemesterNumber).HasColumnName("semesterNumber");
            entity.Property(e => e.StudentId).HasColumnName("studentId");

            entity.HasOne(d => d.AcademicPerformance).WithMany(p => p.StudentDisciplineGrades)
                .HasForeignKey(d => d.AcademicPerformanceId)
                .HasConstraintName("student_discipline_grades_academicPerformanceId_fkey");

            entity.HasOne(d => d.Discipline).WithMany(p => p.StudentDisciplineGrades)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_discipline_grades_disciplineId_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.StudentDisciplineGrades)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_discipline_grades_employeeId_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentDisciplineGrades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_discipline_grades_studentId_fkey");
        });

        modelBuilder.Entity<StudentLicense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_licenses_pkey");

            entity.ToTable("student_licenses");

            entity.HasIndex(e => e.LicenseId, "student_licenses_licenseId_idx_bd8730e9");

            entity.HasIndex(e => e.StudentId, "student_licenses_studentId_idx_bf255322");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LicenseId).HasColumnName("licenseId");
            entity.Property(e => e.StudentId).HasColumnName("studentId");

            entity.HasOne(d => d.License).WithMany(p => p.StudentLicenses)
                .HasForeignKey(d => d.LicenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_licenses_licenseId_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentLicenses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_licenses_studentId_fkey");
        });

        modelBuilder.Entity<StudentLicenseHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_license_history_pkey");

            entity.ToTable("student_license_history");

            entity.HasIndex(e => e.StudentLicenseId, "student_license_history_studentLicenseId_idx_1232b097");

            entity.HasIndex(e => new { e.StudentLicenseId, e.AcademicYear, e.SemesterNumber }, "student_license_history_term_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicYear).HasColumnName("academicYear");
            entity.Property(e => e.SemesterLabel).HasColumnName("semesterLabel");
            entity.Property(e => e.SemesterNumber).HasColumnName("semesterNumber");
            entity.Property(e => e.StudentLicenseId).HasColumnName("studentLicenseId");

            entity.HasOne(d => d.StudentLicense).WithMany(p => p.StudentLicenseHistories)
                .HasForeignKey(d => d.StudentLicenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("student_license_history_studentLicenseId_fkey");
        });

        modelBuilder.Entity<StudyGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("study_groups_pkey");

            entity.ToTable("study_groups");

            entity.HasIndex(e => e.StudyPlanId, "study_groups_study_plan_id_idx_07733d24");

            entity.HasIndex(e => e.StudyProgramId, "study_groups_study_program_id_idx_cdb5c4cf");

            entity.HasIndex(e => e.SuborganizationId, "study_groups_suborganization_id_idx_a016913b");

            entity.HasIndex(e => new { e.SuborganizationId, e.Name }, "study_groups_suborganization_id_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.StudyPlanId).HasColumnName("study_plan_id");
            entity.Property(e => e.StudyProgramId).HasColumnName("study_program_id");
            entity.Property(e => e.SuborganizationId).HasColumnName("suborganization_id");

            entity.HasOne(d => d.StudyPlan).WithMany(p => p.StudyGroups)
                .HasForeignKey(d => d.StudyPlanId)
                .HasConstraintName("study_groups_study_plan_id_fkey");

            entity.HasOne(d => d.StudyProgram).WithMany(p => p.StudyGroups)
                .HasForeignKey(d => d.StudyProgramId)
                .HasConstraintName("study_groups_study_program_id_fkey");

            entity.HasOne(d => d.Suborganization).WithMany(p => p.StudyGroups)
                .HasForeignKey(d => d.SuborganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("study_groups_suborganization_id_fkey");
        });

        modelBuilder.Entity<StudyPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("study_plans_pkey");

            entity.ToTable("study_plans");

            entity.HasIndex(e => new { e.StudyProgramId, e.AdmissionBase, e.AdmissionYear }, "study_plan_program_cohort_key").IsUnique();

            entity.HasIndex(e => e.StudyProgramId, "study_plans_studyProgramId_idx_4b5996c5");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdmissionBase).HasColumnName("admissionBase");
            entity.Property(e => e.AdmissionYear).HasColumnName("admissionYear");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SourceFileName).HasColumnName("sourceFileName");
            entity.Property(e => e.StudyProgramId).HasColumnName("studyProgramId");

            entity.HasOne(d => d.StudyProgram).WithMany(p => p.StudyPlans)
                .HasForeignKey(d => d.StudyProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("study_plans_studyProgramId_fkey");
        });

        modelBuilder.Entity<StudyPlanDiscipline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("study_plan_disciplines_pkey");

            entity.ToTable("study_plan_disciplines");

            entity.HasIndex(e => new { e.StudyPlanId, e.SemesterNumber }, "study_plan_discipline_term_idx");

            entity.HasIndex(e => e.FgosDisciplineId, "study_plan_disciplines_fgosDisciplineId_idx_94293b20");

            entity.HasIndex(e => e.StudyPlanId, "study_plan_disciplines_studyPlanId_idx_5ee30651");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ControlForm).HasColumnName("controlForm");
            entity.Property(e => e.FgosDisciplineId).HasColumnName("fgosDisciplineId");
            entity.Property(e => e.PlannedHours).HasColumnName("plannedHours");
            entity.Property(e => e.SemesterNumber).HasColumnName("semesterNumber");
            entity.Property(e => e.StudyPlanId).HasColumnName("studyPlanId");

            entity.HasOne(d => d.FgosDiscipline).WithMany(p => p.StudyPlanDisciplines)
                .HasForeignKey(d => d.FgosDisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("study_plan_disciplines_fgosDisciplineId_fkey");

            entity.HasOne(d => d.StudyPlan).WithMany(p => p.StudyPlanDisciplines)
                .HasForeignKey(d => d.StudyPlanId)
                .HasConstraintName("study_plan_disciplines_studyPlanId_fkey");
        });

        modelBuilder.Entity<StudyProgram>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("study_programs_pkey");

            entity.ToTable("study_programs");

            entity.HasIndex(e => e.SuborganizationId, "study_programs_suborganizationId_idx_0edb9417");

            entity.HasIndex(e => new { e.SuborganizationId, e.SpecialtyCode }, "study_programs_suborganizationId_specialtyCode_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Qualification).HasColumnName("qualification");
            entity.Property(e => e.SpecialtyCode).HasColumnName("specialtyCode");
            entity.Property(e => e.SuborganizationId).HasColumnName("suborganizationId");

            entity.HasOne(d => d.Suborganization).WithMany(p => p.StudyPrograms)
                .HasForeignKey(d => d.SuborganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("study_programs_suborganizationId_fkey");
        });

        modelBuilder.Entity<Suborganization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suborganizations_pkey");

            entity.ToTable("suborganizations");

            entity.HasIndex(e => e.OrganizationId, "suborganizations_organizationId_idx_2e17ef41");

            entity.HasIndex(e => new { e.OrganizationId, e.Name }, "suborganizations_organizationId_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");

            entity.HasOne(d => d.Organization).WithMany(p => p.Suborganizations)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("suborganizations_organizationId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
