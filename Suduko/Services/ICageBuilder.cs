using Suduko.Models;

namespace Suduko.Services
{
    public interface ICageBuilder
    {
        Board _board { get; set; }

        List<Cage> Build(Board board);
    }
}