using System;
using System.Reflection;
using ClipboardHistoryApp.Helpers;
using ClipboardHistoryApp.Properties;

namespace ClipboardHistoryApp.Classes
{
    public static class Configuration
    {
        #region Properties
        public static int CollectionCapacity
        {
            get { return Settings.Default.CollectionCapacity; }
        }
        
        public static int SnippetNumLines
        {
            get { return Settings.Default.SnippetNumLines; }
        }

        public static int ToolTipHoverDelay
        {
            get { return Settings.Default.ToolTipHoverDelay; }
        }

        public static bool VisualStudioClipboardOnly
        {
            get { return Settings.Default.VisualStudioClipboardOnly; }
        }
        
        public static bool PreventDuplicateItems
        {
            get { return Settings.Default.PreventDuplicateItems; }
        }
        #endregion


        #region Public Methods
        public static bool SaveCollectionCapacity(int capacity)
        {
            if (capacity > 0 && capacity <= 200)
            {
                Settings.Default.CollectionCapacity = capacity;
                Settings.Default.Save();
                return true;
            }
            return false;
        }

        public static bool SaveSnippetNumLines(int numlines)
        {
            if (numlines > 0 && numlines <= 50)
            {
                Settings.Default.SnippetNumLines = numlines;
                Settings.Default.Save();
                return true;
            }
            return false;
        }

        private static bool SaveToolTipHoverDelay(int delay)
        {
            if (delay >= 0 && delay <= 99999)
            {
                Settings.Default.ToolTipHoverDelay = delay;
                Settings.Default.Save();
                return true;
            }
            return false;
        }

        public static void SaveVisualStudioClipboardOnly(bool vsonly)
        {
            Settings.Default.VisualStudioClipboardOnly = vsonly;
            Settings.Default.Save();
        }

        public static void SavePreventDuplicateItems(bool preventduplicate)
        {
            Settings.Default.PreventDuplicateItems = preventduplicate;
            Settings.Default.Save();
        }

        public static bool SaveProperty(string propertyName, object propertyValue)
        {
            object convertedValue = null;
            var configurationPropertyType = GetConfigurationPropertyType(propertyName);
            var isConverted = ExceptionHelper.TrySafe<Exception>(() =>
            {
                convertedValue = Convert.ChangeType(propertyValue, configurationPropertyType);
            });
            return (isConverted && convertedValue != null && ValidateAndSave(propertyName, convertedValue));
        }
        #endregion


        #region Private Methods
        private static Type GetConfigurationPropertyType(string propertyName)
        {
            var propertyInfo = typeof(Configuration).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);
            return propertyInfo.PropertyType;
        }

        private static bool ValidateAndSave(string propertyName, object propertyValue)
        {
            var result = false;
            switch (propertyName)
            {
                case "CollectionCapacity":
                    result = SaveCollectionCapacity((int)propertyValue);
                    break;
                case "SnippetNumLines":
                    result = SaveSnippetNumLines((int)propertyValue);
                    break;
                case "ToolTipHoverDelay":
                    result = SaveToolTipHoverDelay((int)propertyValue);
                    break;
                case "VisualStudioClipboardOnly":
                    SaveVisualStudioClipboardOnly((bool)propertyValue);
                    result = true;
                    break;
                case "PreventDuplicateItems":
                    SavePreventDuplicateItems((bool)propertyValue);
                    result = true;
                    break;
            }
            return result;
        }
        #endregion
    }
}
