using UnityEngine;

public class DiamondText : MonoBehaviour
{
    public Animator Animator;
    
    // Start is called before the first frame update
    void Start()
    {
        AnimatorClipInfo[] clipInfo = Animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject,clipInfo[0].clip.length);
    }
}
