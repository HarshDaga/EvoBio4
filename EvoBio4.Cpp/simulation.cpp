#include "simulation.h"
#include "iteration.h"
#include <iostream>
#include <execution>

using namespace std;

void simulation::run ()
{
	wins.clear ();
	wins[COOPERATOR] = 0;
	wins[DEFECTOR] = 0;
	wins[COOPERATOR | FIX] = 0;
	wins[DEFECTOR | FIX] = 0;
	wins[TIE] = 0;

	vector<int> runs;
	for (int i = 0; i < v.runs; ++i)
		runs.push_back (i);
	int count = 0;
	mutex m;

	for_each (std::execution::par,
	          begin (runs),
	          end (runs),
	          [&] (int)
	          {
		          iteration it {v, strategy_collection};
		          it.run ();

		          m.lock ();
		          ++wins[it.winner];
		          ++count;
		          std::cout << "Finished: " << count << std::endl;
		          m.unlock ();
	          }
	);
}
