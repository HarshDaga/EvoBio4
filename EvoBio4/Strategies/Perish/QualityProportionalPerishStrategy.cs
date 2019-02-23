using EvoBio4.Extensions;
using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Perish
{
	public class QualityProportionalPerishStrategy : IPerishStrategy
	{
		public string Description => "Choose N-1 survivors with probability proportional to quality";

		public Individual Choose ( Iteration iteration ) =>
			iteration.Population.AllIndividuals.ChooseAllButOneBy ( x => x.Quality );
	}
}