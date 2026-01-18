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
    }
}
