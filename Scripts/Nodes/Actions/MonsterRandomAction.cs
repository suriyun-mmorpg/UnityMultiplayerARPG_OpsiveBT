using BehaviorDesigner.Runtime.Tasks;

namespace MultiplayerARPG.OpsiveBT
{
    public class MonsterRandomAction : MonsterActionNode
    {
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

            // If it has target then go to target
            if (!Entity.IsPlayingActionAnimation())
            {
                BaseSkill tempQueueSkill;
                int tempQueueSkillLevel;
                // Random action state to do next time
                if (CharacterDatabase.RandomSkill(Entity, out tempQueueSkill, out tempQueueSkillLevel) && queueSkill.Value != null)
                {
                    queueSkill.Value = tempQueueSkill;
                    queueSkillLevel.Value = tempQueueSkillLevel;
                    // Cooling down
                    if (Entity.IndexOfSkillUsage(SkillUsageType.Skill, queueSkill.Value.DataId) >= 0)
                    {
                        queueSkill.Value = null;
                        queueSkillLevel.Value = 0;
                    }
                }
                else
                {
                    queueSkill.Value = null;
                    queueSkillLevel.Value = 0;
                }
                isLeftHandAttacking.Value = !isLeftHandAttacking.Value;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
