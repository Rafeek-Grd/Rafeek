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

            var fEn = new Faker("en");
            var fAr = new Faker("ar");

            var departmentsData = new[]
            {
                new { Name = "علوم الحاسب", Code = "CS", Description = "قسم علوم الحاسب يركز على التميز الأكاديمي والبحث العلمي والتطوير التكنولوجي." },
                new { Name = "نظم المعلومات", Code = "IS", Description = "قسم نظم المعلومات يركز على إدارة وتحليل النظم والبيانات لدعم اتخاذ القرارات." },
                new { Name = "تكنولوجيا المعلومات", Code = "IT", Description = "قسم تكنولوجيا المعلومات يركز على البنية التحتية للشبكات وأمن المعلومات وحلول الويب." },
                new { Name = "المعلوماتية الحيوية", Code = "BIO", Description = "قسم المعلوماتية الحيوية يدمج بين علوم الحاسب وعلم الأحياء لتحليل البيانات الطبية والحيوية." },
                new { Name = "هندسة البرمجيات", Code = "SWE", Description = "قسم هندسة البرمجيات يركز على تصميم وتطوير وإدارة النظم البرمجية المعقدة بكفاءة وجودة عالية." },
                new { Name = "الذكاء الاصطناعي", Code = "AI", Description = "قسم الذكاء الاصطناعي يركز على تقنيات التعلم الآلي والأنظمة الذكية والروبوتات لمعالجة التحديات الحديثة." }
            };

            var coursesData = new Dictionary<string, List<(string Code, string Title)>>();
            foreach (var d in departmentsData)
            {
                var deptCourses = new List<(string Code, string Title)>();
                for (int level = 1; level <= 4; level++)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        string code = $"{d.Code}{level}{i:D2}";
                        string title = $"{d.Name} - المستوى {level} - الجزء {i}";
                        deptCourses.Add((code, title));
                    }
                }
                coursesData[d.Code] = deptCourses;
            }

            // 1. Roles
            Log("[Seeder] Stage 1: Seeding Roles...");
            var roles = new[] { "Admin", "Staff", "Student", "Professor", "Mentor" };
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
                    Description = d.Description,
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
                                Description = $"مقرر دراسي مكثف في {cData.Title} يقدم المعارف والمهارات الأساسية والمتقدمة في هذا المجال.",
                                CreditHours = new Random().Next(2, 4),
                                DepartmentId = dept.Id,
                                WeeklyLectureHours = 2,
                                WeeklyLabHours = 1,
                                MidtermPercent = 25,
                                FinalPercent = 45,
                                ProjectPercent = 30,
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
            else
            {
                foreach (var course in courses)
                {
                    if (course.WeeklyLectureHours == 0 && course.WeeklyLabHours == 0)
                    {
                        course.WeeklyLectureHours = 2;
                        course.WeeklyLabHours = 1;
                        course.MidtermPercent = 25;
                        course.FinalPercent = 45;
                        course.ProjectPercent = 30;
                    }
                }
                context.Courses.UpdateRange(courses);
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
                    var yearName = academicYears.First(y => y.Id == t.AcademicYearId).Name;
                    var termTypeName = t.TermType switch {
                        Rafeek.Domain.Enums.TermType.Fall => "الفصل الدراسي الأول (الخريف)",
                        Rafeek.Domain.Enums.TermType.Spring => "الفصل الدراسي الثاني (الربيع)",
                        Rafeek.Domain.Enums.TermType.Summer => "الفصل الصيفي",
                        _ => t.TermType.ToString()
                    };
                    return $"{termTypeName} للعام الجامعي {yearName}";
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

            var existingFallback = users.FirstOrDefault(u => u.UserName == "admin_fallback" || u.Email == "admin@rafeek.edu");
            if (existingFallback != null && existingFallback.UserTypes != UserType.Admin)
            {
                Log("[Seeder] Correcting existing fallback admin's UserTypes to Admin...");
                existingFallback.UserTypes = UserType.Admin;
                await userManager.UpdateAsync(existingFallback);
                await identityContext.SaveChangesAsync();
                
                // Refresh list
                users = await identityContext.Users.ToListAsync();
            }
            
            if (!users.Any())
            {
                Log("[Seeder] No users found. Generating 50 new users...");
                var userFaker = new Faker<ApplicationUser>("ar")
                    .RuleFor(u => u.Id, f => Guid.NewGuid())
                    .RuleFor(u => u.UserName, f => fEn.Internet.UserName() + f.UniqueIndex)
                    .RuleFor(u => u.Email, (f, u) => $"{u.UserName}@rafeek.edu")
                    .RuleFor(u => u.FullName, f => f.Name.FullName())
                    .RuleFor(u => u.NationalId, f => "29" + fEn.Random.Number(70, 99) + fEn.Random.Replace("##########").Substring(0, 10))
                    .RuleFor(u => u.IsUniversityEmailActivated, f => true)
                    .RuleFor(u => u.Address, f => f.Address.FullAddress())
                    .RuleFor(u => u.PhoneNumber, f => fEn.PickRandom(new[] { "010", "011", "012", "015" }) + fEn.Random.Replace("########"));

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
                            var dice = fEn.Random.Double();

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
                            else if (dice < 0.88) // 8% Professor
                            {
                                userRoles.Add("Professor");
                                user.UserTypes = UserType.Professor;
                            }
                            else if (dice < 0.92) // 4% Admin
                            {
                                userRoles.Add("Admin");
                                user.UserTypes = UserType.Admin;
                            }
                            else if (dice < 0.94) // 2% Staff
                            {
                                userRoles.Add("Staff");
                                user.UserTypes = UserType.Staff;
                            }
                            else if (dice < 0.97) // 3% Mentor
                            {
                                userRoles.Add("Mentor");
                                user.UserTypes = UserType.Mentor;
                            }
                            else // 3% Professor (previously Professor + Mentor)
                            {
                                userRoles.Add("Professor");
                                user.UserTypes = UserType.Professor;
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
                    var fallbackAdmin = new ApplicationUser { UserName = "admin_fallback", Email = "admin@rafeek.edu", FullName = "مدير النظام", NationalId = "00000000000000", UserTypes = UserType.Admin };
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
            var professorUsers = new List<ApplicationUser>();
            var mentorUsers = new List<ApplicationUser>();
            var staffUsers = new List<ApplicationUser>();

            foreach (var user in users)
            {
                var rolesList = await userManager.GetRolesAsync(user);
                if (rolesList.Contains("Student")) studentUsers.Add(user);
                if (rolesList.Contains("Professor")) professorUsers.Add(user);
                if (rolesList.Contains("Mentor")) mentorUsers.Add(user);
                if (rolesList.Contains("Staff")) staffUsers.Add(user);
            }

            var students = new List<Student>();
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

            // Create Doctors (for Professor + Mentor users)
            var existingDoctors = await context.Doctors.ToListAsync();
            if (!existingDoctors.Any())
            {
                var targetUsers = professorUsers.Any() || mentorUsers.Any()
                    ? professorUsers.Union(mentorUsers).ToList()
                    : users.Skip(5).Take(10).ToList();
                for (int d = 0; d < targetUsers.Count; d++)
                {
                    doctors.Add(new Doctor { Id = Guid.NewGuid(), UserId = targetUsers[d].Id, EmployeeCode = $"DOC{1000 + d}", DepartmentId = fEn.PickRandom(departments).Id, IsAcademicAdvisor = targetUsers[d].UserTypes.HasFlag(UserType.Mentor), CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", IsActive = true });
                }
                await context.Doctors.AddRangeAsync(doctors);
                await context.SaveChangesAsync();
            }
            else { doctors = existingDoctors; }

            // Fixup: Change any user with UserType 12 (Professor|Mentor) to 8 (Professor only)
            bool anyUserTypeFixed = false;
            foreach (var u in users)
            {
                if (u.UserTypes == (UserType.Professor | UserType.Mentor))
                {
                    u.UserTypes = UserType.Professor;
                    anyUserTypeFixed = true;
                }
            }
            if (anyUserTypeFixed)
            {
                await identityContext.SaveChangesAsync();
                Log("[Seeder] Fixed users with combined Professor|Mentor type to Professor only.");
            }

            // Sync IsAcademicAdvisor with actual UserTypes for all doctors
            bool anyAdvisorFixed = false;
            foreach (var doctor in doctors)
            {
                var user = users.FirstOrDefault(u => u.Id == doctor.UserId);
                if (user == null) continue;
                var shouldBeAdvisor = user.UserTypes.HasFlag(UserType.Mentor);
                if (doctor.IsAcademicAdvisor != shouldBeAdvisor)
                {
                    doctor.IsAcademicAdvisor = shouldBeAdvisor;
                    anyAdvisorFixed = true;
                }
            }
            if (anyAdvisorFixed)
            {
                context.Doctors.UpdateRange(doctors);
                await context.SaveChangesAsync();
                Log($"[Seeder] Synced IsAcademicAdvisor for {doctors.Count(d => d.IsAcademicAdvisor)} doctors with Mentor flag.");
            }

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

                    var level = fEn.Random.Int(1, 4);
                    var term = (level * 2) - fEn.Random.Int(0, 1);
                    
                    students.Add(new Student { 
                        Id = studentId, 
                        UserId = user.Id, 
                        UniversityCode = $"{DateTime.UtcNow.Year}{10000 + i}", 
                        DepartmentId = fEn.PickRandom(departments).Id, 
                        AcademicProfileId = profileId,
                        AcademicAdvisorId = advisorList.Any() ? fEn.PickRandom(advisorList).Id : (doctors.Any() ? fEn.PickRandom(doctors).Id : null),
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
                        CompletedCredits = (level - 1) * 30 + fEn.Random.Int(0, 15),
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
            var sections = await context.LectureGroups.Include(s => s.Course).ToListAsync();
            if (!sections.Any())
            {
                foreach (var course in courses)
                {
                    // Every course gets at least 2 sections
                    var sectionCount = fEn.Random.Int(2, 4);
                    for (int i = 0; i < sectionCount; i++)
                    {
                        var startTime = new TimeSpan(fEn.PickRandom(new[] { 8, 10, 12, 14, 16 }), 0, 0);
                        sections.Add(new LectureGroup
                        {
                            Id = Guid.NewGuid(),
                            CourseId = course.Id,
                            DoctorId = doctors.Any() ? fEn.PickRandom(doctors).Id : null,
                            Day = fEn.PickRandom(new[] { "الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس" }),
                            StartTime = startTime,
                            EndTime = startTime.Add(new TimeSpan(2, 0, 0)),
                            Time = $"{startTime:hh\\:mm} - {startTime.Add(new TimeSpan(2, 0, 0)):hh\\:mm}",
                            Capacity = fEn.Random.Int(30, 60),
                            Location = $"مبنى {fEn.Random.AlphaNumeric(1).ToUpper()} - الطابق {fEn.Random.Int(1, 5)} - قاعة {fEn.Random.Number(100, 500)}",
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "Seeder",
                            IsActive = true
                        });
                    }
                }
                context.LectureGroups.AddRange(sections);
                await context.SaveChangesAsync();
            }

            // 8. Enrollments & Grades
            var enrollments = await context.Enrollments.ToListAsync();
            var grades = await context.Grades.ToListAsync();

            if (enrollments.Any() || grades.Any())
            {
                enrollments = new List<Enrollment>();
                grades = new List<Grade>();
            }

            if (!enrollments.Any() && students.Any() && sections.Any())
            {
                Log("[Seeder] Seeding Enrollments and calculating GPA/CGPA...");
                foreach (var s in students)
                {
                    var studentGrades = new List<float>();
                    var coursesToEnroll = new List<LectureGroup>();
                    
                    // 70% focus on Major (Department) courses, 30% Electives
                    var majorSections = sections.Where(sec => sec.Course.DepartmentId == s.DepartmentId).ToList();
                    var electiveSections = sections.Where(sec => sec.Course.DepartmentId != s.DepartmentId).ToList();

                    var enrCount = fEn.Random.Int(5, 7);
                    var majorCount = (int)(enrCount * 0.7);
                    
                    if (majorSections.Any())
                        coursesToEnroll.AddRange(fEn.PickRandom(majorSections, Math.Min(majorCount, majorSections.Count)));
                    
                    if (electiveSections.Any())
                        coursesToEnroll.AddRange(fEn.PickRandom(electiveSections, Math.Min(enrCount - coursesToEnroll.Count, electiveSections.Count)));

                    foreach (var sec in coursesToEnroll)
                    {
                        // Bell-curve grade distribution simulation
                        // WeightedRandom: 10% Excellent (90-95), 40% Good (80-89), 30% Fair (70-79), 15% Pass (60-69), 5% Fail (<60)
                        var gradeCategory = fEn.Random.WeightedRandom(
                            new[] { "A", "B", "C", "D", "F" },
                            new[] { 0.10f, 0.40f, 0.30f, 0.15f, 0.05f }
                        );

                        float score = gradeCategory switch {
                            "A" => (float)Math.Round(fEn.Random.Double(90.0, 98.0), 2),
                            "B" => (float)Math.Round(fEn.Random.Double(80.0, 89.0), 2),
                            "C" => (float)Math.Round(fEn.Random.Double(70.0, 79.0), 2),
                            "D" => (float)Math.Round(fEn.Random.Double(60.0, 69.0), 2),
                            _ => (float)Math.Round(fEn.Random.Double(40.0, 59.0), 2)
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
                            LectureGroupId = sec.Id,
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
                        var avgGpa = studentGrades.Any() ? (float)Math.Round(studentGrades.Average(), 2) : (float)Math.Round(fEn.Random.Double(2.0, 3.8), 2);
                        profile.GPA = Math.Clamp(avgGpa, 0.0f, 4.0f);
                        profile.CGPA = Math.Clamp((float)Math.Round(profile.GPA - fEn.Random.Double(0, 0.2), 2), 0.0f, 4.0f);
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
                    var strengths = new[] { "الالتزام بالحضور", "سرعة الفهم والاستيعاب", "مهارات متميزة في حل المشكلات", "التفكير التحليلي القوي", "العمل الجماعي الرائع", "المهارات التنظيمية العالية" };
                    var weaknesses = new[] { "إدارة الوقت أثناء الامتحانات", "التسويف في تسليم المشاريع", "المشاركة الخجولة في المحاضرات", "التردد في طرح الأسئلة", "تحتاج مهارات البرمجة إلى تدريب إضافي" };
                    var recommendations = new[] {
                        "الاستمرار في المشاركة الفعالة في الأنشطة الطلابية.",
                        "المشاركة في ورش العمل التقنية لزيادة المهارات العملية.",
                        "التركيز على تنظيم جدول المذاكرة وحل التدريبات السابقة.",
                        "استشارة المرشد الأكاديمي لمراجعة خطة التسجيل الدراسي.",
                        "حضور الساعات المكتبية للدعم والمراجعة المستمرة."
                    };
                    var feedbacks = new Faker<AcademicFeedback>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.Strength, f => f.PickRandom(strengths))
                        .RuleFor(x => x.Weakness, f => f.PickRandom(weaknesses))
                        .RuleFor(x => x.Recommendation, f => f.PickRandom(recommendations))
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.AcademicFeedbacks.AddRange(feedbacks);
                    await context.SaveChangesAsync();
                }

                if (!await context.AICourseRecommendations.AnyAsync() && students.Any())
                {
                    var reasons = new[] {
                        "يتماشى هذا المقرر مع اهتمامك بمجال الذكاء الاصطناعي وتطوير الأنظمة.",
                        "يساعدك هذا المقرر على تحسين مستواك وتلبية متطلبات التخرج.",
                        "يُنصح به بناءً على أدائك الممتاز في المقررات التمهيدية السابقة.",
                        "يعد هذا المقرر أساسياً ومطلوباً لفتح مسارات دراسية متقدمة في هندسة البرمجيات.",
                        "سيعزز هذا المقرر مهاراتك العملية والتقنية المطلوبة بشدة في سوق العمل."
                    };
                    var aiReco = new Faker<AICourseRecommendation>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.CourseId, f => f.PickRandom(courses).Id)
                        .RuleFor(x => x.Reason, f => f.PickRandom(reasons))
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.AICourseRecommendations.AddRange(aiReco);
                    await context.SaveChangesAsync();
                }

                if (!await context.CareerSuggestions.AnyAsync() && students.Any())
                {
                    var careerPaths = new[] { "مطور برمجيات كاملة (Full-Stack Developer)", "مهندس بيانات ضخمة (Data Engineer)", "أخصائي أمن سيبراني (Cybersecurity Specialist)", "مهندس تعلم آلي (Machine Learning Engineer)", "محلل نظم معلومات (Systems Analyst)", "مدير مشاريع تقنية (IT Project Manager)" };
                    var justifications = new[] {
                        "أظهرت أداءً متميزاً في المقررات البرمجية والتحليلية.",
                        "لديك شغف ومهارات قوية في تحليل البيانات وحل المشكلات المعقدة.",
                        "يتطابق ملفك الأكاديمي واهتماماتك مع متطلبات هذا المسار المهني الواعد.",
                        "تميزك في مشاريع التخرج والعمل الجماعي يجعلك مؤهلاً للقيادة التقنية.",
                        "سوق العمل يطلب بشدة هذه التخصصات، وأداؤك الأكاديمي يدعم تميزك فيها."
                    };
                    var careers = new Faker<CareerSuggestion>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.CareerPath, f => f.PickRandom(careerPaths))
                        .RuleFor(x => x.Justification, f => f.PickRandom(justifications))
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
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
                    var supportTitles = new[] { "دعم أكاديمي في الرياضيات", "طلب إرشاد نفسي واجتماعي", "تسهيلات لأصحاب الهمم", "دعم مهارات البرمجة الأساسية", "المساعدة في إدارة التوتر قبل الامتحانات" };
                    var supportDescriptions = new[] {
                        "جلسات مخصصة لمراجعة المبادئ الأساسية وحل المسائل الصعبة.",
                        "توفير بيئة داعمة ومستشاري إرشاد للتحدث ومواجهة التحديات اليومية والأكاديمية.",
                        "توفير قاعات مهيأة وأدوات مساعدة لضمان تجربة تعليمية متميزة.",
                        "مراجعة إضافية للمفاهيم الأساسية وتطبيقات عملية مع مهندسي المعمل.",
                        "ورشة عمل وجلسات حوارية لمناقشة أساليب الدراسة الفعالة والتعامل مع ضغط الاختبارات."
                    };
                    var supports = new Faker<StudentSupport>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.Title, f => f.PickRandom(supportTitles))
                        .RuleFor(x => x.Description, f => f.PickRandom(supportDescriptions))
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.StudentSupports.AddRange(supports);
                    await context.SaveChangesAsync();
                }

                if (!await context.Appointments.AnyAsync() && students.Any())
                {
                    var appointmentNotes = new[] { "مناقشة الخطة الدراسية للفصل القادم", "مراجعة درجات الامتحان والاعتراضات إن وجدت", "تقديم استشارة بخصوص التدريب الصيفي", "متابعة الحالة الأكاديمية والإنذارات", "مناقشة فكرة مشروع التخرج" };
                    var appointmentsForSeed = new Faker<Appointment>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.DoctorId, f => f.PickRandom(doctors).Id)
                        .RuleFor(x => x.AppointmentDate, f => f.Date.Future())
                        .RuleFor(x => x.StartTime, f => new TimeSpan(fEn.Random.Int(9, 15), 0, 0))
                        .RuleFor(x => x.EndTime, (f, x) => x.StartTime.Add(new TimeSpan(0, 30, 0)))
                        .RuleFor(x => x.Location, f => "مكتب رقم " + fEn.Random.Number(100, 500))
                        .RuleFor(x => x.Status, f => f.PickRandom<Rafeek.Domain.Enums.AppointmentStatus>())
                        .RuleFor(x => x.Notes, f => f.PickRandom(appointmentNotes))
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
                    var documentTypes = new[] { "بيان درجات رسمي (Transcript)", "إثبات قيد طالب (Enrollment Proof)", "خطاب تدريب صيفي (Summer Internship)", "استخراج بطاقة جامعية (ID Card Request)" };
                    var docsReqForSeed = new Faker<DocumentRequest>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.DocumentType, f => f.PickRandom(documentTypes))
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.DocumentRequests.AddRange(docsReqForSeed);
                    await context.SaveChangesAsync();
                }

                if (!await context.LearningResources.AnyAsync() && courses.Any())
                {
                    var resources = new Faker<LearningResource>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.CourseId, f => f.PickRandom(courses).Id)
                        .RuleFor(x => x.ResourceType, ResourceType.External)
                        .RuleFor(x => x.ResourceUrl, f => fEn.Internet.Url())
                        .RuleFor(x => x.Description, (f, lr) => "مرجع تعليمي وقراءة إضافية مساعدة لفهم مقرر " + courses.First(c => c.Id == lr.CourseId).Title)
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(20);
                    context.LearningResources.AddRange(resources);
                    await context.SaveChangesAsync();
                }

                if (!await context.CampusMapLocations.AnyAsync())
                {
                    var places = new[] { "المكتبة المركزية", "مبنى شؤون الطلاب", "مدرج أورمان", "مدرج ابن خلدون", "معمل الحاسبات الرئيسي", "كافتيريا الكلية", "ملعب الرياضة الرئيسي", "مبنى العميد والوكلاء", "عيادة الكلية الصحية", "مسجد الكلية" };
                    var maps = new Faker<CampusMapLocation>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.Place, f => f.PickRandom(places))
                        .RuleFor(x => x.Building, f => "مبنى " + fEn.Random.AlphaNumeric(1).ToUpper())
                        .RuleFor(x => x.Floor, f => fEn.Random.Int(1, 5).ToString())
                        .RuleFor(x => x.FloorLevel, f => f.PickRandom<Rafeek.Domain.Enums.Floor>())
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(20);
                    context.CampusMapLocations.AddRange(maps);
                    await context.SaveChangesAsync();
                }
            });

            // 15. Assignments & Submissions
            await SeedStageAsync("Assignments & Submissions", async () => {
                if (!await context.Assignments.AnyAsync() && sections.Any())
                {
                    var assignments = new Faker<Assignment>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.LectureGroupId, f => f.PickRandom(sections).Id)
                        .RuleFor(x => x.Title, f => "واجب: " + f.Lorem.Word())
                        .RuleFor(x => x.Description, f => "يرجى حل الأسئلة المرفقة وتقديم الحل قبل الموعد المحدد: " + f.Lorem.Sentence())
                        .RuleFor(x => x.DueDate, f => f.Date.Future())
                        .RuleFor(x => x.TotalScore, f => fEn.Random.Float(10, 100))
                        .RuleFor(x => x.IsActive, f => true)
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.Assignments.AddRange(assignments);
                    await context.SaveChangesAsync();
                }

                if (!await context.AssignmentSubmissions.AnyAsync() && students.Any())
                {
                    var assignmentsList = await context.Assignments.ToListAsync();
                    if (assignmentsList.Any())
                    {
                        var submissions = new Faker<AssignmentSubmission>("ar")
                            .RuleFor(x => x.Id, Guid.NewGuid)
                            .RuleFor(x => x.AssignmentId, f => f.PickRandom(assignmentsList).Id)
                            .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                            .RuleFor(x => x.SubmissionUrl, f => fEn.Internet.Url())
                            .RuleFor(x => x.Feedback, f => f.PickRandom(new[] { "عمل ممتاز وتنسيق رائع!", "إجابات صحيحة ومجهود تشكر عليه.", "يرجى الانتباه لبعض التفاصيل البرمجية في المرة القادمة.", "حل جيد ولكن يحتاج لمزيد من التوضيح.", "تسليم مكتمل ومتقن." }))
                            .RuleFor(x => x.Score, (f, x) => fEn.Random.Float(0, assignmentsList.First(a => a.Id == x.AssignmentId).TotalScore))
                            .RuleFor(x => x.SubmittedAt, f => f.Date.Recent())
                            .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                            .RuleFor(x => x.CreatedBy, "Seeder")
                            .Generate(100);
                        context.AssignmentSubmissions.AddRange(submissions);
                        await context.SaveChangesAsync();
                    }
                }
            });

            // 16. Identity/User side entities (Notifications, Calendars)
            // 15. User-side entities (Notifications, RefreshTokens, Calendars, Chat & Reminders)
            await SeedStageAsync("User-side entities", async () => {
                if (!await context.Notifications.AnyAsync() && users.Any())
                {
                    var notificationTitles = new[] { "تم تسجيل مقرر جديد", "تنبيه: موعد امتحان منتصف الفصل", "تم تحديث جدول المحاضرات", "رسالة جديدة من المرشد الأكاديمي", "تذكير: تسليم الواجب المتأخر" };
                    var notificationMessages = new[] {
                        "لقد تم تسجيلك بنجاح في المقررات الدراسية للفصل الحالي.",
                        "نود تذكيركم بأن اختبار منتصف الفصل سيبدأ الأسبوع المقبل. بالتوفيق للجميع.",
                        "يرجى مراجعة الجدول الدراسي لوجود تعديلات طفيفة في مواعيد المحاضرات والمعامل.",
                        "قام مرشدك الأكاديمي بإرسال ملاحظة جديدة بخصوص خطتك الدراسية.",
                        "لديك واجب مستحق التسليم خلال 24 ساعة القادمة، يرجى تقديم الحل لتفادي خصم الدرجات."
                    };
                    var notifications = new Faker<Notification>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.UserId, f => f.PickRandom(users).Id)
                        .RuleFor(x => x.Title, f => f.PickRandom(notificationTitles))
                        .RuleFor(x => x.Message, f => f.PickRandom(notificationMessages))
                        .RuleFor(x => x.IsRead, f => f.Random.Bool())
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.Notifications.AddRange(notifications);
                    await context.SaveChangesAsync();
                }

                // Add global announcements if not already present
                if (!await context.Notifications.AnyAsync(n => n.UserId == null && n.CourseId == null))
                {
                    var globalNotifications = new List<Notification>
                    {
                        new Notification
                        {
                            Id = Guid.NewGuid(),
                            Title = "تنبيه: تحديث النظام الأكاديمي",
                            Message = "يرجى العلم أنه تم تحديث النظام الأكاديمي رفيق لإضافة ميزات الجداول والتقارير الجديدة.",
                            IsRead = false,
                            CreatedAt = DateTime.UtcNow.AddDays(-5),
                            CreatedBy = "System"
                        },
                        new Notification
                        {
                            Id = Guid.NewGuid(),
                            Title = "إشعار عام: إجازة عيد الأضحى المبارك",
                            Message = "تهنئكم إدارة الجامعة بحلول عيد الأضحى المبارك، ونود إعلامكم بأن الإجازة ستبدأ من الأحد القادم.",
                            IsRead = false,
                            CreatedAt = DateTime.UtcNow.AddDays(-2),
                            CreatedBy = "System"
                        }
                    };
                    context.Notifications.AddRange(globalNotifications);
                    await context.SaveChangesAsync();
                }

                // Add course-specific notifications if not already present
                if (!await context.Notifications.AnyAsync(n => n.CourseId != null) && courses.Any())
                {
                    var courseNotifications = new List<Notification>();
                    foreach (var course in courses.Take(15))
                    {
                        courseNotifications.Add(new Notification
                        {
                            Id = Guid.NewGuid(),
                            Title = "تم تحديث جدول اختبار نصف العام",
                            Message = $"برجاء العلم أنه تم تأجيل أسبوع اختبارات نصف العام لمقرر {course.Title} ليبدأ من 15 نوفمبر.",
                            CourseId = course.Id,
                            IsRead = false,
                            CreatedAt = DateTime.UtcNow.AddDays(-3),
                            CreatedBy = "System"
                        });
                        courseNotifications.Add(new Notification
                        {
                            Id = Guid.NewGuid(),
                            Title = "تم تحديد آخر موعد للتسليمات",
                            Message = $"آخر موعد لتسليم المشروع الخاص بمقرر {course.Title} هو 12 نوفمبر. يرجى التواصل مع المعيد في حال وجود أي استفسار.",
                            CourseId = course.Id,
                            IsRead = false,
                            CreatedAt = DateTime.UtcNow.AddDays(-1),
                            CreatedBy = "System"
                        });
                    }
                    context.Notifications.AddRange(courseNotifications);
                    await context.SaveChangesAsync();
                }

                // Add Announcements if not already present
                if (!await context.Announcements.AnyAsync() && departments.Any())
                {
                    var announcements = new List<Announcement>
                    {
                        new Announcement
                        {
                            Id = Guid.NewGuid(),
                            Title = "إغلاق المكتبة الرئيسية",
                            Content = "برجاء العلم أنه سيتم إغلاق المكتبة الرئيسية للصيانة الدورية اليوم الساعة 5:00 مساءً.",
                            AudienceType = 0, // AllStudents
                            SendInApp = true,
                            SendEmail = true,
                            SendSMS = false,
                            IsUrgent = true,
                            ScheduledAt = DateTime.UtcNow.AddHours(-1),
                            IsDeactivated = false,
                            IsSent = true,
                            CreatedAt = DateTime.UtcNow.AddDays(-1),
                            CreatedBy = "System",
                            IsActive = true
                        },
                        new Announcement
                        {
                            Id = Guid.NewGuid(),
                            Title = "فتح باب التسجيل في المقررات للفصل القادم",
                            Content = "يبدأ التسجيل للفصل الدراسي الخريفي القادم يوم الاثنين الساعة 9:00 صباحاً عبر البوابة الأكاديمية.",
                            AudienceType = 0, // AllStudents
                            SendInApp = true,
                            SendEmail = true,
                            SendSMS = true,
                            IsUrgent = false,
                            ScheduledAt = DateTime.UtcNow.AddDays(1),
                            IsDeactivated = false,
                            IsSent = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "System",
                            IsActive = true
                        },
                        new Announcement
                        {
                            Id = Guid.NewGuid(),
                            Title = "بدء فترة حذف وإضافة المقررات",
                            Content = "فترة الحذف والإضافة للمقررات الدراسية ستبدأ غداً وتستمر لمدة أسبوع كامل.",
                            AudienceType = 1, // SpecificDepartments
                            DepartmentId = departments.First().Id,
                            SendInApp = true,
                            SendEmail = false,
                            SendSMS = false,
                            IsUrgent = false,
                            ScheduledAt = DateTime.UtcNow.AddHours(2),
                            IsDeactivated = false,
                            IsSent = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = "System",
                            IsActive = true
                        }
                    };
                    context.Announcements.AddRange(announcements);
                    await context.SaveChangesAsync();
                }

                // Add CourseRegistrationPeriods if not already present
                if (!await context.CourseRegistrationPeriods.AnyAsync() && courses.Any() && academicTerms.Any())
                {
                    var periods = new List<CourseRegistrationPeriod>();
                    foreach (var course in courses)
                    {
                        foreach (var term in academicTerms)
                        {
                            periods.Add(new CourseRegistrationPeriod
                            {
                                Id = Guid.NewGuid(),
                                CourseId = course.Id,
                                AcademicTermId = term.Id,
                                RegistrationOpeningDate = term.StartDate.AddDays(-14),
                                RegistrationClosingDate = term.StartDate.AddDays(7),
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "Seeder",
                                IsActive = true
                            });
                        }
                    }
                    context.CourseRegistrationPeriods.AddRange(periods);
                    await context.SaveChangesAsync();
                }

                if (!await context.Reminders.AnyAsync() && users.Any())
                {
                    var reminderTitles = new[] { "مذاكرة محاضرة الخوارزميات", "شراء الكتب الدراسية", "حجز موعد مع المرشد", "مراجعة مشروع التخرج", "التحضير لعرض الأسبوع المقبل" };
                    var reminderDescriptions = new[] {
                        "تجهيز وتلخيص المفاهيم وحل المسائل البرمجية الخاصة بالباب الثالث.",
                        "زيارة مكتبة الجامعة لشراء الكتب والمراجع المقررة للفصل الحالي.",
                        "حجز موعد عبر نظام رفيق لمناقشة تعديل وحذف بعض المواد.",
                        "كتابة التقرير الأولي وعمل المخططات المعمارية للمشروع وتقديمها للدكتور المشرف.",
                        "تحضير الشرائح التقديمية وتجربة الإلقاء الفردي استعداداً ليوم العرض."
                    };
                    var reminders = new Faker<Reminder>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.UserId, f => f.PickRandom(users).Id)
                        .RuleFor(x => x.Title, f => f.PickRandom(reminderTitles))
                        .RuleFor(x => x.Description, f => f.PickRandom(reminderDescriptions))
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
                        Title = "جلسة إرشادية " + DateTime.UtcNow.ToShortDateString(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    }).ToList();
                    context.ChatSessions.AddRange(chatSessions);
                    await context.SaveChangesAsync();

                    // Now seed ChatbotQueries with valid Session IDs
                    var sessionIds = chatSessions.Select(s => s.Id).ToList();
                    var chatbotQuestions = new[] { "كيف يمكنني حساب المعدل التراكمي المتوقع؟", "ما هي المواد الاختيارية المتاحة لقسم علوم الحاسب؟", "من هو المرشد الأكاديمي الخاص بي وكيف أتواصل معه؟", "ما هي شروط رفع الإنذار الأكاديمي؟", "هل يمكنني تسجيل 18 ساعة معتمدة هذا الفصل؟" };
                    var chatbotAnswers = new[] {
                        "يمكنك حساب معدلك المتوقع باستخدام أداة محاكاة المعدل التراكمي المدمجة في نظام رفيق.",
                        "المواد الاختيارية المتاحة تشمل أمن الشبكات، معالجة الصور الرقمية، والتعلم العميق.",
                        "يمكنك معرفة مرشدك وحجز موعد معه مباشرة من خلال قسم اللقاءات والدعم الأكاديمي.",
                        "لرفع الإنذار، يجب رفع معدلك التراكمي التراكمي إلى 2.0 على الأقل في الفصل القادم.",
                        "نعم، يُسمح بتسجيل 18 ساعة إذا كان معدلك التراكمي أعلى من 2.5 وبموافقة مرشدك."
                    };
                    var chatbot = new Faker<ChatbotQuery>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.StudentId, f => f.PickRandom(students).Id)
                        .RuleFor(x => x.SessionId, f => f.PickRandom(sessionIds))
                        .RuleFor(x => x.Query, f => f.PickRandom(chatbotQuestions))
                        .RuleFor(x => x.Response, (f, x) => {
                            var idx = Array.IndexOf(chatbotQuestions, x.Query);
                            return idx >= 0 ? chatbotAnswers[idx] : "أنا هنا لمساعدتك في أي استفسار أكاديمي!";
                        })
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

                if (!await context.AcademicCalendars.AnyAsync() && (users.Any() || doctors.Any()))
                {
                    var targetSource = users.Any() ? users.Select(u => u.Id).ToList() : doctors.Select(d => d.UserId).ToList();
                    var eventNames = new[] { "امتحان منتصف الفصل الدراسي", "تسليم المشروع النهائي", "اجتماع المجلس الأكاديمي", "ورشة عمل: أخلاقيات الذكاء الاصطناعي", "إجازة رسمية بالجامعة" };
                    var eventDescriptions = new[] {
                        "امتحان تحريري يشمل المحاضرات من الأولى إلى السادسة.",
                        "يرجى رفع ملفات المشروع البرمجية والتقرير على منصة رفيق.",
                        "اجتماع عمادة الكلية لمناقشة خطة التطوير للفصل القادم.",
                        "جلسة تفاعلية حول تأثير الذكاء الاصطناعي على المجتمع والتعليم.",
                        "عطلة رسمية لجميع الطلاب وأعضاء هيئة التدريس والعاملين."
                    };
                    var calendars = new Faker<AcademicCalendar>("ar")
                        .RuleFor(x => x.Id, Guid.NewGuid)
                        .RuleFor(x => x.TargetUserId, f => f.PickRandom(targetSource))
                        .RuleFor(x => x.EventName, f => f.PickRandom(eventNames))
                        .RuleFor(x => x.Description, (f, x) => {
                            var idx = Array.IndexOf(eventNames, x.EventName);
                            return idx >= 0 ? eventDescriptions[idx] : f.Lorem.Sentence();
                        })
                        .RuleFor(x => x.EventDate, f => f.Date.Future())
                        .RuleFor(x => x.EndDate, (f, x) => x.EventDate.AddDays(fEn.Random.Int(0, 2)))
                        .RuleFor(x => x.StartTime, f => new TimeSpan(fEn.Random.Int(8, 16), 0, 0))
                        .RuleFor(x => x.EndTime, (f, x) => x.StartTime.Add(new TimeSpan(fEn.Random.Int(1, 3), 0, 0)))
                        .RuleFor(x => x.IsAllDay, f => fEn.Random.Bool(0.1f))
                        .RuleFor(x => x.Location, f => "القاعة رقم " + fEn.Random.Number(1, 10))
                        .RuleFor(x => x.EventType, f => f.PickRandom<Rafeek.Domain.Enums.AcademicCalendarEventType>())
                        .RuleFor(x => x.Status, f => f.PickRandom<Rafeek.Domain.Enums.CalendarEventStatus>())
                        .RuleFor(x => x.Visibility, f => f.PickRandom<Rafeek.Domain.Enums.EventVisibility>())
                        .RuleFor(x => x.AcademicTermId, f => f.PickRandom(academicTerms).Id)
                        .RuleFor(x => x.CreatedAt, f => DateTime.UtcNow)
                        .RuleFor(x => x.CreatedBy, "Seeder")
                        .Generate(50);
                    context.AcademicCalendars.AddRange(calendars);
                    await context.SaveChangesAsync();
                }
            });

            await SeedStageAsync("Academic Settings & Grade Scales", async () => {
                if (!await context.AcademicSettings.AnyAsync())
                {
                    context.AcademicSettings.Add(new AcademicSetting
                    {
                        Id = Guid.NewGuid(),
                        MaxHoursPerSemester = 18,
                        CourseCreditHours = 3,
                        AllowOverload = true,
                        IncludeTransferHours = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Seeder"
                    });
                    await context.SaveChangesAsync();
                }

                if (!await context.GradeScales.AnyAsync())
                {
                    var gradeScales = new List<GradeScale>
                    {
                        new GradeScale { Id = Guid.NewGuid(), GradeLetter = "A+", MinPercentage = 93, GpaPoints = 4.0, ArabicDescription = "ممتاز", CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder" },
                        new GradeScale { Id = Guid.NewGuid(), GradeLetter = "A-", MinPercentage = 90, GpaPoints = 3.7, ArabicDescription = "جيد جداً", CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder" },
                        new GradeScale { Id = Guid.NewGuid(), GradeLetter = "B+", MinPercentage = 87, GpaPoints = 3.3, ArabicDescription = "جيد", CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder" },
                        new GradeScale { Id = Guid.NewGuid(), GradeLetter = "B", MinPercentage = 83, GpaPoints = 3.0, ArabicDescription = "جيد", CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder" }
                    };
                    context.GradeScales.AddRange(gradeScales);
                    await context.SaveChangesAsync();
                }
            });

            await context.SaveChangesAsync();
        }
    }
}
