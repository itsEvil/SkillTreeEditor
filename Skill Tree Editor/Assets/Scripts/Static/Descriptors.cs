public enum NodeReward
{
    None,        //defualt
    STAT,        //Stats like below
    NODE_EFFECT, //Item effects but in skill tree
    DAMAGE, //Weapon / Ability damage increase
    RATE_OF_FIRE, //fire rate of weapon
    RESOURCE_COST, //resource cost
    ABILITY_CAST, //Chance to cast an ability based on action

}
public enum Stats : int
{
    Health,
    Mana,
    Strength,
    Dexterity,
    Intelligence,
    Vitality,
    Armor,
    Evasion,
    CritDamage,
    CritChance,
}

public enum DamageType : int
{
    Weapon,
    Ability,
}

public enum CastActions : int
{
    Shooting,
    Hitting,
    Killing,
    GettingHit,
}

public enum AbilityCasts : int
{
    Fireball, //Shoots a fireball projectile which deals aoe damage after 5 tiles or hitting an enemy.
    Storm,    //spawns a storm where enemy is / died
    AcidFloor,//spawns acid floor where enemy is / died
    Enrage,   //makes player larger, and deal more damage for a short period upon getting hit
}

public enum NodeEffects : int
{
    ExtraConsumption, //x% chance to use ability multiple times (100ms between extra casts)
    BloodConversion,  //Using ability costs hp if you have no mana
    StoneDefense,     //Standing still increases your defense by X or X% 
    SizeChange,       //Larger/Smaller //Larger deals more dmg and has more hp but loses some rof and speed while smaller loses hp and dmg for rof and spd
    CooldownReduction,//Chance to remove X seconds or X% of a cooldown when using ability
    Shotgun,          //Chance to shoot a shotgun arc of (currently equipped) projectiles towards cursor which only go current range or 3.5 tiles whichever is smaller
}