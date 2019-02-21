using System;
using System.Diagnostics;
using EvoBio4.Collections;
using EvoBio4.DeathSelectionRules;
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
				MaxTimeSteps               = 2500,
				Runs                       = 1000,
				IncludeConfidenceIntervals = true
			};

			var timer = Stopwatch.StartNew ( );

			var simulation = new Simulation<
				SingleIterationBaseVersion,
				QualityProportionalDeathSelectionRule
			> ( v );

			simulation.Run ( );

			Console.WriteLine ( simulation );

			Console.WriteLine ( timer.Elapsed );

			Console.ReadLine ( );
		}
	}
}