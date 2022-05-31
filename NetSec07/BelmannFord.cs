namespace NetSec07
{
    public partial class BelmannFord
    {
        private readonly bool _verbose;
        private Vertex[] vertices;
        private Edge[] edges;
        private List<Vertex> creationVertexCache = new();


        public BelmannFord(bool verbose = false) => _verbose = verbose;

        // creates an edge from csv data
        // will make sure to reuse any vertex if already created
        // this will ensure every vertex has it current array position saved withing (helpful for the algorithem)
        private Edge CreateEdge(string[] data)
        {
            var u = creationVertexCache.FirstOrDefault(vertex => vertex.Name == data[0]); // if vertex doesn't exist yet, we create a new one
            if (u == null)
            {
                Log($"Vertex with name {data[0]} doesn't exist yet. Creating with index {creationVertexCache.Count}");
                u = new Vertex(data[0], creationVertexCache.Count);
                creationVertexCache.Add(u);
            }
            var v = creationVertexCache.FirstOrDefault(vertex => vertex.Name == data[1]); // analog to above, but for the second vertex in the edge
            if (v == null)
            {
                Log($"Vertex with name {data[1]} doesn't exist yet. Creating with index {creationVertexCache.Count}");
                v = new Vertex(data[1], creationVertexCache.Count);
                creationVertexCache.Add(v);
            }
            return new Edge(u, v, float.Parse(data[2]));
        }

        private void Log(string message)
        {
            if (_verbose)
                Console.WriteLine(message);
        }

        private float[]? DoAlgorithem(Vertex vertex)
        {
            Log($"Current root is {vertex.Name}");
            int indexRoot = Array.IndexOf(vertices, vertex);  // get the index of our starting vertex
            float[] distances = new float[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)         // calculate every distance to every other node
            {
                distances[i] = float.PositiveInfinity;        // at the start every distance is filled with infinity
            }
            distances[indexRoot] = 0;                         // route to itself is always 0

            for (int i = 0; i < vertices.Length - 1; i++)     // we repeate for n-1 times, n is the amount of vertecies
            {
                foreach (var edge in edges)                   // now we calculate each distance for every vertex
                {
                    if ((distances[edge.U.Id] + edge.Weight) < distances[edge.V.Id])
                    {
                        distances[edge.V.Id] = distances[edge.U.Id] + edge.Weight;
                    }
                }
                DisplayCurrentTable(distances, i);
            }

            foreach (var edge in edges)                     // check there is no distance less than zero
            {
                if ((distances[edge.U.Id] + edge.Weight) < distances[edge.V.Id])
                {
                    Console.WriteLine("Something went wrong, negative weight detected.");
                    return null;
                }
            }

            return distances;                               // return final output
        }

        // this function will turn the csv into object representation
        // notable is, edges will share same vertecies
        // this is not perfomant for larger sets, but easier to understand in our case
        // otherwise just use plain arrays
        public BelmannFord ParseCsv(string file)
        {
            var fileContent = File.ReadAllLines(file);
            edges = fileContent
                .Skip(1) //skip csv header
                .Select(line => CreateEdge(line.Split(",")))
                .ToArray();
            vertices = creationVertexCache.ToArray();

            return this;
        }

        // calculate all distances for every vertex
        public void Calculate()
        {
            foreach (var vertex in vertices)
            {
                var distanceTable = DoAlgorithem(vertex); // perform the algorithem for specified vertex
                Console.WriteLine();
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine($"Distance table for vertex {vertex.Name}");
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");

                if (distanceTable == null)
                {
                    Console.WriteLine("Could not be calculated, negative weight");
                }
                else
                {
                    Console.WriteLine(string.Join(" + ", vertices.Select(vertex => vertex.Name.PadLeft(4))));
                    Console.WriteLine(string.Join(" + ", distanceTable.Select(distance => distance.ToString("0.00").PadLeft(4))));
                }
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine();
            }
        }

        public void DisplayCurrentTable(float[]? distanceTable, int round)
        {
            if (distanceTable == null)
            {
                Log("Could not be calculated, negative weight");
            }
            else
            {
                Log($"++++++++++++++++++ Temp table round {round} ++++++++++++++++");
                Log(string.Join(" + ", vertices.Select(vertex => vertex.Name.PadLeft(4))));
                Log(string.Join(" + ", distanceTable.Select(distance => distance.ToString("0.00").PadLeft(4))));
            }
        }
    }
}