using EvoBio4.Extensions;
using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Survival
{
	public class FitnessInverselyProportionalSurvivalStrategy : ISurvivalStrategy
	{
		public string Description => "Choose 1 victim with probability inversely proportional to fitness";

		public Individual Choose ( Iteration iteration ) =>
			iteration.Population.AllIndividuals.ChooseOneBy ( x => 1d / x.Fitness );
	}
}