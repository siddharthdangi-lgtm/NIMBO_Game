using UnityEngine;

public class Collider_Ground3_L1 : MonoBehaviour
{
    [Header("Target Object to Scale")]
    public GameObject targetObject;

    [Header("Target Scale (X only)")]
    public float targetScaleX;
    public float targetPositionX;

    [Header("Lerp Duration")]
    public float duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetObject != null)
        {
            StartCoroutine(LerpScaleX(targetObject, targetScaleX, targetPositionX, duration));
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private System.Collections.IEnumerator LerpScaleX(GameObject obj, float targetX, float targetPosX, float time)
    {
        Vector3 startScale = obj.transform.localScale;
        Vector3 startPos = obj.transform.localPosition;
        Vector3 endScale = new Vector3(targetX, startScale.y, startScale.z);
        Vector3 endPos = new Vector3(targetPosX, startPos.y, startPos.z);
        float elapsed = 0f;

        while (elapsed < time)
        {
            obj.transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / time);
            obj.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = endScale;
        obj.transform.localPosition = endPos;
    }
}
