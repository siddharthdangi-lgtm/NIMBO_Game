using UnityEngine;

public class Collider_Platform1 : MonoBehaviour
{
    [Header("Target Object to Scale")]
    public GameObject targetObject;

    [Header("Target Position (X only)")]
    public float targetPositionX;

    [Header("Lerp Duration")]
    public float duration;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetObject != null)
        {
            StartCoroutine(LerpScaleX(targetObject, targetPositionX, duration));
            
        }
    }

    private System.Collections.IEnumerator LerpScaleX(GameObject obj, float targetPosX, float time)
    {
        Vector3 startPos = obj.transform.localPosition;
        Vector3 endPos = new Vector3(targetPosX, startPos.y, startPos.z);
        float elapsed = 0f;

        while (elapsed < time)
        {
            obj.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.localPosition = endPos;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
