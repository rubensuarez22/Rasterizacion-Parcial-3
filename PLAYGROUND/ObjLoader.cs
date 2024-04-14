using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace PLAYGROUND
{
    public static class ObjLoader
    {
        public static Mesh Load(string path)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<int> indices = new List<int>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    if (line.StartsWith("v "))
                    {
                        Vertex vertex = new Vertex
                        {
                            X = float.Parse(parts[1]),
                            Y = float.Parse(parts[2]),
                            Z = float.Parse(parts[3]),
                            Color = Color.White  // Asigna un color predeterminado o lee el color si está disponible
                        };
                        vertices.Add(vertex);
                    }
                    else if (line.StartsWith("f "))
                    {
                        for (int i = 1; i < parts.Length; i++)
                        {
                            string[] subparts = parts[i].Split('/');
                            int vertexIndex = int.Parse(subparts[0]) - 1;  // Convierte a 0-based index
                            indices.Add(vertexIndex);
                        }
                    }
                }
            }

            return new Mesh { Vertices = vertices, Indexes = indices };
        }


    }
}
