using System;
using System.Collections.Generic;
using System.Text;

namespace HSDRaw.AirRide.Rd
{
    public class KAR_RdDataCommon : HSDAccessor
    {
        public override int TrimmedSize => 0x1C;

        public HSDByteArray Parameters { get => _s.GetReference<HSDByteArray>(0x00); set => _s.SetReference(0x00, value); }

        public KAR_RdCommonColorAnimationScripts ColorAnimationScripts { get => _s.GetReference<KAR_RdCommonColorAnimationScripts>(0x04); set => _s.SetReference(0x04, value); }

        public HSDByteArray UnknownParam { get => _s.GetReference<HSDByteArray>(0x08); set => _s.SetReference(0x08, value); }

        public HSDByteArray FireworkCannon { get => _s.GetReference<HSDByteArray>(0x0C); set => _s.SetReference(0x0C, value); }

        public HSDByteArray ShakeAnimation { get => _s.GetReference<HSDByteArray>(0x10); set => _s.SetReference(0x10, value); }
    }

    public class KAR_RdCommonColorAnimationScripts : HSDAccessor
    {
        public override int TrimmedSize => -1;
    }
}
