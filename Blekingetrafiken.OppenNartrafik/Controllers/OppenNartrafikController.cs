using Blekingetrafiken.OppenNartrafik.Models;
using Blekingetrafiken.OppenNartrafik.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blekingetrafiken.OppenNartrafik.Controllers
{
    [ApiController]
    [Route("api")]
    public class OppenNartrafikController : ControllerBase
    {
        private readonly ILogger<OppenNartrafikController> _logger;
        private readonly IPolygonService _polygonService;

        public OppenNartrafikController(ILogger<OppenNartrafikController> logger, IPolygonService polygonService)
        {
            _logger = logger;
            _polygonService = polygonService;
        }

        [Route("checklocation/{coordinates}")]
        [HttpGet]
        public bool CheckLocation(string coordinates)
        {
            if (!coordinates.Contains(',') && coordinates.Split(",").Count() != 2)
            {
                throw new BadHttpRequestException("Latitude and/or longitude was not provided");
            }
            var coordinate = coordinates.Trim().Split(",");
            return !_polygonService.IsPointInsidePolygons(new Coordinate(coordinate[0], coordinate[1]));
        }
    }
}