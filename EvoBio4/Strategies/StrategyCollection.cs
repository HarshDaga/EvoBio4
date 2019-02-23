using EvoBio4.Strategies.Fitness;
using EvoBio4.Strategies.Perish;
using EvoBio4.Strategies.Reproduction;

namespace EvoBio4.Strategies
{
	public class StrategyCollection : IStrategyCollection
	{
		public IPerishStrategy Perish { get; set; }
		public IFitnessStrategy Fitness { get; set; }
		public IReproductionStrategy Reproduction { get; set; }

		public override string ToString ( ) =>
			"Strategies\n" +
			$"Perish : {Perish.Description}\n" +
			$"Fitness : {Fitness.Description}\n" +
			$"Reproduction: {Reproduction.Description}";
	}
}