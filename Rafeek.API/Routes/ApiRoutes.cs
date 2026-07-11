namespace Rafeek.API.Routes
{
    public static class ApiRoutes
    {
        public const string Domain = "api";
        public const string Version = "v{version:apiVersion}";
        public const string Base = Version + "/" + Domain;

        public static class Uploader
        {
            public const string UploadImage = Base + "/images";
            public const string UploadVideo = Base + "/videos";
            public const string UploadAudio = Base + "/audio";
            public const string UploadFile = Base + "/file";
            public const string UpdateImage = Base + "/images/{name}";
            public const string UpdateVideo = Base + "/videos/{name}";
            public const string DownloadFile = Base + "/file/download";
            public const string UploadListVideo = Base + "/video/uploadmultiple";
            public const string UploadListImage = Base + "/image/uploadmultiple";
        }

        public static class Authentication
        {
            public const string SignUp = Base + "/signup";
            public const string SignIn = Base + "/signin";
            public const string RefreshToken = Base + "/refresh-token";
            public const string ForgotPassword = Base + "/forgot-password";
            public const string ResetPassword = Base + "/reset-password";
            public const string CheckFromConfirmationCode = Base + "/check-confirmation-code";
            public const string ActivateUniversityEmail = Base + "/activate-university-email";
            public const string GetUserProfile = Base + "/profile";
        }

        public static class EventOfAcademicCalendar
        {
            public const string AddEvent = Base + "/events/add";
            public const string GetAllPagginatedEvents = Base + "/events/getall/pagginated";
            public const string GetEventById = Base + "/events/{id}";
            public const string UpdateEvent = Base + "/events/update";
            public const string DeleteEvent = Base + "/events/{id}/delete";
        }

        public static class AcademicYear
        {
            public const string Create = Base + "/academic-year/add";
            public const string Update = Base + "/academic-year/{id}/update";
            public const string Delete = Base + "/academic-year/{id}/delete";
            public const string GetAllPagginated = Base + "/academic-year/getall/pagginated";
            public const string GetById = Base + "/academic-year/{id}";
        }

        public static class AcademicTerm
        {
            public const string Create = Base + "/academic-term/add";
            public const string Update = Base + "/academic-term/{id}/update";
            public const string Delete = Base + "/academic-term/{id}/delete";
            public const string GetAllPagginated = Base + "/academic-term/getall/pagginated";
            public const string GetById = Base + "/academic-term/{id}";
        }

        public static class Student
        {
            public const string AssignStudentsToAcademicAdvisor = Base + "/students/assign-to-academic-advisor";
            public const string SendRequestToGuide = Base + "/students/guidance-request/send";
            public const string GetProfile = Base + "/students/profile";
            public const string AssignToAcademicAdvisor = Base + "/students/assign-advisor";
            public const string RequestGuidance = Base + "/students/request-guidance";
            public const string BookAcademicService = Base + "/students/academic-service/book";
            public const string GetAcademicServiceInitialData = Base + "/students/academic-service/initial-data";
            public const string GetDashboard = Base + "/students/dashboard/{userId}";
            public const string GetSchedule = Base + "/students/schedule";
            public const string UpdateProfile = Base + "/students/profile";
        }

        public static class Advisor
        {
            public const string GetAllGuidanceRequestsPagginated = Base + "/advisor/guidance-requests";
            public const string UpdateGuidanceRequestStatus = Base + "/advisor/guidance-requests/{requestId}";
        }

        public static class Department
        {
            public const string Create = Base + "/department/add";
            public const string Update = Base + "/department/{id}/update";
            public const string Delete = Base + "/department/{id}/delete";
            public const string AssignCourse = Base + "/department/assign-course";
            public const string AssignUser = Base + "/department/assign-user";
            public const string RemoveCourse = Base + "/department/remove-course/{courseId}";
            public const string RemoveUser = Base + "/department/remove-user";
            public const string GetAllPagginated = Base + "/department/getall/pagginated";
            public const string GetAllUsersInDepartmentPagginated = Base + "/department/{id}/users";
            public const string GetByIdOrCode = Base + "/department/id-code";
            public const string GetAllCoursesInDepartment = Base + "/department/{id}/courses";
        }

        public static class ExternalIntegration
        {
            public const string BaseAI = Base + "/ai";
            public const string GetStudentGrades = BaseAI + "/student/{studentId}/grades";
            public const string GetBatchDump = BaseAI + "/analytics/dump";
            public const string GetCatalog = BaseAI + "/course/catalog";
        }

        public static class Admin
        {
            public const string GetDashboard               = Base + "/admin/dashboard";
            public const string GetUsers                   = Base + "/admin/users";
            public const string GetStudentProfile          = Base + "/admin/student/{studentId}/profile";
            public const string GetStaffProfile            = Base + "/admin/staff/{userId}/profile";
            public const string GetExamResults             = Base + "/admin/exam-results";
            public const string GetSecurityDashboard       = Base + "/admin/security/dashboard";
            public const string UpdateSecurityDashboard    = Base + "/admin/security/dashboard";
            public const string GetSettings                = Base + "/admin/settings";
            public const string UpdateSettings             = Base + "/admin/settings";
        }

        public static class AcademicSchedules
        {
            public const string GetAll = Base + "/academic-schedules";
            public const string GetById = Base + "/academic-schedules/{id}";
            public const string Create = Base + "/academic-schedules/add";
            public const string Update = Base + "/academic-schedules/{id}/update";
            public const string Delete = Base + "/academic-schedules/delete";
        }

        public static class ExamSchedules
        {
            public const string GetAll = Base + "/exam-schedules";
            public const string GetById = Base + "/exam-schedules/{id}";
            public const string Create = Base + "/exam-schedules/add";
            public const string Update = Base + "/exam-schedules/{id}/update";
            public const string Delete = Base + "/exam-schedules/delete";
        }

        public static class Staff
        {
            public const string GetStaffDashboard = Base + "/staff/dashboard";

        }

        public static class Generic
        {
            public const string GetProfilesForAdmins = Base + "/generic/profiles";
        }

        public static class Courses
        {
            public const string GetAll    = Base + "/courses";
            public const string GetDetail = Base + "/courses/{courseId}";
        }

        public static class AiIntegration
        {
            public const string GetRecommendations = Base + "/ai/recommendations";
            public const string GenerateTimetable = Base + "/ai/timetable";
            public const string SaveTimetable = Base + "/ai/timetable/save";
            public const string AskAi = Base + "/ai/ask";
            public const string GetAiSessions = Base + "/ai/sessions";
            public const string GetChatHistory = Base + "/ai/history";
            public const string GetAITimetablesByStudent = Base + "/ai/timetable/student/{studentId}";
        }

        public static class Course
        {
            public const string GetAllPagginated = Base + "/courses/getall/pagginated";
            public const string GetById = Base + "/courses/{id}";
            public const string Enroll = Base + "/courses/enroll";
            public const string Drop = Base + "/courses/drop";
            public const string Create = Base + "/courses/add";
            public const string Update = Base + "/courses/{id}/update";
            public const string Delete = Base + "/courses/{id}/delete";
        }

        public static class Reminder
        {
            public const string Create = Base + "/reminders/add";
            public const string Update = Base + "/reminders/{id}/update";
            public const string Delete = Base + "/reminders/{id}/delete";
            public const string GetAllPagginated = Base + "/reminders/getall/pagginated";
        }

        public static class CareerSuggestion
        {
            public const string GetByStudent = Base + "/career-suggestions/student/{studentId}";
        }

        public static class CourseSection
        {
            public const string Create = Base + "/course-sections/add";
            public const string Update = Base + "/course-sections/{id}/update";
            public const string Delete = Base + "/course-sections/{id}/delete";
            public const string GetByCourse = Base + "/course-sections/course/{courseId}";
        }

        public static class StudyPlan
        {
            public const string GetByStudent = Base + "/study-plans/student/{studentId}";
        }

        public static class LearningResource
        {
            public const string GetAll = Base + "/learning-resources";
        }

        public static class GpaCalculator
        {
            public const string Simulate = Base + "/gpa-calculator/simulate";
        }

        public static class DocumentRequest
        {
            public const string Create = Base + "/document-requests";
            public const string GetMyRequests = Base + "/document-requests/my";
            public const string GetAll = Base + "/document-requests";
            public const string UpdateStatus = Base + "/document-requests/{id}/status";
            public const string BulkUpdateStatus = Base + "/document-requests/bulk-status";
            public const string Export = Base + "/document-requests/export";
            public const string Delete = Base + "/document-requests/delete";
        }

        public static class Professor
        {
            public const string GetDashboard = Base + "/professor/dashboard";
            public const string GetSections = Base + "/professor/sections";
            public const string GetSectionStudents = Base + "/professor/sections/{sectionId}/students";
            public const string GetStudentProfile = Base + "/professor/student/{studentId}/profile";
            public const string SubmitGrades = Base + "/professor/sections/{sectionId}/grades";
            public const string GetExamSchedule = Base + "/professor/exam-schedule";
            public const string GetNotifications = Base + "/professor/notifications";

        }

        public static class Assignments
        {
            public const string GetBySection = Base + "/assignments/section/{sectionId}";
            public const string Create = Base + "/assignments";
            public const string GradeSubmission = Base + "/assignments/submissions/{submissionId}/grade";
        }

        public static class Announcement
        {
            public const string Create = Base + "/announcements";
            public const string GetAllPagginated = Base + "/announcements";
            public const string Deactivate = Base + "/announcements/{id}/deactivate";
            public const string Postpone = Base + "/announcements/{id}/postpone";
        }

        public static class StudentSupport
        {
            public const string Create = Base + "/support";
            public const string GetAll = Base + "/support";
            public const string GetMyActive = Base + "/support/my";
            public const string GetById = Base + "/support/{id}";
            public const string Update = Base + "/support/{id}";
            public const string Delete = Base + "/support/delete";
        }
    }
}

