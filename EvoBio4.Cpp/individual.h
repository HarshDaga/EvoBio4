#pragma once
#include "enums.h"
#include <ostream>

class individual
{
public:
	friend std::ostream& operator<< (std::ostream& os, const individual& ind)
	{
		auto name = ind.type == IT_COOPERATOR ? "Cooperator" : "Defector";
		return os
		       << name << "_" << ind.id
		       << " Quality: " << ind.quality
		       << " Fitness: " << ind.fitness;
	}

	friend bool operator< (const individual& lhs, const individual& rhs)
	{
		if (lhs.id < rhs.id)
			return true;
		if (rhs.id < lhs.id)
			return false;
		return lhs.type < rhs.type;
	}

	friend bool operator<= (const individual& lhs, const individual& rhs) { return !(rhs < lhs); }

	friend bool operator> (const individual& lhs, const individual& rhs) { return rhs < lhs; }

	friend bool operator>= (const individual& lhs, const individual& rhs) { return !(lhs < rhs); }

	friend bool operator== (const individual& lhs, const individual& rhs)
	{
		return lhs.id == rhs.id
		       && lhs.type == rhs.type;
	}

	friend bool operator!= (const individual& lhs, const individual& rhs) { return !(lhs == rhs); }

	unsigned id;
	individual_type type;
	double quality = 0;
	double fitness = 0;
	int offspring_count = 0;

	individual () = default;
	individual (unsigned id, individual_type type);
	~individual ();

	std::shared_ptr<individual> reproduce (unsigned id, double quality);
	void normalize (double sum, int size);
};
