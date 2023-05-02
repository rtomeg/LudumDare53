using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static Action <Pickable> pickablePicked;
    public static Action <Pickable> pickableUsed;
    public static Action <Pickable> pickableKeeped;
}
