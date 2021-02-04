using UnityEngine;

public class FogCameraSpawner : MonoBehaviour
{
    public FogCamera fogCamera;
    public CamController camController;
    public float separation;
    public float height;

    private void Awake()
    {
        float distx = Vector3.Distance(new Vector3(camController.minlimitx, 0, 0), new Vector3(camController.maxlimitx, 0, 0)) / separation;
        float distz = Vector3.Distance(new Vector3(0, 0, camController.minlimity), new Vector3(0, 0, camController.maxlimity)) / separation;

        for (int i = Mathf.RoundToInt(-distx) + 1; i < distx; i++)
        {
            for (int j = Mathf.RoundToInt(-distz) + 1; j < distz; j++)
            {
                FogCamera newFogCam = Instantiate(fogCamera);
                newFogCam.transform.parent = transform;
                newFogCam.transform.position = new Vector3(i * separation, height, j * separation);
            }
        }
    }
}
