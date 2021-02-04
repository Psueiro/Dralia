using UnityEngine;

public class FoVAdjuster : MonoBehaviour
{
    Stats parent;

    private void Start()
    {
        parent = GetComponentInParent<Stats>();
    }

    private void Update()
    {
        Adjust();
    }

    public void Adjust()
    {
        transform.GetChild(0).localScale = new Vector3(parent.lineofSightHitboxRadiusFog + transform.position.y, parent.lineofSightHitboxRadiusFog + transform.position.y, 1);
        transform.GetChild(1).localScale = new Vector3(parent.lineofSightRadius + transform.position.y, parent.lineofSightRadius + transform.position.y, 1);
        transform.GetChild(transform.childCount - 1).localScale = new Vector3(parent.lineofSightHitboxRadius, 0.01f, parent.lineofSightHitboxRadius);
    }
}
