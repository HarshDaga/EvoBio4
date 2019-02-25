namespace EvoBio4.Strategies.PostProcess
{
	public interface IPostProcessStrategy : IStrategy
	{
		void Process ( Iteration iteration );
	}
}