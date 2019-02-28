#include "individual_group.h"


individual_group::individual_group (individual_type type): type (type) {}

individual_group::~individual_group () = default;

void individual_group::add (const std::shared_ptr<individual>& ind)
{
	quality += ind->quality;
	individuals.push_back (ind);
}

void individual_group::remove (const std::shared_ptr<individual>& ind)
{
	quality -= ind->quality;
	auto it = std::find (individuals.begin (), individuals.end (), ind);
	individuals.erase (it);
}

void individual_group::populate (int count, double sd)
{
	individuals.clear ();
	auto qualities = utility::next_gaussian_non_negative (10, sd, count);

	for (size_t i = 0; i != qualities.size (); ++i)
	{
		auto ind = std::make_shared<individual> (i + 1, type);
		ind->quality = qualities[i];
		add (ind);
	}
}

void individual_group::normalize (double sum, int size)
{
	quality = 0;
	for (auto& individual : individuals)
	{
		individual->normalize (sum, size);
		quality += individual->quality;
	}
}
