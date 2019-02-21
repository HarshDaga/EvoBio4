using EvoBio4.Collections;
using EvoBio4.Core;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.DeathSelectionRules
{
	public class EquiProbableDeathSelectionRule :
		IDeathSelectionRule<Individual, Variables, Population>
	{
		public Individual ChooseFrom ( Population population )
		{
			var count = population.AllIndividuals.Count;
			var index = Utility.Srs.Next ( count );

			return population.AllIndividuals[index];
		}
	}
}