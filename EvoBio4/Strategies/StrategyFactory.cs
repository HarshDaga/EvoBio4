using EvoBio4.Strategies.Fitness;
using EvoBio4.Strategies.Perish;
using EvoBio4.Strategies.PostProcess;
using EvoBio4.Strategies.Reproduction;

namespace EvoBio4.Strategies
{
	public static class StrategyFactory
	{
		public static class Perish
		{
			public static readonly IPerishStrategy EquiProbable =
				new EquiProbablePerishStrategy ( );

			public static readonly IPerishStrategy QualityProportional =
				new QualityProportionalPerishStrategy ( );

			public static readonly IPerishStrategy FitnessProportional =
				new FitnessProportionalPerishStrategy ( );

			public static readonly IPerishStrategy QualityInverselyProportional =
				new QualityInverselyProportionalPerishStrategy ( );

			public static readonly IPerishStrategy FitnessInverselyProportional =
				new FitnessInverselyProportionalPerishStrategy ( );
		}

		public static class Fitness
		{
			public static readonly IFitnessStrategy Default =
				new DefaultFitnessStrategy ( );

			public static readonly IFitnessStrategy NonReproducingHave0Fitness =
				new NonReproducingHave0FitnessStrategy ( );
		}

		public static class Reproduction
		{
			public static readonly IReproductionStrategy QualityProportional =
				new QualityProportionalReproductionStrategy ( );
		}

		public static class PostProcess
		{
			public static readonly IPostProcessStrategy DoNothing =
				new DoNothingPostProcessStrategy ( );

			public static readonly IPostProcessStrategy Shuffle =
				new ShufflePostProcessStrategy ( );
		}
	}
}