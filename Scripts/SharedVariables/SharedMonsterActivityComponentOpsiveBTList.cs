using BehaviorDesigner.Runtime;
using System.Collections.Generic;

namespace MultiplayerARPG.OpsiveBT
{
    public class SharedMonsterActivityComponentOpsiveBTList : SharedVariable<List<MonsterActivityComponentOpsiveBT>>
    {
        public static implicit operator SharedMonsterActivityComponentOpsiveBTList(List<MonsterActivityComponentOpsiveBT> value) { return new SharedMonsterActivityComponentOpsiveBTList { Value = value }; }
    }
}
