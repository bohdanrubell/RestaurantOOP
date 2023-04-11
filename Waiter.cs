using System;
using System.Collections.Generic;

namespace RestaurantAppOOP;

public partial class Waiter
{
    private int _id;
    private string _name;

    public int Id { 
        get { return _id; } 
        set { _id = value; }
    }

    public string NameWaiter { 
        get { return _name; } 
        set { _name = value; } 
    }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
