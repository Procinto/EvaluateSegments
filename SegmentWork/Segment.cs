using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Segmentspace
{
	public struct Segment
	{
		#region Data members

		public long Left { get; set; }
		public long Right { get; set; }
		public bool IsValid { get; private set; }

		public long Size
		{
			get
			{
				if (IsValid) { return Right - Left + 1; } else { return 0; }
			}
		}

		#endregion

		#region Misc

		private static Random R = new Random(91);

		private Segment Invalidate()
		{
			this.IsValid = false;
			return this;
		}

		#endregion

		#region Create

		// The new() on a struct fills the data with "zeros",
		// so IsValid is initially false, as it should be.

		public Segment CreateNewSegmentOfRandomSize(long newLeft, int maxSize, long maxValue)
		{
			if (newLeft > maxValue) { return this.Invalidate(); }

			int actualSize = R.Next(1, maxSize);
			if (newLeft >= maxValue - actualSize) { return this.Invalidate(); }

			this.Left = newLeft;
			this.Right = newLeft - 1 + actualSize;
			this.IsValid = true;

			return this;
		}

		public Segment CreateWithEndPoints(long left, long right)
		{
			if (right < left) { return this.Invalidate(); }

			this.Left = left;
			this.Right = right;
			this.IsValid = true;

			return this;
		}

		#endregion

	}

	/// <summary>
	/// Extensions for List&lt;Segment&gt; and such.
	/// </summary>
	public static class ExtendListOfSegments
	{
		static Random R = new Random(17);

		/// <summary>
		/// The segment is split into two.
		/// Left part is corrected (the split point is the new right end);
		/// right part is appended (the split point is the new left end).
		/// Null on an error!
		/// </summary>
		public static List<Segment> SplitSegment(this List<Segment> list, int indexOfSegmentToSplit)
		{
			if (null == list || indexOfSegmentToSplit >= list.Count) { return null; }

			Segment s = list[indexOfSegmentToSplit];
			if (s.Size == 1) { return list; } // cannot split

			int splitAt = R.Next(0, (int) (s.Right - s.Left)); // size is expected to fit in int

			// L..R is split as S. This means:
			// 1) if S < R, let T = S+1. Then:
			//    one segment is L..S
			//    another is T..R
			// 2) Otherwise, S=R. Let U = S-1 = R-1. Then:
			//    one segment is L..U
			//    another is R..R
			Segment sAnother = new Segment();

			long pointL = s.Left;
			long pointR = s.Right;

			long pointS, pointT, pointU;
			pointS = pointL + splitAt;

			if (splitAt < s.Size) {
				pointT = pointS + 1L;
				sAnother.CreateWithEndPoints(pointT, pointR);
				s.CreateWithEndPoints(pointL, pointS);
			} else {
				pointU = pointR - 1;
				sAnother.CreateWithEndPoints(pointR, pointR);
				s.CreateWithEndPoints(pointL, pointU);
			}

			// Replace s; add sAnother.
			list[indexOfSegmentToSplit] = s;
			list.Add(sAnother);

			return list;
		}
	}

}
