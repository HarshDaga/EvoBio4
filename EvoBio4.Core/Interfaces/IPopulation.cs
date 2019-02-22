using System.Collections.Generic;
using EvoBio4.Core.Enums;

namespace EvoBio4.Core.Interfaces
{
	public interface IPopulation<TIndividual, in TVariables>
		where TIndividual : class, IIndividual
		where TVariables : IVariables
	{
		IList<TIndividual> AllIndividuals { get; set; }
		ICooperatorGroup<TIndividual> CooperatorGroup { get; set; }
		IDefectorGroup<TIndividual> DefectorGroup { get; set; }

		IIndividualGroup<TIndividual> this [ IndividualType type ] { get; }

		void Init ( TVariables variables );
		IIndividualGroup<TIndividual> Add ( TIndividual individual );
		IIndividualGroup<TIndividual> Remove ( TIndividual individual );

		void Normalize ( );
	}
}