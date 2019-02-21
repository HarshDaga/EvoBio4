using System.Diagnostics.Contracts;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.Core.Abstractions
{
	public abstract class VariablesBase : IVariables
	{
		public int PopulationSize => CooperatorQuantity + DefectorQuantity;
		public int CooperatorQuantity { get; set; }
		public int DefectorQuantity { get; set; }
		public double SdQuality { get; set; }
		public double Y { get; set; }
		public double Relatedness { get; set; }
		public double PercentileCutoff { get; set; }
		public int MaxTimeSteps { get; set; }
		public int Runs { get; set; }
		public double Z { get; set; }
		public bool IncludeConfidenceIntervals { get; set; }

		[Pure]
		IVariables IVariables.Clone ( ) => (VariablesBase) MemberwiseClone ( );

		public override string ToString ( ) =>
			$"{nameof ( CooperatorQuantity )}: {CooperatorQuantity}, " +
			$"{nameof ( DefectorQuantity )}: {DefectorQuantity}\n" +
			$"{nameof ( MaxTimeSteps )}: {MaxTimeSteps}, " +
			$"{nameof ( Runs )}: {Runs}\n" +
			$"{nameof ( SdQuality )}: {SdQuality}, " +
			$"{nameof ( PercentileCutoff )}: {PercentileCutoff}\n" +
			$"{nameof ( Y )}: {Y}, " +
			$"{nameof ( Relatedness )}: {Relatedness}\n" +
			$"{nameof ( Z )}: {Z}\n";

		[Pure]
		public virtual VariablesBase Clone ( ) => (VariablesBase) MemberwiseClone ( );
	}
}