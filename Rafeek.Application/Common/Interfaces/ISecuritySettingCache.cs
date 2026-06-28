using System;

namespace Rafeek.Application.Common.Interfaces
{
    /// <summary>
    /// Cache in-memory singleton للإعدادات الأمنية.
    /// يُحدَّث فور تغييره من لوحة الأدمن — بدون إعادة تشغيل السيرفر.
    /// </summary>
    public interface ISecuritySettingCache
    {
        /// <summary>مدة عيش الـ JWT (مشتقة من SessionTimeoutMinutes)</summary>
        TimeSpan TokenLifetime { get; }

        /// <summary>هل يجب إجبار المستخدمين على تغيير كلمة المرور؟</summary>
        bool IsForcePasswordChangeEnabled { get; }

        /// <summary>يُستدعى من CommandHandler بعد حفظ الإعدادات في DB</summary>
        void Update(int sessionTimeoutMinutes, bool isForcePasswordChangeEnabled);
    }
}
