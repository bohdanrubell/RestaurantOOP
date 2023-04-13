using System;
using System.Collections.Generic;

namespace RestaurantAppOOP.models;

public  class Order
{
    public int Id { get; set; }

    public int IdWaiter { get; set; }

    public DateTime DateOrder { get; set; }

    public int NumberOfTable { get; set; }

    public virtual Waiter IdWaiterNavigation { get; set; } = null!;

    public virtual ICollection<OrderedDish> OrderedDishes { get; set; } = new List<OrderedDish>();
}
