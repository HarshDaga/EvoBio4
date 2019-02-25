namespace EvoBio4.Strategies.PostProcess
{
	public class DoNothingPostProcessStrategy : StrategyBase, IPostProcessStrategy
	{
		public override string Description => "Do nothing";

		public void Process ( Iteration iteration )
		{
		}
	}
}