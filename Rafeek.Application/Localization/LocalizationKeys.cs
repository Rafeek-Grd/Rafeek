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
        }
    }
}
