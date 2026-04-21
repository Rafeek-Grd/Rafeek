using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using System.Security.Cryptography;

namespace Rafeek.Persistence.Seed
{
    public static class RafeekDbSeeder
    {
        private static string LogPath = "SeedingLog.txt";

        private static void Log(string message)
        {
            var logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
            Console.WriteLine(message);
            try { File.AppendAllText(LogPath, logEntry); } catch { }
        }

        private static async Task SeedStageAsync(string stageName, Func<Task> seedAction)
        {
            Log($"[Seeder] --- {stageName} ---");
            try
            {
                await seedAction();
                Log($"[Seeder] Completed: {stageName}");
            }
            catch (Exception ex)
            {
                Log($"[Seeder] ERROR in {stageName}: {ex.Message}");
                // We don't rethrow to allow subsequent stages to proceed
            }
        }

        public static async Task SeedAsync(RafeekDbContext context, RafeekIdentityDbContext identityContext, UserManager<ApplicationUser> userManager)
        {
            if (File.Exists(LogPath)) File.Delete(LogPath);
            Log("[Seeder] Starting Database Seeding...");

            Randomizer.Seed = new Random(8675309);
            context.ChangeTracker.AutoDetectChangesEnabled = false;

            var departmentsData = new[]
            {
                new { Name = "Computer Science & IT", Code = "CS" },
                new { Name = "Artificial Intelligence", Code = "AI" },
                new { Name = "Information Technology", Code = "IT" },
                new { Name = "Information Systems", Code = "IS" },
                new { Name = "Software Engineering", Code = "SWE" },
                new { Name = "Engineering", Code = "ENG" },
                new { Name = "Business Administration", Code = "BUS" },
                new { Name = "Faculty of Medicine", Code = "MED" },
                new { Name = "Faculty of Arts", Code = "ART" }
            };

            var coursesData = new Dictionary<string, (string Code, string Title)[]>
            {
                ["CS"] = new[] { ("CS101", "Introduction to Programming"), ("CS102", "Advanced Programming"), ("CS201", "Data Structures"), ("CS301", "Database Systems") },
                ["AI"] = new[] { ("AI201", "Machine Learning Foundations"), ("AI202", "Neural Networks & Deep Learning"), ("AI401", "Expert Systems") },
                ["IT"] = new[] { ("IT301", "Network Administration"), ("IT302", "Cloud Infrastructure") },
                ["IS"] = new[] { ("IS401", "Management Information Systems"), ("IS402", "Enterprise Resource Planning") },
                ["SWE"] = new[] { ("SWE501", "Software Architecture"), ("SWE502", "Quality Assurance & Testing") },
                ["ENG"] = new[] { ("ENG105", "Circuit Analysis"), ("ENG210", "Thermodynamics"), ("ENG315", "Material Science") },
                ["BUS"] = new[] { ("BUS110", "Macroeconomics"), ("BUS220", "Financial Accounting"), ("BUS330", "Marketing Principles") },
                ["MED"] = new[] { ("MED101", "Human Anatomy"), ("MED201", "Biochemistry"), ("MED301", "Pathology") },
                ["ART"] = new[] { ("ART100", "History of Art"), ("ART210", "Modern Philosophy") }
            };

            // 1. Roles
            Log("[Seeder] Stage 1: Seeding Roles...");
            var roles = new[] { "Admin", "SubAdmin", "Student", "Instructor", "Doctor", "Staff" };
            foreach (var role in roles)
            {
                if (!await identityContext.Roles.AnyAsync(r => r.Name == role))
                {
                    identityContext.Roles.Add(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = role, NormalizedName = role.ToUpper() });
                }
            }
            await identityContext.SaveChangesAsync();

            var departments = await context.Departments.ToListAsync();
            if (!departments.Any())
            {
                departments = departmentsData.Select(d => new Department
                {
                    Id = Guid.NewGuid(),
                    Name = d.Name,
                    Code = d.Code,
                    Description = $"The {d.Name} department focusing on academic excellence and research.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Seeder",
                    IsActive = true
                }).ToList();

                context.Departments.AddRange(departments);
                await context.SaveChangesAsync();
            }

            // 3. Courses
            var courses = await context.Courses.ToListAsync();
            if (!courses.Any())
            {
                foreach (var dept in departments)
                {
                    if (coursesData.TryGetValue(dept.Code, out var deptCourses))
                    {
                        foreach (var cData in deptCourses)
                        {
                            courses.Add(new Course
                            {
                                Id = Guid.NewGuid(),
                                Code = cData.Code,
                                Title = cData.Title,
                                Description = $"An intensive course on {cData.Title} providing core foundational knowledge.",
                                CreditHours = new Random().Next(2, 4),
                                DepartmentId = dept.Id,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "Seeder",
                                IsActive = true
                            });
                        }
                    }
                }
                context.Courses.AddRange(courses);
                await context.SaveChangesAsync();
            }

            // 4. Academic Years
            var yearFaker = new Faker<AcademicYear>("en")
                .RuleFor(y => y.Id, f => Guid.NewGuid())
                .RuleFor(y => y.Name, f => $"202{f.IndexVariable++}/202{f.IndexVariable}")
                .RuleFor(y => y.StartDate, f => f.Date.Past(1))
                .RuleFor(y => y.EndDate, (f, y) => y.StartDate.AddYears(1))
                .RuleFor(y => y.IsCurrentYear, f => false)
                .RuleFor(y => y.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(y => y.CreatedBy, f => "Seeder")
                .RuleFor(y => y.IsActive, f => true);

            var academicYears = await context.AcademicYears.ToListAsync();
            if (!academicYears.Any())
            {
                var currentYear = new AcademicYear
                {
                    Id = Guid.NewGuid(),
                    Name = "2025/2026",
                    StartDate = new DateTime(2025, 9, 1),
                    EndDate = new DateTime(2026, 6, 30),
                    IsCurrentYear = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Seeder",
                    IsActive = true
                };
                academicYears = yearFaker.Generate(2);
                academicYears.Add(currentYear);
                context.AcademicYears.AddRange(academicYears);
                await context.SaveChangesAsync();
            }
            
            // 5. Academic Terms
            var termFaker = new Faker<AcademicTerm>("en")
                .RuleFor(t => t.Id, f => Guid.NewGuid())
                .RuleFor(t => t.AcademicYearId, f => f.PickRandom(academicYears).Id)
                .RuleFor(t => (int)t.TermType, f => f.Random.Int(0, 2)) // 0: Fall, 1: Spring, 2: Summer
                .RuleFor(t => t.Name, (f, t) => {
                    var yearName = academicYears.First(y => y.Id == t.AcademicYearId).Name.Split('/')[0];
                    return $"{t.TermType} {yearName}";
                })
                .RuleFor(t => t.StartDate, (f, t) => {
                    var year = academicYears.First(y => y.Id == t.AcademicYearId);
                    return t.TermType switch {
                        Rafeek.Domain.Enums.TermType.Fall => year.StartDate.AddMonths(1),
                        Rafeek.Domain.Enums.TermType.Spring => year.StartDate.AddMonths(5),
                        Rafeek.Domain.Enums.TermType.Summer => year.StartDate.AddMonths(9),
                        _ => year.StartDate
                    };
                })
                .RuleFor(t => t.EndDate, (f, t) => t.StartDate.AddMonths(4))
                .RuleFor(t => t.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(t => t.CreatedBy, f => "Seeder")
                .RuleFor(t => t.IsActive, f => true);

            var academicTerms = await context.AcademicTerms.ToListAsync();
            if (!academicTerms.Any())
            {
                academicTerms = termFaker.Generate(6);
                context.AcademicTerms.AddRange(academicTerms);
                await context.SaveChangesAsync();
            }

            // 6. Users - Existing or New (Max 50 if empty)
            Log("[Seeder] Stage 6: Checking Users...");
            var users = await identityContext.Users.ToListAsync();
            var f = new Faker("en");
            
            if (!users.Any())
            {
                Log("[Seeder] No users found. Generating 50 new users...");
                var userFaker = new Faker<ApplicationUser>("en")
                    .RuleFor(u => u.Id, f => Guid.NewGuid())
                    .RuleFor(u => u.UserName, f => f.Internet.UserName() + f.UniqueIndex)
                    .RuleFor(u => u.Email, (f, u) => $"{u.UserName}@rafeek.edu")
                    .RuleFor(u => u.FullName, f => f.Name.FullName())
                    .RuleFor(u => u.NationalId, f => "29" + f.Random.Number(70, 99) + f.Random.Replace("##########").Substring(0, 10))
                    .RuleFor(u => u.IsUniversityEmailActivated, f => true)
                    .RuleFor(u => u.Address, f => f.Address.FullAddress())
                    .RuleFor(u => u.PhoneNumber, f => f.PickRandom(new[] { "010", "011", "012", "015" }) + f.Random.Replace("########"));

                var generatedUsers = userFaker.Generate(50);
                int successCount = 0;

                foreach (var user in generatedUsers)
                {
                    try 
                    {
                        var result = await userManager.CreateAsync(user, "P@ssw0rd123!");
                        if (result.Succeeded)
                        {
                            successCount++;
                            var userRoles = new List<string>();
                            var dice = f.Random.Double();

                            // Enrollment logic based on 8 valid combinations
                            if (dice < 0.60) // 60% Students
                            {
                                userRoles.Add("Student");
                                user.UserTypes = UserType.Student;
                            }
                            else if (dice < 0.70) // 10% Staff
                            {
                                userRoles.Add("Staff");
                                user.UserTypes = UserType.Staff;
                            }
                            else if (dice < 0.80) // 10% Instructor
                            {
                                userRoles.Add("Instructor");
                                user.UserTypes = UserType.Instructor;
                            }
                            else if (dice < 0.88) // 8% Doctor
                            {
                                userRoles.Add("Doctor");
                                user.UserTypes = UserType.Doctor;
                            }
                            else if (dice < 0.92) // 4% Admin
                            {
                                userRoles.Add("Admin");
                                user.UserTypes = UserType.Admin;
                            }
                            else if (dice < 0.94) // 2% SubAdmin
                            {
                                userRoles.Add("SubAdmin");
                                user.UserTypes = UserType.SubAdmin;
                            }
                            else if (dice < 0.97) // 3% Doctor + Admin
                            {
                                userRoles.Add("Doctor");
                                userRoles.Add("Admin");
                                user.UserTypes = UserType.Doctor | UserType.Admin;
                            }
                            else // 3% Doctor + SubAdmin
                            {
                                userRoles.Add("Doctor");
                                userRoles.Add("SubAdmin");
                                user.UserTypes = UserType.Doctor | UserType.SubAdmin;
                            }

                            await userManager.AddToRolesAsync(user, userRoles);
                            await userManager.UpdateAsync(user);
                        }
                        else
                        {
                            var errs = string.Join(", ", result.Errors.Select(e => e.Description));
                            Log($"[Seeder] Identity Error for {user.UserName}: {errs}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"[Seeder] Exception creating user {user.UserName}: {ex.Message}");
                    }
                }
                
                // Fallback: If 0 users created, create one default admin to prevent downstream failure
                if (successCount == 0)
                {
                    Log("[Seeder] Critical: 0 users created. Attempting fallback admin user...");
                    var fallbackAdmin = new ApplicationUser { UserName = "admin_fallback", Email = "admin@rafeek.edu", FullName = "System Admin", NationalId = "00000000000000", UserTypes = UserType.Staff };
                    var res = await userManager.CreateAsync(fallbackAdmin, "P@ssw0rd123!");
                    if (res.Succeeded) await userManager.AddToRoleAsync(fallbackAdmin, "Admin");
                }

                await identityContext.SaveChangesAsync();
                
                // Refresh the list to include successfully created users
                users = await identityContext.Users.ToListAsync();
                Log($"[Seeder] Final user database count: {users.Count}");
            }
            
            // Identify role-based user lists
            var studentUsers = new List<ApplicationUser>();
            var instructorUsers = new List<ApplicationUser>();
            var doctorUsers = new List<ApplicationUser>();
            var staffUsers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                var rolesList = await userManager.GetRolesAsync(user);
                if (rolesList.Contains("Student")) studentUsers.Add(user);
                if (rolesList.Contains("Instructor")) instructorUsers.Add(user);
                if (rolesList.Contains("Doctor")) doctorUsers.Add(user);
                if (rolesList.Contains("Staff")) staffUsers.Add(user);
            }

            var students = new List<Student>();
            var instructors = new List<Instructor>();
            var doctors = new List<Doctor>();
            var staffs = new List<Staff>();

            // Create Staff
            Log("[Seeder] Stage 7: Seeding Academic Profiles...");
            var existingStaffs = await context.Staffs.ToListAsync();
            if (!existingStaffs.Any())
            {
                var targetUsers = staffUsers.Any() ? staffUsers : users.Take(5).ToList();
                for (int s = 0; s < targetUsers.Count; s++)
                {
                    staffs.Add(new Staff { Id = Guid.NewGuid(), UserId = targetUsers[s].Id, EmployeeCode = $"STF{1000 + s}", CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true });
                }
                await context.Staffs.AddRangeAsync(staffs);
                await context.SaveChangesAsync();
            }
            else { staffs = existingStaffs; }

            // Create Doctors
            var existingDoctors = await context.Doctors.ToListAsync();
            if (!existingDoctors.Any())
            {
                var targetUsers = doctorUsers.Any() ? doctorUsers : users.Skip(5).Take(10).ToList();
                for (int d = 0; d < targetUsers.Count; d++)
                {
                    doctors.Add(new Doctor { Id = Guid.NewGuid(), UserId = targetUsers[d].Id, EmployeeCode = $"DOC{1000 + d}", DepartmentId = f.PickRandom(departments).Id, IsAcademicAdvisor = f.Random.Bool(0.7f), CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true });
                }
                await context.Doctors.AddRangeAsync(doctors);
                await context.SaveChangesAsync();
            }
            else { doctors = existingDoctors; }

            // Create Instructors
            var existingInstructors = await context.Instructors.ToListAsync();
            if (!existingInstructors.Any())
            {
                // Instructors can be the specific list + all Doctors
                var targetUsers = instructorUsers.Any() ? instructorUsers : users.Skip(15).Take(10).ToList();
                foreach (var user in targetUsers)
                {
                    instructors.Add(new Instructor { Id = Guid.NewGuid(), UserId = user.Id, EmployeeCode = $"INS{f.Random.Number(1000, 9999)}", DepartmentId = f.PickRandom(departments).Id, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true });
                }
                
                // Also ensure Doctors are Instructors (common for Professors)
                foreach (var doc in doctors)
                {
                    if (!instructors.Any(i => i.UserId == doc.UserId))
                    {
                        instructors.Add(new Instructor { Id = Guid.NewGuid(), UserId = doc.UserId, EmployeeCode = $"INS-DOC{f.Random.Number(100, 999)}", DepartmentId = doc.DepartmentId, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true });
                    }
                }

                await context.Instructors.AddRangeAsync(instructors);
                await context.SaveChangesAsync();
            }
            else { instructors = existingInstructors; }

            // Create Students and Academic Profiles
            var studentProfiles = new List<StudentAcademicProfile>();
            var existingStudents = await context.Students.ToListAsync();
            if (!existingStudents.Any())
            {
                var advisorList = doctors.Where(d => d.IsAcademicAdvisor).ToList();
                for (int i = 0; i < studentUsers.Count; i++)
                {
                    var user = studentUsers[i];
                    var studentId = Guid.NewGuid();
                    var profileId = Guid.NewGuid();

                    var level = f.Random.Int(1, 4);
                    var term = (level * 2) - f.Random.Int(0, 1);
                    
                    students.Add(new Student { 
                        Id = studentId, 
                        UserId = user.Id, 
                        UniversityCode = $"{DateTime.UtcNow.Year}{10000 + i}", 
                        DepartmentId = f.PickRandom(departments).Id, 
                        AcademicProfileId = profileId,
                        AcademicAdvisorId = advisorList.Any() ? f.PickRandom(advisorList).Id : (doctors.Any() ? f.PickRandom(doctors).Id : null),
                        Level = level,
                        Term = term,
                        Status = Rafeek.Domain.Enums.StudentStatus.Active,
                        CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true 
                    });

                    studentProfiles.Add(new StudentAcademicProfile {
                        Id = profileId,
                        StudentId = studentId,
                        GPA = 0.0f,
                        CGPA = 0.0f,
                        CompletedCredits = (level - 1) * 30 + f.Random.Int(0, 15),
                        RemainingCredits = 144 - ((level - 1) * 30),
                        Standing = "Good Standing",
                        CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true 
                    });
                }
                await context.Students.AddRangeAsync(students);
                await context.StudentAcademicProfiles.AddRangeAsync(studentProfiles);
                await context.SaveChangesAsync();
                Log($"[Seeder] Seeded {students.Count} Student profiles.");
            }
            else { 
                students = existingStudents; 
                studentProfiles = await context.StudentAcademicProfiles.ToListAsync();
            }

            // 7. Sections
            Log("[Seeder] Stage 8: Seeding Sections...");
            var sections = await context.Sections.ToListAsync();
            if (!sections.Any())
            {
                foreach (var course in courses)
                {
                    // Every course gets at least 2 sections
                    var sectionCount = f.Random.Int(2, 4);
                    for (int i = 0; i < sectionCount; i++)
                    {
                        var startTime = new TimeSpan(f.PickRandom(new[] { 8, 10, 12, 14, 16 }), 0, 0);
                        sections.Add(new Section
                        {
                            Id = Guid.NewGuid(),
                            CourseId = course.Id,
                            InstructorId = instructors.Any() ? f.PickRandom(instructors).Id : (doctors.Any() ? f.PickRandom(doctors).Id : Guid.Empty),
                            Day = f.PickRandom(new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday" }),
                            StartTime = startTime,
                            EndTime = startTime.Add(new TimeSpan(2, 0, 0)),
                            Time = $"{startTime:hh\\:mm} - {startTime.Add(new TimeSpan(2, 0, 0)):hh\\:mm}",
                            Capacity = f.Random.Int(30, 60),
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "Seeder",
                            IsActive = true
                        });
                    }
                }
                context.Sections.AddRange(sections);
                await context.SaveChangesAsync();
            }

            // 8. Enrollments & Grades
            var enrollments = await context.Enrollments.ToListAsync();
            var grades = await context.Grades.ToListAsync();

            // Always clear and re-seed grades/enrollments to ensure GPA consistency and 100-scale scores
            if (enrollments.Any() || grades.Any())
            {
                Log("[Seeder] Clearing existing enrollments and grades for fresh seeding...");
                context.Grades.RemoveRange(grades);
                context.Enrollments.RemoveRange(enrollments);
                await context.SaveChangesAsync();
                
                enrollments = new List<Enrollment>();
                grades = new List<Grade>();
            }

            if (!enrollments.Any() && students.Any() && sections.Any())
            {
                Log("[Seeder] Seeding Enrollments and calculating GPA/CGPA...");
                foreach (var s in students)
                {
                    var studentGrades = new List<float>();
                    var coursesToEnroll = new List<Section>();
                    
                    // 70% focus on Major (Department) courses, 30% Electives
                    var majorSections = sections.Where(sec => sec.Course.DepartmentId == s.DepartmentId).ToList();
                    var electiveSections = sections.Where(sec => sec.Course.DepartmentId != s.DepartmentId).ToList();

                    var enrCount = f.Random.Int(5, 7);
                    var majorCount = (int)(enrCount * 0.7);
                    
                    if (majorSections.Any())
                        coursesToEnroll.AddRange(f.PickRandom(majorSections, Math.Min(majorCount, majorSections.Count)));
                    
                    if (electiveSections.Any())
                        coursesToEnroll.AddRange(f.PickRandom(electiveSections, Math.Min(enrCount - coursesToEnroll.Count, electiveSections.Count)));

                    foreach (var sec in coursesToEnroll)
                    {
                        // Bell-curve grade distribution simulation
                        // WeightedRandom: 10% Excellent (90-95), 40% Good (80-89), 30% Fair (70-79), 15% Pass (60-69), 5% Fail (<60)
                        var gradeCategory = f.Random.WeightedRandom(
                            new[] { "A", "B", "C", "D", "F" },
                            new[] { 0.10f, 0.40f, 0.30f, 0.15f, 0.05f }
                        );

                        float score = gradeCategory switch {
                            "A" => (float)Math.Round(f.Random.Double(90.0, 98.0), 2),
                            "B" => (float)Math.Round(f.Random.Double(80.0, 89.0), 2),
                            "C" => (float)Math.Round(f.Random.Double(70.0, 79.0), 2),
                            "D" => (float)Math.Round(f.Random.Double(60.0, 69.0), 2),
                            _ => (float)Math.Round(f.Random.Double(40.0, 59.0), 2)
                        };
                        
                        string gradeLetter;
                        float gpaPoint;

                        if (score >= 90) { gradeLetter = "A"; gpaPoint = 4.0f; }
                        else if (score >= 85) { gradeLetter = "A-"; gpaPoint = 3.7f; }
                        else if (score >= 80) { gradeLetter = "B+"; gpaPoint = 3.3f; }
                        else if (score >= 75) { gradeLetter = "B"; gpaPoint = 3.0f; }
                        else if (score >= 70) { gradeLetter = "B-"; gpaPoint = 2.7f; }
                        else if (score >= 65) { gradeLetter = "C+"; gpaPoint = 2.3f; }
                        else if (score >= 60) { gradeLetter = "C"; gpaPoint = 2.0f; }
                        else { gradeLetter = "F"; gpaPoint = 0.0f; }

                        studentGrades.Add(gpaPoint);

                        var enr = new Enrollment {
                            Id = Guid.NewGuid(),
                            StudentId = s.Id,
                            CourseId = sec.CourseId,
                            SectionId = sec.Id,
                            Status = "Completed",
                            Grade = gradeLetter,
                            CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true
                        };
                        enrollments.Add(enr);

                        grades.Add(new Grade {
                            Id = Guid.NewGuid(),
                            EnrollmentId = enr.Id,
                            GradeValue = gpaPoint,
                            AbsoluteScore = score,
                            TermGPA = gpaPoint,
                            CGPA = gpaPoint,
                            CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true
                        });
                    }

                    var profile = studentProfiles.FirstOrDefault(p => p.StudentId == s.Id);
                    if (profile != null)
                    {
                        var avgGpa = studentGrades.Any() ? (float)Math.Round(studentGrades.Average(), 2) : (float)Math.Round(f.Random.Double(2.0, 3.8), 2);
                        profile.GPA = Math.Clamp(avgGpa, 0.0f, 4.0f);
                        profile.CGPA = Math.Clamp((float)Math.Round(profile.GPA - f.Random.Double(0, 0.2), 2), 0.0f, 4.0f);
                    }
                }
                context.Enrollments.AddRange(enrollments);
                context.Grades.AddRange(grades);
                context.StudentAcademicProfiles.UpdateRange(studentProfiles);
                await context.SaveChangesAsync();
            }

            // 11. Course Prerequisites
            await SeedStageAsync("Course Prerequisites", async () => {
                var prerequisites = await context.CoursePrerequisites.ToListAsync();
                if (!prerequisites.Any() && courses.Count > 1)
                {
                    var prFaker = new Faker<CoursePrerequisite>("en")
                        .RuleFor(x => x.Id, f => Guid.NewGuid())
                        .RuleFor(x => x.CourseId, f => f.PickRandom(courses).Id)
                        .RuleFor(x => x.PrerequisiteId, f => f.PickRandom(courses).Id)
                        .RuleFor(s => s.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(s => s.CreatedBy, f => "Seeder")
                        .RuleFor(s => s.IsActive, f => true);

                    context.CoursePrerequisites.AddRange(prFaker.Generate(10).Where(p => p.CourseId != p.PrerequisiteId));
                    await context.SaveChangesAsync();
                }
            });

            // 10. AI & Academic Logs (50 records each)
            await SeedStageAsync("AI & Academic Logs", async () => {
                if (!await context.GPASimulatorLogs.AnyAsync() && (students.Any() || users.Any()))
                {
                    var studentSource = students.Any() ? students.Select(s => s.Id).ToList() : users.Take(10).Select(u => u.Id).ToList();
                    var gpaLogs = new Faker<GPASimulatorLog>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.StudentId, f => f.PickRandom(studentSource)).RuleFor(x => x.ExpectedGPA, f => (float)Math.Round(f.Random.Double(2.0, 4.0), 2)).RuleFor(x => x.PredictedCGPA, f => (float)Math.Round(f.Random.Double(2.0, 4.0), 2)).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.GPASimulatorLogs.AddRange(gpaLogs);
                    await context.SaveChangesAsync();
                }

                if (!await context.AcademicFeedbacks.AnyAsync() && students.Any())
                {
                    var feedbacks = new Faker<AcademicFeedback>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.StudentId, f => f.PickRandom(students).Id).RuleFor(x => x.Strength, f => f.Lorem.Word()).RuleFor(x => x.Weakness, f => f.Lorem.Word()).RuleFor(x => x.Recommendation, f => f.Lorem.Sentence()).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.AcademicFeedbacks.AddRange(feedbacks);
                    await context.SaveChangesAsync();
                }

                if (!await context.AICourseRecommendations.AnyAsync() && students.Any())
                {
                    var aiReco = new Faker<AICourseRecommendation>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.StudentId, f => f.PickRandom(students).Id).RuleFor(x => x.CourseId, f => f.PickRandom(courses).Id).RuleFor(x => x.Reason, f => f.Lorem.Sentence()).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.AICourseRecommendations.AddRange(aiReco);
                    await context.SaveChangesAsync();
                }

                if (!await context.CareerSuggestions.AnyAsync() && students.Any())
                {
                    var careers = new Faker<CareerSuggestion>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.StudentId, f => f.PickRandom(students).Id).RuleFor(x => x.CareerPath, f => f.Name.JobTitle()).RuleFor(x => x.Justification, f => f.Lorem.Sentence()).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.CareerSuggestions.AddRange(careers);
                    await context.SaveChangesAsync();
                }

                if (!await context.AnalyticsReports.AnyAsync() && students.Any())
                {
                    var analytics = new Faker<AnalyticsReport>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.StudentId, f => f.PickRandom(students).Id).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.AnalyticsReports.AddRange(analytics);
                    await context.SaveChangesAsync();
                }
            });

            // 12. Study Plans
            await SeedStageAsync("Study Plans", async () => {
                bool studyPlansSeededOrUpdated = false;
                
                if (!await context.StudyPlans.AnyAsync() && students.Any())
                {
                    var studyPlans = new Faker<StudyPlan>("en")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.CourseId, f => f.PickRandom(courses).Id)
                        .RuleFor(x => x.Semester, f => f.PickRandom<Rafeek.Domain.Enums.Semester>())
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    
                    context.StudyPlans.AddRange(studyPlans);
                    studyPlansSeededOrUpdated = true;
                }
                else
                {
                    // Fix existing records that might be stuck on Semester 0 (First) due to the bug
                    var existingBuggyStudyPlans = await context.StudyPlans
                        .Where(sp => sp.Semester == Rafeek.Domain.Enums.Semester.First)
                        .ToListAsync();

                    if (existingBuggyStudyPlans.Any())
                    {
                        var faker = new Faker();
                        foreach (var sp in existingBuggyStudyPlans)
                        {
                            // Assign a random semester from Second to Eighth (1 to 7) to ensure they are varied
                            // and fix the issue where everything is stuck on 0
                            sp.Semester = (Rafeek.Domain.Enums.Semester)faker.Random.Int(1, 7);
                        }
                        context.StudyPlans.UpdateRange(existingBuggyStudyPlans);
                        studyPlansSeededOrUpdated = true;
                    }
                }

                if (studyPlansSeededOrUpdated)
                {
                    await context.SaveChangesAsync();
                }
            });

            // 13. Student Support & Appointments
            await SeedStageAsync("Student Support & Appointments", async () => {
                if (!await context.StudentSupports.AnyAsync() && students.Any())
                {
                    var supports = new Faker<StudentSupport>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.StudentId, f => f.PickRandom(students).Id).RuleFor(x => x.Title, f => f.Lorem.Sentence()).RuleFor(x => x.Description, f => f.Lorem.Paragraph()).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.StudentSupports.AddRange(supports);
                    await context.SaveChangesAsync();
                }

                if (!await context.Appointments.AnyAsync() && students.Any())
                {
                    var appointmentsForSeed = new Faker<Appointment>("en")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.DoctorId, f => f.PickRandom(doctors).Id)
                        .RuleFor(x => x.AppointmentDate, f => f.Date.Future())
                        .RuleFor(x => x.StartTime, f => new TimeSpan(f.Random.Int(9, 15), 0, 0))
                        .RuleFor(x => x.EndTime, (f, x) => x.StartTime.Add(new TimeSpan(0, 30, 0)))
                        .RuleFor(x => x.Location, f => "Office " + f.Random.Number(100, 500))
                        .RuleFor(x => x.Status, f => f.PickRandom<Rafeek.Domain.Enums.AppointmentStatus>())
                        .RuleFor(x => x.Notes, f => f.Lorem.Sentence())
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.Appointments.AddRange(appointmentsForSeed);
                    await context.SaveChangesAsync();
                }
            });

            // 14. Documents, Learning Resources & Maps
            await SeedStageAsync("Documents, Resources & Maps", async () => {
                if (!await context.DocumentRequests.AnyAsync() && students.Any())
                {
                    var docsReqForSeed = new Faker<DocumentRequest>("en")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.DocumentType, f => f.PickRandom(new[] { "Transcript", "Enrollment Proof", "ID" }))
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.DocumentRequests.AddRange(docsReqForSeed);
                    await context.SaveChangesAsync();
                }

                if (!await context.LearningResources.AnyAsync() && courses.Any())
                {
                    var resources = new Faker<LearningResource>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.CourseId, f => f.PickRandom(courses).Id).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(20);
                    context.LearningResources.AddRange(resources);
                    await context.SaveChangesAsync();
                }

                if (!await context.CampusMapLocations.AnyAsync())
                {
                    var maps = new Faker<CampusMapLocation>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.Place, f => f.Company.CompanyName()).RuleFor(x => x.Building, f => "Building " + f.Random.AlphaNumeric(1).ToUpper()).RuleFor(x => x.Floor, f => f.Random.Int(1, 5).ToString()).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(20);
                    context.CampusMapLocations.AddRange(maps);
                    await context.SaveChangesAsync();
                }
            });

            // 12. Identity/User side entities (Notifications, Calendars)
            // 15. User-side entities (Notifications, RefreshTokens, Calendars, Chat & Reminders)
            await SeedStageAsync("User-side entities", async () => {
                if (!await context.Notifications.AnyAsync() && users.Any())
                {
                    var notifications = new Faker<Notification>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.UserId, f => f.PickRandom(users).Id).RuleFor(x => x.Title, f => f.Lorem.Sentence()).RuleFor(x => x.Message, f => f.Lorem.Paragraph()).RuleFor(x => x.IsRead, f => f.Random.Bool()).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.Notifications.AddRange(notifications);
                    await context.SaveChangesAsync();
                }

                if (!await context.Reminders.AnyAsync() && users.Any())
                {
                    var reminders = new Faker<Reminder>("en")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.UserId, f => f.PickRandom(users).Id)
                        .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                        .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                        .RuleFor(x => x.DueDate, f => f.Date.Future())
                        .RuleFor(x => x.IsCompleted, f => f.Random.Bool())
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.Reminders.AddRange(reminders);
                    await context.SaveChangesAsync();
                }

                if (!await context.ChatSessions.AnyAsync() && students.Any())
                {
                    var chatSessions = students.Take(20).Select(s => new ChatSession
                    {
                        Id = Guid.NewGuid(),
                        UserId = s.UserId,
                        Title = "Academic Session " + f.Date.Recent().ToShortDateString(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    }).ToList();
                    context.ChatSessions.AddRange(chatSessions);
                    await context.SaveChangesAsync();

                    // Now seed ChatbotQueries with valid Session IDs
                    var sessionIds = chatSessions.Select(s => s.Id).ToList();
                    var chatbot = new Faker<ChatbotQuery>("en")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.SessionId, f => f.PickRandom(sessionIds))
                        .RuleFor(x => x.Query, f => f.Lorem.Sentence() + "?")
                        .RuleFor(x => x.Response, f => f.Lorem.Sentence())
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.ChatbotQueries.AddRange(chatbot);
                    await context.SaveChangesAsync();
                }

                if (!await context.UserCalendarPreferences.AnyAsync() && users.Any())
                {
                    var userPreferences = users.Take(50).Select(u => new UserCalendarPreference { Id = Guid.NewGuid(), UserId = u.Id, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder" }).ToList();
                    context.UserCalendarPreferences.AddRange(userPreferences);
                    await context.SaveChangesAsync();
                }

                if (!await context.UserLoginHistories.AnyAsync() && users.Any())
                {
                    var userLogins = new Faker<UserLoginHistory>("en").RuleFor(x => x.Id, Guid.NewGuid).RuleFor(x => x.UserId, f => f.PickRandom(users).Id).RuleFor(x => x.LoginTime, f => f.Date.Recent()).RuleFor(x => x.CreatedAt, f => DateTime.UtcNow).RuleFor(x => x.CreatedBy, "Seeder").Generate(50);
                    context.UserLoginHistories.AddRange(userLogins);
                    await context.SaveChangesAsync();
                }

                if (!await context.RefreshTokens.AnyAsync() && users.Any())
                {
                    var refreshTokens = new Faker<RefreshToken>("en")
                        .RuleFor(x => x.JwtId, f => Guid.NewGuid().ToString())
                        .RuleFor(x => x.UserId, f => f.PickRandom(users).Id.ToString())
                        .RuleFor(x => x.Token, f => f.Random.AlphaNumeric(32))
                        .RuleFor(x => x.CreationDate, f => DateTime.UtcNow)
                        .RuleFor(x => x.ExpirationDate, f => DateTime.UtcNow.AddDays(7))
                        .RuleFor(x => x.RemoteIpAddress, f => f.Internet.Ip())
                        .Generate(20);
                    context.RefreshTokens.AddRange(refreshTokens);
                    await context.SaveChangesAsync();
                }

                if (!await context.AcademicCalendars.AnyAsync() && (users.Any() || instructors.Any()))
                {
                    var targetSource = users.Any() ? users.Select(u => u.Id).ToList() : instructors.Select(i => i.UserId).ToList();
                    var calendars = new Faker<AcademicCalendar>("en")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.TargetUserId, f => f.PickRandom(targetSource))
                        .RuleFor(x => x.EventName, f => f.PickRandom(new[] { "Midterm Exam", "Final Project Due", "Academic Council Meeting", "Workshop: AI Ethics", "University Holiday" }))
                        .RuleFor(x => x.Description, f => f.Lorem.Sentence())
                        .RuleFor(x => x.EventDate, f => f.Date.Future())
                        .RuleFor(x => x.EndDate, (f, x) => x.EventDate.AddDays(f.Random.Int(0, 2)))
                        .RuleFor(x => x.StartTime, f => new TimeSpan(f.Random.Int(8, 16), 0, 0))
                        .RuleFor(x => x.EndTime, (f, x) => x.StartTime.Add(new TimeSpan(f.Random.Int(1, 3), 0, 0)))
                        .RuleFor(x => x.IsAllDay, f => f.Random.Bool(0.1f))
                        .RuleFor(x => x.Location, f => "Main Hall " + f.Random.Number(1, 10))
                        .RuleFor(x => x.EventType, f => f.PickRandom<Rafeek.Domain.Enums.AcademicCalendarEventType>())
                        .RuleFor(x => x.Status, f => f.PickRandom<Rafeek.Domain.Enums.CalendarEventStatus>())
                        .RuleFor(x => x.Visibility, f => f.PickRandom<Rafeek.Domain.Enums.EventVisibility>())
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.AcademicCalendars.AddRange(calendars);
                    await context.SaveChangesAsync();
                }
            });

            await context.SaveChangesAsync();
        }
    }
}
