using System.Linq;
using EvoBio4.Extensions;
using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Reproduction
{
	public class FitnessProportionalReproductionStrategy : IReproductionStrategy
	{
		public string Description => "Choose 1 reproducing individual with probability proportional to quality";

		public Individual Choose ( Iteration iteration ) =>
			iteration.CooperatorGroup
				.ReproducingIndividuals
				.Concat ( iteration.DefectorGroup )
				.ChooseOneBy ( x => x.Fitness,
				               iteration.TotalFitness -
				               iteration.CooperatorGroup.NonReproducingIndividuals.Sum ( x => x.Fitness ) );
	}
}