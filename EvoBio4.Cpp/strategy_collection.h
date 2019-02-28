#pragma once
#include "individual.h"
#include <functional>

class iteration;

class strategy_collection
{
public:
	strategy_collection () = default;

	std::function<double  (iteration&)> fitness;
	std::function<std::shared_ptr<individual>  (iteration&)> reproduction;
	std::function<std::shared_ptr<individual>  (iteration&)> survival;
};

namespace strategies
{
	namespace fitness
	{
		double default_fitness (iteration& it);
		double non_reproducing_have_0_fitness (iteration& it);
	}

	namespace reproduction
	{
		std::shared_ptr<individual> fitness_proportional_reproduction (iteration& it);
	}

	namespace survival
	{
		std::shared_ptr<individual> equi_probable_survival (iteration& it);
		std::shared_ptr<individual> quality_proportional_survival (iteration& it);
		std::shared_ptr<individual> fitness_proportional_survival (iteration& it);
		std::shared_ptr<individual> quality_inversely_proportional_survival (iteration& it);
		std::shared_ptr<individual> fitness_inversely_proportional_survival (iteration& it);
	}
}
