using System.Collections.Generic;
using EvoBio4.Core.Enums;
using MathNet.Numerics.Statistics;

namespace EvoBio4.Core.Interfaces
{
	public interface IConfidenceIntervalStats
	{
		int TimeSteps { get; }
		int Runs { get; }
		Dictionary<IndividualType, List<RunningStatistics>> RunningStats { get; }

		List<Dictionary<IndividualType, ConfidenceInterval>> Summary { get; }
		double Z { get; }

		ConfidenceInterval this [ int timeStep,
		                          IndividualType type ] { get; }

		void AddRun ( IDictionary<IndividualType, List<int>> runResult );

		void Compute ( );

		void PrintToCsv ( string fileName );
	}
}