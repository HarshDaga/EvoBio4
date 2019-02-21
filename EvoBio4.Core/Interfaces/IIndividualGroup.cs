using System;
using System.Collections.Generic;
using EvoBio4.Core.Enums;

namespace EvoBio4.Core.Interfaces
{
	public interface IIndividualGroup<TIndividual> : ICollection<TIndividual>
		where TIndividual : class, IIndividual
	{
		IndividualType Type { get; }
		List<TIndividual> Individuals { get; }

		double QualitySum { get; }

		void Populate ( int count,
		                IVariables v );

		string ToTable ( );
		string ToTable ( Func<TIndividual, object> selector );
	}
}