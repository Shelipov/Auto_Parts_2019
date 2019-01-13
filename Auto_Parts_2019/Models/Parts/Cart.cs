using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Auto_Parts_2019.Models.Parts
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Part part, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Part.ID == part.ID)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Part = part,
                    Quantity = quantity,
                    DateLastModified=DateTime.Now
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Part part)
        {
            lineCollection.RemoveAll(l => l.Part.ID == part.ID);
        }

        public double ComputeTotalValue()
        {
            
            return lineCollection.Sum(e => e.Part.Price * e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        [Key]
        public int ID { get; set; }
        public Part Part { get; set; }
        public int Quantity { get; set; }
        public DateTime DateLastModified { get; set; }
        
    }
}
