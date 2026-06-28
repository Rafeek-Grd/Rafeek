using System;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Infrastructure.Services
{
    public class SecuritySettingCache : ISecuritySettingCache
    {
        // Defaults — will be overwritten on startup seed or first admin update
        private TimeSpan _tokenLifetime = TimeSpan.FromMinutes(15);
        private bool _isForcePasswordChangeEnabled = false;

        public TimeSpan TokenLifetime => _tokenLifetime;
        public bool IsForcePasswordChangeEnabled => _isForcePasswordChangeEnabled;

        public void Update(int sessionTimeoutMinutes, bool isForcePasswordChangeEnabled)
        {
            _tokenLifetime = TimeSpan.FromMinutes(sessionTimeoutMinutes);
            _isForcePasswordChangeEnabled = isForcePasswordChangeEnabled;
        }
    }
}
