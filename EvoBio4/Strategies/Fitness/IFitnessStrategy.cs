namespace EvoBio4.Strategies.Fitness
{
	public interface IFitnessStrategy : IStrategy
	{
		double Calculate ( Iteration iteration );
	}
}