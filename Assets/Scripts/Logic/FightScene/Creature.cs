using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Creature : MonoBehaviour
{

}

public class CreatureDatabase:TableDatabase
{
    public string Name;
    public string ModelPath;
    public List<int> SkillList;
}