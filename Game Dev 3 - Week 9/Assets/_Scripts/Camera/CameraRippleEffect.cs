using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevWithhanniel.DesignPattern;


namespace GameDevWithhanniel.CameraStuff
{
    public class CameraRippleEffect : Singleton<CameraRippleEffect>
    {
        //Ripple Variables
        public Material RippleMaterial;
        public float MaxAmount = 50f;

        [Range(0, 1)]
        public float Friction = .9f;
        private float Amount = 0f;

        [HideInInspector] public Vector2 ripplePos;
        private Vector3 pos;
        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();
            pos = cam.WorldToScreenPoint(ripplePos);
        }
        void Update()
        {
            pos = cam.WorldToScreenPoint(ripplePos);
            this.RippleMaterial.SetFloat("_Amount", this.Amount);
            this.Amount *= this.Friction;
        }

        public void Ripple(Vector2 ripplePosition)
        {
            this.ripplePos = ripplePosition;
            this.Amount = this.MaxAmount;
            this.RippleMaterial.SetFloat("_CenterX", pos.x);
            this.RippleMaterial.SetFloat("_CenterY", pos.y);
        }

        void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            Graphics.Blit(src, dst, this.RippleMaterial);
        }
    }

}


