using System;
using System.Collections.Generic;
using System.Text;

namespace Cable.Core;

public class Graph<T>
{
    public GraphItem<T>? Root { get; set; }
}

public class GraphItem<T>
{
    public T? Data { get; set; }

    public List<GraphItem<T>> Children { get; set; } = [];
    public List<GraphItem<T>> Parents { get; set; } = [];
}
