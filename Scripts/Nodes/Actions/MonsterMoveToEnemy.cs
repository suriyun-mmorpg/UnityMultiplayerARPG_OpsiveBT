using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace MultiplayerARPG.OpsiveBT
{
    public class MonsterMoveToEnemy : MonsterActionNode
    {
        public ExtraMovementState extraMovementState = ExtraMovementState.None;

        public override void OnStart()
        {

        }

        public override TaskStatus OnUpdate()
        {
            IDamageableEntity tempTargetEnemy;
            if (!Entity.TryGetTargetEntity(out tempTargetEnemy) || Entity.Characteristic == MonsterCharacteristic.NoHarm)
            {
                // No target, stop attacking
                ClearActionState();
                return TaskStatus.Failure;
            }

            if (tempTargetEnemy.Entity == Entity.Entity || tempTargetEnemy.IsHideOrDead() || !tempTargetEnemy.CanReceiveDamageFrom(Entity.GetInfo()))
            {
                // If target is dead or in safe area stop attacking
                Entity.SetTargetEntity(null);
                ClearActionState();
                return TaskStatus.Failure;
            }

            Vector3 targetPosition = tempTargetEnemy.GetTransform().position;
            float attackDistance = GetAttackDistance();
            if (!OverlappedEntity(tempTargetEnemy.Entity, GetDamageTransform().position, targetPosition, attackDistance))
            {
                Vector3 direction = (targetPosition - Entity.MovementTransform.position).normalized;
                Vector3 position = targetPosition - (direction * (attackDistance - Entity.StoppingDistance));
                Entity.SetExtraMovementState(extraMovementState);
                Entity.PointClickMovement(position);
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }
    }
}
