#pragma once
#include "individual.h"
#include "utility.h"
#include <ostream>

struct individual_group
{
	friend std::ostream& operator<< (std::ostream& os, const individual_group& obj)
	{
		for (auto&& individual : obj.individuals)
			os << individual;
		return os;
	}

	individual_type type;
	double quality = 0;
	double fitness = 0;
	std::vector<std::shared_ptr<individual>> individuals {};

	explicit individual_group (individual_type type);
	~individual_group ();

	void add (const std::shared_ptr<individual>& ind);

	void remove (const std::shared_ptr<individual>& ind);

	void populate (int count, double sd);

	void normalize (double sum, int size);
};
