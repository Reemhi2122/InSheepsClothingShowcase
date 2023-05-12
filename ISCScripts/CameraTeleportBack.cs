using InSheepsClothing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTeleportBack : TeleportBack
{
    InSheepsClothing.Interaction.Camera camera = null;

    new private void Awake()
    {
        base.Awake();
        camera = GetComponent<InSheepsClothing.Interaction.Camera>();
    }

    protected override void ReturnToInitalPosition()
    {
        camera.EjectPicture();
        base.ReturnToInitalPosition();
    }
}
