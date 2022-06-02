using System.Collections.Generic;
using HarmonyLib;
using ModdingUtils.MonoBehaviours;
using UnityEngine;
using System.Reflection;
using UnboundLib;

namespace ModdingUtils.Extensions
{

    public class CharacterDataModifier
    {
        public float health_add = 0f;
        public float health_mult = 1f;
        public float maxHealth_add = 0f;
        public float maxHealth_mult = 1f;

        private float health_delta = 0f;
        private float maxHealth_delta = 0f;
        public static void ApplyCharacterDataModifier(CharacterDataModifier characterDataModifier, CharacterData data)
        {
            characterDataModifier.ApplyCharacterDataModifier(data);
        }
        public void ApplyCharacterDataModifier(CharacterData data)
        {
            health_delta = data.health * health_mult + health_add - data.health;
            maxHealth_delta = data.maxHealth * maxHealth_mult + maxHealth_add - data.maxHealth;

            data.health += health_delta;
            data.maxHealth += maxHealth_delta;

            // update player stuff
            if (this.health_delta != 0f || this.maxHealth_delta != 0f)
            {
                typeof(CharacterStatModifiers).InvokeMember("ConfigureMassAndSize",
                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                    BindingFlags.NonPublic, null, data.stats, new object[] { });
            }

        }
        public static void RemoveCharacterDataModifier(CharacterDataModifier characterDataModifier, CharacterData data, bool clear = true)
        {
            characterDataModifier.RemoveCharacterDataModifier(data, clear);
        }
        public void RemoveCharacterDataModifier(CharacterData data, bool clear = true)
        {
            var a = data.health;
            var b = data.maxHealth;
            var c = data.maxHealth - maxHealth_delta;
            var target_health = (a * c) / b;
            health_delta = data.health - target_health;
            data.health -= health_delta;
            data.maxHealth -= maxHealth_delta;

            // update player stuff
            if (this.health_delta != 0f || this.maxHealth_delta != 0f)
            {
                typeof(CharacterStatModifiers).InvokeMember("ConfigureMassAndSize",
                    BindingFlags.Instance | BindingFlags.InvokeMethod |
                    BindingFlags.NonPublic, null, data.stats, new object[] { });
            }

            // reset deltas

            if (clear)
            {
                health_delta = 0f;
                maxHealth_delta = 0f;
            }

        }
    }
    public class BlockModifier
    {
        public List<GameObject> objectsToSpawn_add = new List<GameObject>();
        public float counter_add = 0f;
        public float counter_mult = 1f;
        public float cdMultiplier_add = 0f;
        public float cdMultiplier_mult = 1f;
        public float cdAdd_add = 0f;
        public float cdAdd_mult = 1f;
        public float forceToAdd_add = 0f;
        public float forceToAdd_mult = 1f;
        public float forceToAddUp_add = 0f;
        public float forceToAddUp_mult = 1f;
        public int additionalBlocks_add = 0;
        public int additionalBlocks_mult = 1;
        public float healing_add = 0f;
        public float healing_mult = 1f;

        private float counter_delta;
        private float cdMultiplier_delta;
        private float cdAdd_delta;
        private float forceToAdd_delta;
        private float forceToAddUp_delta;
        private int additionalBlocks_delta;
        private float healing_delta;

        public static void ApplyBlockModifier(BlockModifier blockModifier, Block block)
        {
            blockModifier.ApplyBlockModifier(block);
        }
        public void ApplyBlockModifier(Block block)
        {
            counter_delta = block.counter * counter_mult + counter_add - block.counter;
            cdMultiplier_delta = block.cdMultiplier * cdMultiplier_mult + cdMultiplier_add - block.cdMultiplier;
            cdAdd_delta = block.cdAdd * cdAdd_mult + cdAdd_add - block.cdAdd;
            forceToAdd_delta = block.forceToAdd * forceToAdd_mult + forceToAdd_add - block.forceToAdd;
            forceToAddUp_delta = block.forceToAddUp * forceToAddUp_mult + forceToAddUp_add - block.forceToAddUp;
            additionalBlocks_delta = block.additionalBlocks * additionalBlocks_mult + additionalBlocks_add - block.additionalBlocks;
            healing_delta = block.healing * healing_mult + healing_add - block.healing;

            foreach (GameObject objectToSpawn in objectsToSpawn_add)
            {
                block.objectsToSpawn.Add(objectToSpawn);
            }

            block.counter += counter_delta;
            block.cdMultiplier += cdMultiplier_delta;
            block.cdAdd += cdAdd_delta;
            block.forceToAdd += forceToAdd_delta;
            block.forceToAddUp += forceToAddUp_delta;
            block.additionalBlocks += additionalBlocks_delta;
            block.healing += healing_delta;
        }
        public static void RemoveBlockModifier(BlockModifier blockModifier, Block block, bool clear = true)
        {
            blockModifier.RemoveBlockModifier(block, clear);
        }
        public void RemoveBlockModifier(Block block, bool clear = true)
        {
            foreach (GameObject objectToSpawn in objectsToSpawn_add)
            {
                block.objectsToSpawn.Remove(objectToSpawn);
            }

            block.counter -= counter_delta;
            block.cdMultiplier -= cdMultiplier_delta;
            block.cdAdd -= cdAdd_delta;
            block.forceToAdd -= forceToAdd_delta;
            block.forceToAddUp -= forceToAddUp_delta;
            block.additionalBlocks -= additionalBlocks_delta;
            block.healing -= healing_delta;

            // reset deltas

            if (clear)
            {
                objectsToSpawn_add = new List<GameObject>();
                counter_delta = 0f;
                cdMultiplier_delta = 0f;
                cdAdd_delta = 0f;
                forceToAdd_delta = 0f;
                forceToAddUp_delta = 0f;
                additionalBlocks_delta = 0;
                healing_delta = 0f;
            }
        }

    }
    public class GunAmmoStatModifier
    {
        public int maxAmmo_mult = 1;
        public int maxAmmo_add = 0;
        public float reloadTimeMultiplier_mult = 1f;
        public float reloadTimeAdd_add = 0f;
        public int currentAmmo_mult = 1;
        public int currentAmmo_add = 0;

        private int maxAmmo_delta;
        private float reloadTimeMultiplier_delta;
        private float reloadTimeAdd_delta;
        public int currentAmmo_delta;

        public static void ApplyGunAmmoStatModifier(GunAmmoStatModifier gunAmmoStatModifier, GunAmmo gunAmmo)
        {
            gunAmmoStatModifier.ApplyGunAmmoStatModifier(gunAmmo);
        }
        public void ApplyGunAmmoStatModifier(GunAmmo gunAmmo)
        {
            maxAmmo_delta = gunAmmo.maxAmmo * maxAmmo_mult + maxAmmo_add - gunAmmo.maxAmmo;
            reloadTimeMultiplier_delta = gunAmmo.reloadTimeMultiplier * reloadTimeMultiplier_mult - gunAmmo.reloadTimeMultiplier;
            reloadTimeAdd_delta = reloadTimeAdd_add;
            currentAmmo_delta = (int)gunAmmo.GetFieldValue("currentAmmo") * currentAmmo_mult + currentAmmo_add - (int)gunAmmo.GetFieldValue("currentAmmo");


            gunAmmo.maxAmmo += maxAmmo_delta;
            gunAmmo.reloadTimeMultiplier += reloadTimeMultiplier_delta;
            gunAmmo.reloadTimeAdd += reloadTimeAdd_delta;
            gunAmmo.SetFieldValue("currentAmmo", (int)gunAmmo.GetFieldValue("currentAmmo") + currentAmmo_delta);

            // if the gun is currently reloading, then set lastMaxAmmo to be the same as MaxAmmo to prevent the bullets from being drawn over the reload ring
            if (((Gun)Traverse.Create(gunAmmo).Field("gun").GetValue()).isReloading)
            {
                Traverse.Create(gunAmmo).Field("lastMaxAmmo").SetValue(gunAmmo.maxAmmo);
            }
            // if the current ammo was changed, redraw the bullet icons
            if (currentAmmo_delta != 0) { GunAmmoStatModifier.ReDrawCurrentBullets(gunAmmo); }
        }
        public static void RemoveGunAmmoStatModifier(GunAmmoStatModifier gunAmmoStatModifier, GunAmmo gunAmmo, bool clear = true)
        {
            gunAmmoStatModifier.RemoveGunAmmoStatModifier(gunAmmo, clear);
        }
        public void RemoveGunAmmoStatModifier(GunAmmo gunAmmo, bool clear = true)
        {
            gunAmmo.maxAmmo -= maxAmmo_delta;
            gunAmmo.reloadTimeMultiplier -= reloadTimeMultiplier_delta;
            gunAmmo.reloadTimeAdd -= reloadTimeAdd_delta;
            gunAmmo.SetFieldValue("currentAmmo", UnityEngine.Mathf.Clamp((int)gunAmmo.GetFieldValue("currentAmmo") - currentAmmo_delta, 0, int.MaxValue));

            bool flag = currentAmmo_delta != 0;

            // reset deltas

            if (clear)
            {
                maxAmmo_delta = 0;
                reloadTimeMultiplier_delta = 0f;
                reloadTimeAdd_delta = 0f;
                currentAmmo_delta = 0;
            }

            // if the gun is currently reloading, then set lastMaxAmmo to be the same as MaxAmmo to prevent the bullets from being drawn over the reload ring
            if (((Gun)Traverse.Create(gunAmmo).Field("gun").GetValue()).isReloading)
            {
                Traverse.Create(gunAmmo).Field("lastMaxAmmo").SetValue(gunAmmo.maxAmmo);
            }
            // if the current ammo was changed, redraw the bullet icons
            if (flag) { GunAmmoStatModifier.ReDrawCurrentBullets(gunAmmo); }
        }
        private static void ReDrawCurrentBullets(GunAmmo gunAmmo)
        {
            for (int i = gunAmmo.populate.transform.childCount - 1; i >= 0; i--)
            {
                if (gunAmmo.populate.transform.GetChild(i).gameObject.activeSelf)
                {
                    Object.Destroy(gunAmmo.populate.transform.GetChild(i).gameObject);
                }
            }
            gunAmmo.populate.times = (int)gunAmmo.GetFieldValue("currentAmmo");
            gunAmmo.populate.DoPopulate();
            typeof(GunAmmo).InvokeMember("SetActiveBullets",
                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                        BindingFlags.NonPublic, null, gunAmmo, new object[] { false });
        }
    }

    public class GunStatModifier
    {
        public float damage_add = 0f;
        public float damage_mult = 1f;
        public float recoilMuiltiplier_add = 0f;
        public float recoilMuiltiplier_mult = 1f;
        public float knockback_add = 0f;
        public float knockback_mult = 1f;
        public float attackSpeed_add = 0f;
        public float attackSpeed_mult = 1f;
        public float projectileSpeed_add = 0f;
        public float projectileSpeed_mult = 1f;
        public float projectielSimulatonSpeed_add = 0f;
        public float projectielSimulatonSpeed_mult = 1f;
        public float gravity_add = 0f;
        public float gravity_mult = 1f;
        public float damageAfterDistanceMultiplier_add = 0f;
        public float damageAfterDistanceMultiplier_mult = 1f;
        public float bulletDamageMultiplier_add = 0f;
        public float bulletDamageMultiplier_mult = 1f;
        public float multiplySpread_add = 0f;
        public float multiplySpread_mult = 1f;
        public float size_add = 0f;
        public float size_mult = 1f;
        public float timeToReachFullMovementMultiplier_add = 0f;
        public float timeToReachFullMovementMultiplier_mult = 1f;
        public int numberOfProjectiles_add = 0;
        public int numberOfProjectiles_mult = 1;
        public int bursts_add = 0;
        public int bursts_mult = 1;
        public int reflects_add = 0;
        public int reflects_mult = 1;
        public int smartBounce_add = 0;
        public int smartBounce_mult = 1;
        public int randomBounces_add = 0;
        public int randomBounces_mult = 1;
        public float timeBetweenBullets_add = 0f;
        public float timeBetweenBullets_mult = 1f;
        public float projectileSize_add = 0f;
        public float projectileSize_mult = 1f;
        public float speedMOnBounce_add = 0f;
        public float speedMOnBounce_mult = 1f;
        public float dmgMOnBounce_add = 0f;
        public float dmgMOnBounce_mult = 1f;
        public float drag_add = 0f;
        public float drag_mult = 1f;
        public float dragMinSpeed_add = 0f;
        public float dragMinSpeed_mult = 1f;
        public float spread_add = 0f;
        public float spread_mult = 1f;
        public float evenSpread_add = 0f;
        public float evenSpread_mult = 1f;
        public float percentageDamage_add = 0f;
        public float percentageDamage_mult = 1f;
        public float slow_add = 0f;
        public float slow_mult = 1f;
        public float destroyBulletAfter_add = 0f;
        public float destroyBulletAfter_mult = 1f;
        public float forceSpecificAttackSpeed_add = 0f;
        public float forceSpecificAttackSpeed_mult = 1f;
        public float explodeNearEnemyRange_add = 0f;
        public float explodeNearEnemyRange_mult = 1f;
        public float explodeNearEnemyDamage_add = 0f;
        public float explodeNearEnemyDamage_mult = 1f;
        public float hitMovementMultiplier_add = 0f;
        public float hitMovementMultiplier_mult = 1f;
        public float attackSpeedMultiplier_add = 0f;
        public float attackSpeedMultiplier_mult = 1f;
        public List<ObjectsToSpawn> objectsToSpawn_add = new List<ObjectsToSpawn>();
        public Color projectileColor = Color.black;

        // extra stuff from extensions
        public float minDistanceMultiplier_add = 0f;
        public float minDistanceMultiplier_mult = 1f;

        private float damage_delta;
        private float recoilMuiltiplier_delta;
        private float knockback_delta;
        private float attackSpeed_delta;
        private float projectileSpeed_delta;
        private float projectielSimulatonSpeed_delta;
        private float gravity_delta;
        private float damageAfterDistanceMultiplier_delta;
        private float bulletDamageMultiplier_delta;
        private float multiplySpread_delta;
        private float size_delta;
        private float timeToReachFullMovementMultiplier_delta;
        private int numberOfProjectiles_delta;
        private int bursts_delta;
        private int reflects_delta;
        private int smartBounce_delta;
        private int randomBounces_delta;
        private float timeBetweenBullets_delta;
        private float projectileSize_delta;
        private float speedMOnBounce_delta;
        private float dmgMOnBounce_delta;
        private float drag_delta;
        private float dragMinSpeed_delta;
        private float spread_delta;
        private float evenSpread_delta;
        private float percentageDamage_delta;
        private float slow_delta;
        private float destroyBulletAfter_delta;
        private float forceSpecificAttackSpeed_delta;
        private float explodeNearEnemyRange_delta;
        private float explodeNearEnemyDamage_delta;
        private float hitMovementMultiplier_delta;
        private float attackSpeedMultiplier_delta;

        private GunColorEffect gunColorEffect;

        // extra
        private float minDistanceMultiplier_delta;


        public static void ApplyGunStatModifier(GunStatModifier gunStatModifier, Gun gun)
        {
            gunStatModifier.ApplyGunStatModifier(gun);
        }
        public void ApplyGunStatModifier(Gun gun)
        {
            // regular expressions protected me against arthritis here.
            damage_delta = gun.damage * damage_mult + damage_add - gun.damage;
            recoilMuiltiplier_delta = gun.recoilMuiltiplier * recoilMuiltiplier_mult + recoilMuiltiplier_add - gun.recoilMuiltiplier;
            knockback_delta = gun.knockback * knockback_mult + knockback_add - gun.knockback;
            attackSpeed_delta = gun.attackSpeed * attackSpeed_mult + attackSpeed_add - gun.attackSpeed;
            projectileSpeed_delta = gun.projectileSpeed * projectileSpeed_mult + projectileSpeed_add - gun.projectileSpeed;
            projectielSimulatonSpeed_delta = gun.projectielSimulatonSpeed * projectielSimulatonSpeed_mult + projectielSimulatonSpeed_add - gun.projectielSimulatonSpeed;
            gravity_delta = gun.gravity * gravity_mult + gravity_add - gun.gravity;
            damageAfterDistanceMultiplier_delta = gun.damageAfterDistanceMultiplier * damageAfterDistanceMultiplier_mult + damageAfterDistanceMultiplier_add - gun.damageAfterDistanceMultiplier;
            bulletDamageMultiplier_delta = gun.bulletDamageMultiplier * bulletDamageMultiplier_mult + bulletDamageMultiplier_add - gun.bulletDamageMultiplier;
            multiplySpread_delta = gun.multiplySpread * multiplySpread_mult + multiplySpread_add - gun.multiplySpread;
            size_delta = gun.size * size_mult + size_add - gun.size;
            timeToReachFullMovementMultiplier_delta = gun.timeToReachFullMovementMultiplier * timeToReachFullMovementMultiplier_mult + timeToReachFullMovementMultiplier_add - gun.timeToReachFullMovementMultiplier;
            numberOfProjectiles_delta = gun.numberOfProjectiles * numberOfProjectiles_mult + numberOfProjectiles_add - gun.numberOfProjectiles;
            bursts_delta = gun.bursts * bursts_mult + bursts_add - gun.bursts;
            reflects_delta = gun.reflects * reflects_mult + reflects_add - gun.reflects;
            smartBounce_delta = gun.smartBounce * smartBounce_mult + smartBounce_add - gun.smartBounce;
            randomBounces_delta = gun.randomBounces * randomBounces_mult + randomBounces_add - gun.randomBounces;
            timeBetweenBullets_delta = gun.timeBetweenBullets * timeBetweenBullets_mult + timeBetweenBullets_add - gun.timeBetweenBullets;
            projectileSize_delta = gun.projectileSize * projectileSize_mult + projectileSize_add - gun.projectileSize;
            speedMOnBounce_delta = gun.speedMOnBounce * speedMOnBounce_mult + speedMOnBounce_add - gun.speedMOnBounce;
            dmgMOnBounce_delta = gun.dmgMOnBounce * dmgMOnBounce_mult + dmgMOnBounce_add - gun.dmgMOnBounce;
            drag_delta = gun.drag * drag_mult + drag_add - gun.drag;
            dragMinSpeed_delta = gun.dragMinSpeed * dragMinSpeed_mult + dragMinSpeed_add - gun.dragMinSpeed;
            spread_delta = gun.spread * spread_mult + spread_add - gun.spread;
            evenSpread_delta = gun.evenSpread * evenSpread_mult + evenSpread_add - gun.evenSpread;
            percentageDamage_delta = gun.percentageDamage * percentageDamage_mult + percentageDamage_add - gun.percentageDamage;
            slow_delta = gun.slow * slow_mult + slow_add - gun.slow;
            destroyBulletAfter_delta = gun.destroyBulletAfter * destroyBulletAfter_mult + destroyBulletAfter_add - gun.destroyBulletAfter;
            forceSpecificAttackSpeed_delta = gun.forceSpecificAttackSpeed * forceSpecificAttackSpeed_mult + forceSpecificAttackSpeed_add - gun.forceSpecificAttackSpeed;
            explodeNearEnemyRange_delta = gun.explodeNearEnemyRange * explodeNearEnemyRange_mult + explodeNearEnemyRange_add - gun.explodeNearEnemyRange;
            explodeNearEnemyDamage_delta = gun.explodeNearEnemyDamage * explodeNearEnemyDamage_mult + explodeNearEnemyDamage_add - gun.explodeNearEnemyDamage;
            hitMovementMultiplier_delta = gun.hitMovementMultiplier * hitMovementMultiplier_mult + hitMovementMultiplier_add - gun.hitMovementMultiplier;
            attackSpeedMultiplier_delta = gun.attackSpeedMultiplier * attackSpeedMultiplier_mult + attackSpeedMultiplier_add - gun.attackSpeedMultiplier;

            minDistanceMultiplier_delta = gun.GetAdditionalData().minDistanceMultiplier * minDistanceMultiplier_mult + minDistanceMultiplier_add - gun.GetAdditionalData().minDistanceMultiplier;

            // apply everything
            gun.damage += damage_delta;
            gun.recoilMuiltiplier += recoilMuiltiplier_delta;
            gun.knockback += knockback_delta;
            gun.attackSpeed += attackSpeed_delta;
            gun.projectileSpeed += projectileSpeed_delta;
            gun.projectielSimulatonSpeed += projectielSimulatonSpeed_delta;
            gun.gravity += gravity_delta;
            gun.damageAfterDistanceMultiplier += damageAfterDistanceMultiplier_delta;
            gun.bulletDamageMultiplier += bulletDamageMultiplier_delta;
            gun.multiplySpread += multiplySpread_delta;
            gun.size += size_delta;
            gun.timeToReachFullMovementMultiplier += timeToReachFullMovementMultiplier_delta;
            gun.numberOfProjectiles += numberOfProjectiles_delta;
            gun.bursts += bursts_delta;
            gun.reflects += reflects_delta;
            gun.smartBounce += smartBounce_delta;
            gun.randomBounces += randomBounces_delta;
            gun.timeBetweenBullets += timeBetweenBullets_delta;
            gun.projectileSize += projectileSize_delta;
            gun.speedMOnBounce += speedMOnBounce_delta;
            gun.dmgMOnBounce += dmgMOnBounce_delta;
            gun.drag += drag_delta;
            gun.dragMinSpeed += dragMinSpeed_delta;
            gun.spread += spread_delta;
            gun.evenSpread += evenSpread_delta;
            gun.percentageDamage += percentageDamage_delta;
            gun.slow += slow_delta;
            gun.destroyBulletAfter += destroyBulletAfter_delta;
            gun.forceSpecificAttackSpeed += forceSpecificAttackSpeed_delta;
            gun.explodeNearEnemyRange += explodeNearEnemyRange_delta;
            gun.explodeNearEnemyDamage += explodeNearEnemyDamage_delta;
            gun.hitMovementMultiplier += hitMovementMultiplier_delta;
            gun.attackSpeedMultiplier += attackSpeedMultiplier_delta;

            List<ObjectsToSpawn> gunObjectsToSpawn = new List<ObjectsToSpawn>(gun.objectsToSpawn);

            foreach (ObjectsToSpawn objectToSpawn in objectsToSpawn_add)
            {
                gunObjectsToSpawn.Add(objectToSpawn);
            }
            gun.objectsToSpawn = gunObjectsToSpawn.ToArray();

            if (projectileColor != Color.black)
            {
                gunColorEffect = gun.player.gameObject.AddComponent<GunColorEffect>();
                gunColorEffect.SetColor(projectileColor);
            }

            gun.GetAdditionalData().minDistanceMultiplier += minDistanceMultiplier_delta;


        }

        public static void RemoveGunStatModifier(GunStatModifier gunStatModifier, Gun gun, bool clear = true)
        {
            gunStatModifier.RemoveGunStatModifier(gun, clear);
        }
        public void RemoveGunStatModifier(Gun gun, bool clear = true)
        {
            gun.damage -= damage_delta;
            gun.recoilMuiltiplier -= recoilMuiltiplier_delta;
            gun.knockback -= knockback_delta;
            gun.attackSpeed -= attackSpeed_delta;
            gun.projectileSpeed -= projectileSpeed_delta;
            gun.projectielSimulatonSpeed -= projectielSimulatonSpeed_delta;
            gun.gravity -= gravity_delta;
            gun.damageAfterDistanceMultiplier -= damageAfterDistanceMultiplier_delta;
            gun.bulletDamageMultiplier -= bulletDamageMultiplier_delta;
            gun.multiplySpread -= multiplySpread_delta;
            gun.size -= size_delta;
            gun.timeToReachFullMovementMultiplier -= timeToReachFullMovementMultiplier_delta;
            gun.numberOfProjectiles -= numberOfProjectiles_delta;
            gun.bursts -= bursts_delta;
            gun.reflects -= reflects_delta;
            gun.smartBounce -= smartBounce_delta;
            gun.randomBounces -= randomBounces_delta;
            gun.timeBetweenBullets -= timeBetweenBullets_delta;
            gun.projectileSize -= projectileSize_delta;
            gun.speedMOnBounce -= speedMOnBounce_delta;
            gun.dmgMOnBounce -= dmgMOnBounce_delta;
            gun.drag -= drag_delta;
            gun.dragMinSpeed -= dragMinSpeed_delta;
            gun.spread -= spread_delta;
            gun.evenSpread -= evenSpread_delta;
            gun.percentageDamage -= percentageDamage_delta;
            gun.slow -= slow_delta;
            gun.destroyBulletAfter -= destroyBulletAfter_delta;
            gun.forceSpecificAttackSpeed -= forceSpecificAttackSpeed_delta;
            gun.explodeNearEnemyRange -= explodeNearEnemyRange_delta;
            gun.explodeNearEnemyDamage -= explodeNearEnemyDamage_delta;
            gun.hitMovementMultiplier -= hitMovementMultiplier_delta;
            gun.attackSpeedMultiplier -= attackSpeedMultiplier_delta;

            List<ObjectsToSpawn> gunObjectsToSpawn = new List<ObjectsToSpawn>(gun.objectsToSpawn);

            foreach (ObjectsToSpawn objectToSpawn in objectsToSpawn_add)
            {
                gunObjectsToSpawn.Remove(objectToSpawn);
            }
            gun.objectsToSpawn = gunObjectsToSpawn.ToArray();

            if (gunColorEffect != null) { gunColorEffect.Destroy(); }

            gun.GetAdditionalData().minDistanceMultiplier -= minDistanceMultiplier_delta;

            // reset deltas

            if (clear)
            {
                damage_delta = 0f;
                recoilMuiltiplier_delta = 0f;
                knockback_delta = 0f;
                attackSpeed_delta = 0f;
                projectileSpeed_delta = 0f;
                projectielSimulatonSpeed_delta = 0f;
                gravity_delta = 0f;
                damageAfterDistanceMultiplier_delta = 0f;
                bulletDamageMultiplier_delta = 0f;
                multiplySpread_delta = 0f;
                size_delta = 0f;
                timeToReachFullMovementMultiplier_delta = 0f;
                numberOfProjectiles_delta = 0;
                bursts_delta = 0;
                reflects_delta = 0;
                smartBounce_delta = 0;
                randomBounces_delta = 0;
                timeBetweenBullets_delta = 0f;
                projectileSize_delta = 0f;
                speedMOnBounce_delta = 0f;
                dmgMOnBounce_delta = 0f;
                drag_delta = 0f;
                dragMinSpeed_delta = 0f;
                spread_delta = 0f;
                evenSpread_delta = 0f;
                percentageDamage_delta = 0f;
                slow_delta = 0f;
                destroyBulletAfter_delta = 0f;
                forceSpecificAttackSpeed_delta = 0f;
                explodeNearEnemyRange_delta = 0f;
                explodeNearEnemyDamage_delta = 0f;
                hitMovementMultiplier_delta = 0f;
                attackSpeedMultiplier_delta = 0f;

                gunColorEffect = null;

                // extra
                minDistanceMultiplier_delta = 0f;
            }

        }


    }


    public class CharacterStatModifiersModifier
    {
        public List<GameObject> objectsToAddToPlayer = new List<GameObject>();
        public float sizeMultiplier_add = 0f;
        public float sizeMultiplier_mult = 1f;
        public float health_add = 0f;
        public float health_mult = 1f;
        public float movementSpeed_add = 0f;
        public float movementSpeed_mult = 1f;
        public float jump_add = 0f;
        public float jump_mult = 1f;
        public float gravity_add = 0f;
        public float gravity_mult = 1f;
        public float slow_add = 0f;
        public float slow_mult = 1f;
        public float slowSlow_add = 0f;
        public float slowSlow_mult = 1f;
        public float fastSlow_add = 0f;
        public float fastSlow_mult = 1f;
        public float secondsToTakeDamageOver_add = 0f;
        public float secondsToTakeDamageOver_mult = 1f;
        public int numberOfJumps_add = 0;
        public int numberOfJumps_mult = 1;
        public float regen_add = 0f;
        public float regen_mult = 1f;
        public float lifeSteal_add = 0f;
        public float lifeSteal_mult = 1f;
        public int respawns_add = 0;
        public int respawns_mult = 1;
        public float tasteOfBloodSpeed_add = 0f;
        public float tasteOfBloodSpeed_mult = 1f;
        public float rageSpeed_add = 0f;
        public float rageSpeed_mult = 1f;
        public float attackSpeedMultiplier_add = 0f;
        public float attackSpeedMultiplier_mult = 1f;

        // extra stuff from extensions
        public float gravityMultiplierOnDoDamage_add = 0f;
        public float gravityMultiplierOnDoDamage_mult = 1f;
        public float gravityDurationOnDoDamage_add = 0f;
        public float gravityDurationOnDoDamage_mult = 1f;
        public float defaultGravityForce_add = 0f;
        public float defaultGravityForce_mult = 1f;
        public float defaultGravityExponent_add = 0f;
        public float defaultGravityExponent_mult = 1f;
        public int murder_add = 0;
        public int murder_mult = 1;

        private List<GameObject> objectsAddedToPlayer = new List<GameObject>();
        private float sizeMultiplier_delta;
        private float health_delta;
        private float movementSpeed_delta;
        private float jump_delta;
        private float gravity_delta;
        private float slow_delta;
        private float slowSlow_delta;
        private float fastSlow_delta;
        private float secondsToTakeDamageOver_delta;
        private int numberOfJumps_delta;
        private float regen_delta;
        private float lifeSteal_delta;
        private int respawns_delta;
        private float tasteOfBloodSpeed_delta;
        private float rageSpeed_delta;
        private float attackSpeedMultiplier_delta;

        // extra stuff from extensions
        private float gravityMultiplierOnDoDamage_delta;
        private float gravityDurationOnDoDamage_delta;
        private float defaultGravityForce_delta;
        private float defaultGravityExponent_delta;
        private int murder_delta;

        public static void ApplyCharacterStatModifiersModifier(CharacterStatModifiersModifier characterStatModifiersModifier, CharacterStatModifiers characterStatModifiers)
        {
            characterStatModifiersModifier.ApplyCharacterStatModifiersModifier(characterStatModifiers);
        }
        public void ApplyCharacterStatModifiersModifier(CharacterStatModifiers characterStatModifiers)
        {
            sizeMultiplier_delta = characterStatModifiers.sizeMultiplier * sizeMultiplier_mult + sizeMultiplier_add - characterStatModifiers.sizeMultiplier;
            health_delta = characterStatModifiers.health * health_mult + health_add - characterStatModifiers.health;
            movementSpeed_delta = characterStatModifiers.movementSpeed * movementSpeed_mult + movementSpeed_add - characterStatModifiers.movementSpeed;
            jump_delta = characterStatModifiers.jump * jump_mult + jump_add - characterStatModifiers.jump;
            gravity_delta = characterStatModifiers.gravity * gravity_mult + gravity_add - characterStatModifiers.gravity;
            slow_delta = characterStatModifiers.slow * slow_mult + slow_add - characterStatModifiers.slow;
            slowSlow_delta = characterStatModifiers.slowSlow * slowSlow_mult + slowSlow_add - characterStatModifiers.slowSlow;
            fastSlow_delta = characterStatModifiers.fastSlow * fastSlow_mult + fastSlow_add - characterStatModifiers.fastSlow;
            secondsToTakeDamageOver_delta = characterStatModifiers.secondsToTakeDamageOver * secondsToTakeDamageOver_mult + secondsToTakeDamageOver_add - characterStatModifiers.secondsToTakeDamageOver;
            numberOfJumps_delta = characterStatModifiers.numberOfJumps * numberOfJumps_mult + numberOfJumps_add - characterStatModifiers.numberOfJumps;
            regen_delta = characterStatModifiers.regen * regen_mult + regen_add - characterStatModifiers.regen;
            lifeSteal_delta = characterStatModifiers.lifeSteal * lifeSteal_mult + lifeSteal_add - characterStatModifiers.lifeSteal;
            respawns_delta = characterStatModifiers.respawns * respawns_mult + respawns_add - characterStatModifiers.respawns;
            tasteOfBloodSpeed_delta = characterStatModifiers.tasteOfBloodSpeed * tasteOfBloodSpeed_mult + tasteOfBloodSpeed_add - characterStatModifiers.tasteOfBloodSpeed;
            rageSpeed_delta = characterStatModifiers.rageSpeed * rageSpeed_mult + rageSpeed_add - characterStatModifiers.rageSpeed;
            attackSpeedMultiplier_delta = characterStatModifiers.attackSpeedMultiplier * attackSpeedMultiplier_mult + attackSpeedMultiplier_add - characterStatModifiers.attackSpeedMultiplier;

            // extra stuff from extensions
            gravityMultiplierOnDoDamage_delta = characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage * gravityMultiplierOnDoDamage_mult + gravityMultiplierOnDoDamage_add - characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage;
            gravityDurationOnDoDamage_delta = characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage * gravityDurationOnDoDamage_mult + gravityDurationOnDoDamage_add - characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage;
            defaultGravityForce_delta = characterStatModifiers.GetAdditionalData().defaultGravityForce * defaultGravityForce_mult + defaultGravityForce_add - characterStatModifiers.GetAdditionalData().defaultGravityForce;
            defaultGravityExponent_delta = characterStatModifiers.GetAdditionalData().defaultGravityExponent * defaultGravityExponent_mult + defaultGravityExponent_add - characterStatModifiers.GetAdditionalData().defaultGravityExponent;
            murder_delta = characterStatModifiers.GetAdditionalData().murder * murder_mult + murder_add - characterStatModifiers.GetAdditionalData().murder;
            
            
            characterStatModifiers.sizeMultiplier += sizeMultiplier_delta;
            characterStatModifiers.health += health_delta;
            characterStatModifiers.movementSpeed += movementSpeed_delta;
            characterStatModifiers.jump += jump_delta;
            characterStatModifiers.gravity += gravity_delta;
            characterStatModifiers.slow += slow_delta;
            characterStatModifiers.slowSlow += slowSlow_delta;
            characterStatModifiers.fastSlow += fastSlow_delta;
            characterStatModifiers.secondsToTakeDamageOver += secondsToTakeDamageOver_delta;
            characterStatModifiers.numberOfJumps += numberOfJumps_delta;
            characterStatModifiers.regen += regen_delta;
            characterStatModifiers.lifeSteal += lifeSteal_delta;
            characterStatModifiers.respawns += respawns_delta;
            characterStatModifiers.tasteOfBloodSpeed += tasteOfBloodSpeed_delta;
            characterStatModifiers.rageSpeed += rageSpeed_delta;
            characterStatModifiers.attackSpeedMultiplier += attackSpeedMultiplier_delta;

            // extra stuff from extensions
            characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage += gravityMultiplierOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage += gravityDurationOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityForce += defaultGravityForce_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityExponent += defaultGravityExponent_delta;
            characterStatModifiers.GetAdditionalData().murder += murder_delta;

            // special stuff
            Player player = characterStatModifiers.GetComponent<Player>();
            foreach (GameObject objectToAddToPlayer in objectsToAddToPlayer)
            {
                GameObject instantiatedObject = Object.Instantiate<GameObject>(objectToAddToPlayer, player.transform.position, player.transform.rotation, player.transform);

                objectsAddedToPlayer.Add(instantiatedObject);
                characterStatModifiers.objectsAddedToPlayer.Add(instantiatedObject);
            }
            if (respawns_delta != 0)
            {
                characterStatModifiers.SetFieldValue("remainingRespawns", (int)characterStatModifiers.GetFieldValue("remainingRespawns") + respawns_delta);
            }


            // update the characterStatModifiers
            characterStatModifiers.WasUpdated();
            typeof(CharacterStatModifiers).InvokeMember("ConfigureMassAndSize",
                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                        BindingFlags.NonPublic, null, characterStatModifiers, new object[] { });

        }
        public static void RemoveCharacterStatModifiersModifier(CharacterStatModifiersModifier characterStatModifiersModifier, CharacterStatModifiers characterStatModifiers, bool clear = true)
        {
            characterStatModifiersModifier.RemoveCharacterStatModifiersModifier(characterStatModifiers, clear); 
        }
        public void RemoveCharacterStatModifiersModifier(CharacterStatModifiers characterStatModifiers, bool clear = true)
        {
            characterStatModifiers.sizeMultiplier -= sizeMultiplier_delta;
            characterStatModifiers.health -= health_delta;
            characterStatModifiers.movementSpeed -= movementSpeed_delta;
            characterStatModifiers.jump -= jump_delta;
            characterStatModifiers.gravity -= gravity_delta;
            characterStatModifiers.slow -= slow_delta;
            characterStatModifiers.slowSlow -= slowSlow_delta;
            characterStatModifiers.fastSlow -= fastSlow_delta;
            characterStatModifiers.secondsToTakeDamageOver -= secondsToTakeDamageOver_delta;
            characterStatModifiers.numberOfJumps -= numberOfJumps_delta;
            characterStatModifiers.regen -= regen_delta;
            characterStatModifiers.lifeSteal -= lifeSteal_delta;
            characterStatModifiers.respawns -= respawns_delta;
            characterStatModifiers.tasteOfBloodSpeed -= tasteOfBloodSpeed_delta;
            characterStatModifiers.rageSpeed -= rageSpeed_delta;
            characterStatModifiers.attackSpeedMultiplier -= attackSpeedMultiplier_delta;

            // extra stuff from extensions
            characterStatModifiers.GetAdditionalData().gravityMultiplierOnDoDamage -= gravityMultiplierOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().gravityDurationOnDoDamage -= gravityDurationOnDoDamage_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityForce -= defaultGravityForce_delta;
            characterStatModifiers.GetAdditionalData().defaultGravityExponent -= defaultGravityExponent_delta;
            characterStatModifiers.GetAdditionalData().murder -= murder_delta;

            // special stuff
            foreach (GameObject objectAddedToPlayer in objectsAddedToPlayer)
            {
                characterStatModifiers.objectsAddedToPlayer.Remove(objectAddedToPlayer);
                if (objectAddedToPlayer != null) { Object.Destroy(objectAddedToPlayer); }

            }

            // reset deltas

            if (clear)
            {
                objectsAddedToPlayer = new List<GameObject>();
                sizeMultiplier_delta = 0f;
                health_delta = 0f;
                movementSpeed_delta = 0f;
                jump_delta = 0f;
                gravity_delta = 0f;
                slow_delta = 0f;
                slowSlow_delta = 0f;
                fastSlow_delta = 0f;
                secondsToTakeDamageOver_delta = 0f;
                numberOfJumps_delta = 0;
                regen_delta = 0f;
                lifeSteal_delta = 0f;
                respawns_delta = 0;
                tasteOfBloodSpeed_delta = 0f;
                rageSpeed_delta = 0f;
                attackSpeedMultiplier_delta = 0f;

                // extra stuff from extensions
                gravityMultiplierOnDoDamage_delta = 0f;
                gravityDurationOnDoDamage_delta = 0f;
                defaultGravityForce_delta = 0f;
                defaultGravityExponent_delta = 0f;
                murder_delta = 0;
            }

            // update the characterStatModifiers
            characterStatModifiers.WasUpdated();
            typeof(CharacterStatModifiers).InvokeMember("ConfigureMassAndSize",
                        BindingFlags.Instance | BindingFlags.InvokeMethod |
                        BindingFlags.NonPublic, null, characterStatModifiers, new object[] { });


        }
    }
    public class GravityModifier
    {
        public float gravityForce_add = 0f;
        public float gravityForce_mult = 1f;
        public float exponent_add = 0f;
        public float exponent_mult = 1f;

        private float gravityForce_delta;
        private float exponent_delta;


        public static void ApplyGravityModifier(GravityModifier gravityModifier, Gravity gravity)
        {
            gravityModifier.ApplyGravityModifier(gravity);
        }
        public void ApplyGravityModifier(Gravity gravity)
        {
            gravityForce_delta = gravity.gravityForce * gravityForce_mult + gravityForce_add - gravity.gravityForce;
            exponent_delta = gravity.exponent * exponent_mult + exponent_add - gravity.exponent;

            gravity.gravityForce += gravityForce_delta;
            gravity.exponent += exponent_delta;

        }
        public static void RemoveGravityModifier(GravityModifier gravityModifier, Gravity gravity, bool clear = true)
        {
            gravityModifier.RemoveGravityModifier(gravity, clear);
        }
        public void RemoveGravityModifier(Gravity gravity, bool clear = true)
        {

            gravity.gravityForce -= gravityForce_delta;
            gravity.exponent -= exponent_delta;

            // reset deltas

            if (clear)
            {
                gravityForce_delta = 0f;
                exponent_delta = 0f;
            }

        }

    }
}
