using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMDataIO.Pmx;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotModelData
    {
        public HashSet<EntityShot> OwnerEntities { get; } = new HashSet<EntityShot>();

        public PmxMorphData VertexMorph { get; set; }

        public PmxBoneData[] Bones { get; }
        public PmxVertexData[] Vertices { get; }
        public int[] Indices { get; }
        public PmxMaterialData[] Materials { get; }
        public String[] Textures { get; }

        public ShotProperty Property { get; }
        public World World { get; }

        public ShotModelData(World world, ShotProperty property)
        {
            Property = property;
            World = world;

            Bones = Property.Type.CreateBones(world, property);

            if (Property.Type.HasMesh)
            {
                Vertices = Property.Type.CreateVertices(world, property);
                Indices = Property.Type.CreateVertexIndices(world, property);
                Materials = Property.Type.CreateMaterials(world, property);
                Textures = Property.Type.CreateTextures(world, property);
            }
            else
            {
                Vertices = new PmxVertexData[0];
                Indices = new int[0];
                Materials = new PmxMaterialData[0];
                Textures = new string[0];
            }
        }

        public PmxMorphData CreateVertexMorph(string morphName, Func<Vector3, Vector3> func)
        {
            if (VertexMorph == null)
            {
                VertexMorph = new PmxMorphData()
                {
                    MorphName = morphName,
                    SlotType = MorphSlotType.RIP,
                    MorphType = MorphType.VERTEX,
                    MorphArray = new IPmxMorphTypeData[Vertices.Length]
                };

                for (int i = 0; i < VertexMorph.MorphArray.Length; i++)
                {
                    var vertex = Vertices[i];
                    var vertexMorph = new PmxMorphVertexData()
                    {
                        Index = vertex.VertexId,
                        Position = func(vertex.Pos)
                    };
                    VertexMorph.MorphArray[i] = vertexMorph;
                }
                AddMorph(VertexMorph);
            }
            return VertexMorph;
        }

        public void AddMorph(PmxMorphData morph)
        {
            World.PmxModel.Morphs.MorphList.Add(morph);
        }
    }
}
