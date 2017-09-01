using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace SolidMath
{
    public class Solid
    {
        public HashSet<Vector3> Vertices { get; } = new HashSet<Vector3>();
        public HashSet<Face> Faces { get; } = new HashSet<Face>();

        public static Solid GetIcosahedron()
        {
            var result = new Solid();

            var vertices = new List<Vector3>
            {
                new Vector3(0, 1, 0),
                new Vector3(0.894425F, 0.447215F, 0.000000F),
                new Vector3(0.276385F, 0.447215F, -0.850640F),
                new Vector3(-0.723600F, 0.447215F, -0.525720F),
                new Vector3(-0.723600F, 0.447215F, 0.525720F),
                new Vector3(0.276385F, 0.447215F, 0.850640F),

                new Vector3(0, -1, 0),
                new Vector3(0.723600F, -0.447215F, 0.525720F),
                new Vector3(0.723600F, -0.447215F, -0.525720F),
                new Vector3(-0.276385F, -0.447215F, -0.850640F),
                new Vector3(-0.894425F, -0.447215F, 0.000000F),
                new Vector3(-0.276385F, -0.447215F, 0.850640F)
            };

            var faces = new List<Face>();

            int index1 = 1;
            faces.Add(new Face(vertices[0], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[0], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[0], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[0], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[0], vertices[index1++], vertices[1]));

            index1 = 7;
            faces.Add(new Face(vertices[6], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[6], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[6], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[6], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[6], vertices[index1++], vertices[7]));

            int index2 = 1;
            index1 = 7;
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[7]));

            index2 = 7;
            index1 = 1;
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[index1]));
            faces.Add(new Face(vertices[index2++], vertices[index1++], vertices[1]));

            vertices.ForEach(v => result.Vertices.Add(v));
            faces.ForEach(f => result.Faces.Add(f));

            return result;
        }

        public Solid Divide(int times)
        {
            var result = this;

            for (int i = 0; i < times; i++)
            {
                result = result.Divide();
            }
            return result;
        }

        public Solid Divide()
        {
            var solid = new Solid();

            foreach (var face in Faces)
            {
                solid.Faces.Add(face);

                var midFace = new Face();
                for (int i = 0, index = 0; i < 3; i++)
                {
                    midFace[i] = (face[index] + face[(index = (index + 1) % 3)]) * 0.5F;
                    solid.Vertices.Add(face[i]);
                    solid.Vertices.Add(midFace[i]);
                }
                solid.Faces.Add(midFace);

                for (int i = 0; i < 3; i++)
                {
                    solid.Faces.Add(new Face()
                    {
                        [0] = face[(i + 1) % 3],
                        [1] = midFace[i],
                        [2] = midFace[(i + 1) % 3]
                    });
                }
            }

            return solid;
        }
    }
}
