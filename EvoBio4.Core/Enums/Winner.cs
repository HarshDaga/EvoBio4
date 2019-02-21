using System;

namespace EvoBio4.Core.Enums
{
	[Flags]
	public enum Winner
	{
		Cooperator = 1 << 0,
		Defector = 1 << 1,
		Fix = 1 << 4,
		Tie = 1 << 5
	}
}