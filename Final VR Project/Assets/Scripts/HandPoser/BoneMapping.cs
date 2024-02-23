using UnityEngine;

[System.Serializable]
public class BoneObject
{
    public Transform[] targetBones = new Transform[0];
    public Transform[] destinationBones = new Transform[0];
}

public class BoneMapping : MonoBehaviour
{
    [Range(0f, 1f), SerializeField] private float Weight = 1f;
    [SerializeField] private BoneObject[] Fingers;

    private bool ShowGizmos = true;

    private void Update()
    {
        if (Weight <= 0f)
        {
            return;
        }

        for (int x = 0; x < Fingers.Length; x++)
        {
            BoneObject finger = Fingers[x];

            if (finger == null)
            {
                continue;
            }

            for (int i = 0; i < finger.destinationBones.Length - 1; i++)
            {
                Quaternion f = Quaternion.Inverse(finger.destinationBones[i].rotation) * finger.targetBones[i].rotation;

                if (Weight < 1f)
                {
                    f = Quaternion.Slerp(Quaternion.identity, f, Weight);
                }

                finger.destinationBones[i].rotation *= f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmos || Fingers == null)
        {
            return;
        }

        for (int x = 0; x < Fingers.Length; x++)
        {
            BoneObject finger = Fingers[x];

            for (int i = 0; i < finger.targetBones.Length; i++)
            {

                if (finger.targetBones[i] == null)
                {
                    continue;
                }

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(finger.targetBones[i].position, 0.003f);

                if (i < finger.targetBones.Length - 1)
                {
                    if (finger.targetBones[i] == null || finger.targetBones[i + 1] == null)
                    {
                        continue;
                    }
                    Gizmos.DrawLine(finger.targetBones[i].position, finger.targetBones[i + 1].position);
                }
            }

            for (int i = 0; i < finger.destinationBones.Length; i++)
            {
                if (finger.destinationBones[i] == null)
                {
                    continue;
                }

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(finger.destinationBones[i].position, 0.003f);

                if (i < finger.destinationBones.Length - 1)
                {
                    if (finger.destinationBones[i] == null || finger.destinationBones[i + 1] == null)
                    {
                        continue;
                    }
                    Gizmos.DrawLine(finger.destinationBones[i].position, finger.destinationBones[i + 1].position);
                }
            }
        }
    }
}
