using System.Collections.Generic;
using System.Diagnostics;

namespace EvoBio4.Core.Extensions
{
	public static class DictionaryExtensions
	{
		[DebuggerStepThrough]
		public static void Increment<T> ( this IDictionary<T, int> dictionary,
		                                  T key )
		{
			dictionary.TryGetValue ( key, out var count );
			dictionary[key] = count + 1;
		}
	}
}