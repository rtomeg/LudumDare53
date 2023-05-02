using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StageData", fileName = "StageData", order = 0)]
public class StageData : ScriptableObject
{
    public GameManager.Stage stage;
    public List<PickableData> objects;
    
    [TextArea(10, 2)]
    public string title;
    [TextArea(10, 10)]
    public string instructions;
}
