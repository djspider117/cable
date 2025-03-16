using System.Collections.Generic;

namespace Cable.Core;

public class GraphLayout<T> where T : ILayoutable
{
    public const int SPACING = 25;
    public const int TOP_OFFSET = 70;

    public void Layout(GraphItem<T> node, int depth = 0, double offset = 0)
    {
        var set = new HashSet<GraphItem<T>>();
        LayoutInternal(node, set, depth, offset);

        foreach (var item in set)
        {
            item.Data!.Y += TOP_OFFSET;
        }
    }

    private void LayoutInternal(GraphItem<T> node, HashSet<GraphItem<T>> visited, int depth = 0, double offset = 0)
    {
        if (visited.Contains(node)) 
            return;

        visited.Add(node);

        node.Data!.X = offset;
        double nextY = 0;
        foreach (var item in node.Parents)
        {
            item.Data!.Y = nextY + node.Data.Y;
            nextY += item.Data.ActualHeight + SPACING;
            LayoutInternal(item, visited, depth + 1, offset - node!.Data!.ActualWidth - SPACING);
        }
    }
}