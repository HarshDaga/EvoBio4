using EvoBio4.Collections;
using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.DeathSelectionRules
{
	public class FitnessInverselyProportionalDeathSelectionRule :
		IDeathSelectionRule<Individual, Variables, Population>
	{
		public Individual ChooseFrom ( Population population ) =>
			population.AllIndividuals.ChooseOneBy ( x => 1d / x.Fitness );
	}
}