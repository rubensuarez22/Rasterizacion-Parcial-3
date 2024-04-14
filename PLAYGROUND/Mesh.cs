using System.Collections.Generic;

namespace PLAYGROUND
{
    public class Mesh
    {
        // mtx   :: ex rotations
        // mtx   :: position
        // mtx   :: scales

        public List<Vertex> Vertices { get; set; } 
        public List<int> Indexes { get; set; }

        public void Render()
        {
        }
    }
}
