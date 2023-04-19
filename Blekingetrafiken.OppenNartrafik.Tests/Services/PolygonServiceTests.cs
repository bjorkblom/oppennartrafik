using Blekingetrafiken.OppenNartrafik.Models;
using Blekingetrafiken.OppenNartrafik.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using System.Xml;

namespace Blekingetrafiken.OppenNartrafik.Tests.Services
{
    public class PolygonServiceTests
    {
        private Mock<ILogger> _logger;

        public PolygonServiceTests()
        {
            _logger = new Mock<ILogger>();
        }

        [Theory]
        [InlineData(99, 1, true)]
        [InlineData(77, 21, true)]
        //[InlineData(100, 30, true)]
        //[InlineData(40, 100, true)]
        [InlineData(101, 15, false)]
        [InlineData(50, 200, false)]
        [InlineData(40, 101, false)]
        [InlineData(-1, 15, false)]
        [InlineData(101, 101, false)]
        public void ShouldHandlePointsInTestDocument(double latitude, double longitude, bool expected)
        {
            var service = new PolygonService(_logger.Object, GetKmlDocument("Test.kml"));
            service.IsPointInsidePolygons(new Coordinate(latitude, longitude)).Should().Be(expected);
        }

        [Theory]
        [InlineData(56.1955252, 15.534434, true)]
        [InlineData(56.20474, 15.53206, true)]
        [InlineData(56.19379, 15.52767, true)]
        [InlineData(56.19383, 15.53692, true)]
        [InlineData(56.18644, 15.53476, false)]
        [InlineData(56.19079, 15.56372, false)]
        [InlineData(56.21329, 15.55866, false)]
        [InlineData(56.23832, 15.50719, false)]
        [InlineData(100.52524261728461, 15.53458, false)]
        public void ShouldHandlePointsInNattraby(double latitude, double longitude, bool expected)
        {
            var service = new PolygonService(_logger.Object, GetKmlDocument("Nattraby.kml"));
            service.IsPointInsidePolygons(new Coordinate(latitude, longitude)).Should().Be(expected);
        }

        [Theory]
        [InlineData(56.1934572, 15.5621167, false)]
        [InlineData(56.22214, 15.49804, false)]
        [InlineData(56.20492, 15.51598, false)]
        [InlineData(56.20521, 15.51083, false)]
        [InlineData(56.20325, 15.50748, false)]
        [InlineData(56.19761, 15.49898, false)]
        [InlineData(56.1926, 15.50121, false)]
        [InlineData(56.1884, 15.49838, false)]
        [InlineData(56.18548, 15.49675, false)]
        [InlineData(56.18286, 15.50284, false)]
        [InlineData(56.18615, 15.51417, false)]
        [InlineData(56.17994, 15.50705, false)]
        [InlineData(56.18677, 15.51864, false)]
        [InlineData(56.18935, 15.53821, false)]
        [InlineData(56.20311, 15.54216, false)]
        [InlineData(56.20721, 15.54224, false)]
        [InlineData(56.17156, 15.29373, false)]
        [InlineData(56.20683, 15.54087, true)]
        [InlineData(56.17975, 15.53117, true)]
        [InlineData(56.17856, 15.51023, true)]
        [InlineData(56.19227, 15.50345, true)]
        [InlineData(56.19733, 15.52096, true)]
        [InlineData(56.1799, 15.58155, true)]
        [InlineData(56.1713, 15.56421, true)]
        [InlineData(56.19112, 15.47066, true)]
        [InlineData(56.16915, 15.28526, true)]
        [InlineData(56.18157, 15.28975, true)]
        [InlineData(56.17911, 15.26752, true)]
        [InlineData(56.44696, 15.01774, true)]

        public void ShouldHandlePointsInOppenNartrafik(double latitude, double longitude, bool expected)
        {
            var service = new PolygonService(_logger.Object, GetKmlDocument("oppen-nartrafik.kml"));
            service.IsPointInsidePolygons(new Coordinate(latitude, longitude)).Should().Be(expected);
        }

        private XmlDocument GetKmlDocument(string name)
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(assemblyPath, $"data/{name}");

            var kmlDocument = new XmlDocument();
            kmlDocument.Load(filePath);
            return kmlDocument;
        }
    }
}
