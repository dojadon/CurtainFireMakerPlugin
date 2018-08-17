using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities
{
    public class ModelRigidCollection
    {
        public List<PmxRigidData> RigidList { get; } = new List<PmxRigidData>();

        public World World { get; }

        public PmxRigidData[] RigidArray => RigidList.ToArray();

        public ModelRigidCollection(World world)
        {
            World = world;
        }

        public void CreateRigids(IEnumerable<ShotModelData> dataList, out PmxRigidData[] rigids)
        {
            rigids = dataList.SelectMany(d => d.Rigids.Values.Select(rigid => Setup(rigid, d.BoneIndexOffset))).ToArray();

            PmxRigidData Setup(PmxRigidData rigid, int offset)
            {
                rigid.BoneId += offset;
                return rigid;
            }
        }
    }
}
