using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace GolfClub.Utilities
{
    internal enum WindowSetting
    {
        Left,
        Top,
        Width,
        Height
    }

    public static class WindowSettingsExtensions
    {
        #region Fields

        private const string WindowSettingsFile = "GolfWindowsSettings.json";
        private static readonly Dictionary<string, double> Settings;
        private static readonly Array WindowSettings = Enum.GetValues(typeof(WindowSetting));

        #endregion Fields

        #region Properties

        static WindowSettingsExtensions()
        {
            Settings = LoadSettings();
        }

        public static Dictionary<string, double> LoadWindowSettings(this Window window)
        {
            foreach (WindowSetting setting in WindowSettings)
            {
                var settingName = window.GetType().Name + setting;
                if (!Settings.ContainsKey(settingName)) continue;
                var value = Settings[settingName];
                SetWindowSetting(window, setting, value);
            }
            return Settings;
        }

        public static void SaveWindowSettings(this Window window, Dictionary<string, double> additionalSettings = null)
        {
            foreach (WindowSetting setting in WindowSettings)
            {
                var settingName = window.GetType().Name + setting;
                double value = GetSettingValue(window, setting);
                Settings[settingName] = value;
            }
            if (additionalSettings != null)
                foreach (KeyValuePair<string, double> additionalSetting in additionalSettings)
                {
                    Settings[additionalSetting.Key] = additionalSetting.Value;
                }
            Settings.Save();
        }

        private static double GetSettingValue(Window window, WindowSetting setting)
        {
            double value;
            switch (setting)
            {
                case WindowSetting.Height:
                    value = window.Height;
                    break;

                case WindowSetting.Left:
                    value = window.Left;
                    break;

                case WindowSetting.Top:
                    value = window.Top;
                    break;

                case WindowSetting.Width:
                    value = window.Width;
                    break;

                default:
                    throw new InvalidEnumArgumentException(@"Invalid WindowSetting: " + setting);
            }
            return value;
        }

        private static Dictionary<string, double> LoadSettings()
        {
            var json = File.Exists(WindowSettingsFile) ? File.ReadAllText(WindowSettingsFile) : "";
            return string.IsNullOrWhiteSpace(json)
                ? new Dictionary<string, double>()
                : JsonConvert.DeserializeObject<Dictionary<string, double>>(json);
        }

        private static void Save(this Dictionary<string, double> settings)
        {
            File.WriteAllText(WindowSettingsFile, JsonConvert.SerializeObject(settings));
        }

        private static void SetWindowSetting(Window window, WindowSetting setting, double value)
        {
            switch (setting)
            {
                case WindowSetting.Height:
                    window.Height = value;
                    break;

                case WindowSetting.Left:

                    window.Left = value;
                    break;

                case WindowSetting.Top:
                    window.Top = value;
                    break;

                case WindowSetting.Width:
                    window.Width = value;
                    break;

                default:
                    throw new InvalidEnumArgumentException(@"Invalid WindowSetting: " + setting);
            }
        }

        #endregion Properties
    }
}