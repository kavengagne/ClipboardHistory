using System;
using System.Linq.Expressions;
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
            set
            {
                if (value > 0 && value <= 200)
                {
                    Settings.Default.CollectionCapacity = value;
                    Settings.Default.Save();
                }
            }
        }

        public static int SnippetNumLines
        {
            get { return Settings.Default.SnippetNumLines; }
            set
            {
                if (value > 0 && value <= 50)
                {
                    Settings.Default.SnippetNumLines = value;
                    Settings.Default.Save();
                }
            }
        }

        public static int ToolTipHoverDelay
        {
            get { return Settings.Default.ToolTipHoverDelay; }
            set
            {
                if (value >= 0 && value <= 99999)
                {
                    Settings.Default.ToolTipHoverDelay = value;
                    Settings.Default.Save();
                }
            }
        }

        public static bool VisualStudioClipboardOnly
        {
            get { return Settings.Default.VisualStudioClipboardOnly; }
            set
            {
                Settings.Default.VisualStudioClipboardOnly = value;
                Settings.Default.Save();
            }
        }

        public static bool PreventDuplicateItems
        {
            get { return Settings.Default.PreventDuplicateItems; }
            set
            {
                Settings.Default.PreventDuplicateItems = value;
                Settings.Default.Save();
            }
        }

        public static bool DisplayTimestamp
        {
            get { return Settings.Default.DisplayTimestamp; }
            set
            {
                Settings.Default.DisplayTimestamp = value;
                Settings.Default.Save();
            }
        }
        #endregion


        #region Public Methods
        public static bool SaveProperty<TProperty, TValue>(Expression<Func<TProperty>> propertyExpression, TValue value)
        {
            var member = propertyExpression.Body as MemberExpression;
            var property = member?.Member as PropertyInfo;
            if (property != null && property.PropertyType == value.GetType())
            {
                return ValidateAndSave(property, value);
            }
            return false;
        }
        #endregion


        #region Private Methods
        private static bool ValidateAndSave(PropertyInfo property, object propertyValue)
        {
            var propertyToSave = typeof(Configuration).GetProperty(property.Name, BindingFlags.Public | BindingFlags.Static);
            if (propertyToSave != null)
            {
                var oldValue = propertyToSave.GetValue(null);
                ExceptionWrapper.TrySafe<Exception>(() => propertyToSave.SetValue(null, propertyValue));
                return !propertyToSave.GetValue(null).Equals(oldValue);
            }
            return false;
        }
        #endregion
    }
}
