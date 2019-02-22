using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EvoBio4.Core.Enums;
using EvoBio4.Core.Extensions;
using EvoBio4.Core.Interfaces;
using NLog;

namespace EvoBio4.Core
{
	public abstract class SingleIterationBase<TIndividual, TPopulation, TVariables> :
		ISingleIteration<TIndividual, TVariables, TPopulation>
		where TIndividual : class, IIndividual, new ( )
		where TPopulation : class, IPopulation<TIndividual, TVariables>, new ( )
		where TVariables : IVariables
	{
		// ReSharper disable once StaticMemberInGenericType
		protected static readonly Logger Logger = LogManager.GetCurrentClassLogger ( );
		public bool IsLoggingEnabled { get; set; }

		public TPopulation Population { get; protected set; }

		public IList<TIndividual> AllIndividuals
		{
			get => Population.AllIndividuals;
			set => Population.AllIndividuals = value;
		}

		public ICooperatorGroup<TIndividual> CooperatorGroup
		{
			get => Population.CooperatorGroup;
			set => Population.CooperatorGroup = value;
		}

		public IDefectorGroup<TIndividual> DefectorGroup
		{
			get => Population.DefectorGroup;
			set => Population.DefectorGroup = value;
		}

		public List<IIndividualGroup<TIndividual>> AllGroups { get; protected set; }

		public TVariables V { get; protected set; }

		public double ForegoneQuality { get; protected set; }
		public double ForegoneFitness { get; protected set; }
		public double TotalFitness { get; protected set; }
		public IHeritabilitySummary Heritability { get; set; }
		public Winner Winner { get; set; }
		public int TimeStepsPassed { get; protected set; }
		public IDeathSelectionRule<TIndividual, TVariables, TPopulation> DeathSelectionRule { get; protected set; }

		public IDictionary<IndividualType, List<int>> GenerationHistory { get; protected set; }

		protected readonly Dictionary<IndividualType, int> LastIds = new Dictionary<IndividualType, int>
		{
			[IndividualType.Cooperator] = 0,
			[IndividualType.Defector]   = 0
		};

		public List<(TIndividual parent, TIndividual offspring)> History;

		protected SingleIterationBase ( )
		{
		}

		protected SingleIterationBase (
			TVariables v,
			IDeathSelectionRule<TIndividual, TVariables, TPopulation> deathSelectionRule,
			bool isLoggingEnabled = false )
		{
			Init ( v, deathSelectionRule, isLoggingEnabled );
		}

		public void Init (
			TVariables v,
			IDeathSelectionRule<TIndividual, TVariables, TPopulation> deathSelectionRule,
			bool isLoggingEnabled = false )
		{
			DeathSelectionRule = deathSelectionRule;
			IsLoggingEnabled   = isLoggingEnabled;
			V                  = v;
			Population         = new TPopulation ( );
			History =
				new List<(TIndividual parent, TIndividual offspring)> ( V.MaxTimeSteps );

			GenerationHistory = new Dictionary<IndividualType, List<int>> ( );
			foreach ( var type in EnumsNET.Enums.GetValues<IndividualType> ( ) )
				GenerationHistory[type] = new List<int> ( V.MaxTimeSteps );

			if ( IsLoggingEnabled )
				Logger.Debug ( $"\n\n{V}\n" );
		}

		public virtual void CreateInitialPopulation ( )
		{
			Population.Init ( V );
			AllGroups = new List<IIndividualGroup<TIndividual>>
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

		[SuppressMessage ( "ReSharper", "InconsistentNaming" )]
		public virtual void CalculateFitness ( )
		{
			var S = DefectorGroup.QualitySum;
			var Z = CooperatorGroup.ReproducingQualitySum;
			var r = V.Relatedness;
			TotalFitness = 0;

			foreach ( var individual in CooperatorGroup )
			{
				var j = individual.Quality;
				individual.Fitness = j * (
					                     1d +
					                     r * ForegoneFitness / Z +
					                     ( 1d - r ) * ForegoneFitness / ( Z + S )
				                     );
				TotalFitness += individual.Fitness;
			}

			foreach ( var individual in DefectorGroup )
			{
				var k = individual.Quality;
				individual.Fitness = k * (
					                     1d +
					                     ( 1d - r ) * ForegoneFitness / ( Z + S )
				                     );
				TotalFitness += individual.Fitness;
			}

			if ( IsLoggingEnabled )
			{
				Logger.Debug ( "\n\nCalculating Fitness:\n" );

				Logger.Debug ( CooperatorGroup.ToTable ( ) );
				Logger.Debug ( DefectorGroup.ToTable ( ) );
			}
		}

		public virtual void ReproduceAndKill ( )
		{
			var victim = DeathSelectionRule.ChooseFrom ( Population );
			var parent = GetParent ( );

			var child = parent.Reproduce ( ++LastIds[parent.Type], V.SdQuality ) as TIndividual;
			Population.Add ( child );
			Population.Remove ( victim );

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
				foreach ( var individual in AllIndividuals )
					Logger.Debug ( individual );
			}
		}

		public abstract void CalculateHeritability ( );

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

			CalculateHeritability ( );
			CalculateWinner ( );

			if ( IsLoggingEnabled )
				Logger.Debug ( $"\n\nWinner: {Winner}" );
		}

		protected abstract TIndividual GetParent ( );

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

		public override string ToString ( ) =>
			$"{GetType ( ).Name} with {DeathSelectionRule.GetType ( ).Name}";
	}
}