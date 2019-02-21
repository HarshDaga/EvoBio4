using System.Collections.Generic;
using EvoBio4.Core.Enums;

namespace EvoBio4.Core.Interfaces
{
	public interface ISimulation
	{
		Dictionary<int, int> TimeStepsCount { get; }
		Dictionary<Winner, int> Wins { get; }
		IConfidenceIntervalStats ConfidenceIntervalStats { get; }

		void Run ( );
	}
}