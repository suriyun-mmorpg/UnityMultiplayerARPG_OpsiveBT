using BehaviorDesigner.Runtime;
using System.Collections.Generic;

namespace MultiplayerARPG.OpsiveBT
{
    public class SharedDamageableEntityList : SharedVariable<List<DamageableEntity>>
    {
        public static implicit operator SharedDamageableEntityList(List<DamageableEntity> value) { return new SharedDamageableEntityList { Value = value }; }
    }
}
