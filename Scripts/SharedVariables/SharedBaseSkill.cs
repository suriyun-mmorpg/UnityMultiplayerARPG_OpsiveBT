using BehaviorDesigner.Runtime;

namespace MultiplayerARPG.OpsiveBT
{
    public class SharedBaseSkill : SharedVariable<BaseSkill>
    {
        public static implicit operator SharedBaseSkill(BaseSkill value) { return new SharedBaseSkill { Value = value }; }
    }
}
