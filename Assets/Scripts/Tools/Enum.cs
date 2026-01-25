
// transition type
public enum TransitionType
{
    SameScene,
    DifferentScene
}

// detination tag
public enum DestinationTag
{
    ENTER,
    A,
    B,
    C
}

// enemy state
public enum EnemyState
{
    GUARD,
    PATROL,
    CHASE,
    DEAD
}

//item type
public enum ItemType
{
    USEABLE,
    WEAPON,
    ARMOR
}

// slot type
public enum SlotType
{
    BAG,
    WEAPON,
    ARMOR,
    ACTION
}

//Rock state
public enum RockState
{
    HitPlayer,
    HitEnemy,
    HitNothing
}

public enum HurtState
{
    Nothing,
    CriticalHurt,
    DizzyHurt,
}


public enum PlayerPosture
{
    Standing,
    Jumping,
    Peaking,
    Falling,
    Landing,
};

public enum PlayerLocomotion
{
    Idle,
    Walk,
    Run,
};

public enum PlayerAttack
{
    Normal,
    Attack,
};



