#pragma once
#include <utility>
#include "variables.h"
#include "strategy_collection.h"
#include <map>

class simulation
{
public:
	variables v;
	strategy_collection strategy_collection;
	std::map<winner, int> wins {};

	simulation (const variables& v, ::strategy_collection strategy_collection)
		: v (v),
		  strategy_collection (std::move (strategy_collection)) {}

	~simulation () = default;

	void run ();
};
