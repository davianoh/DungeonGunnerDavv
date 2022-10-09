using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializeEffect : MonoBehaviour
{
    public IEnumerator MaterializeRoutine(Shader materializeShader, Color materializeColor, float materializeTime, SpriteRenderer[] spriteRendererArray, Material normalMaterial)
    {
        Material materializeMaterial = new Material(materializeShader);
        materializeMaterial.SetColor("_EmissionColor", materializeColor);

        foreach(SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = materializeMaterial;
        }

        float dissolveAmmount = 0f;
        while(dissolveAmmount < 1f)
        {
            dissolveAmmount += Time.deltaTime / materializeTime;
            materializeMaterial.SetFloat("_DissolveAmount", dissolveAmmount);
            yield return null;
        }

        foreach(SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = normalMaterial;
        }
    }

    public IEnumerator WarningSpawnRoutine(SpriteRenderer warningIcon, float warningTime, float warningInterval, SpriteRenderer[] spriteRendererArray, Material normalMaterial)
    {
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.enabled = false;
        }

        while (warningTime > 0)
        {
            warningIcon.enabled = true;
            warningTime -= warningInterval;
            yield return new WaitForSeconds(warningInterval);
            warningIcon.enabled = false;
            warningTime -= warningInterval;
            yield return new WaitForSeconds(warningInterval);
        }

        foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.material = normalMaterial;
        }
        warningIcon.enabled = false;
    }
}
