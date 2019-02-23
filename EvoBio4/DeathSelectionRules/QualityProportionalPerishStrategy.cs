using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;
using EvoBio4.Implementations;

namespace EvoBio4.DeathSelectionRules
{
	public class QualityProportionalPerishStrategy :
		IPerishStrategy<Individual, Variables, Population>
	{
		public Individual ChooseFrom ( Population population ) =>
			population.AllIndividuals.ChooseAllButOneBy ( x => x.Quality );
	}
}