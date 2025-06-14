using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
public class PancakeFlip : MonoBehaviour
{
    [Header("Baktijden")]
    public float bakeTime = 5f;
    public float tolerance = 1f;

    [Header("Flip-instellingen")]
    public float flipUpForce = 3f;
    public float spinTorque = 400f;

    [Header("Pan referentie")]
    public Transform panTransform;

    [Header("Game Logic")]
    public GameLogic gameLogic;

    [Header("Feedback")]
    public UnityEvent onPerfectFlip;
    public UnityEvent onBadFlip;
    public ParticleSystem successParticles;
    public ParticleSystem failParticles;
    public AudioClip successSFX;
    public AudioClip failSFX;

    [Header("UI")]
    public Canvas feedbackCanvas;
    public TextMeshProUGUI feedbackText;

    float timer;
    bool isBaking;
    bool isFlipped;
    InputDevice rightHand;
    bool lastSecondaryButtonState;

    AudioSource aSource;
    Renderer rend;
    Material pancakeMat;
    readonly Color rawColor = new(1f, 0.85f, 0.6f);
    readonly Color goldenBrownColor = new(0.8f, 0.5f, 0.2f);
    readonly Color brownColor = new(0.2f, 0.1f, 0.05f); // Darker brown color for burnt stage
    [Header("Perfect flash instellingen")]
    [Tooltip("Hoe snel de groene flash pulseert")]
    public float flashSpeed = 4f;

    void Awake()
    {
        aSource = GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        if (rend)
        {
            pancakeMat = rend.material;
            pancakeMat.color = rawColor;
        }

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0) rightHand = devices[0];

        if (feedbackCanvas) feedbackCanvas.enabled = false;
    }

    void Update()
    {
        if (!isBaking || isFlipped)
            return;

        // Versnelde timer (diviseer om langzamer te bakken)
        timer += Time.deltaTime / 4f;

        float underThreshold = bakeTime - tolerance;
        float overThreshold = bakeTime + tolerance;

        if (timer < underThreshold)
        {
            // 1) Onderbakt: raw → golden
            float t = timer / underThreshold;
            pancakeMat.color = Color.Lerp(rawColor, goldenBrownColor, t);
        }
        else if (timer <= overThreshold)
        {
            // 2) Perfect window: pulse tussen golden en groen
            float flash = Mathf.PingPong((timer - underThreshold) * flashSpeed, 1f);
            pancakeMat.color = Color.Lerp(goldenBrownColor, Color.green, flash);
        }
        else
        {
            // 3) Verbrand fase: brown → dark burnt, over dezelfde duur als onderThreshold
            float burntT = (timer - overThreshold) / underThreshold;
            burntT = Mathf.Clamp01(burntT);
            Color burntShade = new Color(0.1f, 0.05f, 0.05f);
            pancakeMat.color = Color.Lerp(brownColor, burntShade, burntT);
        }

        // Flip-input (B-knop)
        if (rightHand.isValid &&
            rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out bool state))
        {
            if (state && !lastSecondaryButtonState)
                AttemptFlip();
            lastSecondaryButtonState = state;
        }
    }




    public void StartBaking()
    {
        timer = 0f;
        isBaking = true;
        isFlipped = false;
        lastSecondaryButtonState = false;
        if (feedbackCanvas) feedbackCanvas.enabled = false; // feedback verbergen bij nieuwe poging
        if (pancakeMat != null)
            pancakeMat.color = rawColor;
    }

    void AttemptFlip()
    {
        if (isFlipped) return;
        isFlipped = true;

        float diff = Mathf.Abs(timer - bakeTime);
        bool perfect = diff <= tolerance;

        if (perfect)
        {
            onPerfectFlip.Invoke();
            successParticles?.Play();
            aSource.PlayOneShot(successSFX);
            ShowFeedback("PERFECT!", Color.green);
            gameLogic?.GameComplete();
        }
        else
        {
            onBadFlip.Invoke();
            failParticles?.Play();
            aSource.PlayOneShot(failSFX);
            ShowFeedback(
                (timer > bakeTime
                    ? "TE LAAT! Probeer opnieuw.\nBreng de kom opnieuw naar de pan."
                    : "TE VROEG! Probeer opnieuw.\nBreng de kom opnieuw naar de pan."),
                Color.red);

            Destroy(gameObject, 1.5f); // verwijder pannenkoek na 1.5 seconden

            if (gameLogic != null)
                gameLogic.PrepareRetry(); // laat opnieuw beslag gieten
        }


        PerformFlip();
    }

    void PerformFlip()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (panTransform != null)
            transform.position = new Vector3(
                panTransform.position.x,
                transform.position.y,
                panTransform.position.z
            );

        Vector3 upImp = Vector3.up * flipUpForce;
        rb.AddForce(upImp, ForceMode.Impulse);
        rb.AddRelativeTorque(Vector3.right * spinTorque, ForceMode.Impulse);
    }

    void ShowFeedback(string message, Color col)
    {
        if (feedbackCanvas && feedbackText)
        {
            feedbackText.text = message;
            feedbackText.color = col;
            feedbackCanvas.enabled = true;
        }
    }
}