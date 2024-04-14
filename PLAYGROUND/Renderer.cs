
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PLAYGROUND
{
    public class Renderer
    {
        Canvas canvas;

        public Renderer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        private void Swap(ref Vertex a, ref Vertex b)
        {
            (a, b) = (b, a);
        }

        public void SortByY()
        { }

        public void Interpolate()
        {}

        public void RenderTriangle(Vertex v0, Vertex v1, Vertex v2)
        {
            // Organiza los vértices por su posición en Y para facilitar el procesamiento.
            List<Vertex> vertices = new List<Vertex> { v0, v1, v2 };
            vertices.Sort((vertex1, vertex2) => vertex1.Y.CompareTo(vertex2.Y));

            // Extrae los vértices ordenados.
            Vertex top = vertices[0];
            Vertex middle = vertices[1];
            Vertex bottom = vertices[2];

            // Interpola entre top y bottom y entre top y middle, luego middle y bottom
            int totalHeight = (int)(bottom.Y - top.Y);
            for (int y = (int)top.Y; y <= (int)bottom.Y; y++)
            {
                bool isUpperHalf = y < middle.Y;
                int segmentHeight = isUpperHalf ? (int)(middle.Y - top.Y) : (int)(bottom.Y - middle.Y);
                float alpha = (float)(y - top.Y) / totalHeight;
                float beta = (float)(y - (isUpperHalf ? top.Y : middle.Y)) / segmentHeight;

                Vertex A = InterpolateVertex(top, bottom, alpha);
                Vertex B = isUpperHalf ? InterpolateVertex(top, middle, beta) : InterpolateVertex(middle, bottom, beta);

                if (A.X > B.X) Swap(ref A, ref B);

                for (int x = (int)A.X; x <= (int)B.X; x++)
                {
                    float phi = (B.X - A.X) > 0 ? (float)(x - A.X) / (B.X - A.X) : 0;
                    Vertex P = InterpolateVertex(A, B, phi);
                    canvas.SetPixel(x, y, P.Color);
                }
            }
        }

        private void SwapVertex(ref Vertex a, ref Vertex b)
        {
            (a, b) = (b, a);
        }

        private Vertex InterpolateVertex(Vertex v1, Vertex v2, float factor)
        {
            return new Vertex
            {
                X = (int)(v1.X + (v2.X - v1.X) * factor),
                Y = (int)(v1.Y + (v2.Y - v1.Y) * factor),
                Color = InterpolateColor(v1.Color, v2.Color, factor)
            };
        }

        private Color InterpolateColor(Color c1, Color c2, float factor)
        {
            // Clamps a value between 0 and 255
            int Clamp(int value) => Math.Max(0, Math.Min(255, value));

            int red = Clamp((int)(c1.R + (c2.R - c1.R) * factor));
            int green = Clamp((int)(c1.G + (c2.G - c1.G) * factor));
            int blue = Clamp((int)(c1.B + (c2.B - c1.B) * factor));

            return Color.FromArgb(red, green, blue);
        }


        public void DrawLine()
        { }

        public void RenderMesh(Mesh mesh)
        {
            if (mesh.Indexes.Count % 3 != 0)
            {
                throw new InvalidOperationException("La lista de índices debe ser múltiplo de 3 para representar triángulos.");
            }

            for (int i = 0; i < mesh.Indexes.Count; i += 3)
            {
                int index1 = mesh.Indexes[i];
                int index2 = mesh.Indexes[i + 1];
                int index3 = mesh.Indexes[i + 2];

                if (index1 >= mesh.Vertices.Count || index1 < 0 ||
                    index2 >= mesh.Vertices.Count || index2 < 0 ||
                    index3 >= mesh.Vertices.Count || index3 < 0)
                {
                    throw new ArgumentOutOfRangeException($"Índices fuera de rango: {index1}, {index2}, {index3} con tamaño de vértices: {mesh.Vertices.Count}");
                }

                Vertex v0 = mesh.Vertices[index1];
                Vertex v1 = mesh.Vertices[index2];
                Vertex v2 = mesh.Vertices[index3];

                RenderTriangle(v0, v1, v2);
            }
        }


        public void RenderScene(Scene scene)
        {
            canvas.FastClear();
            //  PROCESS SCENE
            for (int m = 0; m < scene.Models.Count; m++)
            {
                RenderMesh(scene.Models[m]);
            }
            //
            canvas.Refresh();
        }
    }
}
