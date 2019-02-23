namespace EvoBio4.Strategies.PostProcess
{
	public interface IPostProcessStrategy
	{
		string Description { get; }

		void Process ( Iteration iteration );
	}
}