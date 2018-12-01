﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameEvents.OnMapBoundHit != null) GameEvents.OnMapBoundHit();
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameEvents.OnMapBoundLeave != null) GameEvents.OnMapBoundLeave();
    }
}