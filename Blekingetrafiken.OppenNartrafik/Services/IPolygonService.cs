using Blekingetrafiken.OppenNartrafik.Models;

namespace Blekingetrafiken.OppenNartrafik.Services
{
    public interface IPolygonService
    {
        bool IsPointInsidePolygons(Coordinate point);
    }
}