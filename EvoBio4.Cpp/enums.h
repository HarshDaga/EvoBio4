#pragma once
#include <type_traits>

enum winner
{
	COOPERATOR = 1 << 0,
	DEFECTOR = 1 << 1,
	FIX = 1 << 4,
	TIE = 1 << 5
};

enum individual_type
{
	IT_COOPERATOR,
	IT_DEFECTOR
};

inline winner operator | (winner lhs, winner rhs)
{
	using underlying_t = std::underlying_type_t<winner>;
	return static_cast<winner> (static_cast<underlying_t> (lhs) | static_cast<underlying_t> (rhs));
}

inline winner& operator |= (winner& lhs, winner rhs)
{
	lhs = lhs | rhs;
	return lhs;
}
