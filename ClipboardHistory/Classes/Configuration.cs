using System;
using System.Windows.Controls;
using kavengagne.ClipboardHistory.Properties;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;

namespace ClipboardHistory.Classes
{
    public static class Configuration
    {
        #region Properties
        public static bool VisualStudioClipboardOnly
        {
            get { return Settings.Default.VisualStudioClipboardOnly; }
        }

        public static int HistoryCollectionCapacity
        {
            get { return Settings.Default.HistoryCollectionCapacity; }
        }

        public static int CopyDataShortNumLines
        {
            get { return Settings.Default.CopyDataShortNumLines; }
        }
        #endregion


        #region Public Methods
        public static void SaveVisualStudioClipboardOnly(bool vsonly)
        {
            Settings.Default.VisualStudioClipboardOnly = vsonly;
            Settings.Default.Save();
        }

        public static bool SaveCopyDataShortNumLines(int numlines)
        {
            var result = false;
            if (numlines > 0 && numlines <= 500)
            {
                Settings.Default.CopyDataShortNumLines = numlines;
                Settings.Default.Save();
                result = true;
            }
            return result;
        }

        public static bool SaveHistoryCollectionCapacity(int capacity)
        {
            var result = false;
            if (capacity > 0 && capacity <= 500)
            {
                Settings.Default.HistoryCollectionCapacity = capacity;
                Settings.Default.Save();
                result = true;
            }
            return result;
        }

        public static void SavePropertyOrRevert(object sender)
        {
            if (sender is TextBox)
            {
                SaveTextBoxConfigurationProperty(sender as TextBox);
            }

            if (sender is CheckBox)
            {
                SaveCheckBoxConfigurationProperty(sender as CheckBox);
            }
        }
        #endregion


        #region Private Methods
        private static void SaveTextBoxConfigurationProperty(TextBox tb)
        {
            var propertyName = tb.Tag as string;
            var propertyInfo = typeof(Configuration).GetProperty(propertyName,
                                                                 BindingFlags.Public | BindingFlags.Static);
            var propertyValue = propertyInfo.GetValue(null, null);
            var propertyType = propertyInfo.PropertyType;
            var newValue = tb.Text;

            try
            {
                var convertedValue = Convert.ChangeType(newValue, propertyType);
                if (ValidateAndSavePropertyValue(convertedValue, propertyName))
                {
                    return;
                }
            }
            catch (Exception) { }
            
            // Restore original data.
            // if execution comes here, something wrong happened.
            tb.Text = propertyValue.ToString();
        }

        private static void SaveCheckBoxConfigurationProperty(CheckBox cb)
        {
            var propertyName = cb.Tag as string;
            var propertyValue = (cb.IsChecked.HasValue) ? cb.IsChecked.Value : false;
            ValidateAndSavePropertyValue(propertyValue, propertyName);
        }

        private static bool ValidateAndSavePropertyValue(object propertyValue, string propertyName)
        {
            var result = false;
            switch (propertyName)
            {
                case "VisualStudioClipboardOnly":
                    Configuration.SaveVisualStudioClipboardOnly((bool)propertyValue);
                    result = true;
                    break;
                case "HistoryCollectionCapacity":
                    result = Configuration.SaveHistoryCollectionCapacity((int)propertyValue);
                    break;
                case "CopyDataShortNumLines":
                    result = Configuration.SaveCopyDataShortNumLines((int)propertyValue);
                    break;
                default:
                    break;
            }
            return result;
        }
        #endregion
    }
}
