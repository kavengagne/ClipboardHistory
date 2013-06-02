using ClipboardHistory.Classes;
using NUnit.Framework;
using UnitTestHelperBase;

namespace ClipboardHistoryTests.Classes
{
	#region GetStringLines Method
	[TestFixture]
	public class ClipboardDataItem_GetStringLines_Tests
	{
		[Test]
		public void When_Given_Empty_Input_String_Should_Return_Array_Of_1_Line()
		{
			// Prepare
			int numberOfLines = 1;
			string inputString = string.Empty;

			// Act
			string[] inputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { inputString });

			// Assert
			Assert.AreEqual(numberOfLines, inputLines.Length);
		}

		[Test]
		public void When_Given_Single_Line_Input_String_Should_Return_Array_Of_1_Line()
		{
			// Prepare
			int numberOfLines = 1;
			string inputString = "Input String Line1";

			// Act
			string[] inputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { inputString });

			// Assert
			Assert.AreEqual(numberOfLines, inputLines.Length);
		}

		[Test]
		public void When_Given_2_Lines_Input_String_With_CRLF_Should_Return_Array_Of_2_Lines()
		{
			// Prepare
			int numberOfLines = 2;
			string inputString = string.Format("Input String Line 1 {0}Input String Line 2", System.Environment.NewLine);

			// Act
			string[] inputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { inputString });

			// Assert
			Assert.AreEqual(numberOfLines, inputLines.Length);
		}

		[Test]
		public void When_Given_2_Lines_Input_String_With_CR_Should_Return_Array_Of_2_Lines()
		{
			// Prepare
			int numberOfLines = 2;
			string inputString = string.Format("Input String Line 1 {0}Input String Line 2", System.Environment.NewLine);

			// Act
			string[] inputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { inputString });

			// Assert
			Assert.AreEqual(numberOfLines, inputLines.Length);
		}

		[Test]
		public void When_Given_2_Lines_Input_String_With_LF_Should_Return_Array_Of_2_Lines()
		{
			// Prepare
			int numberOfLines = 2;
			string inputString = string.Format("Input String Line 1 {0}Input String Line 2", System.Environment.NewLine);

			// Act
			string[] inputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { inputString });

			// Assert
			Assert.AreEqual(numberOfLines, inputLines.Length);
		}

		[Test]
		public void When_Given_11_Lines_Input_String_With_Mixed_CRLF_Should_Return_Array_Of_11_Lines()
		{
			// Prepare
			int numberOfLines = 11;
			string inputString = string.Format(
				"Input String Line 1 {0}Input String Line 2{1}Input String Line 3{4}Input String Line 4{3}" +
				"Input String Line 5{4}Input String Line 6{5}Input String Line 7{6}Input String Line 8{7}" +
				"Input String Line 9{8}Input String Line 10{9}Input String Line 11",
				"\r", "\r", "\n", "\r", "\r\n", "\n", "\n", "\r\n", "\r\n", "\r");

			// Act
			string[] inputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { inputString });

			// Assert
			Assert.AreEqual(numberOfLines, inputLines.Length);
		}
	}
	#endregion


	#region GetCopyDataLinesByNumber Method
	[TestFixture]
	public class ClipboardDataItem_GetStringStrippedToNumberOfLines_Tests
	{
		[Test]
		public void When_Given_Empty_Input_String_With_NumLines_Of_4_Should_Return_Input_String()
		{
			// Prepare
			int numberOfLines = 4;
			string inputString = string.Empty;

			// Act
			string outputString = (string)UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringStrippedToNumberOfLines", new object[2] { inputString, numberOfLines });

			// Assert
			Assert.AreEqual(inputString, outputString);
		}

		[Test]
		public void When_Given_2_Lines_Input_String_With_NumLines_Of_4_Should_Return_Input_String()
		{
			// Prepare
			int numberOfLines = 4;
			string inputString = string.Format("Input String Line 1 {0}Input String Line 2", System.Environment.NewLine);

			// Act
			string outputString = (string)UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringStrippedToNumberOfLines", new object[2] { inputString, numberOfLines });

			// Assert
			Assert.AreEqual(inputString, outputString);
		}

		[Test(Description = "The additional line is used to indicate the number of lines in CopyDataFull")]
		public void When_Given_6_Lines_Input_String_With_NumLines_Of_4_Should_Return_String_With_5_Lines()
		{
			// Prepare
			int numberOfLines = 4;
			string inputString = string.Format(
				"Input String Line 1 {0}Input String Line 2 {0}Input String Line 3 {0}" +
				"Input String Line 4 {0}Input String Line 5 {0}Input String Line 6",
				System.Environment.NewLine);

			// Act
			string outputString = (string)UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringStrippedToNumberOfLines", new object[2] { inputString, numberOfLines });

			// Assert
			string[] lines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { outputString });
			Assert.AreEqual(numberOfLines + 1, lines.Length);
		}

		[Test]
		public void When_Given_6_Lines_Input_String_With_NumLines_Of_4_Line_Number_5_Should_Indicate_Number_Of_Lines_Total()
		{
			// Prepare
			int numberOfLines = 4;
			string inputString = string.Format(
				"Input String Line 1 {0}Input String Line 2 {0}Input String Line 3 {0}" +
				"Input String Line 4 {0}Input String Line 5 {0}Input String Line 6",
				System.Environment.NewLine);

			// Act
			string outputString = (string)UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringStrippedToNumberOfLines", new object[2] { inputString, numberOfLines });

			// Assert
			string[] inputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { inputString });
			string[] outputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { outputString });
			string lastLine = outputLines[outputLines.Length - 1];
			Assert.IsTrue(lastLine.Contains(string.Format("{0} lines", inputLines.Length)));
		}

		[Test]
		public void When_Given_6_Lines_Input_String_With_NumLines_Of_0_Or_Less_Should_Return_Empty_String()
		{
			// Prepare
			int numberOfLines = 0;
			string inputString = string.Format(
				"Input String Line 1 {0}Input String Line 2 {0}Input String Line 3 {0}" +
				"Input String Line 4 {0}Input String Line 5 {0}Input String Line 6",
				System.Environment.NewLine);

			// Act
			string outputString = (string)UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringStrippedToNumberOfLines", new object[2] { inputString, numberOfLines });

			// Assert
			Assert.AreEqual(string.Empty, outputString);
		}
	} 
	#endregion


	#region Constructors
	[TestFixture]
	public class ClipboardDataItem_Constructors_Tests
	{
		[Test]
		public void When_Creating_Object_With_Empty_String_CopyDataFull_Property_Should_Be_Same_As_Input_String()
		{
			// Prepare
			string inputString = "";

			// Act
			ClipboardDataItem clipboardDataItem = new ClipboardDataItem(inputString);

			// Assert
			Assert.AreEqual(inputString, clipboardDataItem.CopyDataFull);
		}

		[Test]
		public void When_Creating_Object_With_String_CopyDataFull_Property_Should_Be_Same_As_Input_String()
		{
			// Prepare
			string inputString = "Input String Line1 \n-Maybe Line2 \r-Maybe Line 3 \r\n-Maybe Line 4";

			// Act
			ClipboardDataItem clipboardDataItem = new ClipboardDataItem(inputString);

			// Assert
			Assert.AreEqual(inputString, clipboardDataItem.CopyDataFull);
		}

		[Test]
		public void When_Creating_Object_With_String_CopyDataShort_Property_Should_Be_Not_Empty_Or_Null()
		{
			// Prepare
			string inputString = "Input String Line1 \n-Maybe Line2 \r-Maybe Line 3 \r\n-Maybe Line 4";

			// Act
			ClipboardDataItem clipboardDataItem = new ClipboardDataItem(inputString);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(clipboardDataItem.CopyDataShort));
		}

		[Test]
		public void When_Creating_Object_With_String_CopyDataShort_Property_Should_Be_At_Least_One_Line_Of_Input_String()
		{
			// Prepare
			string inputString = "Input String Line1 \n-Maybe Line2 \r-Maybe Line 3 \r\n-Maybe Line 4";
			
			// Act
			ClipboardDataItem clipboardDataItem = new ClipboardDataItem(inputString);

			// Assert
			string[] inputLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { inputString });
			string[] shortLines = (string[])UnitTestHelper.RunStaticMethod(typeof(ClipboardDataItem), "GetStringLines", new object[1] { clipboardDataItem.CopyDataShort });
			Assert.AreEqual(inputLines[0], shortLines[0]);
		}

		[Test]
		public void When_Creating_Object_With_String_Only_IsErrorMessage_Property_Should_Be_False()
		{
			// Prepare
			string inputString = "Input String Line1 \n-Maybe Line2 \r-Maybe Line 3 \r\n-Maybe Line 4";

			// Act
			ClipboardDataItem clipboardDataItem = new ClipboardDataItem(inputString);

			// Assert
			Assert.IsFalse(clipboardDataItem.IsErrorMessage);
		}

		[Test]
		public void When_Creating_Object_With_String_And_Boolean_True_IsErrorMessage_Property_Should_Be_True()
		{
			// Prepare
			string inputString = "Input String Line1 \n-Maybe Line2 \r-Maybe Line 3 \r\n-Maybe Line 4";

			// Act
			ClipboardDataItem clipboardDataItem = new ClipboardDataItem(inputString, true);

			// Assert
			Assert.IsTrue(clipboardDataItem.IsErrorMessage);
		}
	}
	#endregion


	#region Properties
	[TestFixture]
	public class ClipboardDataItem_Properties_Tests
	{
		[Test]
		public void When_Changing_CopyDataFull_Property_CopyDataShort_Should_Change()
		{
			// Prepare
			string inputString1 = "Input1 String Line1 \n-Maybe1 Line2 \r-Maybe1 Line 3 \r\n-Maybe1 Line 4";
			string inputString2 = "Input2 String Line1 \n-Maybe2 Line2 \r-Maybe2 Line 3 \r\n-Maybe2 Line 4";
			ClipboardDataItem clipboardDataItem = new ClipboardDataItem(inputString1);

			// Act
			clipboardDataItem.CopyDataFull = inputString2;

			// Assert
			Assert.AreEqual(inputString2, clipboardDataItem.CopyDataFull);
			Assert.AreEqual(inputString2, clipboardDataItem.CopyDataShort);
		}
	}
	#endregion
}
