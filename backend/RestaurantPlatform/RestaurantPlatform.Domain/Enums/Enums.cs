using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantPlatform.Domain.Enums
{
    public enum UserRole { Customer, Admin }
    public enum OrderStatus { Placed, Confirmed, Preparing, Ready, Completed, Cancelled }
    public enum OrderType { Pickup, Delivery, DineIn, Takeaway }
    public enum ReservationStatus { Pending, Confirmed, Cancelled }
    public enum PaymentStatus { Pending, Paid, Failed }
}
