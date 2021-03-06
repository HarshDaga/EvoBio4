﻿using System;
using System.Diagnostics;
using EvoBio4.Implementations;
using EvoBio4.Strategies;
using static EvoBio4.Strategies.StrategyFactory;

namespace EvoBio4
{
	public class Program
	{
		public static void Main ( )
		{
			Console.Title = "Evo Bio 4";

			var v = new Variables
			{
				CooperatorQuantity         = 10,
				DefectorQuantity           = 10,
				SdQuality                  = 1,
				Y                          = .8,
				Relatedness                = .15,
				PercentileCutoff           = 10,
				Z                          = 1.96,
				MaxTimeSteps               = 5000,
				Runs                       = 10000,
				IncludeConfidenceIntervals = true
			};

			var strategyCollection = new StrategyCollection
			{
				Survival     = Survival.QualityProportional,
				Fitness      = Fitness.NonReproducingHave0Fitness,
				Reproduction = Reproduction.FitnessProportional,
				PostProcess  = PostProcess.DoNothing
			};

			Simulate<Iteration> ( v, strategyCollection );
		}

		public static void Simulate<TVersion> ( Variables v,
		                                        IStrategyCollection strategyCollection )
			where TVersion : Iteration, new ( )
		{
			var timer = Stopwatch.StartNew ( );

			var simulation = new Simulation<TVersion> ( v, strategyCollection );

			simulation.LogRun ( 5, 2 );

			simulation.Run ( );

			Console.WriteLine ( simulation );

			Console.WriteLine ( timer.Elapsed );

			Console.ReadLine ( );
		}
	}
}