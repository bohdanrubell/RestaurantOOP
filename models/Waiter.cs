using System;
using System.Collections.Generic;

namespace RestaurantAppOOP.models;

public  class Waiter
{
    public int Id { get; set; }
    
    public string NameWaiter { get; set; }
    

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
