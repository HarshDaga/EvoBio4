using System.IO;
using System.Linq;
using EvoBio4.Collections;
using EvoBio4.Core;
using EvoBio4.Core.Interfaces;

namespace EvoBio4
{
	public class Simulation<TIteration, TDeathSelectionRule> :
		SimulationBase<Individual, TIteration, Population,
			HeritabilitySummary, Variables, TDeathSelectionRule>
		where TIteration : SingleIterationBase<Individual, Population, Variables>, new ( )
		where TDeathSelectionRule : IDeathSelectionRule<Individual, Variables, Population>, new ( )
	{
		public Simulation ( Variables v ) : base ( v )
		{
		}

		public override void PrintConfidenceIntervals ( string fileName )
		{
			ConfidenceIntervalStats.Compute ( );
			ConfidenceIntervalStats.PrintToCsv ( "ConfidenceIntervals.csv" );

			var properties = new[]
			{
				"Quality",
				"Variance Quality",
				"Covariance Quality",
				"Reproduction",
				"Variance Reproduction",
				"Covariance Reproduction"
			};

			var ci = "\n\n\nHeritability Confidence Intervals:\n\n";

			for ( var i = 0; i < properties.Length; i++ )
			{
				var property = properties[i];
				var interval = Utility.CalculateConfidenceInterval (
					HeritabilitySummaries.Select ( x => x.Values[i] ).ToList ( ),
					V.Z );
				ci += $"{property,-33}: {interval}\n";
			}

			var lines = ci.Split ( '\n' ).Select ( x => x.TrimEnd ( ) );
			File.WriteAllLines ( fileName, lines );
		}
	}
}