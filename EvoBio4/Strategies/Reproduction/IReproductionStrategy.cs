using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Reproduction
{
	public interface IReproductionStrategy
	{
		string Description { get; }

		Individual Choose ( Iteration iteration );
	}
}