using System;
using System.Windows.Controls;
using kavengagne.ClipboardHistory.Properties;
using System.Diagnostics;
using System.Reflection;

namespace ClipboardHistory.Classes
{
    public class Configuration
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
            if (numlines > 0)
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
            if (capacity > 0)
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
            var info = tb.Tag as ConfigurationPropertyInfo;
            var propertyValue = tb.Text;

            var propertyInfo = typeof(Configuration).GetProperty(
                    info.PropertyName, BindingFlags.Public | BindingFlags.Static);
            var oldValue = propertyInfo.GetValue(null, null);
            var propertyType = propertyInfo.PropertyType;

            try
            {
                var convertedValue = Convert.ChangeType(propertyValue, propertyType);
                if (ValidateAndSavePropertyValue(convertedValue, info.PropertyName))
                {
                    return;
                }
            }
            catch (Exception) { }
            
            // Restore original data.
            // if execution comes here, something wrong happened.
            tb.Text = oldValue.ToString();
        }

        private static void SaveCheckBoxConfigurationProperty(CheckBox cb)
        {
            var info = cb.Tag as ConfigurationPropertyInfo;
            var propertyValue = (cb.IsChecked.HasValue) ? cb.IsChecked.Value : false;
            ValidateAndSavePropertyValue(propertyValue, info.PropertyName);
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


    public class ConfigurationPropertyInfo
    {
        public string PropertyName { get; set; }
    }
}
