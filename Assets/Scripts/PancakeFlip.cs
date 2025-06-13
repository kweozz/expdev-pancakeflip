    using UnityEngine;
    using UnityEngine.XR;
    using System.Collections.Generic;

    [RequireComponent(typeof(Rigidbody))]
    public class PancakeFlip : MonoBehaviour
    {
        [Header("Baktijden")]
        public float bakeTime  = 5f;
        public float tolerance = 1f;

        [Header("Flip-instellingen")]
        [Tooltip("Opwaartse impuls (lager = zachter)")]
        public float flipUpForce      = 3f;
        [Tooltip("Voorwaartse impuls (lager = kortere boog)")]
        public float flipForwardForce = 1f;
        [Tooltip("Rotatietorque (lager = minder spin)")]
        public float spinTorque       = 400f;

        // intern
        float timer;
        bool  isBaking;
        bool  canFlip;
        bool  isFlipped;

        // XR‐input
        InputDevice rightHand;
        bool        lastSecondaryButtonState;

        // visuals
        Renderer rend;
        Material pancakeMat;
        readonly Color rawColor   = new(1f, 0.85f, 0.6f);
        readonly Color brownColor = new(0.4f, 0.2f, 0.05f);

        void Awake()
        {
            // Materiaal‐instance
            rend = GetComponent<Renderer>();
            if (rend)
            {
                pancakeMat       = rend.material;
                pancakeMat.color = rawColor;
            }

            // Physics aanzetten
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity  = true;

            // Rechter controller opzoeken
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
            if (devices.Count > 0) rightHand = devices[0];
        }

        void Update()
        {
            if (!isBaking || isFlipped) return;

            // Bak‐timer + visuele bruining
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / bakeTime);
            if (pancakeMat != null)
                pancakeMat.color = Color.Lerp(rawColor, brownColor, t);

            // Vanaf binnen tolerance mag je flippen
            if (timer >= bakeTime - tolerance)
                canFlip = true;

            // Lees B‐knop van controller
            if (canFlip &&
                rightHand.isValid &&
                rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out bool state))
            {
                if (state && !lastSecondaryButtonState)
                    AttemptFlip();
                lastSecondaryButtonState = state;
            }
        }

        /// <summary>
        /// Start het bakproces; roep aan vanuit PourBatter.SpawnPancake()
        /// </summary>
        public void StartBaking()
        {
            timer                     = 0f;
            isBaking                  = true;
            canFlip                   = false;
            isFlipped                 = false;
            lastSecondaryButtonState  = false;
        }

        void AttemptFlip()
        {
            if (!canFlip || isFlipped) return;
            isFlipped = true;

            float diff = Mathf.Abs(timer - bakeTime);
            if (diff <= tolerance)
                Debug.Log("✅ Perfect flip!");
            else if (timer > bakeTime + tolerance)
                Debug.Log("🔥 Te laat – verbrand!");
            else
                Debug.Log("🤢 Te vroeg – rauw!");

            PerformFlip();
        }

        void PerformFlip()
        {
            var rb = GetComponent<Rigidbody>();
            // Reset velocity voor consistente flips
            rb.linearVelocity        = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Impulsen: omhoog + een tikje vooruit
            Vector3 upImp  = transform.up      * flipUpForce;
            Vector3 fwdImp = transform.forward * flipForwardForce;
            rb.AddForce(upImp + fwdImp, ForceMode.Impulse);

            // Rotatie om eigen X‐as
            rb.AddRelativeTorque(Vector3.right * spinTorque, ForceMode.Impulse);
        }
    }