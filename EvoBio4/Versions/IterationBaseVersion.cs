using System.Linq;
using EvoBio4.Core.Abstractions;
using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;
using EvoBio4.Implementations;
using MathNet.Numerics.Statistics;
using MoreLinq;

namespace EvoBio4.Versions
{
	public class IterationBaseVersion :
		IterationBase<Individual, Variables, Population, IterationBaseVersion>
	{
		public IterationBaseVersion ( )
		{
		}

		public IterationBaseVersion (
			Variables v,
			PerishStrategyDelegate<Individual, Variables, Population, IterationBaseVersion> perishStrategy,
			bool isLoggingEnabled ) :
			base ( v, perishStrategy, isLoggingEnabled )
		{
		}

		protected override Individual GetParent ( )
		{
			return CooperatorGroup
				.ReproducingIndividuals
				.Concat ( DefectorGroup )
				.ChooseOneBy ( x => x.Fitness,
				               TotalFitness - CooperatorGroup.NonReproducingIndividuals.Sum ( x => x.Fitness ) );
		}

		public override void CalculateHeritability ( )
		{
			if ( TimeStepsPassed <= 2 )
				return;

			var groups = History
				.GroupBy ( x => x.parent )
				.Where ( x => x.Any ( ) )
				.Select ( x => ( parent: x.Key, offsprings: x.Select ( y => y.offspring ).ToList ( ) ) )
				.ToList ( );

			var covQuality = groups.Select ( x => x.parent.Quality )
				.PopulationCovariance ( groups.Select ( x => x.offsprings.Average ( y => y.Quality ) ) );

			var varQuality = groups
				.Select ( x => x.parent.Quality )
				.PopulationVariance ( );

			groups = History
				.Take ( History.Count - 1 )
				.GroupBy ( x => x.parent )
				.Where ( x => x.Any ( ) )
				.Select ( x => ( parent: x.Key, offsprings: x.Select ( y => y.offspring ).ToList ( ) ) )
				.ToList ( );
			var covReproduction = groups.Select ( x => (double) x.parent.OffspringCount )
				.PopulationCovariance ( groups.Select ( x => x.offsprings.Average ( y => y.OffspringCount ) ) );
			var varReproduction = groups
				.Select ( x => x.parent.OffspringCount * 1d )
				.PopulationVariance ( );

			Heritability = new HeritabilitySummary
			{
				Quality                = covQuality / varQuality,
				VarianceQuality        = varQuality,
				CovarianceQuality      = covQuality,
				Reproduction           = covReproduction / varReproduction,
				VarianceReproduction   = varReproduction,
				CovarianceReproduction = covReproduction
			};

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nHeritability Calculations: \n" );
				var generations = History
					.GroupBy ( x => x.parent )
					.Select ( x => ( parent: x.Key,
					                 offsprings: x.Select ( y => y.offspring )
						                 .ToList ( ) ) )
					.Batch ( V.PopulationSize )
					.Select ( x => x.ToList ( ) )
					.ToList ( );
				for ( var i = 0; i < generations.Count; i++ )
				{
					var generation = generations[i];
					Logger.Debug ( $"\n\nTime Step#{i + 1}\n" );
					foreach ( var (parent, offsprings) in generation )
					{
						Logger.Debug ( $"{parent} -> {offsprings.Count} Offsprings:" );
						foreach ( var offspring in offsprings )
							Logger.Debug ( $"\t{offspring} OffspringCount: {offspring.OffspringCount}" );
					}
				}

				Logger.Debug ( $"\n{Heritability}" );
			}
		}
	}
}