using Model.Interfaces;

namespace Models
{
    public class Dimensions : IDimensions
    {
        public Dimensions()
        {
        }

        public Dimensions(decimal length, decimal width, decimal height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        public decimal Length { get; }
        public decimal Width { get; }
        public decimal Height { get; }
    }
}