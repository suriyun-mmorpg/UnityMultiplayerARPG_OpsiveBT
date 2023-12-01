using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace MultiplayerARPG.OpsiveBT
{
    public class MonsterTurnToEnemy : MonsterActionNode
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("Turn to enemy speed")]
        public float turnToEnemySpeed = 800f;
        [Min(1f)]
        public float successAngle = 10f;

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

            if (tempTargetEnemy.GetObjectId() == Entity.ObjectId || tempTargetEnemy.IsDeadOrHideFrom(Entity) || !tempTargetEnemy.CanReceiveDamageFrom(Entity.GetInfo()))
            {
                // If target is dead or in safe area stop attacking
                Entity.SetTargetEntity(null);
                ClearActionState();
                return TaskStatus.Failure;
            }

            Vector3 currentPosition = Entity.EntityTransform.position;
            Vector3 targetPosition = tempTargetEnemy.GetTransform().position;
            Vector3 lookAtDirection = (targetPosition - currentPosition).normalized;
            if (lookAtDirection.sqrMagnitude > 0)
            {
                if (CurrentGameInstance.DimensionType == DimensionType.Dimension3D)
                {
                    Quaternion currentLookAtRotation = Entity.GetLookRotation();
                    Vector3 lookRotationEuler = Quaternion.LookRotation(lookAtDirection).eulerAngles;
                    lookRotationEuler.x = 0;
                    lookRotationEuler.z = 0;
                    Quaternion nextLookAtRotation = Quaternion.RotateTowards(currentLookAtRotation, Quaternion.Euler(lookRotationEuler), turnToEnemySpeed * Time.deltaTime);
                    Entity.SetLookRotation(nextLookAtRotation);

                    if (Quaternion.Angle(currentLookAtRotation, nextLookAtRotation) >= successAngle)
                        return TaskStatus.Running;
                }
                else
                {
                    // Update 2D direction
                    Entity.SetLookRotation(Quaternion.LookRotation(lookAtDirection));
                    return TaskStatus.Success;
                }
            }
            return TaskStatus.Success;
        }
    }
}
