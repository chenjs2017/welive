﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace WeLive
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants
        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string CookieKey = "cookie";
        static readonly string CookieDefault = string.Empty;
        #endregion

        public static string Cookie
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(CookieKey, CookieDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(CookieKey, value);
            }
        }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserId) && !string.IsNullOrWhiteSpace(Cookie);
        public static string UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(UserIdKey, UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(UserIdKey, value);
            }
        }

        public static void ResetCookie()
        {
            UserId = string.Empty;
            Cookie = string.Empty;
        }
    }
}
