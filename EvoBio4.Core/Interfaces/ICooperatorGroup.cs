using System.Collections.Generic;

namespace EvoBio4.Core.Interfaces
{
	public interface ICooperatorGroup<TIndividual> : IIndividualGroup<TIndividual>
		where TIndividual : class, IIndividual
	{
		List<TIndividual> ReproducingIndividuals { get; }
		List<TIndividual> NonReproducingIndividuals { get; }

		double ReproducingQualitySum { get; }
		double NonReproducingQualitySum { get; }

		void Split ( double threshold );
	}
}