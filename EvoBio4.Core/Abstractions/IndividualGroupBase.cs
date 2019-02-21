using System;
using System.Collections;
using System.Collections.Generic;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;

// ReSharper disable PossibleNullReferenceException

namespace EvoBio4.Core.Abstractions
{
	public abstract class IndividualGroupBase<TIndividual> :
		IIndividualGroup<TIndividual>
		where TIndividual : class, IIndividual
	{
		public virtual IndividualType Type { get; }
		public List<TIndividual> Individuals { get; protected set; }

		public double QualitySum { get; protected set; }
		public double FitnessSum { get; protected set; }

		public int Count => Individuals.Count;
		public bool IsReadOnly => false;

		protected IndividualGroupBase ( IndividualType type )
		{
			Type = type;
		}

		public virtual void Add ( TIndividual individual )
		{
			QualitySum += individual.Quality;
			Individuals.Add ( individual );
		}

		public virtual void Clear ( ) => Individuals.Clear ( );

		public virtual bool Contains ( TIndividual individual ) => Individuals.Contains ( individual );

		public virtual void CopyTo ( TIndividual[] array,
		                             int arrayIndex ) =>
			Individuals.CopyTo ( array, arrayIndex );

		public virtual bool Remove ( TIndividual individual )
		{
			if ( Individuals.Remove ( individual ) )
			{
				QualitySum -= individual.Quality;
				return true;
			}

			return false;
		}

		public string ToTable ( )
		{
			return ToTable ( x => new
				{
					x.Id,
					Quality = $"{x.Quality:F4}",
					Fitness = $"{x.Fitness:F4}"
				}
			);
		}

		public string ToTable ( Func<TIndividual, object> selector )
		{
			var table = Individuals.ToTable ( selector );

			return $"{Type}\n{table}";
		}

		public IEnumerator<TIndividual> GetEnumerator ( ) => Individuals.GetEnumerator ( );

		IEnumerator IEnumerable.GetEnumerator ( ) => ( (IEnumerable) Individuals ).GetEnumerator ( );

		public abstract void Populate ( int count,
		                                IVariables v );

		public override string ToString ( ) =>
			$"{Type}, {Count}";
	}
}