using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClipboardHistoryApp.Classes;
using ClipboardHistoryApp.ViewModels;

namespace ClipboardHistoryApp.Controls
{
    public partial class HistoryListControl : IDisposable
    {
        #region Constructor
        public HistoryListControl()
        {
            InitializeComponent();
        }
        #endregion


        #region Private Methods
        private void LoadConfigurationValues()
        {
            ToolTipService.SetInitialShowDelay(ListBoxHistory, Configuration.ToolTipHoverDelay);
        }
        #endregion


        #region Event Handlers
        private void MyToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfigurationValues();
            var viewModel = DataContext as HistoryListViewModel;
            viewModel?.SetVisualStudioHandle(Process.GetCurrentProcess().MainWindowHandle);
        }

        private void ListBoxHistory_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = ListBoxHistory.SelectedItem;
            if (selectedItem != null)
            {
                var viewModel = DataContext as HistoryListViewModel;
                viewModel?.CopyToClipboardCommand.Execute(selectedItem);
            }
        }
        #endregion


        #region Implement IDisposable Interface
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~HistoryListControl()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var viewModel = DataContext as HistoryListViewModel;
                viewModel?.Dispose();
            }
        }
        #endregion
    }
}
