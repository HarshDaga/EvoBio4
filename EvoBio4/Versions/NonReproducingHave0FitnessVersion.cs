using System.Diagnostics.CodeAnalysis;

namespace EvoBio4.Versions
{
	internal class NonReproducingHave0FitnessVersion : SingleIterationBaseVersion
	{
		[SuppressMessage ( "ReSharper", "InconsistentNaming" )]
		public override void CalculateFitness ( )
		{
			var S = DefectorGroup.QualitySum;
			var Z = CooperatorGroup.ReproducingQualitySum;
			var r = V.Relatedness;
			TotalFitness = 0;

			foreach ( var individual in CooperatorGroup.ReproducingIndividuals )
			{
				var j = individual.Quality;
				individual.Fitness = j * (
					                     1d +
					                     r * ForegoneFitness / Z +
					                     ( 1d - r ) * ForegoneFitness / ( Z + S )
				                     );
				TotalFitness += individual.Fitness;
			}

			foreach ( var individual in DefectorGroup )
			{
				var k = individual.Quality;
				individual.Fitness = k * (
					                     1d +
					                     ( 1d - r ) * ForegoneFitness / ( Z + S )
				                     );
				TotalFitness += individual.Fitness;
			}

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nCalculating Fitness:\n" );

				Logger.Debug ( CooperatorGroup.ToTable ( ) );
				Logger.Debug ( DefectorGroup.ToTable ( ) );
			}
		}
	}
}