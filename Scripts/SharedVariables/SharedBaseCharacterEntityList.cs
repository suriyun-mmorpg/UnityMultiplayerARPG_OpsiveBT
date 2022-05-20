using BehaviorDesigner.Runtime;
using System.Collections.Generic;

namespace MultiplayerARPG.OpsiveBT
{
    public class SharedBaseCharacterEntityList : SharedVariable<List<BaseCharacterEntity>>
    {
        public static implicit operator SharedBaseCharacterEntityList(List<BaseCharacterEntity> value) { return new SharedBaseCharacterEntityList { Value = value }; }
    }
}
