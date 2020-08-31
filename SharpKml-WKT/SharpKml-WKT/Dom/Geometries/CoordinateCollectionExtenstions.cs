using SharpKml.Base;
using SharpKml.Dom;
using System.Collections.Generic;
using System.Linq;

namespace SharpKml_WKT.Dom.Geometries
{
	/// <summary>
	/// Provides extension methods for <see cref="Polygon"/> objects.
	/// </summary>
	public static class CoordinateCollectionExtenstions
	{
		public static Vector[][] AsVectorCoordinates(this CoordinateCollection coordinateCollection)
		{
            List<List<Vector>> coordinates = new List<List<Vector>> {new List<Vector>()};
            coordinates[0].AddRange(coordinateCollection);
			return coordinates.Select(c => c.ToArray()).ToArray();
		}
	}
}
