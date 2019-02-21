using System;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.Core.Abstractions
{
	public abstract class IndividualBase : IIndividual, IEquatable<IndividualBase>
	{
		public int Id { get; }
		public Guid Guid { get; } = Guid.NewGuid ( );
		public IndividualType Type { get; }
		public double Quality { get; protected set; }
		public double Fitness { get; set; }

		public int OffspringCount { get; set; }

		public string Name => $"{Type}_{Id}";
		public string PaddedName => $"{Name,-15}";
		public bool IsPerished { get; set; }

		protected IndividualBase ( IndividualType type,
		                           int id )
		{
			Type = type;
			Id   = id;
		}

		bool IEquatable<IndividualBase>.Equals ( IndividualBase other ) => Equals ( other );

		public bool Equals ( IIndividual other )
		{
			if ( other is null ) return false;
			return ReferenceEquals ( this, other ) || Guid.Equals ( other.Guid );
		}

		public abstract IIndividual Reproduce ( int id,
		                                        double sd );

		public virtual void Normalize ( double qualitySum,
		                                int populationSize )
		{
			Quality /= qualitySum / 10d / populationSize;
		}

		public override bool Equals ( object obj )
		{
			if ( obj is null ) return false;
			if ( ReferenceEquals ( this, obj ) ) return true;
			return obj.GetType ( ) == GetType ( ) && Equals ( (IndividualBase) obj );
		}

		public override int GetHashCode ( ) => Guid.GetHashCode ( );

		public static bool operator == ( IndividualBase left,
		                                 IndividualBase right ) => Equals ( left, right );

		public static bool operator != ( IndividualBase left,
		                                 IndividualBase right ) => !Equals ( left, right );
	}
}