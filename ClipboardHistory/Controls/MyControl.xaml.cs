using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClipboardHistory.Classes;
using System.Collections;


// TODO: KG - Add a view on CopyDataFull when Double-Click item in ListBox.
// TODO: KG - Add something to show that we are not displaying the full text.

namespace kavengagne.ClipboardHistory
{
	/// <summary>
	/// Interaction logic for MyControl.xaml
	/// </summary>
	public partial class MyControl : UserControl, IDisposable
	{
        #region Fields
		private IntPtr _visualStudioHandle = IntPtr.Zero;
		private ClipboardUpdateNotifier _clipboardUpdateNotifier = null;
		private HistoryCollection _historyCollection = null;
		#endregion


		#region Properties
        // TODO: KG - Only used for Tests, Should be removed soon.
		public HistoryCollection HistoryCollection
		{
			get { return this._historyCollection; }
		}
		#endregion


		#region Constructor
		public MyControl()
		{
			InitializeComponent();
			InitializeClipboardUpdateNotifier();
			InitializeHistoryCollection();
			this.DataContext = this;
            AddClipboardDataToHistoryCollection();
		}
		#endregion


		#region Private Methods
		private void InitializeClipboardUpdateNotifier()
		{
			_clipboardUpdateNotifier = new ClipboardUpdateNotifier(ClipboardUpdateNotifier_ClipboardUpdate);
			_clipboardUpdateNotifier.EnableNotifications();
		}

		private void InitializeHistoryCollection()
		{
			_historyCollection = new HistoryCollection();
		}

        private void AddClipboardDataToHistoryCollection()
        {
            AddStringToHistoryCollection(GetClipboardTextOrEmpty());
        }

		private void AddStringToHistoryCollection(string text)
		{
			if (string.IsNullOrEmpty(text)) return;
			_historyCollection.AddItem(new ClipboardDataItem(text));
		}

        private void AddErrorMessageToHistoryCollection(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage)) return;
            _historyCollection.AddItem(new ClipboardDataItem(errorMessage, true));
        }

		private void CopyHistoryCollectionLineToClipboard(int lineNumber)
		{
			if ((lineNumber < 0) || (lineNumber >= _historyCollection.Count)) return;
			SetClipboardTextOrError(_historyCollection[lineNumber].CopyDataFull);
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
                    string.Format("Exception: {0}\nMost likely, another application is hooking the clipboard.", ex.Message));
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
                    string.Format("Exception: {0}\nMost likely, another application is hooking the clipboard.", ex.Message));
			}
			_clipboardUpdateNotifier.EnableNotifications();
		}

		private static bool IsControlKeyDown()
		{
			return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
		}

        private void ValidateAndSaveConfigurations(object sender)
        {
            Configuration.SavePropertyOrRevert(sender);
            this.HistoryCollection.Refresh();
        }
        
        private void LoadValidationRules()
        {
            // TODO: KG - I hate this solution, but keeping it for now.
            this.tbHistoryCollectionCapacity.Tag = "HistoryCollectionCapacity";
            this.tbCopyDataShortNumLines.Tag = "CopyDataShortNumLines";
            this.cbVisualStudioClipboardOnly.Tag = "VisualStudioClipboardOnly";
        }

        private void LoadConfigurationValues()
        {
            this.tbHistoryCollectionCapacity.Text = Configuration.HistoryCollectionCapacity.ToString();
            this.tbCopyDataShortNumLines.Text = Configuration.CopyDataShortNumLines.ToString();
            this.cbVisualStudioClipboardOnly.IsChecked = Configuration.VisualStudioClipboardOnly;
        }
		#endregion


		#region Event Handlers
        private void MyToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _visualStudioHandle = Process.GetCurrentProcess().MainWindowHandle;
            LoadValidationRules();
            LoadConfigurationValues();
        }

		private void ClipboardUpdateNotifier_ClipboardUpdate(object sender, EventArgs e)
		{
			ClipboardEventArgs clipboardEvent = e as ClipboardEventArgs;
			if (Configuration.VisualStudioClipboardOnly == true)
			{
				if (clipboardEvent.Hwnd != _visualStudioHandle) return;
			}
            AddClipboardDataToHistoryCollection();
		}

		private void lbHistory_KeyDown(object sender, KeyEventArgs e)
		{
			ListBox listbox = (ListBox)sender;
			if ((Key.C == e.Key) && IsControlKeyDown())
			{
				CopyHistoryCollectionLineToClipboard(listbox.SelectedIndex);
				e.Handled = true;
			}
		}

        private void textBoxLostFocus(object sender, RoutedEventArgs e)
        {
            // Needed for Control to Update correctly.
            e.Handled = false;
            ValidateAndSaveConfigurations(sender);
        }
		#endregion


		#region IDisposable Interface / Resources Cleanup
		public void Dispose()
		{
			Dispose(true);
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				_clipboardUpdateNotifier.Dispose();
				_clipboardUpdateNotifier = null;
				_historyCollection = null;
			}
		}
		#endregion
    }
}
