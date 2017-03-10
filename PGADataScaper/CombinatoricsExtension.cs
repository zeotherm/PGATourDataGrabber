using System.Collections.Generic;
using System.Linq;

namespace PGADataScaper
{
	public static class CombinatoricsExtension
	{
		public static IEnumerable<IEnumerable<T>> CombinationsTakeN<T>(this IEnumerable<T> elements, int k)
		{
			return k == 0 ? new[] { new T[0] } :
			  elements.SelectMany((e, i) =>
				elements.Skip(i + 1).CombinationsTakeN(k - 1).Select(c => (new[] { e }).Concat(c)));
		}
	}
}
