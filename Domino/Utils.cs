

namespace Domino.Utils;

public class NullParentException : Exception {
    public NullParentException(string message) : base(message) {}
}

public class NonNullNodeException : Exception {
    public NonNullNodeException(string message) : base(message) {}
}
public class Graph<T> {
    public HashSet<GraphLink<T>> Links { get; protected set; }
    public HashSet<GraphNode<T>> Nodes { get; protected set; }
    public GraphNode<T> Root { get; }

    public Graph(T root) {
        this.Nodes = new HashSet<GraphNode<T>>();
        this.Links = new HashSet<GraphLink<T>>();
        
        this.Root = new GraphNode<T>(root);
        Nodes.Add(this.Root);
    }

    public void AddNodes(GraphNode<T> parent, int n) {
        if (parent.Value == null)
            throw new NullParentException("Parent node must not be null");
        
        for (int i = 0; i < n; i++) {
            GraphNode<T> newNode = new GraphNode<T>();

            this.Nodes.Add(newNode);
            this.Links.Add(new GraphLink<T>(parent, newNode));
        }
    }

    public IEnumerable<GraphLink<T>> GetLinksWithFreeNodes() {
        return this.Links.Where(link => link.To.Value == null);
    }
    public IEnumerable<GraphNode<T>> GetFreeNodes() {
        return this.Nodes.Where(node => node.Value == null);
    }
}

public class GraphNode<T> {
    public T? Value { get; private set; }

    public GraphNode() {}

    public GraphNode(T value) {
        this.Value = value;
    }

    public void SetValue(T value) {
        if (this.Value != null)
            throw new NonNullNodeException("Node already contains a value");
        this.Value = value;
    }
}

public class GraphLink<T> {
    public GraphNode<T> From { get; }
    public GraphNode<T> To { get; }

    public GraphLink(GraphNode<T> from, GraphNode<T> to) {
        this.From = from;
        this.To = to;
    }
}

public static class Combinatorics {
    public static void GenerateVariations(int n, int[] variation, int length, List<int[]> collection) {
        if (length == variation.Length) {
            collection.Add(new int[] {variation[0], variation[1]});
            return;
        }
        for (int i = 0; i <= n; i++) {
            variation[length] = i;
            GenerateVariations(n, variation, length + 1, collection);
        }
    }
}