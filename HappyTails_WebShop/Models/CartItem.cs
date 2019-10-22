using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyTails_WebShop.Models
{
    public class CartItem
    {
        private Product prod = new Product();
        private int quantity;


        public Product Prod
        {
            get { return prod; }
            set { prod = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }


        public CartItem(Product product, int quantity)
        {
            this.prod = product;
            this.quantity = quantity;
        }
    }
}