using System;
using System.Reflection;
using System.Windows.Controls;
using ClipboardHistoryApp.Properties;

namespace ClipboardHistoryApp.Classes
{
    public static class Configuration
    {
        #region Properties
        public static int HistoryCollectionCapacity
        {
            get { return Settings.Default.HistoryCollectionCapacity; }
        }
        
        public static int CopyDataShortNumLines
        {
            get { return Settings.Default.CopyDataShortNumLines; }
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
        public static bool SaveHistoryCollectionCapacity(int capacity)
        {
            var result = false;
            if (capacity > 0 && capacity <= 200)
            {
                Settings.Default.HistoryCollectionCapacity = capacity;
                Settings.Default.Save();
                result = true;
            }
            return result;
        }

        public static bool SaveCopyDataShortNumLines(int numlines)
        {
            var result = false;
            if (numlines > 0 && numlines <= 50)
            {
                Settings.Default.CopyDataShortNumLines = numlines;
                Settings.Default.Save();
                result = true;
            }
            return result;
        }

        private static bool SaveToolTipHoverDelay(int delay)
        {
            var result = false;
            if (delay >= 0 && delay <= 99999)
            {
                Settings.Default.ToolTipHoverDelay = delay;
                Settings.Default.Save();
                result = true;
            }
            return result;
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

        public static bool SavePropertyOrRevert(object sender)
        {
            if (sender is TextBox)
            {
                return SaveTextBoxConfigurationProperty(sender as TextBox);
            }
            if (sender is CheckBox)
            {
                return SaveCheckBoxConfigurationProperty(sender as CheckBox);
            }
            return false;
        }
        #endregion


        #region Private Methods
        private static bool SaveTextBoxConfigurationProperty(TextBox tb)
        {
            var propertyName = (String)tb.Tag;
            var propertyInfo = typeof(Configuration).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);
            var propertyValue = propertyInfo.GetValue(null, null);
            var propertyType = propertyInfo.PropertyType;
            var newValue = tb.Text;

            try
            {
                var convertedValue = Convert.ChangeType(newValue, propertyType);
                if (ValidateAndSavePropertyValue(convertedValue, propertyName))
                {
                    return true;
                }
            }
            catch
            {
                tb.Text = propertyValue.ToString();
            }
            
            // Restore original data.
            // if execution comes here, something wrong happened.
            tb.Text = propertyValue.ToString();
            return false;
        }

        private static bool SaveCheckBoxConfigurationProperty(CheckBox cb)
        {
            var propertyName = cb.Tag as string;
            var propertyValue = cb.IsChecked.HasValue && cb.IsChecked.Value;
            return ValidateAndSavePropertyValue(propertyValue, propertyName);
        }

        private static bool ValidateAndSavePropertyValue(object propertyValue, string propertyName)
        {
            var result = false;
            switch (propertyName)
            {
                case "HistoryCollectionCapacity":
                    result = SaveHistoryCollectionCapacity((int)propertyValue);
                    break;
                case "CopyDataShortNumLines":
                    result = SaveCopyDataShortNumLines((int)propertyValue);
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
