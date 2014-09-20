using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Segmentspace;

namespace TestSegmentWork
{
	[TestClass]
	public class TestSW
	{
		[TestMethod]
		public void TestSegment()
		{
			Segment s;

			s = new Segment();
			Assert.IsFalse(s.IsValid);			

			s.CreateWithEndPoints(3, 2);
			Assert.IsFalse(s.IsValid);
			Assert.IsFalse(s.Size > 0);

			s.CreateWithEndPoints(7, 7);
			Assert.IsTrue(s.IsValid);
			Assert.AreEqual(1, s.Size);

			int size = int.MaxValue; // heh
			s.CreateWithEndPoints(1, size);
			Assert.IsTrue(s.IsValid);
			Assert.AreEqual(size, s.Size);

			// CreateNextSegmentOfRandomSize
			// TODO

		}

		private List<Segment> GenerateListOfSegments()
		{
			List<Segment> someList = new List<Segment>();
			someList.Add(new Segment().CreateWithEndPoints(100, 199));
			someList.Add(new Segment().CreateWithEndPoints(1000, 1999));
			someList.Add(new Segment().CreateWithEndPoints(500, 500));
			return someList;
		}

		[TestMethod]
		public void TestListOfSegments()
		{
			List<Segment> theList = new List<Segment>();
			List<Segment> ret;

			ret = theList.SplitSegment(0);
			Assert.IsNull(ret);

			theList = GenerateListOfSegments();
			ret = theList.SplitSegment(theList.Count);
			Assert.IsNull(ret);
			Assert.IsNotNull(theList);

			int countBeforeSplit;
	
			countBeforeSplit = theList.Count;
			
			theList.SplitSegment(2);
			Assert.IsNotNull(theList);
			Assert.AreEqual(countBeforeSplit, theList.Count);

			long oldLeft, oldRight;
			oldLeft = theList[1].Left;
			oldRight = theList[1].Right;

			theList.SplitSegment(1);
			Assert.IsNotNull(theList);
			Assert.AreEqual(countBeforeSplit + 1, theList.Count);
			// Check the splitter segments end points.
			// Assume the left part replaced the split segment,
			// and the right part was appended to the list.
			long firstLeft, firstRight, secondLeft, secondRight;
			firstLeft = theList[1].Left;
			firstRight = theList[1].Right;
			secondLeft = theList[theList.Count - 1].Left;
			secondRight = theList[theList.Count - 1].Right;

			Assert.AreEqual(oldLeft, firstLeft);
			Assert.AreEqual(firstRight, secondLeft - 1);
			Assert.AreEqual(oldRight, secondRight);
		}
	}
}
