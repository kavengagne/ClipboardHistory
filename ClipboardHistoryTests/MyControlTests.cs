using System;
using ClipboardHistoryApp.Classes;
using ClipboardHistoryApp.Controls;
using NUnit.Framework;
using System.Reflection;
using System.Windows;
using System.Runtime.InteropServices;


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


	#region AddStringToHistoryCollection Method
	[TestFixture]
	public class MyControl_AddStringToHistoryCollection_Tests
	{
		[Test, STAThread]
		public void When_Given_Empty_String_HistoryCollection_Should_Not_Contain_Anymore_Lines()
		{
			// Prepare
			MyControl myControl = new MyControl();
			string inputString = "";
			int expectedNumberOfLines = myControl.HistoryCollection.Count;

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "AddStringToHistoryCollection", myControl, new object[] { inputString });

			// Assert
			Assert.AreEqual(expectedNumberOfLines, myControl.HistoryCollection.Count);
		}

		[Test, STAThread]
		public void When_Given_Valid_String_HistoryCollection_Should_Contain_1_More_Line()
		{
			// Prepare
			MyControl myControl = new MyControl();
			string inputString = "My Test Line Should Be Added";
			int expectedNumberOfLines = myControl.HistoryCollection.Count + 1;

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "AddStringToHistoryCollection", myControl, new object[] { inputString });

			// Assert
			Assert.AreEqual(expectedNumberOfLines, myControl.HistoryCollection.Count);
		}

		[Test, STAThread]
		public void When_Given_Valid_String_HistoryCollection_First_Line_CopyDataFull_Property_Should_Be_Same_As_Input_String()
		{
			// Prepare
			MyControl myControl = new MyControl();
			string inputString = "My Test Line Should Be Added";

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "AddStringToHistoryCollection", myControl, new object[] { inputString });

			// Assert
			int count = myControl.HistoryCollection.Count;
			ClipboardDataItem item = myControl.HistoryCollection[0];
			Assert.AreEqual(inputString, item.CopyDataFull);
		}
	}
	#endregion


	#region AddClipboardDataToHistoryCollection Method
	[TestFixture]
	public class MyControl_AddClipboardDataToHistoryCollection_Tests
	{
		[Test, STAThread]
		public void When_Calling_Method_HistoryCollection_Should_Contain_1_More_Line()
		{
			// Prepare
			MyControl myControl = new MyControl();
			int linesInitial = myControl.HistoryCollection.Count;

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "AddClipboardDataToHistoryCollection", myControl, null);

			// Assert
			Assert.AreEqual(linesInitial + 1, myControl.HistoryCollection.Count);
		}

		[Test, STAThread]
		public void When_Calling_Method_HistoryCollection_Last_Line_CopyDataFull_Property_Should_Be_Same_As_Clipboard_Content()
		{
			// Prepare
			MyControl myControl = new MyControl();
			string clipboardContent = Clipboard.GetText();

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "AddClipboardDataToHistoryCollection", myControl, null);

			// Assert
			int count = myControl.HistoryCollection.Count;
			ClipboardDataItem item = myControl.HistoryCollection[count - 1];
			Assert.AreEqual(clipboardContent, item.CopyDataFull);
		}
	}
	#endregion


	#region CopyHistoryCollectionLineToClipboard Method
	[TestFixture]
	public class MyControl_CopyHistoryCollectionLineToClipboard_Tests
	{
		[Test, STAThread]
		public void When_Given_Negative_Index_Clipboard_Should_Not_Change()
		{
			// Prepare
			MyControl myControl = new MyControl();
			int inputIndex = -5;
			string expectedClipboardContent = Clipboard.GetText();

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "CopyHistoryCollectionLineToClipboard", myControl, new object[] { inputIndex });

			// Assert
			Assert.AreEqual(expectedClipboardContent, Clipboard.GetText());
		}

		[Test, STAThread]
		public void When_Given_Overflow_Index_Clipboard_Should_Not_Change()
		{
			// Prepare
			MyControl myControl = new MyControl();
			string expectedClipboardContent = Clipboard.GetText();
			myControl.HistoryCollection.Clear();
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 1"));
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 2"));
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 3"));
			int inputIndex = myControl.HistoryCollection.Count;

			// Act
			UnitTestHelper.RunInstanceMethod(typeof(MyControl), "CopyHistoryCollectionLineToClipboard", myControl, new object[] { inputIndex });

			// Assert
			Assert.AreEqual(expectedClipboardContent, Clipboard.GetText());
		}

		[Test, STAThread]
		public void When_Given_Valid_Index_Clipboard_Should_Change()
		{
			// Prepare
			MyControl myControl = new MyControl();
			myControl.HistoryCollection.Clear();
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 1"));
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 2"));
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 3"));
			try {
				UnitTestHelper.RunInstanceMethod(typeof(MyControl), "CopyHistoryCollectionLineToClipboard", myControl, new object[] { 0 });
				string expectedClipboardContent = Clipboard.GetText();
				int inputIndex = 1;

				// Act
				UnitTestHelper.RunInstanceMethod(typeof(MyControl), "CopyHistoryCollectionLineToClipboard", myControl, new object[] { inputIndex });

				// Assert
				Assert.AreNotEqual(expectedClipboardContent, Clipboard.GetText());
			}
			catch (COMException)
			{
				Assert.Fail("Please Rerun this test, The COMExcepton is caused by another program hooking the Clipboard.");
			}
		}

		[Test, STAThread]
		public void When_Given_Valid_Index_Clipboard_Should_Be_Same_As_HistoryCollection_Line_CopyDataFull_Property()
		{
			// Prepare
			MyControl myControl = new MyControl();
			myControl.HistoryCollection.Clear();
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 1"));
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 2"));
			myControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 3"));
			try
			{
				int inputIndex = 0;
				ClipboardDataItem expectedClipboardDataItem = myControl.HistoryCollection[inputIndex];
				UnitTestHelper.RunInstanceMethod(typeof(MyControl), "CopyHistoryCollectionLineToClipboard", myControl, new object[] { 0 });

				// Act
				UnitTestHelper.RunInstanceMethod(typeof(MyControl), "CopyHistoryCollectionLineToClipboard", myControl, new object[] { inputIndex });

				// Assert
				Assert.AreEqual(expectedClipboardDataItem.CopyDataFull, Clipboard.GetText());
			}
			catch (COMException)
			{
				Assert.Fail("Please Rerun this test, The COMExcepton is caused by another program hooking the Clipboard.");
			}
		}
	}
	#endregion
}
