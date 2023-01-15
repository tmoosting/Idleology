using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class InnovationManager : MonoBehaviour
{

    public ContentUI contentUI;
    public GameObject innovationBodyPrefab;

    private List<Innovation> _innovationsInfluence;
    private List<Innovation> _innovationsForce;
    private List<Innovation> _innovationsTech;
    private List<InnovationBody> _innovationBodies;

    public void InitializeInnovations(bool newGame)
    {
        _innovationBodies = new List<InnovationBody>();
        // load all Innovations from resources folder
        LoadInnovationsFromResources();
        
        // place in UI
 
        foreach (var innov in _innovationsInfluence)
            SpawnInnovationBody(innov, contentUI.innovationsParentInfluence);
        foreach (var innov in _innovationsForce)
            SpawnInnovationBody(innov, contentUI.innovationsParentForce);
        foreach (var innov in _innovationsTech)
            SpawnInnovationBody(innov, contentUI.innovationsParentTech);
    }  
    private void LoadInnovationsFromResources()
    {
        _innovationsInfluence = new List<Innovation>();
        _innovationsForce = new List<Innovation>();
        _innovationsTech = new List<Innovation>();
        
        // Influence
        string path = "Innovations/Influence"; 
        Object[] influenceObjects = Resources.LoadAll(path);
        Innovation[] influences = new Innovation[influenceObjects.Length];
        influenceObjects.CopyTo(influences, 0);
        _innovationsInfluence = influences.ToList();  
        
        // Force
         path = "Innovations/Force"; 
        Object[] forceObjects = Resources.LoadAll(path);
        Innovation[] forces = new Innovation[forceObjects.Length];
        forceObjects.CopyTo(forces, 0);
        _innovationsForce = forces.ToList(); 
        
        // Tech
        path = "Innovations/Tech"; 
        Object[] techObjects = Resources.LoadAll(path);
        Innovation[] techs = new Innovation[techObjects.Length];
        techObjects.CopyTo(techs, 0);
        _innovationsTech = techs.ToList(); 
        
    } 
    private void SpawnInnovationBody(Innovation innov, Transform parent)
    {
        GameObject obj = Instantiate(innovationBodyPrefab, parent);
        obj.GetComponent<InnovationBody>().LoadInnovation(innov);
        _innovationBodies.Add( obj.GetComponent<InnovationBody>());
    }
    public void ValidateInnovationBodies()
    {
        foreach (InnovationBody body in _innovationBodies)
            if (body != null)
              body.ValidateInnovationBody();
    }

    
    
    public void HandleInnovationPurchaseEffects(Innovation innovation)
    {
        Debug.Log("IMMEDIATE EFFECT for " + innovation.name);
    }


    public List<Innovation> GetAllInnovations()
    {
        List<Innovation> innovations = _innovationsInfluence.ToList();
        innovations.AddRange(_innovationsForce);
        innovations.AddRange(_innovationsTech);
        return innovations;
    }
}
