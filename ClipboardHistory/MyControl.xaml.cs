using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClipboardHistory.Classes;
using System.Runtime.InteropServices;

// TODO: Create a IClipboardData interface to abstract common Clipboard Data objects properties.
// TODO: Create a Add a Field named IsErrorMessage in IClipboardData interface.
// TODO: Create ClipboardDataItem : IClipboardData object to hold Clipboard Data Items.
// TODO: Create ClipboardDataError : IClipboardData object to hold Clipboard Data Items which are Error messages.
// TODO: Create a Collection of IClipboardData to hold Clipboard Data Items.
// TODO: Change History ListBox to use DataBinding on the Collection of IClipboardData.
// TODO: Create an ItemTemplate in Xaml to style the ListBox Items depending on (Type? or IsErrorMessage?)

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


		#region Members
		private ClipboardUpdateNotifier _clipboardUpdateNotifier;
		#endregion


		#region Constructor
		public MyControl()
		{
			InitializeComponent();
			InitializeClipboardUpdateNotifier();
		}
		#endregion


		#region Private Methods
		private void InitializeClipboardUpdateNotifier()
		{
			this._clipboardUpdateNotifier = new ClipboardUpdateNotifier(ClipboardUpdateNotifier_ClipboardUpdate);
			this._clipboardUpdateNotifier.EnableNotifications();
		}

		private void AddStringToHistoryList(string text, ListBox listbox)
		{
			MaintainHistoryListCapacity(listbox, HISTORY_LIST_CAPACITY);
			listbox.Items.Insert(0, text);
		}

		private void MaintainHistoryListCapacity(ListBox listbox, int capacity)
		{
			int count = listbox.Items.Count;
			if (count == capacity)
			{
				listbox.Items.RemoveAt(count - 1);
			}
		}

		private void CopySelectedLineToClipboard(ListBox listbox, int index)
		{
			if (listbox.SelectedIndex >= 0)
			{
				this._clipboardUpdateNotifier.DisableNotifications();
				try
				{
					Clipboard.SetText((string)listbox.SelectedItem);
				}
				catch (COMException comEx)
				{
					AddStringToHistoryList("COMException: " + comEx.Message + "\nMost likely, another application is hooking the clipboard.", listbox);
				}
				this._clipboardUpdateNotifier.EnableNotifications();
			}
		}

		private static bool IsControlKeyDown()
		{
			return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
		}
		#endregion


		#region Event Handlers
		private void ClipboardUpdateNotifier_ClipboardUpdate(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText())
			{
				this.
				AddStringToHistoryList(Clipboard.GetText(), this.lbHistory);
			}
		}

		private void lbHistory_KeyDown(object sender, KeyEventArgs e)
		{
			if ((Key.C == e.Key) && IsControlKeyDown())
			{
				CopySelectedLineToClipboard(this.lbHistory, this.lbHistory.SelectedIndex);
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
			}
		}
		#endregion
	}
}