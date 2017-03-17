using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleCube : Interactible
{

    protected override void GazeEntered()
    {
        base.GazeEntered();

        transform.Rotate(new Vector3(45, 45, 45));
    }

    protected override void GazeExited()
    {
        base.GazeExited();

        transform.Rotate(new Vector3(-45, -45, -45));
    }

    protected override void OnSelect()
    {
        base.OnSelect();

        Destroy(this.gameObject);
    }

}
