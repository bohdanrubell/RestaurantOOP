using System;
using System.Collections.Generic;

namespace RestaurantAppOOP;

public partial class Menu
{
    private int _id;
    private string _name;
    private string _description;
    private decimal _price;

    public int Id { get { return _id; } set { _id = value; } }

    public string Name { get { return _name; } set { _name = value; } }
    public string? Description { get { return _description; } set { _description = value; } }

    public decimal Cost { get { return _price; } set { _price = value; } }

    public virtual ICollection<OrderedDish> OrderedDishes { get; } = new List<OrderedDish>();
}
