using EvoBio4.Strategies.Fitness;
using EvoBio4.Strategies.PostProcess;
using EvoBio4.Strategies.Reproduction;
using EvoBio4.Strategies.Survival;

namespace EvoBio4.Strategies
{
	public interface IStrategyCollection
	{
		ISurvivalStrategy Survival { get; }
		IFitnessStrategy Fitness { get; }
		IReproductionStrategy Reproduction { get; }
		IPostProcessStrategy PostProcess { get; }
	}
}