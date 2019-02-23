using System;
using MathNet.Numerics.Statistics;

namespace EvoBio4.Extensions
{
	public static class RunningStatisticsExtensions
	{
		public static ConfidenceInterval ConfidenceInterval ( this RunningStatistics statistics,
		                                                      double z )
		{
			var sd = statistics.StandardDeviation;
			var mean = statistics.Mean;
			var error = sd / Math.Sqrt ( statistics.Count );

			return new ConfidenceInterval ( mean, z * error );
		}
	}
}