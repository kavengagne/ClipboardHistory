using System;
using ClipboardHistoryApp.Classes;
using ClipboardHistoryApp.Controls;
using NUnit.Framework;
using System.Reflection;
using System.Windows;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
namespace ClipboardHistoryTests
{
    #region Properties
    [TestFixture]
    public class HistoryListControl_Properties_Tests
    {
        [Test, STAThread]
        public void When_Creating_Object_ClipboardDataItemCollection_Property_Should_Be_Initialized()
        {
            // Prepare
            var historyListControl = new HistoryListControl();

            // Act
            Object item = historyListControl.HistoryCollection;

            // Assert
            Assert.IsNotNull(item);
        }

        [Test, STAThread]
        public void When_Creating_Object_DataContext_Property_Should_Be_Not_Null()
        {
            // Prepare
            var historyListControl = new HistoryListControl();

            // Act
            Object item = historyListControl.DataContext;

            // Assert
            Assert.IsNotNull(item);
        }
    }
    #endregion


    #region Fields
    public class HistoryListControl_Fields_Tests
    {
        [Test, STAThread]
        public void When_Creating_Object_ListBox_Field_Should_Be_Initialized()
        {
            // Prepare
            var historyListControl = new HistoryListControl();

            // Act
            FieldInfo info = typeof(HistoryListControl).GetField("ListBoxHistory", BindingFlags.Instance | BindingFlags.NonPublic);

            // Assert
            Assert.IsNotNull(info, "Field does not exist.");
            Assert.IsNotNull(info.GetValue(historyListControl), "Field is not initialized");
        }

        [Test, STAThread]
        public void When_Creating_Object_ClipboardUpdateNotifier_Field_Should_Be_Initialized()
        {
            // Prepare
            var historyListControl = new HistoryListControl();

            // Act
            FieldInfo info = typeof(HistoryListControl).GetField("_clipboardUpdateNotifier", BindingFlags.Instance | BindingFlags.NonPublic);

            // Assert
            Assert.IsNotNull(info, "Field does not exist.");
            Assert.IsNotNull(info.GetValue(historyListControl), "Field is not initialized");
        }

        [Test, STAThread]
        public void When_Creating_Object_ClipboardDataItemCollection_Field_Should_Be_Initialized()
        {
            // Prepare
            var historyListControl = new HistoryListControl();

            // Act
            FieldInfo info = typeof(HistoryListControl).GetField("_historyCollection", BindingFlags.Instance | BindingFlags.NonPublic);

            // Assert
            Assert.IsNotNull(info, "Field does not exist.");
            Assert.IsNotNull(info.GetValue(historyListControl), "Field is not initialized");
        }
    }
    #endregion


    #region AddStringToHistoryCollection Method
    [TestFixture]
    public class HistoryListControl_AddStringToHistoryCollection_Tests
    {
        [Test, STAThread]
        public void When_Given_Empty_String_HistoryCollection_Should_Not_Contain_Anymore_Lines()
        {
            // Prepare
            var historyListControl = new HistoryListControl();
            const string inputString = "";
            int expectedNumberOfLines = historyListControl.HistoryCollection.Count;

            // Act
            UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "AddStringToHistoryCollection", historyListControl, new object[] { inputString });

            // Assert
            Assert.AreEqual(expectedNumberOfLines, historyListControl.HistoryCollection.Count);
        }

        [Test, STAThread]
        public void When_Given_Valid_String_HistoryCollection_Should_Contain_1_More_Line()
        {
            // Prepare
            var historyListControl = new HistoryListControl();
            const string inputString = "My Test Line Should Be Added";
            int expectedNumberOfLines = historyListControl.HistoryCollection.Count + 1;

            // Act
            UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "AddStringToHistoryCollection", historyListControl, new object[] { inputString });

            // Assert
            Assert.AreEqual(expectedNumberOfLines, historyListControl.HistoryCollection.Count);
        }

        [Test, STAThread]
        public void When_Given_Valid_String_HistoryCollection_First_Line_CopyDataFull_Property_Should_Be_Same_As_Input_String()
        {
            // Prepare
            var historyListControl = new HistoryListControl();
            const string inputString = "My Test Line Should Be Added";

            // Act
            UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "AddStringToHistoryCollection", historyListControl, new object[] { inputString });

            // Assert
            ClipboardDataItem item = historyListControl.HistoryCollection[0];
            Assert.AreEqual(inputString, item.Data);
        }
    }
    #endregion


    #region AddClipboardDataToHistoryCollection Method
    [TestFixture]
    public class HistoryListControl_AddClipboardDataToHistoryCollection_Tests
    {
        [Test, STAThread]
        public void When_Calling_Method_HistoryCollection_Should_Contain_1_More_Line()
        {
            // Prepare
            var historyListControl = new HistoryListControl();
            int linesInitial = historyListControl.HistoryCollection.Count;

            // Act
            UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "AddClipboardDataToHistoryCollection", historyListControl, null);

            // Assert
            Assert.AreEqual(linesInitial + 1, historyListControl.HistoryCollection.Count);
        }

        [Test, STAThread]
        public void When_Calling_Method_HistoryCollection_Last_Line_CopyDataFull_Property_Should_Be_Same_As_Clipboard_Content()
        {
            // Prepare
            var historyListControl = new HistoryListControl();
            string clipboardContent = Clipboard.GetText();

            // Act
            UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "AddClipboardDataToHistoryCollection", historyListControl, null);

            // Assert
            int count = historyListControl.HistoryCollection.Count;
            ClipboardDataItem item = historyListControl.HistoryCollection[count - 1];
            Assert.AreEqual(clipboardContent, item.Data);
        }
    }
    #endregion


    #region CopyHistoryCollectionLineToClipboard Method
    [TestFixture]
    public class HistoryListControl_CopyHistoryCollectionLineToClipboard_Tests
    {
        [Test, STAThread]
        public void When_Given_Negative_Index_Clipboard_Should_Not_Change()
        {
            // Prepare
            var historyListControl = new HistoryListControl();
            const int inputIndex = -5;
            string expectedClipboardContent = Clipboard.GetText();

            // Act
            UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "CopyHistoryCollectionLineToClipboard", historyListControl, new object[] { inputIndex });

            // Assert
            Assert.AreEqual(expectedClipboardContent, Clipboard.GetText());
        }

        [Test, STAThread]
        public void When_Given_Overflow_Index_Clipboard_Should_Not_Change()
        {
            // Prepare
            var historyListControl = new HistoryListControl();
            string expectedClipboardContent = Clipboard.GetText();
            historyListControl.HistoryCollection.Clear();
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 1"));
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 2"));
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 3"));
            int inputIndex = historyListControl.HistoryCollection.Count;

            // Act
            UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "CopyHistoryCollectionLineToClipboard", historyListControl, new object[] { inputIndex });

            // Assert
            Assert.AreEqual(expectedClipboardContent, Clipboard.GetText());
        }

        [Test, STAThread]
        public void When_Given_Valid_Index_Clipboard_Should_Change()
        {
            // Prepare
            var historyListControl = new HistoryListControl();
            historyListControl.HistoryCollection.Clear();
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 1"));
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 2"));
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 3"));
            try {
                UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "CopyHistoryCollectionLineToClipboard", historyListControl, new object[] { 0 });
                string expectedClipboardContent = Clipboard.GetText();
                const int inputIndex = 1;

                // Act
                UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "CopyHistoryCollectionLineToClipboard", historyListControl, new object[] { inputIndex });

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
            var historyListControl = new HistoryListControl();
            historyListControl.HistoryCollection.Clear();
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 1"));
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 2"));
            historyListControl.HistoryCollection.Insert(0, new ClipboardDataItem("line 3"));
            try
            {
                const int inputIndex = 0;
                ClipboardDataItem expectedClipboardDataItem = historyListControl.HistoryCollection[inputIndex];
                UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "CopyHistoryCollectionLineToClipboard", historyListControl, new object[] { 0 });

                // Act
                UnitTestHelper.RunInstanceMethod(typeof(HistoryListControl), "CopyHistoryCollectionLineToClipboard", historyListControl, new object[] { inputIndex });

                // Assert
                Assert.AreEqual(expectedClipboardDataItem.Data, Clipboard.GetText());
            }
            catch (COMException)
            {
                Assert.Fail("Please Rerun this test, The COMExcepton is caused by another program hooking the Clipboard.");
            }
        }
    }
    #endregion
}
