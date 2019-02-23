using System;
using System.Collections.Generic;
using System.Linq;
using EvoBio4.Enums;
using EvoBio4.Extensions;
using EvoBio4.Implementations;
using EvoBio4.Strategies;
using MathNet.Numerics.Statistics;
using MoreLinq;
using NLog;

namespace EvoBio4
{
	public class Iteration
	{
		protected static readonly Logger Logger = LogManager.GetCurrentClassLogger ( );
		public bool IsLoggingEnabled { get; set; }

		public Population Population { get; protected set; }

		public IList<Individual> AllIndividuals
		{
			get => Population.AllIndividuals;
			set => Population.AllIndividuals = value;
		}

		public CooperatorGroup CooperatorGroup
		{
			get => Population.CooperatorGroup;
			set => Population.CooperatorGroup = value;
		}

		public DefectorGroup DefectorGroup
		{
			get => Population.DefectorGroup;
			set => Population.DefectorGroup = value;
		}

		public List<IndividualGroupBase> AllGroups { get; protected set; }

		public Variables V { get; protected set; }

		public double ForegoneQuality { get; protected set; }
		public double ForegoneFitness { get; protected set; }
		public double TotalFitness { get; protected set; }
		public HeritabilitySummary Heritability { get; set; }
		public Winner Winner { get; set; }
		public int TimeStepsPassed { get; protected set; }

		public virtual IStrategyCollection StrategyCollection { get; protected set; }

		public IDictionary<IndividualType, List<int>> GenerationHistory { get; protected set; }

		public List<(Individual parent, Individual offspring)> History { get; protected set; }

		protected readonly Dictionary<IndividualType, int> LastIds = new Dictionary<IndividualType, int>
		{
			[IndividualType.Cooperator] = 0,
			[IndividualType.Defector]   = 0
		};

		public virtual void Init ( Variables v,
		                           IStrategyCollection strategyCollection,
		                           bool isLoggingEnabled = false )
		{
			V                  = v;
			StrategyCollection = strategyCollection;
			IsLoggingEnabled   = isLoggingEnabled;

			History = new List<(Individual parent, Individual offspring)> ( V.MaxTimeSteps );

			GenerationHistory = new Dictionary<IndividualType, List<int>> ( );
			foreach ( var type in EnumsNET.Enums.GetValues<IndividualType> ( ) )
				GenerationHistory[type] = new List<int> ( V.MaxTimeSteps );

			if ( IsLoggingEnabled )
				Logger.Debug ( $"\n\n{V}\n" );
		}

		public virtual void CreateInitialPopulation ( )
		{
			Population = new Population ( );
			Population.Init ( V );
			AllGroups = new List<IndividualGroupBase>
			{
				Population.CooperatorGroup,
				Population.DefectorGroup
			};

			LastIds[IndividualType.Cooperator] = CooperatorGroup.Count;
			LastIds[IndividualType.Defector]   = DefectorGroup.Count;

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nInitial Population:\n" );

				Logger.Debug ( CooperatorGroup.ToTable ( ) );
				Logger.Debug ( DefectorGroup.ToTable ( ) );
			}
		}

		public virtual void SplitCooperators ( )
		{
			var threshold = AllIndividuals
				.Select ( x => x.Quality )
				.OrderBy ( x => x )
				.ToList ( )
				.AtPercentile ( V.PercentileCutoff );
			CooperatorGroup.Split ( threshold );

			ForegoneQuality = CooperatorGroup.NonReproducingQualitySum;
			ForegoneFitness = V.Y * ForegoneQuality;

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nSplitting Cooperators:\n" );

				Logger.Debug ( $"Threshold quality at {V.PercentileCutoff / 100d:P} = {threshold}" );
				Logger.Debug ( $"Reproducing Cooperators = {CooperatorGroup.ReproducingIndividuals.Count}" );
				Logger.Debug ( $"Non Reproducing Cooperators = {CooperatorGroup.NonReproducingIndividuals.Count}" );
				Logger.Debug ( $"Foregone Quality = {ForegoneQuality}" );
				Logger.Debug ( $"Foregone Fitness = {ForegoneFitness}" );
			}
		}

		public virtual void CalculateFitness ( )
		{
			TotalFitness = StrategyCollection.Fitness.Calculate ( this );
		}

		public virtual void ReproduceAndKill ( )
		{
			var victim = StrategyCollection.Perish.Choose ( this );
			var parent = StrategyCollection.Reproduction.Choose ( this );

			var child = parent.Reproduce ( ++LastIds[parent.Type], V.SdQuality );
			Population.Swap ( victim, child );

			History.Add ( ( parent, child ) );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nReproduce And Kill\n" );
				Logger.Debug ( $"{parent} ->\n\t {child}" );
				Logger.Debug ( $"Chosen victim: {victim}" );
			}
		}

		public virtual void Normalize ( )
		{
			Population.Normalize ( );

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nNormalization\n" );
				Logger.Debug ( $"Quality Sum = {AllGroups.Sum ( x => x.QualitySum )}" );
				Logger.Debug ( CooperatorGroup.ToTable ( ) );
				Logger.Debug ( DefectorGroup.ToTable ( ) );
			}
		}

		public virtual bool SimulateGeneration ( )
		{
			SplitCooperators ( );
			CalculateFitness ( );
			ReproduceAndKill ( );
			Normalize ( );
			AddGenerationHistory ( );

			return AllGroups.Any ( x => x.Count == V.PopulationSize );
		}

		public virtual void Run ( )
		{
			if ( IsLoggingEnabled )
				Logger.Debug ( $"{this}" );

			CreateInitialPopulation ( );
			AddGenerationHistory ( );

			for ( TimeStepsPassed = 0; TimeStepsPassed < V.MaxTimeSteps; ++TimeStepsPassed )
			{
				if ( IsLoggingEnabled )
					Logger.Debug ( $"\nTime Step #{TimeStepsPassed + 1}\n\n" );
				if ( SimulateGeneration ( ) )
					break;
			}

			for ( var i = TimeStepsPassed + 1; i < V.MaxTimeSteps; ++i )
				AddGenerationHistory ( );

			CalculateHeritability ( );
			CalculateWinner ( );

			if ( IsLoggingEnabled )
				Logger.Debug ( $"\n\nWinner: {Winner}" );
		}

		protected void AddGenerationHistory ( )
		{
			foreach ( var group in AllGroups )
				GenerationHistory[group.Type].Add ( group.Count );
		}

		protected virtual void CalculateWinner ( )
		{
			AllGroups = AllGroups.OrderByDescending ( x => x.Individuals.Count ).ToList ( );
			if ( AllGroups[0].Count > AllGroups[1].Count )
			{
				switch ( AllGroups[0].Type )
				{
					case IndividualType.Cooperator:
						Winner = Winner.Cooperator;
						break;
					case IndividualType.Defector:
						Winner = Winner.Defector;
						break;
					default:
						throw new ArgumentOutOfRangeException ( );
				}

				if ( AllGroups[0].Count == V.PopulationSize )
					Winner |= Winner.Fix;
			}
			else
			{
				Winner = Winner.Tie;
			}
		}

		public virtual void CalculateHeritability ( )
		{
			if ( TimeStepsPassed <= 2 )
				return;

			var groups = History
				.GroupBy ( x => x.parent )
				.Where ( x => x.Any ( ) )
				.Select ( x => ( parent: x.Key, offsprings: x.Select ( y => y.offspring ).ToList ( ) ) )
				.ToList ( );

			var covQuality = groups.Select ( x => x.parent.Quality )
				.PopulationCovariance ( groups.Select ( x => x.offsprings.Average ( y => y.Quality ) ) );

			var varQuality = groups
				.Select ( x => x.parent.Quality )
				.PopulationVariance ( );

			groups = History
				.Take ( History.Count - 1 )
				.GroupBy ( x => x.parent )
				.Where ( x => x.Any ( ) )
				.Select ( x => ( parent: x.Key, offsprings: x.Select ( y => y.offspring ).ToList ( ) ) )
				.ToList ( );
			var covReproduction = groups.Select ( x => (double) x.parent.OffspringCount )
				.PopulationCovariance ( groups.Select ( x => x.offsprings.Average ( y => y.OffspringCount ) ) );
			var varReproduction = groups
				.Select ( x => x.parent.OffspringCount * 1d )
				.PopulationVariance ( );

			Heritability = new HeritabilitySummary
			{
				Quality                = covQuality / varQuality,
				VarianceQuality        = varQuality,
				CovarianceQuality      = covQuality,
				Reproduction           = covReproduction / varReproduction,
				VarianceReproduction   = varReproduction,
				CovarianceReproduction = covReproduction
			};

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nHeritability Calculations: \n" );
				var generations = History
					.GroupBy ( x => x.parent )
					.Select ( x => ( parent: x.Key,
					                 offsprings: x.Select ( y => y.offspring )
						                 .ToList ( ) ) )
					.Batch ( V.PopulationSize )
					.Select ( x => x.ToList ( ) )
					.ToList ( );
				for ( var i = 0; i < generations.Count; i++ )
				{
					var generation = generations[i];
					Logger.Debug ( $"\n\nTime Step#{i + 1}\n" );
					foreach ( var (parent, offsprings) in generation )
					{
						Logger.Debug ( $"{parent} -> {offsprings.Count} Offsprings:" );
						foreach ( var offspring in offsprings )
							Logger.Debug ( $"\t{offspring} OffspringCount: {offspring.OffspringCount}" );
					}
				}

				Logger.Debug ( $"\n{Heritability}" );
			}
		}

		public override string ToString ( ) =>
			$"{GetType ( ).Name} with {StrategyCollection}";
	}
}