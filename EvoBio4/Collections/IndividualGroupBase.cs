using System.Collections.Generic;
using EvoBio4.Core;
using EvoBio4.Core.Abstractions;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.Collections
{
	public abstract class IndividualGroupBase : IndividualGroupBase<Individual>
	{
		protected IndividualGroupBase ( ) : base ( default )
		{
		}

		protected IndividualGroupBase ( IndividualType type ) : base ( type )
		{
		}

		public override void Populate ( int count,
		                                IVariables v )
		{
			Individuals = new List<Individual> ( count );
			var qualities = Utility.NextGaussianNonNegativeSymbols ( 10,
			                                                         v.SdQuality,
			                                                         count );
			for ( var i = 0; i < qualities.Length; i++ )
			{
				var individual = new Individual ( Type, i + 1, qualities[i] );
				Add ( individual );
			}
		}
	}
}