using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using CsvHelper;
using EnumsNET;
using EvoBio4.Core;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;
using MathNet.Numerics.Statistics;

namespace EvoBio4.Implementations
{
	[SuppressMessage ( "ReSharper", "UnusedAutoPropertyAccessor.Local" )]
	[SuppressMessage ( "ReSharper", "MemberCanBePrivate.Local" )]
	public class ConfidenceIntervalStats : IConfidenceIntervalStats
	{
		public static IEnumerable<IndividualType> IndividualTypes => Enums.GetValues<IndividualType> ( );

		public int TimeSteps { get; }
		public int Runs { get; }
		public List<Dictionary<IndividualType, ConfidenceInterval>> Summary { get; private set; }
		public Dictionary<IndividualType, List<RunningStatistics>> RunningStats { get; }
		public double Z { get; }

		public ConfidenceInterval this [ int timeStep,
		                                 IndividualType type ]
		{
			get
			{
				if ( Summary.Count < timeStep )
					return default;
				if ( Summary[timeStep].TryGetValue ( type, out var result ) )
					return result;
				return default;
			}
		}

		public ConfidenceIntervalStats ( int timeSteps,
		                                 int runs,
		                                 double z )
		{
			TimeSteps    = timeSteps + 1;
			Runs         = runs;
			Z            = z;
			RunningStats = new Dictionary<IndividualType, List<RunningStatistics>> ( );

			foreach ( var type in IndividualTypes )
			{
				RunningStats[type] = new List<RunningStatistics> ( TimeSteps );
				for ( var i = 0; i < TimeSteps; i++ )
					RunningStats[type].Add ( new RunningStatistics ( ) );
			}
		}

		public void Add ( IDictionary<IndividualType, List<int>> iterationResult )
		{
			foreach ( var kp in iterationResult )
			{
				var type = kp.Key;
				var survivors = kp.Value;

				for ( var timeStep = 0; timeStep < survivors.Count; timeStep++ )
					RunningStats[type][timeStep].Push ( survivors[timeStep] );
			}
		}

		public void Compute ( )
		{
			Summary = new List<Dictionary<IndividualType, ConfidenceInterval>> ( TimeSteps );

			for ( var i = 0; i < TimeSteps; i++ )
				Summary.Add (
					new Dictionary<IndividualType, ConfidenceInterval> (
						Enums.GetMemberCount<IndividualType> ( ) ) );

			foreach ( var kp in RunningStats )
			{
				var type = kp.Key;
				var stats = kp.Value;
				for ( var i = 0; i < stats.Count; i++ )
					Summary[i][type] = stats[i].ConfidenceInterval ( Z );
			}
		}

		public void PrintToCsv ( string fileName )
		{
			using ( var csv = new CsvWriter ( File.CreateText ( fileName ) ) )
			{
				csv.WriteRecords ( Summary.Select ( ( x,
				                                      i ) => new CsvRecord ( i, x ) ) );
			}
		}

		private class CsvRecord
		{
			public int Generation { get; }
			public double CooperatorHigh { get; }
			public double CooperatorLow { get; }
			public double CooperatorMean { get; }
			public double DefectorHigh { get; }
			public double DefectorLow { get; }
			public double DefectorMean { get; }

			public CsvRecord ( int generation,
			                   IDictionary<IndividualType, ConfidenceInterval> record )
			{
				Generation     = generation;
				CooperatorLow  = record[IndividualType.Cooperator].Low;
				CooperatorMean = record[IndividualType.Cooperator].Mean;
				CooperatorHigh = record[IndividualType.Cooperator].High;
				DefectorLow    = record[IndividualType.Defector].Low;
				DefectorMean   = record[IndividualType.Defector].Mean;
				DefectorHigh   = record[IndividualType.Defector].High;
			}
		}
	}
}