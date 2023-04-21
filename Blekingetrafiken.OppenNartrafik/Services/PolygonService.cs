using Blekingetrafiken.OppenNartrafik.Models;
using System.Text.RegularExpressions;
using System.Xml;

namespace Blekingetrafiken.OppenNartrafik.Services
{
    public class PolygonService : IPolygonService
    {
        private readonly ILogger _logger;
        private readonly XmlDocument _kmlDocument;

        public PolygonService(ILogger logger, XmlDocument kmlDocument)
        {
            _logger = logger;
            _kmlDocument = kmlDocument;
        }

        public bool IsPointInsidePolygons(Coordinate point)
        {
            var polygons = GetPolygons();
            var count = polygons.Where(polygon => IsPointInsidePolygon(point, polygon));
            return count.Count() > 0;
        }

        private IEnumerable<Polygon> GetPolygons()
        {
            var polygonNodes = GetNodes(_kmlDocument, "//kml:Polygon");
            return polygonNodes.Cast<XmlNode>().Select(GetPolygon);
        }

        private Polygon GetPolygon(XmlNode node)
        {
            XmlNodeList? coordinatesNodes = GetNodes(node, "kml:outerBoundaryIs/kml:LinearRing/kml:coordinates");
            var polygon = new Polygon();
            var polygonPoints = new List<Coordinate>();
            foreach (XmlNode coordinatesNode in coordinatesNodes)
            {
                string input = coordinatesNode.InnerText;
                string pattern = @"[\t\r\n\s]+";
                string replacement = " ";
                string output = Regex.Replace(input, pattern, replacement);

                string[] coordinatesArray = output.Split(' ');
                foreach (string coordinate in coordinatesArray)
                {
                    var coord = coordinate.Trim();
                    string[] latLon = coord.Split(',');
                    if (latLon.Count() > 1)
                    {
                        var lon = latLon[0];
                        var lat = latLon[1];
                        polygonPoints.Add(new Coordinate(lat, lon));
                    }
                }
            }
            polygon.Coordinates = polygonPoints;
            return polygon;
        }

        private XmlNodeList? GetNodes(XmlNode node, string selector)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(_kmlDocument.NameTable);
            nsmgr.AddNamespace("kml", "http://www.opengis.net/kml/2.2");
            return node.SelectNodes(selector, nsmgr);
        }

        public static bool IsPointInsidePolygon(Coordinate point, Polygon polygon)
        {
            if (polygon.Coordinates.Count < 3)
            {
                throw new ArgumentException("Polygon must have at least three vertices.");
            }

            int intersectionCount = 0;
            float x1, x2, y1, y2;

            for (int i = 0; i < polygon.Coordinates.Count; i++)
            {
                x1 = (float)polygon.Coordinates[i].Longitude;
                y1 = (float)polygon.Coordinates[i].Latitude;

                int nextIndex = (i + 1) % polygon.Coordinates.Count;
                x2 = (float)polygon.Coordinates[nextIndex].Longitude;
                y2 = (float)polygon.Coordinates[nextIndex].Latitude;

                if (((y1 > point.Latitude) != (y2 > point.Latitude)) && (point.Longitude < (x2 - x1) * (point.Latitude - y1) / (y2 - y1) + x1))
                {
                    intersectionCount++;
                }
            }

            return (intersectionCount % 2 == 1);
        }
    }
}
