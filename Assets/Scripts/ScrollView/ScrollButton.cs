using UnityEngine;

public class ScrollButton : MonoBehaviour
{
    public void OnClick()
    {
        Instantiate(this.gameObject, this.transform.parent);
    }
}