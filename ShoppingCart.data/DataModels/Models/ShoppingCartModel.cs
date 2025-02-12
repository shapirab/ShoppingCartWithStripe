﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.DataModels.Models
{
    public class ShoppingCartModel
    {
        public required string Id { get; set; }
        public IEnumerable<CartItem> Items { get; set; } = [];
    }
}
