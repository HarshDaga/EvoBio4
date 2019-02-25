using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Reproduction
{
	public interface IReproductionStrategy : IStrategy
	{
		Individual Choose ( Iteration iteration );
	}
}