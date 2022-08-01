using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;

namespace MultiplayerARPG.OpsiveBT
{
    public class MonsterFindEnemy : MonsterActionNode
    {
        [Tooltip("If this is TRUE, monster will attacks buildings")]
        public bool isAttackBuilding = false;

        public override void OnStart()
        {

        }

        public override TaskStatus OnUpdate()
        {
            if (FindEnemyFunc())
                return TaskStatus.Success;
            return TaskStatus.Failure;
        }

        /// <summary>
        /// Return `TRUE` if found enemy
        /// </summary>
        /// <returns></returns>
        public bool FindEnemyFunc()
        {
            IDamageableEntity targetEntity;
            if (!Entity.TryGetTargetEntity(out targetEntity) || targetEntity.Entity == Entity.Entity ||
                 targetEntity.IsDead() || !targetEntity.CanReceiveDamageFrom(Entity.GetInfo()))
            {
                BaseCharacterEntity enemy;

                for (int i = enemies.Value.Count - 1; i >= 0; --i)
                {
                    enemy = enemies.Value[i];
                    enemies.Value.RemoveAt(i);
                    if (enemy != null && enemy.Entity != Entity.Entity && !enemy.IsDead() &&
                        enemy.CanReceiveDamageFrom(Entity.GetInfo()))
                    {
                        // Found target, attack it
                        Entity.SetAttackTarget(enemy);
                        return true;
                    }
                }

                // If no target enemy or target enemy is dead, Find nearby character by layer mask
                enemies.Value.Clear();
                if (Entity.IsSummoned)
                {
                    // Find enemy around summoner
                    enemies.Value.AddRange(Entity.FindAliveCharacters<BaseCharacterEntity>(
                        Entity.Summoner.EntityTransform.position,
                        CharacterDatabase.SummonedVisualRange,
                        false,
                        true, 
                        Entity.IsSummoned));
                }
                else
                {
                    enemies.Value.AddRange(Entity.FindAliveCharacters<BaseCharacterEntity>(
                        CharacterDatabase.VisualRange,
                        false, 
                        true, 
                        Entity.IsSummoned));
                }

                for (int i = enemies.Value.Count - 1; i >= 0; --i)
                {
                    enemy = enemies.Value[i];
                    enemies.Value.RemoveAt(i);
                    if (enemy != null && enemy.Entity != Entity.Entity && !enemy.IsDead() &&
                        enemy.CanReceiveDamageFrom(Entity.GetInfo()))
                    {
                        // Found target, attack it
                        Entity.SetAttackTarget(enemy);
                        return true;
                    }
                }

                if (!isAttackBuilding)
                    return false;
                // Find building to attack
                List<BuildingEntity> buildingEntities = Entity.FindAliveDamageableEntities<BuildingEntity>(CharacterDatabase.VisualRange, CurrentGameInstance.buildingLayer.Mask);
                foreach (BuildingEntity buildingEntity in buildingEntities)
                {
                    // Attack target settings
                    if (buildingEntity == null || buildingEntity.Entity == Entity.Entity ||
                        buildingEntity.IsDead() || !buildingEntity.CanReceiveDamageFrom(Entity.GetInfo()))
                    {
                        // If building is null or cannot receive damage from monster, skip it
                        continue;
                    }
                    if (Entity.Summoner != null)
                    {
                        if (Entity.Summoner.Id.Equals(buildingEntity.CreatorId))
                        {
                            // If building was built by summoner, skip it
                            continue;
                        }
                    }
                    // Found target, attack it
                    Entity.SetAttackTarget(buildingEntity);
                    return true;
                }
            }

            return true;
        }
    }
}
