using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PokemonDamageMessage
{
    public enum BattleOrder { First, After }

    string skillName;
    int skillDamage;
    BattleOrder battleOrder;
    //PokemonSkillType _skillType;
}
