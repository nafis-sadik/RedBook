﻿using System;
using System.Collections.Generic;

namespace Identity.Data.Entities;

public partial class Application
{
    public int Id { get; set; }

    public string ApplicationName { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
