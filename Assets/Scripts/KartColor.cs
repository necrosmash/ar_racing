using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartColor : MonoBehaviour
{
    public List<Color> colours = new List<Color>();
    public List<Color> coloursInUse = new List<Color>();

    // Create public variables for the colour and the renderer
    public Color carColour;

    public MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        colours.Add(Color.red);
        colours.Add(Color.green);
        colours.Add(Color.blue);
        colours.Add(Color.yellow);
        colours.Add(Color.cyan);
        colours.Add(Color.magenta);

        // Set the colour to a random colour from the list
        carColour = colours[Random.Range(0, colours.Count)];

        // for all childern
        /*foreach (Transform child in transform)
        {
            // if child has a mesh renderer
            if (child.GetComponent<MeshRenderer>() != null)
            {
                // Get the renderer
                meshRenderer = child.GetComponent<MeshRenderer>();

                // render the colour to the cat
                meshRenderer.material.color = carColour;
            }
        }*/
        foreach (Transform child in transform)
        {
            // get the mesh renderer of child
            meshRenderer = child.GetComponent<MeshRenderer>();
            // if child has a mesh renderer
            if (meshRenderer != null)
            {
                // render the colour to the cat
                meshRenderer.material.color = carColour;
            }
        }

        // get the childern of the child called "wings"
        foreach (Transform child in transform.Find("wings"))
        {
            // get the mesh renderer of child
            meshRenderer = child.GetComponent<MeshRenderer>();
            // if child has a mesh renderer
            if (meshRenderer != null)
            {
                // render the colour to the cat
                meshRenderer.material.color = carColour;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
