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
            public const string GetDashboard = Base + "/students/dashboard/{userId}";
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

        public static class AiIntegration
        {
            public const string GetRecommendations = Base + "/ai/recommendations";
        }
    }
}
