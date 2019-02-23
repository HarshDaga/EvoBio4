using System.Collections.Generic;
using EvoBio4.Core.Enums;

namespace EvoBio4.Core.Interfaces
{
	public delegate TIndividual PerishStrategyDelegate<out TIndividual, TVariables, TPopulation, in TIteration> (
		TIteration iteration )
		where TIndividual : class, IIndividual, new ( )
		where TVariables : IVariables
		where TPopulation : IPopulation<TIndividual, TVariables>
		where TIteration : IIteration<TIndividual, TVariables, TPopulation, TIteration>, new ( );

	public interface IIteration<TIndividual, TVariables, TPopulation, TIteration>
		where TIndividual : class, IIndividual, new ( )
		where TVariables : IVariables
		where TPopulation : IPopulation<TIndividual, TVariables>
		where TIteration : IIteration<TIndividual, TVariables, TPopulation, TIteration>, new ( )
	{
		bool IsLoggingEnabled { get; set; }
		TVariables V { get; }
		TPopulation Population { get; }
		IList<TIndividual> AllIndividuals { get; }
		IHeritabilitySummary Heritability { get; }
		Winner Winner { get; }
		int TimeStepsPassed { get; }

		PerishStrategyDelegate<TIndividual, TVariables, TPopulation, TIteration> PerishStrategy { get; }

		IDictionary<IndividualType, List<int>> GenerationHistory { get; }

		void Init ( TVariables v,
		            PerishStrategyDelegate<TIndividual, TVariables, TPopulation, TIteration> perishStrategy,
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