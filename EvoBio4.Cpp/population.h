#pragma once
#include "individual_group.h"
#include "cooperator_group.h"
#include "variables.h"

struct population
{
	population (const population& other) = default;
	population (population&& other) noexcept = default;
	population& operator= (const population& other) = default;
	population& operator= (population&& other) noexcept = default;

	cooperator_group cooperator_group {};
	individual_group defector_group {IT_DEFECTOR};
	std::vector<std::shared_ptr<individual>> individuals {};

	population () = default;
	~population () = default;

	void init (const variables& v);

	void add (std::shared_ptr<individual> ind);

	void remove (const std::shared_ptr<individual>& ind);

	void swap (const std::shared_ptr<individual>& remove, const std::shared_ptr<individual>& add) noexcept;

	void normalize ();
};
