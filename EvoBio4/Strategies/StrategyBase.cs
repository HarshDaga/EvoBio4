using NLog;

namespace EvoBio4.Strategies
{
	public abstract class StrategyBase : IStrategy
	{
		protected static readonly Logger Logger = LogManager.GetCurrentClassLogger ( );
		public abstract string Description { get; }

		public override string ToString ( ) => Description;
	}
}