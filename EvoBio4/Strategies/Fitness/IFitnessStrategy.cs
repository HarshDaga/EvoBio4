namespace EvoBio4.Strategies.Fitness
{
	public interface IFitnessStrategy
	{
		string Description { get; }

		double Calculate ( Iteration iteration );
	}
}