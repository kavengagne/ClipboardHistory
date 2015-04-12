using System;
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
        }
        #endregion


        private void ListBoxHistory_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = ListBoxHistory.SelectedItem;
            if (selectedItem != null)
            {
                var viewModel = DataContext as HistoryListViewModel;
                if (viewModel != null)
                {
                    viewModel.CopyToClipboardCommand.Execute(selectedItem);
                }
            }
        }


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
                if (viewModel != null)
                {
                    viewModel.Dispose();
                }
            }
        }
        #endregion
    }
}
