using System;
using UnityEngine;

namespace TW.Scripts
{
    public class Item : MonoBehaviour
    {
        public static int Count;


        public static void ResetCount()
        {
            Count = 0;
        }

        public ParticleSystem particles;

        private bool isScored;

        private void OnEnable()
        {
            Count++;
        }

        public void Score()
        {
            if (isScored) return;
            Count--;
            isScored = true;
            Game.Instance.Score();

            if (particles != null)
            {
                var spawn = Instantiate(particles, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}