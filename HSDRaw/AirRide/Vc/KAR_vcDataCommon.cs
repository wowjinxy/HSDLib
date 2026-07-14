namespace HSDRaw.AirRide.Vc
{
    public enum KAR_vcMachineKind
    {
        Warp,
        Compact,
        Winged,
        Shadow,
        Hydra,
        Bulk,
        Slick,
        Formula,
        Dragoon,
        Wagon,
        Rocket,
        Swerve,
        Turbo,
        Jet,
        Flight,
        Free,
        Steer,
        WingKirby,
        WingMetaKnight,
        WheelNormal,
        WheelKirby,
        WheelieBike,
        RexWheelie,
        WheelieScooter,
        WheelDedede,
        WheelVsDedede,
    }

    public class KAR_vcDataCommon : HSDAccessor
    {
        public override int TrimmedSize => 0x44;

        public KAR_vcCommonWordArray CommonParameterTable { get => _s.GetReference<KAR_vcCommonWordArray>(0x00); set => _s.SetReference(0x00, value); }

        public HSDArrayAccessor<KAR_vcCommonResourceEntry> ResourceTable { get => _s.GetReference<HSDArrayAccessor<KAR_vcCommonResourceEntry>>(0x04); set => _s.SetReference(0x04, value); }

        public KAR_vcCommonWordArray PhysicsParameterTable { get => _s.GetReference<KAR_vcCommonWordArray>(0x08); set => _s.SetReference(0x08, value); }

        public HSDArrayAccessor<KAR_vcCommonMachineClassSfxParam> MachineClassSfxParams { get => _s.GetReference<HSDArrayAccessor<KAR_vcCommonMachineClassSfxParam>>(0x0C); set => _s.SetReference(0x0C, value); }

        public HSDFixedLengthPointerArrayAccessor<HSDArrayAccessor<KAR_vcCommonSoundEntry>> VehicleSoundTable { get => _s.GetReference<HSDFixedLengthPointerArrayAccessor<HSDArrayAccessor<KAR_vcCommonSoundEntry>>>(0x10); set => _s.SetReference(0x10, value); }

        public HSDFixedLengthPointerArrayAccessor<HSDAccessor> x14 { get => _s.GetReference<HSDFixedLengthPointerArrayAccessor<HSDAccessor>>(0x14); set => _s.SetReference(0x14, value); }

        public HSDAccessor x18 { get => _s.GetReference<HSDAccessor>(0x18); set => _s.SetReference(0x18, value); }

        public KAR_vcCommonWordArray x1C { get => _s.GetReference<KAR_vcCommonWordArray>(0x1C); set => _s.SetReference(0x1C, value); }

        public KAR_vcCommonSpawnData SpawnData { get => _s.GetReference<KAR_vcCommonSpawnData>(0x20); set => _s.SetReference(0x20, value); }

        public KAR_vcCommonWordArray x24 { get => _s.GetReference<KAR_vcCommonWordArray>(0x24); set => _s.SetReference(0x24, value); }

        public KAR_vcCommonWordArray x28 { get => _s.GetReference<KAR_vcCommonWordArray>(0x28); set => _s.SetReference(0x28, value); }

        public KAR_vcCommonWordArray x2C { get => _s.GetReference<KAR_vcCommonWordArray>(0x2C); set => _s.SetReference(0x2C, value); }
    }

    public class KAR_vcCommonWordArray : HSDAccessor
    {
        public int WordCount => _s.Length / 4;

        public KAR_vcCommonWord[] Words { get => _s.GetEmbeddedAccessorArray<KAR_vcCommonWord>(0x00, WordCount); set => _s.SetEmbeddedAccessorArray(0x00, value); }
    }

    public class KAR_vcCommonWord : HSDAccessor
    {
        public override int TrimmedSize => 0x04;

        public float Float { get => _s.GetFloat(0x00); set => _s.SetFloat(0x00, value); }

        public int Int { get => _s.GetInt32(0x00); set => _s.SetInt32(0x00, value); }

        public uint UInt { get => (uint)_s.GetInt32(0x00); set => _s.SetInt32(0x00, (int)value); }
    }

    public class KAR_vcCommonResourceEntry : HSDAccessor
    {
        public override int TrimmedSize => 0x08;

        public HSDAccessor Data { get => _s.GetReference<HSDAccessor>(0x00); set => _s.SetReference(0x00, value); }

        public byte x04 { get => _s.GetByte(0x04); set => _s.SetByte(0x04, value); }

        public byte x05 { get => _s.GetByte(0x05); set => _s.SetByte(0x05, value); }

        public byte x06 { get => _s.GetByte(0x06); set => _s.SetByte(0x06, value); }

        public byte x07 { get => _s.GetByte(0x07); set => _s.SetByte(0x07, value); }
    }

    public class KAR_vcCommonMachineClassSfxParam : HSDAccessor
    {
        public override int TrimmedSize => 0x1C;

        public int x00 { get => _s.GetInt32(0x00); set => _s.SetInt32(0x00, value); }

        public float VolumeScale { get => _s.GetFloat(0x04); set => _s.SetFloat(0x04, value); }

        public float x08 { get => _s.GetFloat(0x08); set => _s.SetFloat(0x08, value); }

        public float x0C { get => _s.GetFloat(0x0C); set => _s.SetFloat(0x0C, value); }

        public float x10 { get => _s.GetFloat(0x10); set => _s.SetFloat(0x10, value); }

        public float x14 { get => _s.GetFloat(0x14); set => _s.SetFloat(0x14, value); }

        public float x18 { get => _s.GetFloat(0x18); set => _s.SetFloat(0x18, value); }
    }

    public class KAR_vcCommonSpawnData : HSDAccessor
    {
        public override int TrimmedSize => 0x14;

        public KAR_vcCommonWordArray SpawnParamTableA { get => _s.GetReference<KAR_vcCommonWordArray>(0x00); set => _s.SetReference(0x00, value); }

        public KAR_vcCommonWordArray SpawnParamTableB { get => _s.GetReference<KAR_vcCommonWordArray>(0x04); set => _s.SetReference(0x04, value); }

        public HSDArrayAccessor<KAR_vcCommonSpawnDesc> SpawnDesc { get => _s.GetReference<HSDArrayAccessor<KAR_vcCommonSpawnDesc>>(0x08); set => _s.SetReference(0x08, value); }

        public HSDArrayAccessor<KAR_vcCommonWeightedSpawnKind> WeightedSpawnKindTable { get => _s.GetReference<HSDArrayAccessor<KAR_vcCommonWeightedSpawnKind>>(0x0C); set => _s.SetReference(0x0C, value); }

        public HSDArrayAccessor<KAR_vcCommonMaxSpawnWeights> MaxSpawnWeights { get => _s.GetReference<HSDArrayAccessor<KAR_vcCommonMaxSpawnWeights>>(0x10); set => _s.SetReference(0x10, value); }
    }

    public class KAR_vcCommonSpawnDesc : HSDAccessor
    {
        public override int TrimmedSize => 0x6C;

        public float MatchProgress { get => _s.GetFloat(0x00); set => _s.SetFloat(0x00, value); }

        public float WarpChance { get => GetChance(KAR_vcMachineKind.Warp); set => SetChance(KAR_vcMachineKind.Warp, value); }

        public float CompactChance { get => GetChance(KAR_vcMachineKind.Compact); set => SetChance(KAR_vcMachineKind.Compact, value); }

        public float WingedChance { get => GetChance(KAR_vcMachineKind.Winged); set => SetChance(KAR_vcMachineKind.Winged, value); }

        public float ShadowChance { get => GetChance(KAR_vcMachineKind.Shadow); set => SetChance(KAR_vcMachineKind.Shadow, value); }

        public float HydraChance { get => GetChance(KAR_vcMachineKind.Hydra); set => SetChance(KAR_vcMachineKind.Hydra, value); }

        public float BulkChance { get => GetChance(KAR_vcMachineKind.Bulk); set => SetChance(KAR_vcMachineKind.Bulk, value); }

        public float SlickChance { get => GetChance(KAR_vcMachineKind.Slick); set => SetChance(KAR_vcMachineKind.Slick, value); }

        public float FormulaChance { get => GetChance(KAR_vcMachineKind.Formula); set => SetChance(KAR_vcMachineKind.Formula, value); }

        public float DragoonChance { get => GetChance(KAR_vcMachineKind.Dragoon); set => SetChance(KAR_vcMachineKind.Dragoon, value); }

        public float WagonChance { get => GetChance(KAR_vcMachineKind.Wagon); set => SetChance(KAR_vcMachineKind.Wagon, value); }

        public float RocketChance { get => GetChance(KAR_vcMachineKind.Rocket); set => SetChance(KAR_vcMachineKind.Rocket, value); }

        public float SwerveChance { get => GetChance(KAR_vcMachineKind.Swerve); set => SetChance(KAR_vcMachineKind.Swerve, value); }

        public float TurboChance { get => GetChance(KAR_vcMachineKind.Turbo); set => SetChance(KAR_vcMachineKind.Turbo, value); }

        public float JetChance { get => GetChance(KAR_vcMachineKind.Jet); set => SetChance(KAR_vcMachineKind.Jet, value); }

        public float FlightChance { get => GetChance(KAR_vcMachineKind.Flight); set => SetChance(KAR_vcMachineKind.Flight, value); }

        public float FreeChance { get => GetChance(KAR_vcMachineKind.Free); set => SetChance(KAR_vcMachineKind.Free, value); }

        public float SteerChance { get => GetChance(KAR_vcMachineKind.Steer); set => SetChance(KAR_vcMachineKind.Steer, value); }

        public float WingKirbyChance { get => GetChance(KAR_vcMachineKind.WingKirby); set => SetChance(KAR_vcMachineKind.WingKirby, value); }

        public float WingMetaKnightChance { get => GetChance(KAR_vcMachineKind.WingMetaKnight); set => SetChance(KAR_vcMachineKind.WingMetaKnight, value); }

        public float WheelNormalChance { get => GetChance(KAR_vcMachineKind.WheelNormal); set => SetChance(KAR_vcMachineKind.WheelNormal, value); }

        public float WheelKirbyChance { get => GetChance(KAR_vcMachineKind.WheelKirby); set => SetChance(KAR_vcMachineKind.WheelKirby, value); }

        public float WheelieBikeChance { get => GetChance(KAR_vcMachineKind.WheelieBike); set => SetChance(KAR_vcMachineKind.WheelieBike, value); }

        public float RexWheelieChance { get => GetChance(KAR_vcMachineKind.RexWheelie); set => SetChance(KAR_vcMachineKind.RexWheelie, value); }

        public float WheelieScooterChance { get => GetChance(KAR_vcMachineKind.WheelieScooter); set => SetChance(KAR_vcMachineKind.WheelieScooter, value); }

        public float WheelDededeChance { get => GetChance(KAR_vcMachineKind.WheelDedede); set => SetChance(KAR_vcMachineKind.WheelDedede, value); }

        public float WheelVsDededeChance { get => GetChance(KAR_vcMachineKind.WheelVsDedede); set => SetChance(KAR_vcMachineKind.WheelVsDedede, value); }

        private float GetChance(KAR_vcMachineKind kind)
        {
            return _s.GetFloat(0x04 + (int)kind * 4);
        }

        private void SetChance(KAR_vcMachineKind kind, float value)
        {
            _s.SetFloat(0x04 + (int)kind * 4, value);
        }
    }

    public class KAR_vcCommonWeightedSpawnKind : HSDAccessor
    {
        public override int TrimmedSize => 0x08;

        public int Kind { get => _s.GetInt32(0x00); set => _s.SetInt32(0x00, value); }

        public int Weight { get => _s.GetInt32(0x04); set => _s.SetInt32(0x04, value); }
    }

    public class KAR_vcCommonMaxSpawnWeights : HSDAccessor
    {
        public override int TrimmedSize => 0x94;

        public float OneAdditionalMachineWeight { get => _s.GetFloat(0x00); set => _s.SetFloat(0x00, value); }

        public float TwoAdditionalMachinesWeight { get => _s.GetFloat(0x04); set => _s.SetFloat(0x04, value); }

        public float ThreeAdditionalMachinesWeight { get => _s.GetFloat(0x08); set => _s.SetFloat(0x08, value); }

        public float FourAdditionalMachinesWeight { get => _s.GetFloat(0x0C); set => _s.SetFloat(0x0C, value); }

        public float FiveAdditionalMachinesWeight { get => _s.GetFloat(0x10); set => _s.SetFloat(0x10, value); }

        public float SixAdditionalMachinesWeight { get => _s.GetFloat(0x14); set => _s.SetFloat(0x14, value); }

        public float SevenAdditionalMachinesWeight { get => _s.GetFloat(0x18); set => _s.SetFloat(0x18, value); }

        public float EightAdditionalMachinesWeight { get => _s.GetFloat(0x1C); set => _s.SetFloat(0x1C, value); }

        public float NineAdditionalMachinesWeight { get => _s.GetFloat(0x20); set => _s.SetFloat(0x20, value); }

        public float TenAdditionalMachinesWeight { get => _s.GetFloat(0x24); set => _s.SetFloat(0x24, value); }

        public float ElevenAdditionalMachinesWeight { get => _s.GetFloat(0x28); set => _s.SetFloat(0x28, value); }

        public int SpawnKind00 { get => GetSpawnKind(0); set => SetSpawnKind(0, value); }

        public int SpawnKind01 { get => GetSpawnKind(1); set => SetSpawnKind(1, value); }

        public int SpawnKind02 { get => GetSpawnKind(2); set => SetSpawnKind(2, value); }

        public int SpawnKind03 { get => GetSpawnKind(3); set => SetSpawnKind(3, value); }

        public int SpawnKind04 { get => GetSpawnKind(4); set => SetSpawnKind(4, value); }

        public int SpawnKind05 { get => GetSpawnKind(5); set => SetSpawnKind(5, value); }

        public int SpawnKind06 { get => GetSpawnKind(6); set => SetSpawnKind(6, value); }

        public int SpawnKind07 { get => GetSpawnKind(7); set => SetSpawnKind(7, value); }

        public int SpawnKind08 { get => GetSpawnKind(8); set => SetSpawnKind(8, value); }

        public int SpawnKind09 { get => GetSpawnKind(9); set => SetSpawnKind(9, value); }

        public int SpawnKind10 { get => GetSpawnKind(10); set => SetSpawnKind(10, value); }

        public int SpawnKind11 { get => GetSpawnKind(11); set => SetSpawnKind(11, value); }

        public int SpawnKind12 { get => GetSpawnKind(12); set => SetSpawnKind(12, value); }

        public int SpawnKind13 { get => GetSpawnKind(13); set => SetSpawnKind(13, value); }

        public int SpawnKind14 { get => GetSpawnKind(14); set => SetSpawnKind(14, value); }

        public int SpawnKind15 { get => GetSpawnKind(15); set => SetSpawnKind(15, value); }

        public int SpawnKind16 { get => GetSpawnKind(16); set => SetSpawnKind(16, value); }

        public int SpawnKind17 { get => GetSpawnKind(17); set => SetSpawnKind(17, value); }

        public int SpawnKind18 { get => GetSpawnKind(18); set => SetSpawnKind(18, value); }

        public int SpawnKind19 { get => GetSpawnKind(19); set => SetSpawnKind(19, value); }

        public int SpawnKind20 { get => GetSpawnKind(20); set => SetSpawnKind(20, value); }

        public int SpawnKind21 { get => GetSpawnKind(21); set => SetSpawnKind(21, value); }

        public int SpawnKind22 { get => GetSpawnKind(22); set => SetSpawnKind(22, value); }

        public int SpawnKind23 { get => GetSpawnKind(23); set => SetSpawnKind(23, value); }

        public int SpawnKind24 { get => GetSpawnKind(24); set => SetSpawnKind(24, value); }

        public int SpawnKind25 { get => GetSpawnKind(25); set => SetSpawnKind(25, value); }

        private int GetSpawnKind(int index)
        {
            return _s.GetInt32(0x2C + index * 4);
        }

        private void SetSpawnKind(int index, int value)
        {
            _s.SetInt32(0x2C + index * 4, value);
        }
    }

    public class KAR_vcCommonSoundEntry : HSDAccessor
    {
        public override int TrimmedSize => 0x94;

        public int SFXx00 { get => GetSfx(0x00); set => SetSfx(0x00, value); }

        public int SFXx04 { get => GetSfx(0x04); set => SetSfx(0x04, value); }

        public int SFXx08 { get => GetSfx(0x08); set => SetSfx(0x08, value); }

        public int SFXx0C { get => GetSfx(0x0C); set => SetSfx(0x0C, value); }

        public int SFXx10 { get => GetSfx(0x10); set => SetSfx(0x10, value); }

        public int SFXx14 { get => GetSfx(0x14); set => SetSfx(0x14, value); }

        public int SFXx18 { get => GetSfx(0x18); set => SetSfx(0x18, value); }

        public int SFXx1C { get => GetSfx(0x1C); set => SetSfx(0x1C, value); }

        public int SFXx20 { get => GetSfx(0x20); set => SetSfx(0x20, value); }

        public int SFXx24 { get => GetSfx(0x24); set => SetSfx(0x24, value); }

        public int SFXx28 { get => GetSfx(0x28); set => SetSfx(0x28, value); }

        public int SFXx2C { get => GetSfx(0x2C); set => SetSfx(0x2C, value); }

        public int SFXx30 { get => GetSfx(0x30); set => SetSfx(0x30, value); }

        public int SFXx34 { get => GetSfx(0x34); set => SetSfx(0x34, value); }

        public int SFXx38 { get => GetSfx(0x38); set => SetSfx(0x38, value); }

        public int SFXx3C { get => GetSfx(0x3C); set => SetSfx(0x3C, value); }

        public int SFXx40 { get => GetSfx(0x40); set => SetSfx(0x40, value); }

        public int SFXx44 { get => GetSfx(0x44); set => SetSfx(0x44, value); }

        public int SFXx48 { get => GetSfx(0x48); set => SetSfx(0x48, value); }

        public int SFXx4C { get => GetSfx(0x4C); set => SetSfx(0x4C, value); }

        public int SFXx50 { get => GetSfx(0x50); set => SetSfx(0x50, value); }

        public int SFXx54 { get => GetSfx(0x54); set => SetSfx(0x54, value); }

        public int SFXx58 { get => GetSfx(0x58); set => SetSfx(0x58, value); }

        public int SFXx5C { get => GetSfx(0x5C); set => SetSfx(0x5C, value); }

        public int SFXx60 { get => GetSfx(0x60); set => SetSfx(0x60, value); }

        public int SFXx64 { get => GetSfx(0x64); set => SetSfx(0x64, value); }

        public int SFXx68 { get => GetSfx(0x68); set => SetSfx(0x68, value); }

        public int SFXx6C { get => GetSfx(0x6C); set => SetSfx(0x6C, value); }

        public int SFXx70 { get => GetSfx(0x70); set => SetSfx(0x70, value); }

        public int SFXx74 { get => GetSfx(0x74); set => SetSfx(0x74, value); }

        public int SFXx78 { get => GetSfx(0x78); set => SetSfx(0x78, value); }

        public int SFXx7C { get => GetSfx(0x7C); set => SetSfx(0x7C, value); }

        public int SFXx80 { get => GetSfx(0x80); set => SetSfx(0x80, value); }

        public int SFXx84 { get => GetSfx(0x84); set => SetSfx(0x84, value); }

        public int SFXx88 { get => GetSfx(0x88); set => SetSfx(0x88, value); }

        public int SFXx8C { get => GetSfx(0x8C); set => SetSfx(0x8C, value); }

        public int SFXx90 { get => GetSfx(0x90); set => SetSfx(0x90, value); }

        private int GetSfx(int offset)
        {
            return _s.GetInt32(offset);
        }

        private void SetSfx(int offset, int value)
        {
            _s.SetInt32(offset, value);
        }
    }
}
