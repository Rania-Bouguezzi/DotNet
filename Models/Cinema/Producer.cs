using System;
using System.Collections.Generic;

namespace CinemaManager_X.Models.Cinema;

public partial class Producer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Nationalty { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
