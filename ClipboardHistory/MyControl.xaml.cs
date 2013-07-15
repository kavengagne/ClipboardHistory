using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClipboardHistory.Classes;
//using kavengagne.ClipboardHistory.Properties;


// TODO: Create an ItemTemplate in Xaml to style the ListBox Items depending on IsErrorMessage.

namespace kavengagne.ClipboardHistory
{
	/// <summary>
	/// Interaction logic for MyControl.xaml
	/// </summary>
	public partial class MyControl : UserControl, IDisposable
	{
		#region Constants
		#endregion


		#region Fields
		private IntPtr _visualStudioHandle = IntPtr.Zero;
		private ClipboardUpdateNotifier _clipboardUpdateNotifier = null;
		private HistoryCollection _historyCollection = null;
		#endregion


		#region Properties
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
			_historyCollection = new HistoryCollection(Configuration.HistoryCollectionCapacity);
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
				text = string.Format("Exception: {0}\nMost likely, another application is hooking the clipboard.", ex.Message);
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
				AddStringToHistoryCollection(string.Format("Exception: {0}\nMost likely, another application is hooking the clipboard.", ex.Message));
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
        }
        
        private void LoadConfigurations()
        {
            _visualStudioHandle = Process.GetCurrentProcess().MainWindowHandle;
            LoadConfigurationElements();
        }

        private void LoadConfigurationElements()
        {
            this.tbHistoryCollectionCapacity.Tag = new ConfigurationPropertyInfo() {
                PropertyName = "HistoryCollectionCapacity"
            };
            this.tbHistoryCollectionCapacity.Text = Configuration.HistoryCollectionCapacity.ToString();

            this.tbCopyDataShortNumLines.Tag = new ConfigurationPropertyInfo()
            {
                PropertyName = "CopyDataShortNumLines"
            };
            this.tbCopyDataShortNumLines.Text = Configuration.CopyDataShortNumLines.ToString();

            this.cbVisualStudioClipboardOnly.Tag = new ConfigurationPropertyInfo()
            {
                PropertyName = "VisualStudioClipboardOnly"
            };
            this.cbVisualStudioClipboardOnly.IsChecked = Configuration.VisualStudioClipboardOnly;
        }
		#endregion


		#region Event Handlers
        private void MyToolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConfigurations();
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