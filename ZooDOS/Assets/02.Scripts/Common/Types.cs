﻿// enum 모음

public enum PlayerUnitType
{
    Nothing,
    Defender,
    Sniper,
    Medic,
}

public enum AttackType
{
    Nothing,
    Damage,
    Heal,
}

public enum EnemyUnitType
{
    Nothing,
    ShortRange,
    LongRange,
}

public enum TileType
{
    None,
    Restricted,
    Ground,
    Hill,
    EnemyEntryPoint,
    DefensePoint,
    RestrictedGround,
}
