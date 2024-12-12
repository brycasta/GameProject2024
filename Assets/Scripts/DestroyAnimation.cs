using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public void OnAnimationComplete() // Call this function at the end of the animation
    {
        Destroy(gameObject);
    }
}

