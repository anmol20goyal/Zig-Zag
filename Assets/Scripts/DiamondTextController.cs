using UnityEngine;

public class DiamondTextController : MonoBehaviour
{
    private static DiamondText DiamondText;
    private static GameObject Canvas;
    
    public static void Initialize()
    {
        Canvas = GameObject.Find("Canvas");
        DiamondText = Resources.Load<DiamondText>("Prefabs/DiamondTextParent");
    }

    public static void createFloatingText(Transform location)
    {
        DiamondText instance = Instantiate(DiamondText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(Canvas.transform,false);
        instance.transform.position = screenPosition;
    }
}
