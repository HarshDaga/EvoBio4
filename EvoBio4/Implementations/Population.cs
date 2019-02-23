using System.Collections.Generic;
using System.Linq;
using EvoBio4.Enums;
using MoreLinq;

namespace EvoBio4.Implementations
{
	public class Population
	{
		public IList<Individual> AllIndividuals { get; set; }

		public CooperatorGroup CooperatorGroup { get; set; }
		public DefectorGroup DefectorGroup { get; set; }

		public IndividualGroupBase this [ IndividualType type ]
		{
			get
			{
				if ( type == IndividualType.Cooperator )
					return CooperatorGroup;
				return DefectorGroup;
			}
		}

		public void Init ( Variables v )
		{
			CooperatorGroup = new CooperatorGroup ( );
			DefectorGroup   = new DefectorGroup ( );

			CooperatorGroup.Populate ( v.CooperatorQuantity, v );
			DefectorGroup.Populate ( v.DefectorQuantity, v );

			AllIndividuals = CooperatorGroup.Concat ( DefectorGroup ).ToList ( );
		}

		public void Add ( Individual individual )
		{
			if ( individual.Type == IndividualType.Cooperator )
				CooperatorGroup.Add ( individual );
			else DefectorGroup.Add ( individual );

			AllIndividuals.Add ( individual );
		}

		public bool Remove ( Individual individual )
		{
			AllIndividuals.Remove ( individual );
			if ( individual.Type == IndividualType.Cooperator )
				return CooperatorGroup.Remove ( individual );

			return DefectorGroup.Remove ( individual );
		}

		public void Swap ( Individual remove,
		                   Individual add )
		{
			if ( remove.Type == IndividualType.Cooperator )
				CooperatorGroup.Remove ( remove );
			else DefectorGroup.Remove ( remove );

			if ( add.Type == IndividualType.Cooperator )
				CooperatorGroup.Add ( add );
			else DefectorGroup.Add ( add );

			var index = AllIndividuals.IndexOf ( remove );
			AllIndividuals[index] = add;
		}

		public void Normalize ( )
		{
			var sum = CooperatorGroup.QualitySum + DefectorGroup.QualitySum;
			var n = AllIndividuals.Count;

			CooperatorGroup.Normalize ( sum, n );
			DefectorGroup.Normalize ( sum, n );
		}

		public void Shuffle ( )
		{
			AllIndividuals.Shuffle ( Utility.Srs );
		}
	}
}