using HSDRaw.Common;
using HSDRaw.Common.Animation;
using HSDRaw.Tools;
using System.Collections.Generic;
using System.Linq;

namespace HSDRawViewer.Extensions
{
    public static class HSDExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static HSD_MatAnimJoint GenerateMatAnimJoint(this HSD_JOBJ node)
        {
            HSD_MatAnimJoint joint = new();

            if (node.Dobj != null)
                foreach (HSD_DOBJ v in node.Dobj.List)
                {
                    if (joint.MaterialAnimation == null)
                        joint.MaterialAnimation = new HSD_MatAnim();
                    else
                        joint.MaterialAnimation.Add(new HSD_MatAnim() { });
                }

            foreach (HSD_JOBJ v in node.Children)
            {
                joint.AddChild(GenerateMatAnimJoint(v));
            }

            return joint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public static HSD_AOBJ ToAObj(this List<FOBJ_Player> tracks, AOBJ_Flags flags)
        {
            HSD_AOBJ aobj = new()
            {
                EndFrame = tracks.Max(t => t.FrameCount),
                Flags = flags,
            };

            foreach (var t in tracks)
                aobj.AddTrack(t);

            return aobj;
        }
    }
}
