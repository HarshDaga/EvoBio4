using EvoBio4.Core.Interfaces;

namespace EvoBio4.Collections
{
	public class HeritabilitySummary : IHeritabilitySummary
	{
		public double Quality
		{
			get => Values[0];
			set => Values[0] = value;
		}

		public double VarianceQuality
		{
			get => Values[1];
			set => Values[1] = value;
		}

		public double CovarianceQuality
		{
			get => Values[2];
			set => Values[2] = value;
		}

		public double Reproduction
		{
			get => Values[3];
			set => Values[3] = value;
		}

		public double VarianceReproduction
		{
			get => Values[4];
			set => Values[4] = value;
		}

		public double CovarianceReproduction
		{
			get => Values[5];
			set => Values[5] = value;
		}

		public int ValueCount { get; }
		public double[] Values { get; }

		public HeritabilitySummary ( )
		{
			ValueCount = 6;
			Values     = new double[ValueCount];
		}

		public override string ToString ( ) =>
			$"Quality:                   {Quality,8:F8}\n" +
			$"Variance Quality:          {VarianceQuality,8:F8}\n" +
			$"Covariance Quality:        {CovarianceQuality,8:F8}\n\n" +
			$"Reproduction:              {Reproduction,8:F8}\n" +
			$"Variance Reproduction:     {VarianceReproduction,8:F8}\n" +
			$"Covariance Reproduction:   {CovarianceReproduction,8:F8}";
	}
}