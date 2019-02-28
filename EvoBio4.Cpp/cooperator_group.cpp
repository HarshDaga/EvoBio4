#include "cooperator_group.h"


cooperator_group::cooperator_group () : individual_group (IT_COOPERATOR) {}

cooperator_group::~cooperator_group () = default;

void cooperator_group::split (double threshold)
{
	reproducing.clear ();
	non_reproducing.clear ();
	reproducing.reserve (individuals.size ());
	non_reproducing.reserve (individuals.size ());
	quality_of.reproducing = 0;
	quality_of.non_reproducing = 0;

	for (auto&& individual : individuals)
	{
		if (individual->quality < threshold)
		{
			non_reproducing.emplace_back (individual);
			quality_of.non_reproducing += individual->quality;
		}
		else
		{
			reproducing.emplace_back (individual);
			quality_of.reproducing += individual->quality;
		}
	}

	quality = quality_of.reproducing + quality_of.non_reproducing;
}
