using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefrigeratorExercise.Etities
{
    public enum TypeOfFood { Food, Drinking };
    public enum Kosher { Parrve, Dairy, Meats };

    public class Item
    {
        private static int nextItemId = 0;


        public int  ItemId { get; }
        public string ItemName { get; set; }
        public int IdOfShelf { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double ItemSpace {  get; set; }
        public TypeOfFood TypeOfFood { get; set; }
        public Kosher Kosher { get; set; }  


        public Item( string itemName, double itemSpace,string expiryDate,TypeOfFood typeOfFood, Kosher kosher)
        {
            ItemId = ++nextItemId;
            ItemName = itemName;
            ExpiryDate = DateTime.ParseExact(expiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            ItemSpace = itemSpace;
            TypeOfFood = typeOfFood;
            Kosher = kosher;
        }

        public override string ToString()
        {
            return $"Item ID: {ItemId}\nItem Name: {ItemName}\nShelf ID: {IdOfShelf}\nExpiry Date: {ExpiryDate}\nItem Space: {ItemSpace} square meters";
        }
    }
}

