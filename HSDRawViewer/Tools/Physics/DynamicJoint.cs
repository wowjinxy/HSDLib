using HSDRaw.Common;
using HSDRawViewer.Rendering.Models;
using OpenTK.Mathematics;

namespace HSDRawViewer.Tools.Physics
{
    public class DynamicJoint
    {
        public HSD_JOBJ Desc;
        public LiveJObj Live;

        public Vector3 Translation;
        public Vector3 Rotation;
        public Vector3 Scale;

        // Current state
        public Vector3 WorldPosition;

        // Angular motion
        public Vector3 AngularAxis = Vector3.UnitX;
        public float AngularVelocity;

        // Rest state
        public Vector3 RestRotation;

        // Bone data
        public float BoneLength;
        public float InverseBoneLength;

        // Parameters
        public float FollowDamping;
        public float RestPoseStiffness;
        public float RotationLimit;
        public float InertiaDamping;
        public float Resistance;

        // Misc
        public int DominantAxis;

        public DynamicJoint(LiveJObj joint)
        {
            Desc = joint.Desc;
            Live = joint;

            Translation = new Vector3(Desc.TX, Desc.TY, Desc.TZ);
            Rotation = new Vector3(Desc.RX, Desc.RY, Desc.RZ);
            Scale = new Vector3(Desc.SX, Desc.SY, Desc.SZ);

            RestRotation = Rotation;
        }
    }
}
