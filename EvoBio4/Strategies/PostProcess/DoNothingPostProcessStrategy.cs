namespace EvoBio4.Strategies.PostProcess
{
	public class DoNothingPostProcessStrategy : IPostProcessStrategy
	{
		public string Description => "Do nothing";

		public void Process ( Iteration iteration )
		{
		}
	}
}