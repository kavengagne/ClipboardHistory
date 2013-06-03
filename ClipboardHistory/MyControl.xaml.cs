using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClipboardHistory.Classes;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Diagnostics;

// TODO: Create an ItemTemplate in Xaml to style the ListBox Items depending on IsErrorMessage.

namespace kavengagne.ClipboardHistory
{
	/// <summary>
	/// Interaction logic for MyControl.xaml
	/// </summary>
	public partial class MyControl : UserControl, IDisposable
	{
		#region Constants
		// TODO: Migrate this to Visual Studio Options Panel
		private const int HISTORY_LIST_CAPACITY = 25;
		#endregion


		#region Fields
		private ClipboardUpdateNotifier _clipboardUpdateNotifier;
		private HistoryCollection _historyCollection;
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
			AddClipboardDataToHistoryList();
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
			this._historyCollection = new HistoryCollection(HISTORY_LIST_CAPACITY);
		}

		private void AddClipboardDataToHistoryList()
		{
			AddStringToHistoryList(GetClipboardTextOrEmpty());
		}

		private void AddStringToHistoryList(string text)
		{
			this._historyCollection.Insert(0, new ClipboardDataItem(text));
		}

		private void CopySelectedLineToClipboard(int index)
		{
			if ((index >= 0) && (index < this._historyCollection.Count))
			{
				SetClipboardTextOrError(this._historyCollection[index].CopyDataFull);
			}
		}

		private static bool IsControlKeyDown()
		{
			return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
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
				AddStringToHistoryList(string.Format("Exception: {0}\nMost likely, another application is hooking the clipboard.", ex.Message));
			}
			this._clipboardUpdateNotifier.EnableNotifications();
		}
		#endregion


		#region Event Handlers
		private void ClipboardUpdateNotifier_ClipboardUpdate(object sender, EventArgs e)
		{
			AddClipboardDataToHistoryList();
		}

		private void lbHistory_KeyDown(object sender, KeyEventArgs e)
		{
			ListBox listbox = (ListBox)sender;
			if ((Key.C == e.Key) && IsControlKeyDown())
			{
				CopySelectedLineToClipboard(listbox.SelectedIndex);
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
	}
}