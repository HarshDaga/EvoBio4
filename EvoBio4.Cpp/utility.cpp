#include "utility.h"
#include <chrono>

using namespace std;
using namespace chrono;

random_device utility::rng;
default_random_engine utility::generator {
	static_cast<unsigned> (system_clock::now ().time_since_epoch ().count ())
};
const uniform_real_distribution<double> utility::REAL_DISTRIBUTION_DOUBLE {0., 1.};
const uniform_real_distribution<float> utility::REAL_DISTRIBUTION_FLOAT {0.f, 1.f};


double utility::next_double ()
{
	return REAL_DISTRIBUTION_DOUBLE (rng);
}

double utility::next_float ()
{
	return REAL_DISTRIBUTION_FLOAT (rng);
}

int utility::next_int (int max)
{
	return rng () % max;
}

double utility::next_gaussian (double mean, double sd)
{
	auto dist = normal_distribution<double> {mean, sd};
	return dist (generator);
}

double utility::next_gaussian_non_negative (double mean, double sd)
{
	auto dist = normal_distribution<double> {mean, sd};
	return max (0., dist (generator));
}

vector<double> utility::next_gaussian_non_negative (double mean, double sd, int n)
{
	vector<double> result;
	result.reserve (n);
	auto dist = normal_distribution<double> {mean, sd};
	for (int i = 0; i < n; ++i)
		result.push_back (max (0., dist (generator)));

	return result;
}
