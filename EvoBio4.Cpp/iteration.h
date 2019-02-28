#pragma once
#include <utility>
#include "population.h"
#include "strategy_collection.h"

class iteration
{
public:
	iteration (variables v, strategy_collection strategy_collection);

	population population;
	variables v;
	winner winner;
	int time_steps_passed;
	strategy_collection strategy_collection;
	unsigned last_ids[2] = {0};

	double foregone_quality = 0;
	double foregone_fitness = 0;
	double total_fitness = 0;

	~iteration () = default;

	void create_initial_population ();
	void split_cooperators ();
	void calculate_fitness ();
	void reproduce_kill ();
	void normalize ();
	bool simulate_time_step ();
	void run ();
	void calculate_winner ();
};
