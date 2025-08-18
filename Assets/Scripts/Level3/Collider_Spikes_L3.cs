using UnityEngine;

public class Collider_Spikes_L3 : MonoBehaviour
{
    [Header("Target Object to Scale")]
    public GameObject targetObject;

    [Header("Target Pos (Y only)")]
    public float targetPositionY;

    [Header("Lerp Duration")]
    public float duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetObject != null)
        {
            StartCoroutine(LerpScaleX(targetObject, targetPositionY, duration));
            targetObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private System.Collections.IEnumerator LerpScaleX(GameObject obj, float targetPosY, float time)
    {
        Vector3 startPos = obj.transform.localPosition;
        Vector3 endPos = new Vector3(startPos.x, targetPosY, startPos.z);
        float elapsed = 0f;

        while (elapsed < time)
        {
            obj.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.localPosition = endPos;
    }
}
