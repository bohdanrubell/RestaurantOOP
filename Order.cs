using System;
using System.Collections.Generic;

namespace RestaurantAppOOP;

public partial class Order
{
    private int _id;
    private int _idWaiter;
    private DateTime _dateOrder;
    private int _numTable;

    public int Id { get { return _id; } set { _id = value; } }

    public int IdWaiter { get { return _idWaiter; } set { _idWaiter = value; } }

    public DateTime DateOrder { get { return _dateOrder; } set { _dateOrder = value; } }

    public int NumberOfTable { get { return _numTable; } set { _numTable = value; } }

    public virtual Waiter IdWaiterNavigation { get; set; } = null!;

    public virtual ICollection<OrderedDish> OrderedDishes { get; set; } = new List<OrderedDish>();
}
