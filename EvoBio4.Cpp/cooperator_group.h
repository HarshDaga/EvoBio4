#pragma once
#include "individual_group.h"

struct cooperator_group :
		individual_group
{
	std::vector<std::shared_ptr<individual>> reproducing {};
	std::vector<std::shared_ptr<individual>> non_reproducing {};

	struct quality_t
	{
		double reproducing = 0;
		double non_reproducing = 0;
	} quality_of {0};

	cooperator_group ();
	~cooperator_group ();

	void split (double threshold);
};
