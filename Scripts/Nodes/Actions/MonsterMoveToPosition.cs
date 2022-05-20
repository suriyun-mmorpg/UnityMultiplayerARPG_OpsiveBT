using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace MultiplayerARPG.OpsiveBT
{
    public class MonsterMoveToPosition : MonsterActionNode
    {
        public SharedVector3 moveToPosition;
        public float tolerance = 1.0f;
        public ExtraMovementState extraMovementState = ExtraMovementState.None;

        public override void OnStart()
        {
            Entity.SetTargetEntity(null);
            Entity.SetExtraMovementState(extraMovementState);
            Entity.PointClickMovement(moveToPosition.Value);
        }

        public override TaskStatus OnUpdate()
        {
            if (Vector3.Distance(Entity.CacheTransform.position, moveToPosition.Value) < tolerance)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
