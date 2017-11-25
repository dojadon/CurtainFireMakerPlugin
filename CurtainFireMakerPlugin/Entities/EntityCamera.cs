using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuPlugin;
using VecMath;

namespace CurtainFireMakerPlugin.Entities
{
    public class EntityCamera : Entity
    {
        private CameraCollection Cameras { get; }

        private Matrix4 ViewProjMatrix { get; set; }

        internal EntityCamera(World world, CameraCollection cameras) : base(world)
        {
            Cameras = cameras;
        }

        protected override void UpdateLocalMat()
        {
            Camera camera = GetActiveCamera();

            Matrix4 view = CreateViewMatrix(camera);
            Matrix4 proj = CreateProjectionMatrix(camera);

            ViewProjMatrix = view * proj;
        }

        private Matrix4 CreateViewMatrix(Camera camera)
        {
            Vector3 pos = Vector3.Zero;
            Vector3 eular = Vector3.Zero;
            float radius = 0.0F;

            foreach (var layer in camera.Layers)
            {
                var frame = layer.Frames.GetFrame(World.FrameCount);

                pos += frame.Position;
                eular += frame.Angle;
                radius += frame.Radius;
            }
            radius /= camera.Layers.Count;

            Vector3 eye = pos + -Vector3.UnitZ * radius * Matrix3.RotationX(eular.x) * Matrix3.RotationY(eular.y);
            Vector3 up = Vector3.UnitY * Matrix3.RotationZ(eular.z);

            return Matrix4.LookAt(pos, eye, up);
        }

        private Matrix4 CreateProjectionMatrix(Camera camera)
        {
            float fovy = camera.Layers[0].Frames.GetFrame(World.FrameCount).Fov;
            var info = World.Scene.SystemInformation;
            var size = info.OutputScreenSize;

            return Matrix4.Perspective(size.Width, size.Height, info.NearPlane, info.FarPlane, fovy);
        }

        private Camera GetActiveCamera()
        {
            return Cameras[0];
        }
    }
}
