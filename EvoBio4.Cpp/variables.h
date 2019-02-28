#pragma once
#include <ostream>

using std::endl;

class variables
{
public:
	friend std::ostream& operator<< (std::ostream& os, const variables& v)
	{
		return os
		       << "cooperator_quantity: " << v.cooperator_quantity << endl
		       << "defector_quantity: " << v.defector_quantity << endl
		       << "sd_quality: " << v.sd_quality << endl
		       << "y: " << v.y << endl
		       << "relatedness: " << v.relatedness << endl
		       << "percentile_cutoff: " << v.percentile_cutoff << endl
		       << "z: " << v.z << endl
		       << "max_time_steps: " << v.max_time_steps << endl
		       << "runs: " << v.runs << endl;
	}

	variables (int cooperator_quantity,
	           int defector_quantity,
	           double sd_quality,
	           double y,
	           double relatedness,
	           double percentile_cutoff,
	           double z,
	           int max_time_steps,
	           int runs)
		: cooperator_quantity (cooperator_quantity),
		  defector_quantity (defector_quantity),
		  sd_quality (sd_quality),
		  y (y),
		  relatedness (relatedness),
		  percentile_cutoff (percentile_cutoff),
		  z (z),
		  max_time_steps (max_time_steps),
		  runs (runs) {}

	variables (const variables& other) = default;
	variables (variables&& other) noexcept = default;
	variables& operator= (const variables& other) = default;
	variables& operator= (variables&& other) noexcept = default;

	int cooperator_quantity = 100;
	int defector_quantity = 100;
	double sd_quality = 1;
	double y = 0.8;
	double relatedness = 0.1;
	double percentile_cutoff = 10;
	double z = 1.96;
	int max_time_steps = 10000;
	int runs = 1000;

	variables () = default;

	~variables () = default;
};
