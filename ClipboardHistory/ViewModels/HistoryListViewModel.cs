using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ClipboardHistoryApp.Classes;
using ClipboardHistoryApp.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace ClipboardHistoryApp.ViewModels
{
    public class HistoryListViewModel : ViewModelBase, IDisposable
    {
        #region Fields
        private readonly IntPtr _visualStudioHandle;
        private ClipboardUpdateNotifier _clipboardUpdateNotifier;
        #endregion Fields


        #region Properties
        public HistoryCollection HistoryCollection { get; private set; }
        public HistoryConfiguration HistoryConfiguration { get; set; }
        
        public ICommand CopyToClipboardCommand { get; set; }
        #endregion Properties


        #region Constructors
        public HistoryListViewModel()
        {
            HistoryCollection = new HistoryCollection();
            HistoryConfiguration = new HistoryConfiguration(HistoryCollection);
            
            InitializeClipboardUpdateNotifier();
            AddClipboardDataToHistoryCollection();
            
            _visualStudioHandle = Process.GetCurrentProcess().MainWindowHandle;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CopyToClipboardCommand = new RelayCommand<ClipboardDataItem>(CopyToClipboard, CanCopyToClipboard);
        }
        #endregion Constructors


        #region Commands
        private bool CanCopyToClipboard(ClipboardDataItem item)
        {
            return true;
        }

        private void CopyToClipboard(ClipboardDataItem item)
        {
            SetClipboardTextOrError(item.Data);
        }
        #endregion Commands


        #region Private Methods
        private void InitializeClipboardUpdateNotifier()
        {
            _clipboardUpdateNotifier = new ClipboardUpdateNotifier(ClipboardUpdateNotifier_ClipboardUpdate);
            _clipboardUpdateNotifier.EnableNotifications();
        }

        private void ClipboardUpdateNotifier_ClipboardUpdate(object sender, EventArgs e)
        {
            var clipboardEvent = e as ClipboardEventArgs;
            if (Configuration.VisualStudioClipboardOnly)
            {
                if (clipboardEvent != null && clipboardEvent.Hwnd != _visualStudioHandle) return;
            }
            AddClipboardDataToHistoryCollection();
        }

        private void AddClipboardDataToHistoryCollection()
        {
            AddStringToHistoryCollection(GetClipboardTextOrEmpty());
        }

        private void AddStringToHistoryCollection(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            HistoryCollection.AddItem(new ClipboardDataItem(text));
        }

        private void AddErrorMessageToHistoryCollection(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage)) return;
            HistoryCollection.AddItem(new ClipboardDataItem(errorMessage, true));
        }

        private string GetClipboardTextOrEmpty()
        {
            string text = string.Empty;
            if (!Clipboard.ContainsText()) return text;
            try
            {
                text = Clipboard.GetText();
            }
            catch (Exception ex)
            {
                text = string.Empty;
                AddErrorMessageToHistoryCollection(
                    string.Format("Exception: {0}\nMost likely, another application is hooking the clipboard.",
                                  ex.Message));
            }
            return text;
        }

        private void SetClipboardTextOrError(string text)
        {
            _clipboardUpdateNotifier.DisableNotifications();
            try
            {
                Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                AddErrorMessageToHistoryCollection(
                    string.Format("Exception: {0}\nMost likely, another application is hooking the clipboard.",
                                  ex.Message));
            }
            _clipboardUpdateNotifier.EnableNotifications();
        }
        #endregion Private Methods


        #region Implement IDisposable Interface
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~HistoryListViewModel()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_clipboardUpdateNotifier != null)
                {
                    _clipboardUpdateNotifier.Dispose();
                    _clipboardUpdateNotifier = null;
                }
            }
        }
        #endregion
    }
}