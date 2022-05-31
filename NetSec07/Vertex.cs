namespace NetSec07
{
    public partial class BelmannFord
    {
        public class Vertex
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public Vertex(string name, int id) => (Name, Id) = (name, id);

            public override string ToString()
            {
                return Name;
            }
        }
    }
}