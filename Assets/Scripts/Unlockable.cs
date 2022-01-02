using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public interface IUnlockable
{
    /*public Generator.Type _prerequisiteGenerator { get; set; }
    public Modifier.Type prerequisiteModifier{ get; set; }
    public int prerequisiteWorkers { get; set; }*/

    public void Unlock();



}
