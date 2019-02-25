using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Survival
{
	public interface ISurvivalStrategy : IStrategy
	{
		Individual Choose ( Iteration iteration );
	}
}