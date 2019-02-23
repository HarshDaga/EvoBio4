using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Perish
{
	public interface IPerishStrategy
	{
		string Description { get; }

		Individual Choose ( Iteration iteration );
	}
}