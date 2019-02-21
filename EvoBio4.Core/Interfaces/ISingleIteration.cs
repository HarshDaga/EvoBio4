using System.Collections.Generic;
using EvoBio4.Core.Enums;

namespace EvoBio4.Core.Interfaces
{
	public interface ISingleIteration<TIndividual, TVariables, TPopulation>
		where TIndividual : class, IIndividual, new ( )
		where TVariables : IVariables
		where TPopulation : IPopulation<TIndividual, TVariables>
	{
		bool IsLoggingEnabled { get; set; }
		TVariables V { get; }
		TPopulation Population { get; }
		IList<TIndividual> AllIndividuals { get; }
		IHeritabilitySummary Heritability { get; }
		Winner Winner { get; }
		int TimeStepsPassed { get; }

		IDeathSelectionRule<TIndividual, TVariables, TPopulation> DeathSelectionRule { get; }

		IDictionary<IndividualType, List<int>> GenerationHistory { get; }

		void Init (
			TVariables v,
			IDeathSelectionRule<TIndividual, TVariables, TPopulation> deathSelectionRule,
			bool isLoggingEnabled = false );

		void CreateInitialPopulation ( );
		void SplitCooperators ( );
		void CalculateFitness ( );
		void ReproduceAndKill ( );
		void Normalize ( );
		void CalculateHeritability ( );

		bool SimulateGeneration ( );
		void Run ( );
	}
}