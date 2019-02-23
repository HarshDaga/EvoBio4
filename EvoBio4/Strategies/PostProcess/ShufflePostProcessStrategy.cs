namespace EvoBio4.Strategies.PostProcess
{
	public class ShufflePostProcessStrategy : IPostProcessStrategy
	{
		public string Description => "Shuffle the list of individuals";

		public void Process ( Iteration iteration )
		{
			iteration.Population.Shuffle ( );
		}
	}
}