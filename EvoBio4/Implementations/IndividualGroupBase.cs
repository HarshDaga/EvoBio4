using System;
using System.Collections;
using System.Collections.Generic;
using EvoBio4.Enums;
using EvoBio4.Extensions;

namespace EvoBio4.Implementations
{
	public abstract class IndividualGroupBase : ICollection<Individual>
	{
		public virtual IndividualType Type { get; }
		public List<Individual> Individuals { get; protected set; }

		public double QualitySum { get; protected set; }
		public double FitnessSum { get; protected set; }

		public int Count => Individuals.Count;
		public bool IsReadOnly => false;

		protected IndividualGroupBase ( )
		{
		}

		protected IndividualGroupBase ( IndividualType type )
		{
			Type = type;
		}


		public virtual void Add ( Individual individual )
		{
			QualitySum += individual.Quality;
			Individuals.Add ( individual );
		}

		public virtual void Clear ( ) => Individuals.Clear ( );

		public virtual bool Contains ( Individual individual ) => Individuals.Contains ( individual );

		public virtual void CopyTo ( Individual[] array,
		                             int arrayIndex ) =>
			Individuals.CopyTo ( array, arrayIndex );

		public virtual bool Remove ( Individual individual )
		{
			if ( Individuals.Remove ( individual ) )
			{
				QualitySum -= individual.Quality;
				return true;
			}

			return false;
		}

		public IEnumerator<Individual> GetEnumerator ( ) => Individuals.GetEnumerator ( );

		IEnumerator IEnumerable.GetEnumerator ( ) => ( (IEnumerable) Individuals ).GetEnumerator ( );

		public virtual void Populate ( int count,
		                               Variables v )
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

		public void Normalize ( double qualitySum,
		                        int populationSize )
		{
			QualitySum = 0;
			foreach ( var individual in Individuals )
			{
				individual.Normalize ( qualitySum, populationSize );
				QualitySum += individual.Quality;
			}
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

		public string ToTable ( Func<Individual, object> selector )
		{
			var table = Individuals.ToTable ( selector );

			return $"{Type}\n{table}";
		}

		public override string ToString ( ) =>
			$"{Type}, {Count}";
	}
}