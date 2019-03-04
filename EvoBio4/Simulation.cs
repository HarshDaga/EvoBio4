using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using EvoBio4.Enums;
using EvoBio4.Implementations;
using EvoBio4.Strategies;
using MathNet.Numerics.Statistics;
using ShellProgressBar;

namespace EvoBio4
{
	public class Simulation<TIteration>
		where TIteration : Iteration, new ( )
	{
		public const int NumberWidth = 8;

		public Dictionary<Winner, int> Wins { get; }
		public HeritabilitySummary HeritabilityMean { get; }
		public HeritabilitySummary HeritabilitySd { get; }
		public List<HeritabilitySummary> HeritabilitySummaries { get; }
		public ConfidenceIntervalStats ConfidenceIntervalStats { get; protected set; }

		public Dictionary<int, int> TimeStepsCount { get; }

		public Variables V { get; }

		public IStrategyCollection StrategyCollection { get; }

		public readonly int DegreeOfParallelism = Environment.ProcessorCount * 2;
		protected readonly object SyncLock = new object ( );

		public Simulation ( Variables v,
		                    IStrategyCollection strategyCollection )
		{
			V                  = v;
			StrategyCollection = strategyCollection;

			Wins = new Dictionary<Winner, int>
			{
				[Winner.Cooperator]              = 0,
				[Winner.Defector]                = 0,
				[Winner.Cooperator | Winner.Fix] = 0,
				[Winner.Defector | Winner.Fix]   = 0,
				[Winner.Tie]                     = 0
			};
			TimeStepsCount = Enumerable
				.Range ( 0, V.MaxTimeSteps + 1 )
				.ToDictionary ( x => x, x => 0 );

			HeritabilitySummaries = new List<HeritabilitySummary> ( V.Runs );
			HeritabilityMean      = new HeritabilitySummary ( );
			HeritabilitySd        = new HeritabilitySummary ( );

			if ( V.IncludeConfidenceIntervals )
				ConfidenceIntervalStats = new ConfidenceIntervalStats ( V.MaxTimeSteps, V.Runs, V.Z );
		}

		public void LogRun ( int logTimeSteps,
		                     int logRuns )
		{
			var timeSteps = V.MaxTimeSteps;
			var runs = V.Runs;
			V.MaxTimeSteps = logTimeSteps;
			V.Runs         = logRuns;

			var iteration = new TIteration ( );
			iteration.Init ( V, StrategyCollection, true );
			iteration.Run ( );

			V.MaxTimeSteps = timeSteps;
			V.Runs         = runs;
		}

		public virtual void Run ( )
		{
			var options = ProgressBarOptions.Default;
			options.EnableTaskBarProgress = true;

			using ( var pbar = new ProgressBar ( V.Runs, "Simulating", options ) )
			{
				ParallelEnumerable
					.Range ( 0, V.Runs )
					.ForAll ( i =>
						{
							var iteration = new TIteration ( );
							iteration.Init ( V, StrategyCollection );
							iteration.Run ( );

							lock ( SyncLock )
							{
								++Wins[iteration.Winner];
								++TimeStepsCount[iteration.TimeStepsPassed];
								ConfidenceIntervalStats?.AddRun ( iteration.GenerationHistory );
							}

							if ( iteration.TimeStepsPassed > 2 )
								HeritabilitySummaries.Add ( iteration.Heritability );

							// ReSharper disable once AccessToDisposedClosure
							pbar.Tick ( );
						}
					);
			}

			for ( var i = 0; i < HeritabilityMean.ValueCount; i++ )
			{
				var index = i;
				( HeritabilityMean.Values[index], HeritabilitySd.Values[index] ) =
					HeritabilitySummaries.Select ( x => x.Values[index] ).MeanStandardDeviation ( );
			}

			PrintHeritabilitySummaries ( "Heritability.csv" );
			if ( V.IncludeConfidenceIntervals )
				PrintConfidenceIntervals ( "ConfidenceIntervals.txt" );
		}

		public void PrintHeritabilitySummaries ( string fileName )
		{
			using ( var csv = new CsvWriter ( File.CreateText ( fileName ) ) )
				csv.WriteRecords ( HeritabilitySummaries );
		}

		public void PrintConfidenceIntervals ( string fileName )
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

		public override string ToString ( )
		{
			var result = $"\n\n{V}\n\n";
			result += $"{typeof ( TIteration ).Name} with {StrategyCollection}\n\n";
			result += string.Join ( "\n", Wins.Select ( x => $"{x.Key,16} = {x.Value}" ) );
			result += $"\n\n\nHeritability Mean:\n{HeritabilityMean}\n";
			result += $"\nHeritability Standard Deviation:\n{HeritabilitySd}\n";

			result += BuildHistogram ( );

			return result;
		}

		private string BuildHistogram ( )
		{
			var max = TimeStepsCount.Values.Max ( );
			var result = "\nGenerations Count:\n";
			result += string.Join (
				"\n",
				TimeStepsCount
					.Where ( x => x.Value != 0 )
					.Select ( pair => $"{pair.Key,-3} {pair.Value,-5} " +
					                  $"{new string ( '█', pair.Value * 100 / max )}" ) );
			return result;
		}
	}
}