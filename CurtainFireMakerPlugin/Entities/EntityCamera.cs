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

        private Matrix4 ModelViewProj { get; set; }

        internal EntityCamera(World world, CameraCollection cameras) : base(world)
        {
            Cameras = cameras;
        }

        protected override void UpdateLocalMat()
        {
            Camera camera = GetActiveCamera();

        }

        private Camera GetActiveCamera()
        {
            return Cameras[0];
        }
    }
}
