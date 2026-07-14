namespace HSDRaw.AirRide.Vc
{
    public class KAR_vcDataKindStar : HSDAccessor
    {
        public override int TrimmedSize => 0x30;

        public HSDArrayAccessor<KAR_vcKindResourceEntry> ResourceTable { get => _s.GetReference<HSDArrayAccessor<KAR_vcKindResourceEntry>>(0x00); set => _s.SetReference(0x00, value); }

        public HSDIntArray IndexTable { get => _s.GetReference<HSDIntArray>(0x04); set => _s.SetReference(0x04, value); }

        public KAR_vcStarCommonAttributes CommonAttributes { get => _s.GetReference<KAR_vcStarCommonAttributes>(0x08); set => _s.SetReference(0x08, value); }

        public KAR_vcCommonWordArray SfxParamTable { get => _s.GetReference<KAR_vcCommonWordArray>(0x0C); set => _s.SetReference(0x0C, value); }

        public HSDArrayAccessor<KAR_vcStatScaleRange> StatScalingTable { get => _s.GetReference<HSDArrayAccessor<KAR_vcStatScaleRange>>(0x10); set => _s.SetReference(0x10, value); }

        public HSDFloatArray x14 { get => _s.GetReference<HSDFloatArray>(0x14); set => _s.SetReference(0x14, value); }

        public int x18 { get => _s.GetInt32(0x18); set => _s.SetInt32(0x18, value); }

        public int x1C { get => _s.GetInt32(0x1C); set => _s.SetInt32(0x1C, value); }

        public int x20 { get => _s.GetInt32(0x20); set => _s.SetInt32(0x20, value); }

        public int x24 { get => _s.GetInt32(0x24); set => _s.SetInt32(0x24, value); }

        public int x28 { get => _s.GetInt32(0x28); set => _s.SetInt32(0x28, value); }

        public int x2C { get => _s.GetInt32(0x2C); set => _s.SetInt32(0x2C, value); }
    }

    public class KAR_vcDataKindWheel : HSDAccessor
    {
        public override int TrimmedSize => 0x24;

        public HSDArrayAccessor<KAR_vcKindResourceEntry> ResourceTable { get => _s.GetReference<HSDArrayAccessor<KAR_vcKindResourceEntry>>(0x00); set => _s.SetReference(0x00, value); }

        public HSDIntArray IndexTable { get => _s.GetReference<HSDIntArray>(0x04); set => _s.SetReference(0x04, value); }

        public KAR_vcWheelCommonAttributes CommonAttributes { get => _s.GetReference<KAR_vcWheelCommonAttributes>(0x08); set => _s.SetReference(0x08, value); }

        public KAR_vcCommonWordArray SfxParamTable { get => _s.GetReference<KAR_vcCommonWordArray>(0x0C); set => _s.SetReference(0x0C, value); }

        public HSDArrayAccessor<KAR_vcStatScaleRange> StatScalingTable { get => _s.GetReference<HSDArrayAccessor<KAR_vcStatScaleRange>>(0x10); set => _s.SetReference(0x10, value); }

        public int x14 { get => _s.GetInt32(0x14); set => _s.SetInt32(0x14, value); }

        public int x18 { get => _s.GetInt32(0x18); set => _s.SetInt32(0x18, value); }

        public int x1C { get => _s.GetInt32(0x1C); set => _s.SetInt32(0x1C, value); }

        public int x20 { get => _s.GetInt32(0x20); set => _s.SetInt32(0x20, value); }
    }

    public class KAR_vcKindResourceEntry : HSDAccessor
    {
        public override int TrimmedSize => 0x0C;

        public HSDAccessor DataA { get => _s.GetReference<HSDAccessor>(0x00); set => _s.SetReference(0x00, value); }

        public HSDAccessor DataB { get => _s.GetReference<HSDAccessor>(0x04); set => _s.SetReference(0x04, value); }

        public byte x08 { get => _s.GetByte(0x08); set => _s.SetByte(0x08, value); }

        public byte x09 { get => _s.GetByte(0x09); set => _s.SetByte(0x09, value); }

        public byte x0A { get => _s.GetByte(0x0A); set => _s.SetByte(0x0A, value); }

        public byte x0B { get => _s.GetByte(0x0B); set => _s.SetByte(0x0B, value); }
    }

    public class KAR_vcStatScaleRange : HSDAccessor
    {
        public override int TrimmedSize => 0x08;

        public float LowRatioScale { get => _s.GetFloat(0x00); set => _s.SetFloat(0x00, value); }

        public float HighRatioScale { get => _s.GetFloat(0x04); set => _s.SetFloat(0x04, value); }
    }

    public class KAR_vcStarCommonAttributes : HSDAccessor
    {
        public override int TrimmedSize => 0xAC;

        public float x00 { get => _s.GetFloat(0x00); set => _s.SetFloat(0x00, value); }

        public float x04 { get => _s.GetFloat(0x04); set => _s.SetFloat(0x04, value); }

        public float x08 { get => _s.GetFloat(0x08); set => _s.SetFloat(0x08, value); }

        public float x0C { get => _s.GetFloat(0x0C); set => _s.SetFloat(0x0C, value); }

        public float x10 { get => _s.GetFloat(0x10); set => _s.SetFloat(0x10, value); }

        public float x14 { get => _s.GetFloat(0x14); set => _s.SetFloat(0x14, value); }

        public float x18 { get => _s.GetFloat(0x18); set => _s.SetFloat(0x18, value); }

        public float MaxSpeed { get => _s.GetFloat(0x1C); set => _s.SetFloat(0x1C, value); }

        public float x20 { get => _s.GetFloat(0x20); set => _s.SetFloat(0x20, value); }

        public float x24 { get => _s.GetFloat(0x24); set => _s.SetFloat(0x24, value); }

        public float x28 { get => _s.GetFloat(0x28); set => _s.SetFloat(0x28, value); }

        public KAR_vcCommonWord[] x2C_A8 { get => _s.GetEmbeddedAccessorArray<KAR_vcCommonWord>(0x2C, 0x20); set => _s.SetEmbeddedAccessorArray(0x2C, value); }
    }

    public class KAR_vcWheelCommonAttributes : HSDAccessor
    {
        public override int TrimmedSize => 0x1C;

        public float x00 { get => _s.GetFloat(0x00); set => _s.SetFloat(0x00, value); }

        public float x04 { get => _s.GetFloat(0x04); set => _s.SetFloat(0x04, value); }

        public float x08 { get => _s.GetFloat(0x08); set => _s.SetFloat(0x08, value); }

        public float x0C { get => _s.GetFloat(0x0C); set => _s.SetFloat(0x0C, value); }

        public float x10 { get => _s.GetFloat(0x10); set => _s.SetFloat(0x10, value); }

        public float x14 { get => _s.GetFloat(0x14); set => _s.SetFloat(0x14, value); }

        public float x18 { get => _s.GetFloat(0x18); set => _s.SetFloat(0x18, value); }
    }
}
