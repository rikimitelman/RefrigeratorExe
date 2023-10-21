using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefrigeratorExercise.Etities
{

    public class Shelf
    {
        private static int nextShelfId = 0;
        public int ShelfId { get;}
        public int ShelfFloor { get; set; }
        public double ShelfSpace { get; set; }
        public List<Item>Items { get; set; }

        public override string ToString()
        {
            return $"Shelf ID: {ShelfId}\nFloor Number: {ShelfFloor}\nSpace: {ShelfSpace} square meters";
        }
        public Shelf(int ShelfFloor,Double ShelfSpace) {
            this.ShelfId = ++nextShelfId;
            this.ShelfFloor = ShelfFloor ;
            this.ShelfSpace = ShelfSpace ;
            Items = new List<Item>();
        }
        public double SpaceInTheShelf()
        {
            double space = 0;
            foreach (Item item in Items)
            {
                space += item.ItemSpace;
            }
            return this.ShelfSpace - space;
        }
    }
}
