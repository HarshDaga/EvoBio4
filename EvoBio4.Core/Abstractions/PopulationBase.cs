using System;
using System.Collections.Generic;
using System.Linq;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.Core.Abstractions
{
	public abstract class PopulationBase<TIndividual, TVariables> :
		IPopulation<TIndividual, TVariables>
		where TIndividual : class, IIndividual
		where TVariables : IVariables
	{
		public IList<TIndividual> AllIndividuals { get; set; }

		public ICooperatorGroup<TIndividual> CooperatorGroup { get; set; }
		public IDefectorGroup<TIndividual> DefectorGroup { get; set; }

		public IIndividualGroup<TIndividual> this [ IndividualType type ]
		{
			get
			{
				if ( type == IndividualType.Cooperator )
					return CooperatorGroup;
				return DefectorGroup;
			}
		}

		public void Init ( TVariables variables )
		{
			Create ( variables );
		}

		public IIndividualGroup<TIndividual> Add ( TIndividual individual )
		{
			var group = this[individual.Type];
			group.Add ( individual );
			AllIndividuals.Add ( individual );
			return group;
		}

		public IIndividualGroup<TIndividual> Remove ( TIndividual individual )
		{
			var group = this[individual.Type];
			group.Remove ( individual );
			AllIndividuals.Remove ( individual );
			return group;
		}

		public (List<TIndividual> chosen, List<TIndividual> rejected) ChooseBy ( int amount,
		                                                                         Func<TIndividual, double> selector ) =>
			AllIndividuals.ChooseBy ( amount, selector );

		public List<TIndividual> RepetitiveChooseBy ( int amount,
		                                              Func<TIndividual, double> selector )
		{
			var cumulative = AllIndividuals.Select ( selector ).CumulativeSum ( ).ToList ( );
			var total = cumulative.Last ( );

			var parents = new List<TIndividual> ( amount );
			for ( var i = 0; i < amount; i++ )
			{
				var target = Utility.NextDouble * total;
				var index = cumulative.BinarySearch ( target );
				if ( index < 0 )
					index = ~index;

				parents.Add ( AllIndividuals[index] );
			}

			return parents;
		}

		protected abstract void Create ( TVariables variables );
	}
}