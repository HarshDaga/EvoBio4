#include "variables.h"
#include "strategy_collection.h"
#include "simulation.h"
#include <chrono>
#include <iostream>

using namespace strategies;
using namespace std;
using namespace chrono;

int main (int argc, char* argv[])
{
	variables v
	{
		100,
		100,
		1,
		0.8,
		0.1,
		10,
		1.96,
		15000,
		10000
	};

	strategy_collection strategy_collection;

	strategy_collection.fitness = fitness::non_reproducing_have_0_fitness;
	strategy_collection.reproduction = reproduction::fitness_proportional_reproduction;
	strategy_collection.survival = survival::fitness_inversely_proportional_survival;

	auto start = steady_clock::now ();

	simulation sim {v, strategy_collection};
	sim.run ();

	auto end = steady_clock::now ();
	auto diff = end - start;
	std::cout << std::chrono::duration<double, std::milli> (diff).count () << " ms" << endl;

	return 0;
}
