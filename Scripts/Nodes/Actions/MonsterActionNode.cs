using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace MultiplayerARPG.OpsiveBT
{
    public abstract class MonsterActionNode : Action
    {
        public SharedMonsterActivityComponentOpsiveBT activityComp;
        public SharedDamageableEntityList enemies;
        public SharedBaseSkill queueSkill;
        public SharedInt queueSkillLevel;
        public SharedBool isLeftHandAttacking;

        public BaseMonsterCharacterEntity Entity { get { return activityComp.Value.Entity; } }
        public MonsterCharacter CharacterDatabase { get { return activityComp.Value.CharacterDatabase; } }
        public GameInstance CurrentGameInstance { get { return GameInstance.Singleton; } }

        protected void ClearActionState()
        {
            queueSkill.Value = null;
            isLeftHandAttacking.Value = false;
        }

        protected Transform GetDamageTransform()
        {
            return queueSkill.Value != null ? queueSkill.Value.GetApplyTransform(Entity, isLeftHandAttacking.Value) :
                Entity.GetWeaponDamageInfo(null).GetDamageTransform(Entity, isLeftHandAttacking.Value);
        }

        protected float GetAttackDistance()
        {
            return queueSkill.Value != null && queueSkill.Value.IsAttack ? queueSkill.Value.GetCastDistance(Entity, queueSkillLevel.Value, isLeftHandAttacking.Value) :
                Entity.GetAttackDistance(isLeftHandAttacking.Value);
        }

        protected bool OverlappedEntity<T>(T entity, Vector3 measuringPosition, Vector3 targetPosition, float distance)
            where T : BaseGameEntity
        {
            if (Vector3.Distance(measuringPosition, targetPosition) <= distance)
                return true;
            // Target is far from controlling entity, try overlap the entity
            return Entity.FindPhysicFunctions.IsGameEntityInDistance(entity, measuringPosition, distance, false);
        }
    }
}
