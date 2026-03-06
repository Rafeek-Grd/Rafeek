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

        public static class AcademicCalendar
        {
            public const string AddEvent = Base + "/academic-calendar/events";
            public const string GetEvents = Base + "/academic-calendar/events";
            public const string GetEventById = Base + "/academic-calendar/events/{id}";
            public const string UpdateEvent = Base + "/academic-calendar/events/{id}";
            public const string DeleteEvent = Base + "/academic-calendar/events/{id}";
        }
    }
}
