﻿using EvoBio4.Collections;
using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.DeathSelectionRules
{
	public class FitnessProportionalDeathSelectionRule :
		IDeathSelectionRule<Individual, Variables, Population>
	{
		public Individual ChooseFrom ( Population population ) =>
			population.AllIndividuals.ChooseAllButOneBy ( x => x.Fitness );
	}
}