using HSDRaw.Common;
using HSDRaw.Common.Animation;

namespace HSDRaw.AirRide
{
    public class KAR_vsLegendaryData : HSDAccessor
    {
        public override int TrimmedSize => 0x14;

        public KAR_vsLegendaryModelAnimation PrimaryModelAnimation { get => _s.GetReference<KAR_vsLegendaryModelAnimation>(0x00); set => _s.SetReference(0x00, value); }

        public KAR_vsLegendaryModelAnimation SecondaryModelAnimation { get => _s.GetReference<KAR_vsLegendaryModelAnimation>(0x04); set => _s.SetReference(0x04, value); }

        public KAR_vsLegendaryEffectResource AssemblyEffectResource { get => _s.GetReference<KAR_vsLegendaryEffectResource>(0x08); set => _s.SetReference(0x08, value); }

        public int x0C { get => _s.GetInt32(0x0C); set => _s.SetInt32(0x0C, value); }

        public int x10 { get => _s.GetInt32(0x10); set => _s.SetInt32(0x10, value); }
    }

    public class KAR_vsLegendaryModelAnimation : HSDAccessor
    {
        public override int TrimmedSize => 0x0C;

        public HSD_JOBJ RootJoint { get => _s.GetReference<HSD_JOBJ>(0x00); set => _s.SetReference(0x00, value); }

        public KAR_vsLegendarySplineAnimation SplineAnimation { get => _s.GetReference<KAR_vsLegendarySplineAnimation>(0x04); set => _s.SetReference(0x04, value); }

        public HSD_MatAnimJoint MaterialAnimation { get => _s.GetReference<HSD_MatAnimJoint>(0x08); set => _s.SetReference(0x08, value); }
    }

    public class KAR_vsLegendarySplineAnimation : HSDAccessor
    {
    }

    public class KAR_vsLegendaryEffectResource : HSDAccessor
    {
        public override int TrimmedSize => 0x04;

        public HSDAccessor EffectData { get => _s.GetReference<HSDAccessor>(0x00); set => _s.SetReference(0x00, value); }
    }
}
