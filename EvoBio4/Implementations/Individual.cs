using System;
using System.Collections.Generic;
using EvoBio4.Enums;

namespace EvoBio4.Implementations
{
	public class Individual : IEquatable<Individual>
	{
		public int Id { get; }
		public IndividualType Type { get; }
		public double Quality { get; protected set; }
		public double Fitness { get; set; }

		public int OffspringCount { get; set; }

		public string Name => $"{Type}_{Id}";
		public string PaddedName => $"{Name,-15}";
		public bool IsPerished { get; set; }

		public Individual ( ) : this ( default, default )
		{
		}

		public Individual ( IndividualType type,
		                    int id )
		{
			Type = type;
			Id   = id;
		}

		public Individual (
			IndividualType type,
			int id,
			double quality
		) : this ( type, id )
		{
			Quality = quality;
		}

		public bool Equals ( Individual other ) =>
			other != null && Id == other.Id && Type == other.Type;

		public Individual Reproduce ( int id,
		                              double sd )
		{
			++OffspringCount;
			var quality = Utility.NextGaussianNonNegative ( Quality, sd );
			return new Individual ( Type, id, quality );
		}

		public virtual void Normalize ( double qualitySum,
		                                int populationSize )
		{
			Quality /= qualitySum / 10d / populationSize;
		}

		public override bool Equals ( object obj ) =>
			Equals ( obj as Individual );

		public override int GetHashCode ( )
		{
			var hashCode = 0x4F08716D;
			hashCode = hashCode * -0x5AAAAAD7 + Id.GetHashCode ( );
			hashCode = hashCode * -0x5AAAAAD7 + Type.GetHashCode ( );
			return hashCode;
		}

		public override string ToString ( ) =>
			$"{PaddedName} {nameof ( Quality )}: {Quality,8:F4} {nameof ( Fitness )}: {Fitness,8:F4}";

		public static bool operator == ( Individual individual1,
		                                 Individual individual2 ) =>
			EqualityComparer<Individual>.Default.Equals ( individual1, individual2 );

		public static bool operator != ( Individual individual1,
		                                 Individual individual2 ) =>
			!( individual1 == individual2 );
	}
}