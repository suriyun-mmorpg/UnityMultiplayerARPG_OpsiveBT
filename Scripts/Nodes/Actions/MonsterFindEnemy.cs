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

        public override void OnEnd()
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
            if (!Entity.TryGetTargetEntity(out IDamageableEntity targetEntity) || targetEntity.Entity == Entity.Entity || targetEntity.IsDead() || !targetEntity.CanReceiveDamageFrom(Entity.GetInfo()))
            {
                bool isSummonedAndSummonerExisted = Entity.IsSummonedAndSummonerExisted;
                // Find one enemy from previously found list
                if (FindOneEnemyFromList(isSummonedAndSummonerExisted))
                    return true;

                // If no target enemy or target enemy is dead, Find nearby character by layer mask
                enemies.Value.Clear();
                int overlapMask = CurrentGameInstance.playerLayer.Mask | CurrentGameInstance.monsterLayer.Mask;
                if (isAttackBuilding)
                    overlapMask |= CurrentGameInstance.buildingLayer.Mask;
                if (isSummonedAndSummonerExisted)
                {
                    // Find enemy around summoner
                    enemies.Value.AddRange(Entity.FindAliveEntities<DamageableEntity>(
                        Entity.Summoner.EntityTransform.position,
                        CharacterDatabase.SummonedVisualRange,
                        false, /* Don't find an allies */
                        true,  /* Find an enemies */
                        true,  /* Find an neutral */
                        overlapMask));
                }
                else
                {
                    enemies.Value.AddRange(Entity.FindAliveEntities<DamageableEntity>(
                        CharacterDatabase.VisualRange,
                        false, /* Don't find an allies */
                        true,  /* Find an enemies */
                        false, /* Don't find an neutral */
                        overlapMask));
                }
                // Find one enemy from a found list
                if (FindOneEnemyFromList(isSummonedAndSummonerExisted))
                    return true;
            }

            return true;
        }

        private bool FindOneEnemyFromList(bool isSummonedAndSummonerExisted)
        {
            DamageableEntity enemy;
            for (int i = enemies.Value.Count - 1; i >= 0; --i)
            {
                enemy = enemies.Value[i];
                enemies.Value.RemoveAt(i);
                if (enemy == null || enemy.Entity == Entity || enemy.IsDead() || !enemy.CanReceiveDamageFrom(Entity.GetInfo()))
                {
                    // If enemy is null or cannot receive damage from monster, skip it
                    continue;
                }
                if (isAttackBuilding && isSummonedAndSummonerExisted && enemy is BuildingEntity buildingEntity && Entity.Summoner.Id == buildingEntity.CreatorId)
                {
                    // If building was built by summoner, skip it
                    continue;
                }
                // Found target, attack it
                Entity.SetAttackTarget(enemy);
                return true;
            }
            return false;
        }
    }
}
