using System;
using System.Collections.Generic;
using System.Linq;
using EvoBio4.Core.Abstractions;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;
using MathNet.Numerics.Statistics;
using MoreLinq;
using ShellProgressBar;

namespace EvoBio4.Core
{
	public abstract class SimulationBase<TIndividual, TIteration, TPopulation,
	                                     THeritability, TVariables, TDeathSelectionRule> :
		ISimulation
		where TIndividual : class, IIndividual, new ( )
		where TVariables : IVariables, new ( )
		where TIteration : ISingleIteration<TIndividual, TVariables, TPopulation>, new ( )
		where THeritability : IHeritabilitySummary, new ( )
		where TDeathSelectionRule : IDeathSelectionRule<TIndividual, TVariables, TPopulation>, new ( )
		where TPopulation : IPopulation<TIndividual, TVariables>
	{
		public const int NumberWidth = 8;

		public Dictionary<Winner, int> Wins { get; }
		public THeritability HeritabilityMean { get; }
		public THeritability HeritabilitySd { get; }
		public List<IHeritabilitySummary> HeritabilitySummaries { get; }
		public IConfidenceIntervalStats ConfidenceIntervalStats { get; }

		public Dictionary<int, int> TimeStepsCount { get; }

		public readonly int DegreeOfParallelism = Environment.ProcessorCount * 2;
		protected readonly object SyncLock = new object ( );

		public readonly TVariables V;

		protected SimulationBase ( TVariables v )
		{
			var deathSelectionRule = new TDeathSelectionRule ( );
			V = v;
			Wins = new Dictionary<Winner, int>
			{
				[Winner.Cooperator]              = 0,
				[Winner.Defector]                = 0,
				[Winner.Cooperator | Winner.Fix] = 0,
				[Winner.Defector | Winner.Fix]   = 0,
				[Winner.Tie]                     = 0
			};
			TimeStepsCount = Enumerable
				.Range ( 1, V.MaxTimeSteps )
				.ToDictionary ( x => x, x => 0 );

			HeritabilitySummaries = new List<IHeritabilitySummary> ( V.Runs );
			HeritabilityMean      = new THeritability ( );
			HeritabilitySd        = new THeritability ( );

			if ( V.IncludeConfidenceIntervals )
				ConfidenceIntervalStats = new ConfidenceIntervalStats ( V.MaxTimeSteps, V.Runs, V.Z );

			var timeSteps = V.MaxTimeSteps;
			var runs = V.Runs;
			V.MaxTimeSteps = 5;
			V.Runs         = 2;

			var iteration = new TIteration ( );
			iteration.Init ( V, deathSelectionRule, true );
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
							var deathSelectionRule = new TDeathSelectionRule ( );
							var iteration = new TIteration ( );
							iteration.Init ( V, deathSelectionRule );
							iteration.Run ( );

							lock ( SyncLock )
							{
								++Wins[iteration.Winner];
								++TimeStepsCount[iteration.TimeStepsPassed];
								ConfidenceIntervalStats?.Add ( iteration.GenerationHistory );
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

			if ( V.IncludeConfidenceIntervals )
				PrintConfidenceIntervals ( "ConfidenceIntervals.txt" );
		}

		public abstract void PrintConfidenceIntervals ( string fileName );

		public override string ToString ( )
		{
			var result = $"\n\n{V}\n\n";
			result += string.Join ( "\n", Wins.Select ( x => $"{x.Key,16} = {x.Value}" ) );
			result += $"\n\n\nHeritability Mean:\n{HeritabilityMean}\n";
			result += $"\nHeritability Standard Deviation:\n{HeritabilitySd}\n";

			var max = TimeStepsCount.Values.Max ( );
			result += "\nTime Steps Count:\n";
			var count = TimeStepsCount.Values
				.CumulativeSum ( )
				.ToList ( )
				.TakeUntil ( x => x >= V.Runs )
				.Count ( );
			result += string.Join (
				"\n",
				TimeStepsCount
					.Take ( count )
					.Where ( x => x.Value != 0 )
					.Select ( pair => $"{pair.Key,-3} {pair.Value,-5} " +
					                  $"{new string ( '█', pair.Value * 100 / max )}" ) );

			return result;
		}
	}
}