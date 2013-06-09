using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClipboardHistory.Classes;
using System.Diagnostics;

// TODO: Create an ItemTemplate in Xaml to style the ListBox Items depending on IsErrorMessage.
// TODO: Add Configurations to Visual Studio Options Box

namespace kavengagne.ClipboardHistory
{
	/// <summary>
	/// Interaction logic for MyControl.xaml
	/// </summary>
	public partial class MyControl : UserControl, IDisposable
	{
		#region Constants
		// TODO: Migrate this to Visual Studio Options Panel
		private const int HISTORY_COLLECTION_CAPACITY = 25;
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
			this._clipboardUpdateNotifier = new ClipboardUpdateNotifier(ClipboardUpdateNotifier_ClipboardUpdate);
			this._clipboardUpdateNotifier.EnableNotifications();
		}

		private void InitializeHistoryCollection()
		{
			this._historyCollection = new HistoryCollection(HISTORY_COLLECTION_CAPACITY);
		}

		private void AddClipboardDataToHistoryCollection()
		{
			AddStringToHistoryCollection(GetClipboardTextOrEmpty());
		}

		private void AddStringToHistoryCollection(string text)
		{
			if (string.IsNullOrEmpty(text)) return;
			this._historyCollection.AddItem(new ClipboardDataItem(text));
		}

		private void CopyHistoryCollectionLineToClipboard(int lineNumber)
		{
			if ((lineNumber < 0) || (lineNumber >= this._historyCollection.Count)) return;
			SetClipboardTextOrError(this._historyCollection[lineNumber].CopyDataFull);
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
			this._clipboardUpdateNotifier.DisableNotifications();
			try
			{
				Clipboard.SetText(text);
			}
			catch (Exception ex)
			{
				AddStringToHistoryCollection(string.Format("Exception: {0}\nMost likely, another application is hooking the clipboard.", ex.Message));
			}
			this._clipboardUpdateNotifier.EnableNotifications();
		}

		private static bool IsControlKeyDown()
		{
			return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
		}
		#endregion


		#region Event Handlers
		private void SaveConfigurations(object sender, RoutedEventArgs e)
		{
			// TODO: Save Configurations to a file or something.
			// VisualStudioClipboardOnly
			// HistoryCollectionCapacity
			// CopyDataShortNumLines
		}

		private void LoadConfigurations(object sender, RoutedEventArgs e)
		{
			// TODO: Load Configurations from a file or something.
			this._visualStudioHandle = Process.GetCurrentProcess().MainWindowHandle;
			// VisualStudioClipboardOnly
			// HistoryCollectionCapacity
			// CopyDataShortNumLines
		}

		private void ClipboardUpdateNotifier_ClipboardUpdate(object sender, EventArgs e)
		{
			ClipboardEventArgs clipboardEvent = e as ClipboardEventArgs;
			if (this.cbVisualStudioClipboardOnly.IsChecked == true)
			{
				if (clipboardEvent.Hwnd != this._visualStudioHandle) return;
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
				this._clipboardUpdateNotifier.Dispose();
				this._clipboardUpdateNotifier = null;
				this._historyCollection = null;
			}
		}
		#endregion

		private void SaveConfigurations()
		{

		}

		private void LoadConfigurations()
		{

		}
	}
}