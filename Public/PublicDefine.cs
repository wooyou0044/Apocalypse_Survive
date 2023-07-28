using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicDefine
{
    public enum eSceneType
    {
        StartScene = 0,
        FirstScene,
        MainScene,
        LobbyScene,
        IngameScene,
        MapScene,
        BankScene
    }

    public enum eBGMType
    {
        UIScene                 = 0,
        MapScene_Night,
        MapScene_Morning,
    }

    public enum eVoiceType
    {
        Player_Attack            = 0,
        Player_PickUp,
        Player_Hit,
        Player_JUMP,
        Player_Dead,

        Zombie_NormalIdle1     = 20,
        Zombie_NormalIdle2,
        Zombie_WALK,
        Zombie_RUN,
        Zombie_ATTACK1,
        Zombie_ATTACK2,
        Zombie_HIT,
        Zombie_DEATH,

    }

    public enum eEnemyKind
    {
        Zombie_Single           = 0,
        Zombie_Couple
    }

    public enum eActionState
    {
        IDLE                    = 0,
        MOVE,
        AIMING,
        ATTACK,
        PICKUP,
        MAKE,
        SLEEP,
        OPEN_DOOR,
        JUMP,
        DIE
    }

    public enum eEnemyActionState
    {
        IDLE                    = 0,
        WALK,
        RUN,
        BACKHOME,
        ATTACK,
        DEATH
    }

    public enum eWeaponKind
    {
        NONE                    = 0,
        KNIFE,
        GRENADE,
        AXE,
        HAMMER,
        GUN,
        RIFFLE,
        BAT,
    }

    public enum ePickUP_Location
    {
        LOW                     = 0,
        MIDDLE,
        HIGH
    }

    public enum eEffectSoundType
    {
        Button                  = 0,
        StartButton1,
        StartButton2,
        StartButton3,
        ManualButton,
        BankLockButton,
        LockOpen,
        LockError,
        FirstButton,
        SignUpNLoginButton,
        LockErase,
        Cut_Tree,
        Have_Gold,
        Fire_Gun,
        Fall_Gun_Empty_Bullet,
        Fire_Riffle,
        Fall_Riffle_Empty_Bullet,
        Reload,
        Explode_Grenade,
        Fall_On_Grass,
        Fall_In_Building,
        Throw_Grenade,
        Make_Fence,
        PickUp_Item,
        PickUp_Weapon,
        Eat_Item,
        Eat_Water,
        Move_Inventory,
        Attack_BatNKnifeNAxeNHammer,
        Zombie_Attack,
        Zombie2_Attack_Gun,
        Have_BulletEmpty,
    }

    public enum eItemType
    {
        Survive_Kit             = 0,
        Moisture,
        Clue,
        HPPotion,
        Ingredient
    }

    public enum eRoamingType
    {
        RandomPosition          = 0,
        RandomPoint,
        PatrolPoint,
        BackNForth
    }

    public enum eLocation
    {
        OutDoor                 = 0,
        InDoor,
    }

    public enum eBuildingType
    {
        Hospital                = 0,
        Bank,
        Map,
        ClearMap
    }
}
