#include "iteration.h"
#include <utility>

using namespace std;

iteration::iteration (variables v, ::strategy_collection strategy_collection) :
	v (v),
	winner (TIE),
	time_steps_passed (0),
	strategy_collection (std::move (strategy_collection)) {}

void iteration::create_initial_population ()
{
	population.init (v);

	last_ids[IT_COOPERATOR] = population.cooperator_group.individuals.size ();
	last_ids[IT_DEFECTOR] = population.defector_group.individuals.size ();
}

void iteration::split_cooperators ()
{
	int index = static_cast<int> ((population.individuals.size () - 1) * v.percentile_cutoff / 100);
	vector<std::shared_ptr<individual>> sorted (population.individuals.begin (), population.individuals.end ());
	nth_element (
		sorted.begin (),
		sorted.begin () + index,
		sorted.end (),
		[] (const std::shared_ptr<individual> a, const std::shared_ptr<individual> b)
		{
			return a->quality < b->quality;
		}
	);
	auto& threshold = sorted[index];
	population.cooperator_group.split (threshold->quality);

	foregone_quality = population.cooperator_group.quality_of.non_reproducing;
	foregone_fitness = v.y * foregone_quality;
}

void iteration::calculate_fitness ()
{
	total_fitness = strategy_collection.fitness (*this);
}

void iteration::reproduce_kill ()
{
	auto victim = strategy_collection.survival (*this);
	auto parent = strategy_collection.reproduction (*this);

	auto q = utility::next_gaussian_non_negative (parent->quality, v.sd_quality);
	auto child = parent->reproduce (++last_ids[parent->type], q);
	population.swap (victim, child);
}

void iteration::normalize ()
{
	population.normalize ();
}

bool iteration::simulate_time_step ()
{
	split_cooperators ();
	calculate_fitness ();
	reproduce_kill ();
	normalize ();

	return population.cooperator_group.individuals.empty () ||
	       population.defector_group.individuals.empty ();
}

void iteration::run ()
{
	create_initial_population ();

	for (time_steps_passed = 0; time_steps_passed < v.max_time_steps; ++time_steps_passed)
		if (simulate_time_step ())
			break;

	calculate_winner ();
}

void iteration::calculate_winner ()
{
	if (population.cooperator_group.individuals.size () > population.defector_group.individuals.size ())
		winner = COOPERATOR;
	else if (population.cooperator_group.individuals.size () < population.defector_group.individuals.size ())
		winner = DEFECTOR;
	else winner = TIE;

	if (population.cooperator_group.individuals.empty () ||
	    population.defector_group.individuals.empty ())
		winner |= FIX;
}
