using System;
using EvoBio4.Core.Enums;

namespace EvoBio4.Core.Interfaces
{
	public interface IIndividual : IEquatable<IIndividual>
	{
		Guid Guid { get; }
		IndividualType Type { get; }
		int Id { get; }
		int OffspringCount { get; set; }
		double Quality { get; }
		double Fitness { get; set; }
		string Name { get; }
		string PaddedName { get; }

		IIndividual Reproduce ( int id,
		                        double sd );

		void Normalize ( double qualitySum,
		                 int populationSize );

		bool Equals ( object obj );
		int GetHashCode ( );
	}
}