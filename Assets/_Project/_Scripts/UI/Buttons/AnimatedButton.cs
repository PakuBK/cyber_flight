using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimatedButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextField;
    [SerializeField] [Range(1f, 5f)]
    private float speed = 1f;
    [SerializeField] [Range(1f, 10f)]
    private float characterSpacingScale = 5f;
    private float timer;
    private void OnEnable() {
        StartCoroutine(TextSpreadAnimation());
    }
    
    private void OnDisable() {
        StopAllCoroutines();
    }


    private IEnumerator TextSpreadAnimation() {
        while (true) {
            timer += Time.deltaTime;
            TextField.characterSpacing = Mathf.Abs(oscillate(timer, speed, characterSpacingScale));
            yield return null;
        }
    }

    float oscillate(float time, float speed, float scale)
    {
        return Mathf.Cos(time * speed / Mathf.PI) * scale;
    }
}
