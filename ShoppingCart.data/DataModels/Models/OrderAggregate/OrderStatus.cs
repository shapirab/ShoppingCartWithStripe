﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.data.DataModels.Models.OrderAggregate
{
    public enum OrderStatus
    {
        PENDING,
        PAYMENT_RECIEVED,
        PAYMENT_FAILED,
        PAYMENT_MISMATCH,
        REFUNDED
    }
}
