#pragma once
#include <random>

namespace utility
{
	extern std::random_device rng;
	extern std::default_random_engine generator;
	const extern std::uniform_real_distribution<double> REAL_DISTRIBUTION_DOUBLE;
	const extern std::uniform_real_distribution<float> REAL_DISTRIBUTION_FLOAT;

	double next_double ();
	double next_float ();
	int next_int (int max);


	double next_gaussian (double mean,
	                      double sd);
	double next_gaussian_non_negative (double mean,
	                                   double sd);
	std::vector<double> next_gaussian_non_negative (double mean,
	                                                double sd,
	                                                int n);


	template <typename T, typename Func>
	T choose_one_of_value (std::vector<T>& vec, const Func& selector)
	{
		std::vector<T> values;
		std::copy_if (vec.begin (),
		              vec.end (),
		              std::back_inserter (values),
		              selector
		);

		int index = next_int (values.size ());
		return values[index];
	}

	template <typename T, typename Func>
	T choose_one_by (std::vector<T>& vec, const Func& selector, double sum)
	{
		double cumulative = 0.;
		auto target = next_double () * sum;
		for (auto& item : vec)
		{
			cumulative += selector (item);
			if (cumulative > target)
				return item;
		}

		if (vec.empty ())
			return {};
		return vec.back ();
	}

	template <typename T, typename Func>
	T choose_one_by (std::vector<T>& vec, const Func& selector)
	{
		double cumulative = 0.;
		double sum = 0.;

		for (auto& item : vec)
		{
			sum += selector (item);
			if (isinf (sum))
				return choose_one_of_value (vec, [&] (const T& elem) { return isinf ((selector (elem))); });
		}

		auto target = next_double () * sum;
		for (auto& item : vec)
		{
			cumulative += selector (item);
			if (cumulative > target)
				return item;
		}

		return vec.back ();
	}

	template <typename T, typename Func>
	T choose_all_but_one_by (std::vector<T>& vec, const Func& selector)
	{
		double remaining = 0.;
		auto n = vec.size ();
		std::vector<double> p;

		p.reserve (vec.size ());
		for (auto& item : vec)
		{
			auto x = selector (item);
			if (x == 0)
				return choose_one_of_value (vec, [&] (const T& elem) { return selector (elem) == 0; });

			p.push_back (x);
			remaining += x;
		}

		auto rem_index = n * (n - 1) / 2;
		for (auto i = 0u; i < n - 1; ++i)
		{
			auto target = next_double () * remaining;
			auto index = 0u;

			auto sum = p[0];
			while (sum <= target)
				sum += p[++index];

			remaining -= p[index];
			p[index] = 0;
			rem_index -= index;
		}

		return vec[rem_index];
	}
};
