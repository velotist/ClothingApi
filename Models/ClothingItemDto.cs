using System;
using System.Drawing;

namespace ClothingApi.Models
{
    public class ClothingItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Season { get; set; }
        public Color? Color { get; set; }
    }
}
