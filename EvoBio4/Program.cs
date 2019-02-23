using System;
using System.Diagnostics;
using EvoBio4.Core.Abstractions;
using EvoBio4.Core.Interfaces;
using EvoBio4.DeathSelectionRules;
using EvoBio4.Implementations;
using EvoBio4.Versions;

namespace EvoBio4
{
	public class Program
	{
		public static void Main ( )
		{
			Console.Title = "Evo Bio 4";

			var v = new Variables
			{
				CooperatorQuantity         = 100,
				DefectorQuantity           = 100,
				SdQuality                  = 1,
				Y                          = .8,
				Relatedness                = .15,
				PercentileCutoff           = 10,
				Z                          = 1.96,
				MaxTimeSteps               = 250000,
				Runs                       = 10000,
				IncludeConfidenceIntervals = false
			};

			Simulate<
				NonReproducingHave0FitnessVersion,
				FitnessProportionalPerishStrategy
			> ( v );
		}

		public static void Simulate<TVersion, TDeathSelectionRule> ( Variables v )
			where TVersion : SingleIterationBase<Individual, Population, Variables>, new ( )
			where TDeathSelectionRule : IPerishStrategy<Individual, Variables, Population>, new ( )
		{
			var timer = Stopwatch.StartNew ( );

			var simulation = new Simulation<
				TVersion,
				TDeathSelectionRule
			> ( v );

			simulation.Run ( );

			Console.WriteLine ( simulation );

			Console.WriteLine ( timer.Elapsed );

			Console.ReadLine ( );
		}
	}
}