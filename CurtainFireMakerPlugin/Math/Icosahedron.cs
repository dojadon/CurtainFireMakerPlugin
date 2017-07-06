using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;
using CurtainFireMakerPlugin.Collections;

namespace CurtainFireMakerPlugin.Math
{
    struct Face
    {
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;

        public Face(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }

    public class Icosahedron
    {
        private readonly MultiDictionary<int, Vector3> vertexMap = new MultiDictionary<int, Vector3>();
        private readonly MultiDictionary<int, Face> faceMap = new MultiDictionary<int, Face>();

        public static readonly Icosahedron Instance = new Icosahedron();

        private Icosahedron()
        {
            var vertices = new List<Vector3>();

            vertices.Add(new Vector3(0, 1, 0));
            vertices.Add(new Vector3(0.894425F, 0.447215F, 0.000000F));
            vertices.Add(new Vector3(0.276385F, 0.447215F, -0.850640F));
            vertices.Add(new Vector3(-0.723600F, 0.447215F, -0.525720F));
            vertices.Add(new Vector3(-0.723600F, 0.447215F, 0.525720F));
            vertices.Add(new Vector3(0.276385F, 0.447215F, 0.850640F));

            vertices.Add(new Vector3(0, -1, 0));
            vertices.Add(new Vector3(0.723600F, -0.447215F, 0.525720F));
            vertices.Add(new Vector3(0.723600F, -0.447215F, -0.525720F));
            vertices.Add(new Vector3(-0.276385F, -0.447215F, -0.850640F));
            vertices.Add(new Vector3(-0.894425F, -0.447215F, 0.000000F));
            vertices.Add(new Vector3(-0.276385F, 0.447215F, 0.850640F));

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

            foreach (var v in vertices)
            {
                vertexMap.Add(0, v);
            }

            foreach (var f in faces)
            {
                faceMap.Add(0, f);
            }
        }

        private void CreateVertices(int level)
        {
            if (level > 0 && !this.vertexMap.ContainsKey(level))
            {
                if (!this.vertexMap.ContainsKey(level - 1))
                {
                    this.CreateVertices(level - 1);
                }

                var faces = this.faceMap[level - 1];
                var vertices = this.vertexMap[level - 1];

                foreach (var face in faces)
                {
                    var v12 = (face.v1 + face.v2) * 0.5F;
                    var v23 = (face.v2 + face.v3) * 0.5F;
                    var v31 = (face.v3 + face.v1) * 0.5F;

                    v12.Normalize();
                    v23.Normalize();
                    v31.Normalize();

                    faces.Add(new Face(face.v1, v31, v12));
                    faces.Add(new Face(face.v2, v12, v23));
                    faces.Add(new Face(face.v3, v23, v31));
                    faces.Add(new Face(v12, v23, v31));

                    if (!vertices.Contains(v12))
                    {
                        vertices.Add(v12);
                    }
                    if (!vertices.Contains(v23))
                    {
                        vertices.Add(v23);
                    }
                    if (!vertices.Contains(v31))
                    {
                        vertices.Add(v31);
                    }
                }
            }
        }

        public void GetVertices(Action<Vector3> action, int level)
        {
            if (level >= 0)
            {
                this.GetVertices(action, level - 1);
                this.vertexMap[level].ForEach(action);
            }
        }
    }
}
