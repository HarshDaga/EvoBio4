using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Perish
{
	public class EquiProbablePerishStrategy : IPerishStrategy
	{
		public string Description => "Any 1 victim at random";

		public Individual Choose ( Iteration iteration )
		{
			var count = iteration.Population.AllIndividuals.Count;
			var index = Utility.Srs.Next ( count );

			return iteration.Population.AllIndividuals[index];
		}
	}
}