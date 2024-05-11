using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPerkable 
{
    string Name { get; }
    string Description { get; }
    int Cost { get; }
    void Execute(TowerStats towerStats);
}
