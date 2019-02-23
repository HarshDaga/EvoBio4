using EvoBio4.Strategies.Fitness;
using EvoBio4.Strategies.Perish;
using EvoBio4.Strategies.PostProcess;
using EvoBio4.Strategies.Reproduction;

namespace EvoBio4.Strategies
{
	public interface IStrategyCollection
	{
		IPerishStrategy Perish { get; }
		IFitnessStrategy Fitness { get; }
		IReproductionStrategy Reproduction { get; }
		IPostProcessStrategy PostProcess { get; }
	}
}