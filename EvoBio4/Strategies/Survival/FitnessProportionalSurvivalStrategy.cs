using EvoBio4.Extensions;
using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Survival
{
	public class FitnessProportionalSurvivalStrategy : ISurvivalStrategy
	{
		public string Description => "Choose N-1 survivors with probability proportional to fitness";

		public Individual Choose ( Iteration iteration ) =>
			iteration.Population.AllIndividuals.ChooseAllButOneBy ( x => x.Fitness );
	}
}