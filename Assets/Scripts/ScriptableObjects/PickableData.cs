using UnityEngine;

[CreateAssetMenu(menuName = "PickableData", fileName = "PickableData", order = 0)]
public class PickableData : ScriptableObject
{
    public Sprite storedSprite;
    public Sprite displaySprite;
    [TextArea(2, 2)]
    public string instructionInList;
    public int boxNumber;
}
