using System.Diagnostics.Contracts;
using EvoBio4.Core.Abstractions;

// ReSharper disable InconsistentNaming

namespace EvoBio4.Collections
{
	public class Variables : VariablesBase
	{
		[Pure]
		public override VariablesBase Clone ( ) => (Variables) MemberwiseClone ( );
	}
}