using EvoBio4.Core;
using EvoBio4.Core.Interfaces;
using EvoBio4.Implementations;

namespace EvoBio4.DeathSelectionRules
{
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