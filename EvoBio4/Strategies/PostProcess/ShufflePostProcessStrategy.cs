namespace EvoBio4.Strategies.PostProcess
{
	public class ShufflePostProcessStrategy : StrategyBase, IPostProcessStrategy
	{
		public override string Description => "Shuffle the list of individuals";

		public void Process ( Iteration iteration )
		{
			iteration.Population.Shuffle ( );
		}
	}
}