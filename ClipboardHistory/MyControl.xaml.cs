using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClipboardHistory.Classes;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

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


		#region Fields
		private ClipboardUpdateNotifier _clipboardUpdateNotifier;
		private ObservableCollection<ClipboardDataItem> _clipboardDataItemCollection;
		#endregion


		#region Properties
		public ObservableCollection<ClipboardDataItem> ClipboardDataItemCollection
		{
			get { return this._clipboardDataItemCollection; }
		} 
		#endregion


		#region Constructor
		public MyControl()
		{
			InitializeComponent();
			InitializeClipboardUpdateNotifier();
			InitializeClipboardDataItemCollection();
			this.DataContext = this;
			AddClipboardDataToHistoryList(this.lbHistory);
		}
		#endregion


		#region Private Methods
		private void InitializeClipboardUpdateNotifier()
		{
			this._clipboardUpdateNotifier = new ClipboardUpdateNotifier(ClipboardUpdateNotifier_ClipboardUpdate);
			this._clipboardUpdateNotifier.EnableNotifications();
		}

		private void InitializeClipboardDataItemCollection()
		{
			this._clipboardDataItemCollection = new ObservableCollection<ClipboardDataItem>();
		}

		private void AddClipboardDataToHistoryList(ListBox listbox)
		{
			if (Clipboard.ContainsText())
			{
				this.AddStringToHistoryList(Clipboard.GetText());
			}
		}

		private void AddStringToHistoryList(string text)
		{
			MaintainHistoryListCapacity(HISTORY_LIST_CAPACITY);
			this._clipboardDataItemCollection.Insert(0, new ClipboardDataItem(text));
		}

		private void MaintainHistoryListCapacity(int capacity)
		{
			int count = this._clipboardDataItemCollection.Count;
			if (count == capacity)
			{
				this._clipboardDataItemCollection.RemoveAt(count - 1);
			}
		}

		private void CopySelectedLineToClipboard(int index)
		{
			if ((index >= 0) && (index < this._clipboardDataItemCollection.Count))
			{
				this._clipboardUpdateNotifier.DisableNotifications();
				try
				{
					Clipboard.SetText(this._clipboardDataItemCollection[index].CopyDataFull);
				}
				catch (COMException comEx)
				{
					AddStringToHistoryList("COMException: " + comEx.Message + "\nMost likely, another application is hooking the clipboard.");
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
			AddClipboardDataToHistoryList(this.lbHistory);
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
				this._clipboardDataItemCollection = null;
			}
		}
		#endregion
	}
}