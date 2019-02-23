using EvoBio4.Core;
using EvoBio4.Core.Interfaces;
using EvoBio4.Implementations;
using EvoBio4.Versions;

namespace EvoBio4.DeathSelectionRules
{
	public static class PerishStrategies<TIteration>
		where TIteration : IterationBaseVersion, IIteration<Individual, Variables, Population, TIteration>, new ( )
	{
		public static PerishStrategyDelegate<Individual, Variables, Population, TIteration> EquiProbable =>
			iteration =>
			{
				var count = iteration.Population.AllIndividuals.Count;
				var index = Utility.Srs.Next ( count );

				return iteration.Population.AllIndividuals[index];
			};
	}

	public class EquiProbablePerishStrategy :
		IPerishStrategy<Individual, Variables, Population>
	{
		public Individual ChooseFrom ( Population population )
		{
			var count = population.AllIndividuals.Count;
			var index = Utility.Srs.Next ( count );

			return population.AllIndividuals[index];
		}
	}
}