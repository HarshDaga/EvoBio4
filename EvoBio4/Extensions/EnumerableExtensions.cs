using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tababular;

namespace EvoBio4.Extensions
{
	public static class EnumerableExtensions
	{
		public static (List<T> chosen, List<T> rejected) ChooseBy<T> ( this IEnumerable<T> allIndividuals,
		                                                               int amount,
		                                                               Func<T, double> selector )
		{
			var backup = allIndividuals.ToList ( );
			var cumulative = backup
				.Select ( selector )
				.CumulativeSum ( )
				.ToList ( );
			var total = cumulative.Last ( );

			var chosenIndividuals = new List<T> ( amount );

			for ( var i = 0; i < amount; i++ )
			{
				var target = Utility.NextDouble * total;
				var index = cumulative.BinarySearch ( target );
				if ( index < 0 )
					index = Math.Min ( ~index, backup.Count - 1 );

				var chosen = backup[index];
				chosenIndividuals.Add ( chosen );
				backup.RemoveAt ( index );
				cumulative.RemoveAt ( index );

				for ( var j = index; j < cumulative.Count; j++ )
					cumulative[j] -= selector ( chosen );
				total -= selector ( chosen );
			}

			return ( chosenIndividuals, backup );
		}

		public static T ChooseOneBy1Pass<T> ( this IEnumerable<T> allIndividuals,
		                                      Func<T, double> selector )
		{
			var cumulative = allIndividuals
				.CumulativeSum ( selector )
				.ToList ( );
			var total = cumulative.Last ( );

			var target = Utility.NextDouble * total.sum;

			int Comparison ( (double sum, T) x,
			                 (double sum, T) y ) =>
				x.sum.CompareTo ( y.sum );

			var index = cumulative.BinarySearch ( ( target, default ),
			                                      Comparer<(double, T)>.Create ( Comparison )
			);
			if ( index < 0 )
				index = Math.Min ( ~index, cumulative.Count - 1 );

			return cumulative[index].item;
		}

		public static T ChooseOneBy<T> ( this IEnumerable<T> enumerable,
		                                 Func<T, double> selector )
		{
			var backup = enumerable.ToList ( );
			var cumulative = backup
				.Select ( selector )
				.CumulativeSum ( )
				.ToList ( );
			var total = cumulative.Last ( );

			var target = Utility.NextDouble * total;
			var index = cumulative.BinarySearch ( target );
			if ( index < 0 )
				index = Math.Min ( ~index, backup.Count - 1 );

			return backup[index];
		}

		public static T ChooseOneBy<T> ( this IEnumerable<T> enumerable,
		                                 Func<T, double> selector,
		                                 double sum )
		{
			var cumulative = 0d;
			var target = Utility.NextDouble * sum;
			T last = default;
			foreach ( var individual in enumerable )
			{
				cumulative += selector ( individual );
				if ( cumulative >= target )
					return individual;
				last = individual;
			}

			return last;
		}

		public static T ChooseAllButOneBy<T> ( this IEnumerable<T> enumerable,
		                                       Func<T, double> selector )
		{
			var list = enumerable.ToList ( );
			var n = list.Count;
			var p = new List<double> ( list.Select ( selector ) );

			var remaining = 0d;
			foreach ( var item in list )
			{
				var pCur = selector ( item );
				if ( pCur == 0 )
				{
					var zeroes = list.Where ( x => selector ( x ) == 0 ).ToList ( );
					var i = Utility.Srs.Next ( zeroes.Count );
					return zeroes[i];
				}

				remaining += pCur;
			}

			var remainingIndex = n * ( n - 1 ) / 2;
			var randoms = Utility.Srs.NextDoubles ( n - 1 );

			for ( var i = 0; i < n - 1; i++ )
			{
				var target = randoms[i] * remaining;
				var index = 0;
				var sum = p[0];
				while ( sum <= target )
					sum += p[++index];

				remaining      -= p[index];
				p[index]       =  0;
				remainingIndex -= index;
			}

			return list[remainingIndex];
		}

		[DebuggerStepThrough]
		public static IEnumerable<(double sum, T item)> CumulativeSum<T> ( this IEnumerable<T> sequence,
		                                                                   Func<T, double> selector )
		{
			var sum = 0d;
			foreach ( var item in sequence )
			{
				sum += selector ( item );
				yield return ( sum, item );
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<double> CumulativeSum ( this IEnumerable<double> sequence )
		{
			var sum = 0d;
			foreach ( var item in sequence )
			{
				sum += item;
				yield return sum;
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<int> CumulativeSum ( this IEnumerable<int> sequence )
		{
			var sum = 0;
			foreach ( var item in sequence )
			{
				sum += item;
				yield return sum;
			}
		}

		public static string ToTable<T> ( this IEnumerable<T> enumerable,
		                                  Func<T, object> selector,
		                                  int maxWidth = 80 )
		{
			var hints = new Hints {MaxTableWidth = maxWidth};
			var formatter = new TableFormatter ( hints );

			return formatter.FormatObjects ( enumerable.Select ( selector ) );
		}

		public static string Join<T> ( this IEnumerable<T> enumerable,
		                               string separator ) =>
			string.Join ( separator, enumerable );
	}
}