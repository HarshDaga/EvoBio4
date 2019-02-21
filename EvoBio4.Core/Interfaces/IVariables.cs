namespace EvoBio4.Core.Interfaces
{
	public interface IVariables
	{
		int PopulationSize { get; }
		int CooperatorQuantity { get; set; }
		int DefectorQuantity { get; set; }
		double SdQuality { get; set; }
		double Y { get; set; }
		double Relatedness { get; set; }
		double PercentileCutoff { get; }
		int MaxTimeSteps { get; set; }
		int Runs { get; set; }
		bool IncludeConfidenceIntervals { get; set; }
		double Z { get; set; }

		IVariables Clone ( );
		string ToString ( );
	}
}