using BehaviorDesigner.Runtime;

namespace MultiplayerARPG.OpsiveBT
{
    public class SharedMonsterActivityComponentOpsiveBT : SharedVariable<MonsterActivityComponentOpsiveBT>
    {
        public static implicit operator SharedMonsterActivityComponentOpsiveBT(MonsterActivityComponentOpsiveBT value) { return new SharedMonsterActivityComponentOpsiveBT { Value = value }; }
    }
}
