#include "population.h"

using namespace std;

void population::init (const variables& v)
{
	cooperator_group.populate (v.cooperator_quantity, v.sd_quality);
	defector_group.populate (v.defector_quantity, v.sd_quality);

	individuals.reserve (v.cooperator_quantity + v.defector_quantity);

	copy (cooperator_group.individuals.begin (), cooperator_group.individuals.end (), back_inserter (individuals));
	copy (defector_group.individuals.begin (), defector_group.individuals.end (), back_inserter (individuals));
}

#define GET_GROUP(x) ((x)->type == IT_COOPERATOR ? cooperator_group : defector_group)

void population::add (std::shared_ptr<individual> ind)
{
	GET_GROUP (ind).add (ind);
	individuals.emplace_back (ind);
}

void population::remove (const std::shared_ptr<individual>& ind)
{
	GET_GROUP (ind).remove (ind);
	auto it = find (individuals.begin (), individuals.end (), ind);
	individuals.erase (it);
}

void population::swap (const std::shared_ptr<individual>& remove, const std::shared_ptr<individual>& add) noexcept
{
	GET_GROUP (remove).remove (remove);
	GET_GROUP (add).add (add);

	auto it = find (individuals.begin (), individuals.end (), remove);
	auto index = distance (individuals.begin (), it);

	individuals[index] = add;
}

#undef GET_GROUP

void population::normalize ()
{
	auto sum = cooperator_group.quality + defector_group.quality;
	auto n = individuals.size ();

	cooperator_group.normalize (sum, n);
	defector_group.normalize (sum, n);
}
