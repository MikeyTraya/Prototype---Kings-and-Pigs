using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingsAndPigs
{
    public class ContinuesDamage : MonoBehaviour
    {
        public static bool IsDamaged { get; private set; }
        private bool _isBeingDamaged = false;

        private void Update()
        {
            if (_isBeingDamaged)
            {
                StartCoroutine(DamageCooldown());
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _isBeingDamaged = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _isBeingDamaged = false;
        }

        IEnumerator DamageCooldown()
        {
            IsDamaged = true;
            yield return new WaitForSeconds(.1f);
            IsDamaged = false;
        }
    }
}
