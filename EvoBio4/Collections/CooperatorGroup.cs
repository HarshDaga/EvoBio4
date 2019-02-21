using System.Collections.Generic;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.Collections
{
	public class CooperatorGroup : IndividualGroupBase, ICooperatorGroup<Individual>
	{
		public List<Individual> ReproducingIndividuals { get; private set; }
		public List<Individual> NonReproducingIndividuals { get; private set; }
		public double ReproducingQualitySum { get; private set; }
		public double NonReproducingQualitySum { get; private set; }

		public CooperatorGroup ( ) :
			base ( IndividualType.Cooperator )
		{
		}

		public void Split ( double threshold )
		{
			ReproducingIndividuals    = new List<Individual> ( Individuals.Count );
			NonReproducingIndividuals = new List<Individual> ( Individuals.Count );
			ReproducingQualitySum     = 0;
			NonReproducingQualitySum  = 0;

			foreach ( var individual in Individuals )
				if ( individual.Quality < threshold )
				{
					NonReproducingIndividuals.Add ( individual );
					NonReproducingQualitySum += individual.Quality;
				}
				else
				{
					ReproducingIndividuals.Add ( individual );
					ReproducingQualitySum += individual.Quality;
				}

			QualitySum = ReproducingQualitySum + NonReproducingQualitySum;
		}
	}
}