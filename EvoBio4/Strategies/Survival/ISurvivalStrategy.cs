using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Survival
{
	public interface ISurvivalStrategy
	{
		string Description { get; }

		Individual Choose ( Iteration iteration );
	}
}