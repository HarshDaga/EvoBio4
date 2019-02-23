﻿using EvoBio4.Strategies.Fitness;
using EvoBio4.Strategies.PostProcess;
using EvoBio4.Strategies.Reproduction;
using EvoBio4.Strategies.Survival;

namespace EvoBio4.Strategies
{
	public class StrategyCollection : IStrategyCollection
	{
		public ISurvivalStrategy Survival { get; set; }
		public IFitnessStrategy Fitness { get; set; }
		public IReproductionStrategy Reproduction { get; set; }
		public IPostProcessStrategy PostProcess { get; set; }

		public override string ToString ( ) =>
			"Strategies\n" +
			$"Perish : {Survival.Description}\n" +
			$"Fitness : {Fitness.Description}\n" +
			$"Reproduction: {Reproduction.Description}\n" +
			$"Post Process: {PostProcess.Description}";
	}
}