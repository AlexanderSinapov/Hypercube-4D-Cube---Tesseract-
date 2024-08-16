using Raylib_cs;
using System;
using System.Numerics;

namespace tesseract
{
    internal class Program
    {
        static Vector4[] v;

        static void Main(string[] args)
        {
            Raylib.InitWindow(800, 600, "Tesseract");

            v = new Vector4[16];
            for (int i = 0; i < 16; i++)
            {
                int x = (i & 1) == 0 ? 1 : -1;
                int y = (i & 2) == 0 ? 1 : -1;
                int z = (i & 4) == 0 ? 1 : -1;
                int w = (i & 8) == 0 ? 1 : -1;

                v[i] = new Vector4(x, y, z, w);
            }

            while (!Raylib.WindowShouldClose())
            {
                UpdateTesseract();
                DrawTesseract();
            }

            Raylib.CloseWindow();
        }

        static void UpdateTesseract()
        {
            float angle = Raylib.GetFrameTime() * .5f;

           for (int i = 0; i < v.Length; i++)
            {
                v[i] = Rotate4D(v[i], angle, 0, 3);
                v[i] = Rotate4D(v[i], angle, 1, 3);
                v[i] = Rotate4D(v[i], angle, 2, 3);
            }
        }

        static void DrawTesseract()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            for (int i = 0; i < v.Length; i++)
            {
                Vector3 projV = Project4Dto3D(v[i], 5.0f);

                int screenX = (int)(projV.X * 100 + 400);
                int screenY = (int)(projV.Y * 100 + 300);

                Raylib.DrawCircle(screenX, screenY, 4, Color.White);

                for (int j = 0; j < 4; j++)
                {
                    int adjacentVertexIndex = i ^ (1 << j);

                    if (i < adjacentVertexIndex)
                    {
                        Vector3 adjacentProjectedVertex = Project4Dto3D(v[adjacentVertexIndex], 5.0f);

                        int adjacentScreenX = (int)(adjacentProjectedVertex.X * 100 + 400);
                        int adjacentScreenY = (int)(adjacentProjectedVertex.Y * 100 + 300);

                        Raylib.DrawLine(screenX, screenY, adjacentScreenX, adjacentScreenY, Color.Blue);
                    }
                }
            }

            Raylib.EndDrawing();
        }

        static Vector3 Project4Dto3D(Vector4 p, float d)
        {
            float w = d / (d - p.W);
            return new Vector3(p.X * w, p.Y * w, p.Z * w);
        }

        static Vector4 Rotate4D(Vector4 p, float a, int dim1, int dim2)
        {
            float cosTheta = (float)Math.Cos(a);
            float sinTheta = (float)Math.Sin(a);

            float temp1 = p[dim1];
            float temp2 = p[dim2];

            p[dim1] = cosTheta * temp1 - sinTheta * temp2;
            p[dim2] = sinTheta * temp1 + cosTheta * temp2;

            return p;
        }
    }
}