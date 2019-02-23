using EvoBio4.Strategies.Fitness;
using EvoBio4.Strategies.PostProcess;
using EvoBio4.Strategies.Reproduction;
using EvoBio4.Strategies.Survival;

namespace EvoBio4.Strategies
{
	public static class StrategyFactory
	{
		public static class Survive
		{
			public static readonly ISurvivalStrategy EquiProbable =
				new EquiProbableSurvivalStrategy ( );

			public static readonly ISurvivalStrategy QualityProportional =
				new QualityProportionalSurvivalStrategy ( );

			public static readonly ISurvivalStrategy FitnessProportional =
				new FitnessProportionalSurvivalStrategy ( );

			public static readonly ISurvivalStrategy QualityInverselyProportional =
				new QualityInverselyProportionalSurvivalStrategy ( );

			public static readonly ISurvivalStrategy FitnessInverselyProportional =
				new FitnessInverselyProportionalSurvivalStrategy ( );
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