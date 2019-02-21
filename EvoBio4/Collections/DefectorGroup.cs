﻿using EvoBio4.Core.Enums;
using EvoBio4.Core.Interfaces;

namespace EvoBio4.Collections
{
	public class DefectorGroup : IndividualGroupBase, IDefectorGroup<Individual>
	{
		public DefectorGroup ( ) :
			base ( IndividualType.Defector )
		{
		}
	}
}