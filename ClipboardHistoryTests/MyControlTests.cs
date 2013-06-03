using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using kavengagne.ClipboardHistory;
using System.Reflection;
using System.Threading;
using ClipboardHistory.Classes;
using System.Collections.ObjectModel;
using UnitTestHelperBase;
using System.Windows;

namespace ClipboardHistoryTests
{
	#region Properties
	[TestFixture]
	public class MyControl_Properties_Tests
	{
		[Test, STAThread]
		public void When_Creating_Object_ClipboardDataItemCollection_Property_Should_Be_Initialized()
		{
			// Prepare
			MyControl myControl = new MyControl();

			// Act
			Object item = myControl.HistoryCollection;

			// Assert
			Assert.IsNotNull(item);
		}

		[Test, STAThread]
		public void When_Creating_Object_DataContext_Property_Should_Be_Not_Null()
		{
			// Prepare
			MyControl myControl = new MyControl();

			// Act
			Object item = myControl.DataContext;

			// Assert
			Assert.IsNotNull(item);
		}
	}
	#endregion


	#region Fields
	public class MyControl_Fields_Tests
	{
		[Test, STAThread]
		public void When_Creating_Object_ListBox_Field_Should_Be_Initialized()
		{
			// Prepare
			MyControl myControl = new MyControl();
			
			// Act
			FieldInfo info = typeof(MyControl).GetField("lbHistory", BindingFlags.Instance | BindingFlags.NonPublic);

			// Assert
			Assert.IsNotNull(info, "Field does not exist.");
			Assert.IsNotNull(info.GetValue(myControl), "Field is not initialized");
		}

		[Test, STAThread]
		public void When_Creating_Object_ClipboardUpdateNotifier_Field_Should_Be_Initialized()
		{
			// Prepare
			MyControl myControl = new MyControl();

			// Act
			FieldInfo info = typeof(MyControl).GetField("_clipboardUpdateNotifier", BindingFlags.Instance | BindingFlags.NonPublic);
			
			// Assert
			Assert.IsNotNull(info, "Field does not exist.");
			Assert.IsNotNull(info.GetValue(myControl), "Field is not initialized");
		}

		[Test, STAThread]
		public void When_Creating_Object_ClipboardDataItemCollection_Field_Should_Be_Initialized()
		{
			// Prepare
			MyControl myControl = new MyControl();

			// Act
			FieldInfo info = typeof(MyControl).GetField("_historyCollection", BindingFlags.Instance | BindingFlags.NonPublic);

			// Assert
			Assert.IsNotNull(info, "Field does not exist.");
			Assert.IsNotNull(info.GetValue(myControl), "Field is not initialized");
		}
	} 
	#endregion


	#region AddClipboardDataToHistoryList Method
	[TestFixture]
	public class MyControl_AddClipboardDataToHistoryList_Tests
	{
		[Test, STAThread]
		public void When_Calling_Method_ListBox_Should_Contain_1_More_Line()
		{
			// Prepare
			MyControl myControl = new MyControl();
			int linesInitial = myControl.HistoryCollection.Count;

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "AddClipboardDataToHistoryList", myControl, null);

			// Assert
			Assert.AreEqual(linesInitial + 1, myControl.HistoryCollection.Count);
		}

		[Test, STAThread]
		public void When_Calling_Method_ListBox_Last_Line_Should_Be_Same_As_Clipboard_Content()
		{
			// Prepare
			MyControl myControl = new MyControl();
			string clipboardContent = Clipboard.GetText();

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "AddClipboardDataToHistoryList", myControl, null);

			// Assert
			int count = myControl.HistoryCollection.Count;
			ClipboardDataItem item = myControl.HistoryCollection[count - 1];
			Assert.AreEqual(clipboardContent, item.CopyDataFull);
		}
	}
	#endregion


	#region AddStringToHistoryList Method
	[TestFixture]
	public class MyControl_AddStringToHistoryList_Tests
	{
		[Test, STAThread]
		public void When_()
		{

		}
	}
	#endregion


	#region MaintainHistoryListCapacity Method
	[TestFixture]
	public class MyControl_MaintainHistoryListCapacity_Tests
	{
		[Test, STAThread]
		public void When_()
		{

		}
	}
	#endregion


	#region CopySelectedLineToClipboard Method
	[TestFixture]
	public class MyControl_CopySelectedLineToClipboard_Tests
	{
		[Test, STAThread]
		public void When_()
		{

		}
	}
	#endregion


	#region IsControlKeyDown Method
	[TestFixture]
	public class MyControl_IsControlKeyDown_Tests
	{
		[Test, STAThread]
		public void When_()
		{

		}
	}
	#endregion


	#region ClipboardUpdateNotifier_ClipboardUpdate Event Handler
	[TestFixture]
	public class MyControl_ClipboardUpdateNotifier_ClipboardUpdate_Tests
	{
		[Test, STAThread]
		public void When_()
		{

		}
	}
	#endregion


	#region lbHistory_KeyDown Event Handler
	[TestFixture]
	public class MyControl_lbHistory_KeyDown_Tests
	{
		[Test, STAThread]
		public void When_()
		{

		}
	}
	#endregion
}
