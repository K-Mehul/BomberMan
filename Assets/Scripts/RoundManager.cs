using System.Collections;
using UnityEngine;
using TMPro; // Use this if using TextMeshPro

public class RoundManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private float displayTime = 2f;
    [SerializeField] private AudioClip roundSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        ShowRound();
    }

    public void ShowRound()
    {
        roundText.text = $"Find a door";
        roundText.gameObject.SetActive(true);
        StartCoroutine(AnimateRoundText());

        if (roundSound != null)
        {
            audioSource.PlayOneShot(roundSound);
        }
    }

    private IEnumerator AnimateRoundText()
    {
        // Reset scale and opacity
        roundText.transform.localScale = Vector3.zero;
        roundText.alpha = 0f;

        // Scale up and fade in
        float timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.5f;
            roundText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            roundText.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        // Wait displayTime seconds
        yield return new WaitForSeconds(displayTime);

        // Hide after
        roundText.gameObject.SetActive(false);
    }
}
