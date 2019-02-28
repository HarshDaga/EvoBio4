#include "individual.h"


individual::individual (unsigned id,
                        individual_type type)
	: id (id),
	  type (type) {}

individual::~individual () = default;

std::shared_ptr<individual> individual::reproduce (unsigned id, double quality)
{
	++offspring_count;
	auto child = std::make_shared<individual> (id, type);
	child->quality = quality;

	return child;
}

void individual::normalize (double sum, int size)
{
	quality /= sum / 10.0 / size;
	fitness = 0;
}
