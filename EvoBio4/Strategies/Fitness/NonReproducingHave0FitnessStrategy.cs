﻿using System.Diagnostics.CodeAnalysis;

namespace EvoBio4.Strategies.Fitness
{
	public class NonReproducingHave0FitnessStrategy : StrategyBase, IFitnessStrategy
	{
		public override string Description => "Non Reproducing Cooperators have 0 fitness";

		[SuppressMessage ( "ReSharper", "InconsistentNaming" )]
		public double Calculate ( Iteration iteration )
		{
			var S = iteration.DefectorGroup.QualitySum;
			var Z = iteration.CooperatorGroup.ReproducingQualitySum;
			var r = iteration.V.Relatedness;
			var totalFitness = 0d;

			foreach ( var individual in iteration.CooperatorGroup.NonReproducingIndividuals )
				individual.Fitness = 0;

			foreach ( var individual in iteration.CooperatorGroup.ReproducingIndividuals )
			{
				var j = individual.Quality;
				individual.Fitness = j * (
					                     1d +
					                     r * iteration.ForegoneFitness / Z +
					                     ( 1d - r ) * iteration.ForegoneFitness / ( Z + S )
				                     );
				totalFitness += individual.Fitness;
			}

			foreach ( var individual in iteration.DefectorGroup )
			{
				var k = individual.Quality;
				individual.Fitness = k * (
					                     1d +
					                     ( 1d - r ) * iteration.ForegoneFitness / ( Z + S )
				                     );
				totalFitness += individual.Fitness;
			}

			if ( iteration.IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nCalculating Fitness:\n" );

				Logger.Debug ( iteration.CooperatorGroup.ToTable ( ) );
				Logger.Debug ( iteration.DefectorGroup.ToTable ( ) );
			}

			return totalFitness;
		}
	}
}