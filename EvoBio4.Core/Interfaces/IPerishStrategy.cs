﻿namespace EvoBio4.Core.Interfaces
{
	public interface IPerishStrategy<out TIndividual, TVariables, in TPopulation>
		where TIndividual : class, IIndividual, new ( )
		where TVariables : IVariables
		where TPopulation : IPopulation<TIndividual, TVariables>
	{
		TIndividual ChooseFrom ( TPopulation population );
	}
}