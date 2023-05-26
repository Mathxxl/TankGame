using System.Collections;
using UnityEngine;

namespace Entities.Player.Ultimate
{
    public class UltimateInvincible : Ultimate
    {
        private MortalEntity _thisEntity;
        protected override void DoUltimate()
        {
            if (entity is not MortalEntity tempEntity) return;
            if (data.duration <= 0) return;

            _thisEntity = tempEntity;
            StartCoroutine(Doing());
        }

        private IEnumerator Doing()
        {
            _thisEntity.Invincible = true;
            yield return new WaitForSeconds(data.duration);
            _thisEntity.Invincible = false;
        }
    }
}
