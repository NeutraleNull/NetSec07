namespace NetSec07
{
    public partial class BelmannFord
    {
        public class Edge
        {
            public Vertex U;
            public Vertex V;
            public float Weight;

            public Edge(Vertex u, Vertex v, float weight) => (U, V, Weight) = (u, v, weight);
            public override string ToString()
            {
                return $"[{U}, {V}, {Weight}]";
            }
        }
    }
}