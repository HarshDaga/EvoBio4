using System;
using System.Collections.Generic;
using EvoBio4.Core;
using EvoBio4.Core.Abstractions;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.Collections
{
	public class Individual : IndividualBase, IEquatable<Individual>
	{
		public Individual ( ) : base ( default, default )
		{
		}

		public Individual ( IndividualType type,
		                    int id ) : base ( type, id )
		{
		}

		public Individual (
			IndividualType type,
			int id,
			double quality
		) : base ( type, id )
		{
			Quality = quality;
		}

		public bool Equals ( Individual other ) =>
			other != null && Id == other.Id && Type == other.Type;

		public override IIndividual Reproduce ( int id,
		                                        double sd )
		{
			++OffspringCount;
			var quality = Utility.NextGaussianNonNegative ( Quality, sd );
			return new Individual ( Type, id, quality );
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