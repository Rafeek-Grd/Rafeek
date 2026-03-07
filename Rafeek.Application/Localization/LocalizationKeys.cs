namespace Rafeek.Application.Localization
{
    public static class LocalizationKeys
    {
        public static class AcionResultMessage
        {
            public static readonly KeyString Ok = new("OkActionResultMessage");
            public static readonly KeyString Created = new("CreatedActionResultMessage");
            public static readonly KeyString Accepted = new("AcceptedActionResultMessage");
            public static readonly KeyString Deleted = new("DeletedActionResultMessage");
        }

        public record ExceptionMessage
        {
            public static readonly KeyString Validation = new("ValidationExceptionMessage");
            public static readonly KeyString InvalidModelState = new("InvalidModelStateExceptionMessage");
            public static readonly KeyString NotFound = new("NotFoundExceptionMessage");
            public static readonly KeyString BadRequest = new("BadRequestExceptionMessage");
            public static readonly KeyString Unauthorized = new("UnauthorizedExceptionMessage");
            public static readonly KeyString UnknownException = new("UnknownExceptionMessage");
        }

        public record TokenMessages
        {
            public static readonly KeyString NotValid = new("TokenNotValid");
            public static readonly KeyString NotFound = new("TokenNotFound");
            public static readonly KeyString Expired = new("TokenExpired");
            public static readonly KeyString Required = new("TokenRequired");
        }

        public record UserMessages
        {
            public static readonly KeyString InvalidSignIn = new("InvalidSignIn");
            public static readonly KeyString EmailAlreadyExistedbefore = new("EmailAlreadyExistedbefore");
            public static readonly KeyString GenderIsNotValid = new("GenderIsNotValid");
            public static readonly KeyString PasswordRequired = new("PasswordRequired");
            public static readonly KeyString PasswordValid = new("PasswordValid");
            public static readonly KeyString PasswordConfirmNotEqual = new("PasswordConfirmNotEqual");
            public static readonly KeyString FullNameRequired = new("FullNameRequired");
            public static readonly KeyString PhoneRequired = new("PhoneRequired");
            public static readonly KeyString EmailRequired = new("EmailRequired");
            public static readonly KeyString NationalNumberRequired = new("NationalNumberRequired");
            public static readonly KeyString UserTypeRequired = new("UserTypeRequired");
            public static readonly KeyString PasswordMinLength = new("PasswordMinLength");
            public static readonly KeyString FailedSignUp = new("FailedSignUp");
            public static readonly KeyString PrimayRoleInvalid = new("PrimayRoleInvalid");
            public static readonly KeyString AdditionalRolesInvalid = new("AdditionalRolesInvalid");
            public static readonly KeyString NotFound = new("UserNotFound");
            public static readonly KeyString Locked = new("UserLocked");
            public static readonly KeyString EmailUnVerified = new("EmailUnVerified");
            public static readonly KeyString ResetTokenInvalid = new("ResetTokenInvalid");
            public static readonly KeyString PasswordResetSuccess = new("PasswordResetSuccess");
            public static readonly KeyString EmailNotFoundBefore = new("EmailNotFoundBefore");
            public static readonly KeyString EmailAlreadyActivated = new("EmailAlreadyActivated");
            public static readonly KeyString FailedToActivateEmail = new("FailedToActivateEmail");
            public static readonly KeyString EmailActivatedSuccessfully = new("EmailActivatedSuccessfully");
        }

        public record UploadFileMessages
        {
            public static readonly KeyString PalceRequried = new("PlaceNotValid");
            public static readonly KeyString PalceNotValid = new("PlaceNotSupported");
            public static readonly KeyString Requried = new("FilePalceNotValid");
            public static readonly KeyString FileNotFound = new("FileNotFound");
            public static readonly KeyString NameRequried = new("FileNameRequried");
            public static readonly KeyString FileNotValid = new("FileNotValid");
            public static readonly KeyString FileUploadFailed = new("FileUploadFailed");
            public static readonly KeyString FileFaildToDeleted = new("FaildToDeleted");
            public static readonly KeyString InvalidContentType = new("InvalidContentType");
        }

        public record GlobalValidationMessages
        {
            public static readonly KeyString EmailExist = new("EmailExist");
            public static readonly KeyString NationalNumberExist = new("NationalNumberExist");
            public static readonly KeyString PhoneNumberExist = new("PhoneNumberExist");
            public static readonly KeyString UserCodeExist = new("UserCodeExist");
            public static readonly KeyString PhoneInvalid = new("PhoneInvalid");
            public static readonly KeyString EmailInvalid = new("EmailInvalid");
            public static readonly KeyString EmailDomainInvalid = new("EmailDomainInvalid");
            public static readonly KeyString UniversityCodeMultipleAttemps = new("UniversityCodeMultipleAttemps");
            public static readonly KeyString UniversityEmailMultipleAttemps = new("UniversityEmailMultipleAttemps");
            public static readonly KeyString InvalidToken = new("InvalidToken");
            public static readonly KeyString EmailNotFound = new("EmailNotFound");
            public static readonly KeyString EmailNotActivated = new("EmailNotActivated");
        }

        public record AcademicCalendar
        {
            public static readonly KeyString EventNameRequired = new("EventNameRequired");
            public static readonly KeyString MaxLengthOfEventNameExceededRequiredLength = new("MaxLengthOfEventNameExceededRequiredLength");
            public static readonly KeyString EventDateRequired = new("EventDateRequired");
            public static readonly KeyString StartTimeRequired = new("StartTimeRequired");
            public static readonly KeyString EndTimeRequired = new("EndTimeRequired");
            public static readonly KeyString EndTimeMustBeGreaterThanStartTime = new("EndTimeMustBeGreaterThanStartTime");
            public static readonly KeyString LocationRequired = new("LocationRequired");
            public static readonly KeyString MaxLengthOfLocationExceededRequiredLength = new("MaxLengthOfLocationExceededRequiredLength");
            public static readonly KeyString TargetUserIdRequired = new("TargetUserIdRequired");
            public static readonly KeyString EndDateMustBeGreaterThanOrEqualEventDate = new("EndDateMustBeGreaterThanOrEqualEventDate");
            public static readonly KeyString RecurrenceEndDateRequired = new("RecurrenceEndDateRequired");
            public static readonly KeyString RecurrenceEndDateMustBeAfterEventDate = new("RecurrenceEndDateMustBeAfterEventDate");
            public static readonly KeyString CourseIdRequiredForExamEvent = new("CourseIdRequiredForExamEvent");
        }

        public record EmailTemplates
        {
            public record EmailTemplatesKeys
            {
                public KeyString? Subject { get; set; }
                public KeyString? Template { get; set; }
                public KeyString? Message { get; set; }
            }

            public static EmailTemplatesKeys ForgotPassword => new EmailTemplatesKeys()
            {
                Subject = new("PasswordResetSubject"),
                Template = new("PasswordResetTemplate"),
                Message = new("PasswordResetMessage")
            };

            public static EmailTemplatesKeys SendUserCredentials => new EmailTemplatesKeys()
            {
                Subject = new("SendUserCredentialsSubject"),
                Template = new("SendUserCredentialsTemplate"),
                Message = new("SendUserCredentialsMessage")
            };
        }
    }
}
