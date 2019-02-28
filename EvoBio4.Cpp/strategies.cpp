#include "iteration.h"
#include "strategy_collection.h"

using namespace std;

namespace strategies
{
	namespace fitness
	{
		double default_fitness (iteration& it)
		{
			auto s = it.population.defector_group.quality;
			auto z = it.population.cooperator_group.quality_of.reproducing;
			auto r = it.v.relatedness;

			double total_fitness = 0;

			for (auto& individual : it.population.cooperator_group.individuals)
			{
				auto j = individual->quality;
				individual->fitness = j *
				                      (
					                      1. +
					                      r * it.foregone_fitness / z +
					                      (1. - r) * it.foregone_fitness / (z + s)
				                      );
				total_fitness += individual->fitness;
			}

			for (auto& individual : it.population.defector_group.individuals)
			{
				auto k = individual->quality;
				individual->fitness = k *
				                      (
					                      1. +
					                      (1. - r) * it.foregone_fitness / (z + s)
				                      );
				total_fitness += individual->fitness;
			}

			return total_fitness;
		}

		double non_reproducing_have_0_fitness (iteration& it)
		{
			auto s = it.population.defector_group.quality;
			auto z = it.population.cooperator_group.quality_of.reproducing;
			auto r = it.v.relatedness;

			double total_fitness = 0;

			for (auto&& individual : it.population.cooperator_group.non_reproducing)
				individual->fitness = 0;

			for (auto&& individual : it.population.cooperator_group.reproducing)
			{
				auto j = individual->quality;
				individual->fitness = j *
				                      (
					                      1. +
					                      r * it.foregone_fitness / z +
					                      (1. - r) * it.foregone_fitness / (z + s)
				                      );
				total_fitness += individual->fitness;
			}

			for (auto&& individual : it.population.defector_group.individuals)
			{
				auto k = individual->quality;
				individual->fitness = k *
				                      (
					                      1. +
					                      (1. - r) * it.foregone_fitness / (z + s)
				                      );
				total_fitness += individual->fitness;
			}

			return total_fitness;
		}
	}

	namespace reproduction
	{
		std::shared_ptr<individual> fitness_proportional_reproduction (iteration& it)
		{
			vector<std::shared_ptr<individual>> individuals {
				it.population.cooperator_group.reproducing.begin (),
				it.population.cooperator_group.reproducing.end ()
			};
			individuals.insert (individuals.end (),
			                    it.population.defector_group.individuals.begin (),
			                    it.population.defector_group.individuals.end ());

			return utility::choose_one_by (
				individuals,
				[] (const std::shared_ptr<individual>& ind) { return ind->fitness; }
			);
		}
	}

	namespace survival
	{
		std::shared_ptr<individual> equi_probable_survival (iteration& it)
		{
			auto index = utility::next_int (it.population.individuals.size ());

			return it.population.individuals[index];
		}

		std::shared_ptr<individual> quality_proportional_survival (iteration& it)
		{
			return utility::choose_all_but_one_by (
				it.population.individuals,
				[] (const std::shared_ptr<individual> ind) { return ind->quality; }
			);
		}

		std::shared_ptr<individual> fitness_proportional_survival (iteration& it)
		{
			return utility::choose_all_but_one_by (
				it.population.individuals,
				[] (const std::shared_ptr<individual> ind) { return ind->fitness; }
			);
		}

		std::shared_ptr<individual> quality_inversely_proportional_survival (iteration& it)
		{
			return utility::choose_one_by (
				it.population.individuals,
				[] (const std::shared_ptr<individual> ind) { return 1. / ind->quality; }
			);
		}

		std::shared_ptr<individual> fitness_inversely_proportional_survival (iteration& it)
		{
			return utility::choose_one_by (
				it.population.individuals,
				[] (const std::shared_ptr<individual> ind) { return 1. / ind->fitness; }
			);
		}
	}
}
