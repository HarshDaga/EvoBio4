using System.Linq;
using EvoBio4.Core.Abstractions;

namespace EvoBio4.Implementations
{
	public class Population : PopulationBase<Individual, Variables>
	{
		protected override void Create ( Variables v )
		{
			CooperatorGroup = new CooperatorGroup ( );
			DefectorGroup   = new DefectorGroup ( );

			CooperatorGroup.Populate ( v.CooperatorQuantity, v );
			DefectorGroup.Populate ( v.DefectorQuantity, v );

			AllIndividuals = CooperatorGroup.Concat ( DefectorGroup ).ToList ( );
		}
	}
}