using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;

namespace Segmentspace
{
	public struct ParametersOfExperiment
	{
		public static long NoMoreIterationsThan = 999999L;
		public static long NoLargerValueThan = 999999999999L;
		public static int MaxSegmentSize = 20000;
		public static int MaxNumberOfConsecutiveSplits = 3;
	}

	class GenerateSegments
	{
		// Holds data until flashed to the database.
		static List<Segment> AllSegments;

		/// <summary>
		/// Just for GenerateSegments
		/// </summary>
		private static Random R = new Random(57);

		/// <summary>
		/// Generate in memory.
		/// </summary>
		/// <returns></returns>
		static List<Segment> GenerateList()
		{
			/// The whole bunch, or the next bunch, of segments.
			AllSegments = new List<Segment>();
			/// Start with this.
			/// (Todo: Perhaps read from the database.)
			long nextAvailableValue = 1L;

			for (int iteration = 0; iteration < ParametersOfExperiment.NoMoreIterationsThan; iteration++) {
				// Create the next segment.
				Segment s = new Segment().CreateNewSegmentOfRandomSize(nextAvailableValue,
					ParametersOfExperiment.MaxSegmentSize, ParametersOfExperiment.NoLargerValueThan);
				AllSegments.Add(s);

				// Split zero or more segments.
				int numberOfSplits = R.Next(0, ParametersOfExperiment.MaxNumberOfConsecutiveSplits);
				for (int split = 0; split < numberOfSplits; split++) {
					// Choose a random segment, split it, and update the list of segments.
					int xSegmentToSplit = R.Next(1, AllSegments.Count);
					AllSegments.SplitSegment(xSegmentToSplit);
				}

				// Stop if reached the max value.
				nextAvailableValue += s.Size;
				if (nextAvailableValue > ParametersOfExperiment.NoLargerValueThan) { break; }

			}

			return AllSegments;
		}

		/// <summary>
		/// Store the segments in the database.
		/// </summary>
		/// <param name="listOfSegments"></param>
		static void FlushTheList(IList<Segment> listOfSegments)
		{
			// TODO HERE
		}

		static void Main(string[] args)
		{
			var ListOfSegments = GenerateList();
			FlushTheList(ListOfSegments);
		}
	}
}
