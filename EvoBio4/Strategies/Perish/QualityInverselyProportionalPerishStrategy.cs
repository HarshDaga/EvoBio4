﻿using EvoBio4.Extensions;
using EvoBio4.Implementations;

namespace EvoBio4.Strategies.Perish
{
	public class QualityInverselyProportionalPerishStrategy : IPerishStrategy
	{
		public string Description => "Choose 1 victim with probability inversely proportional to quality";

		public Individual Choose ( Iteration iteration ) =>
			iteration.Population.AllIndividuals.ChooseOneBy ( x => 1d / x.Quality );
	}
}