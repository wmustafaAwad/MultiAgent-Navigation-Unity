using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator : MonoBehaviour
{
    public List<unit> allUnits;
    // Start is called before the first frame update

    public List<unit> getallUnits()
    {
        return allUnits;
    }


    void Awake() {
        Debug.Log(name + " got the communicator and array of others with first element as " + allUnits[0].name); //Waleed
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
