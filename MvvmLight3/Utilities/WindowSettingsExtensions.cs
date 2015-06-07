using GolfClub.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace GolfClub.Utilities
{
    internal enum WindowPositionsSetting
    {
        Left,
        Top,
        Width,
        Height
    }

    public static class WindowSettingsExtensions
    {
        #region Fields

        private static readonly string WindowSettingsFile = "GolfWindowsSettings.json";
        private static readonly Dictionary<string, double> WindowSettings;
        private static readonly Array WindowPositionSettings = Enum.GetValues(typeof(WindowPositionsSetting));

        #endregion Fields

        #region Properties

        static WindowSettingsExtensions()
        {
            bool usedOldSettings;
            if (Settings.Default.UseNewWindowSettings)
            {
                WindowSettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "GolfWindowsSettings.json");
                usedOldSettings = false;
            }
            else
            {
                WindowSettingsFile = "GolfWindowsSettings.json";
                Settings.Default.UseNewWindowSettings = true;
                Settings.Default.Save();
                usedOldSettings = true;
            }
            WindowSettings = LoadSettings();

            if (!usedOldSettings) return;
            // JIC we loaded from the old location
            File.Delete(WindowSettingsFile);
            // And make sure we're using the new location
            WindowSettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "GolfWindowsSettings.json");
        }

        public static Dictionary<string, double> LoadWindowSettings(this Window window)
        {
            foreach (WindowPositionsSetting setting in WindowPositionSettings)
            {
                var settingName = window.GetType().Name + setting;
                if (!WindowSettings.ContainsKey(settingName)) continue;
                var value = WindowSettings[settingName];
                SetWindowSetting(window, setting, value);
            }
            return WindowSettings;
        }

        public static void SaveWindowSettings(this Window window, Dictionary<string, double> additionalSettings = null)
        {
            foreach (WindowPositionsSetting setting in WindowPositionSettings)
            {
                var settingName = window.GetType().Name + setting;
                double value = GetSettingValue(window, setting);
                WindowSettings[settingName] = value;
            }
            if (additionalSettings != null)
                foreach (KeyValuePair<string, double> additionalSetting in additionalSettings)
                {
                    WindowSettings[additionalSetting.Key] = additionalSetting.Value;
                }
            WindowSettings.Save();
        }

        private static double GetSettingValue(Window window, WindowPositionsSetting setting)
        {
            double value;
            switch (setting)
            {
                case WindowPositionsSetting.Height:
                    value = window.Height;
                    break;

                case WindowPositionsSetting.Left:
                    value = window.Left;
                    break;

                case WindowPositionsSetting.Top:
                    value = window.Top;
                    break;

                case WindowPositionsSetting.Width:
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

        private static void SetWindowSetting(Window window, WindowPositionsSetting setting, double value)
        {
            switch (setting)
            {
                case WindowPositionsSetting.Height:
                    window.Height = value;
                    break;

                case WindowPositionsSetting.Left:

                    window.Left = value;
                    break;

                case WindowPositionsSetting.Top:
                    window.Top = value;
                    break;

                case WindowPositionsSetting.Width:
                    window.Width = value;
                    break;

                default:
                    throw new InvalidEnumArgumentException(@"Invalid WindowSetting: " + setting);
            }
        }

        #endregion Properties
    }
}