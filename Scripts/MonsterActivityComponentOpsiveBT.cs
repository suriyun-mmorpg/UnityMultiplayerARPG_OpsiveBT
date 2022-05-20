using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG.OpsiveBT
{
    [DefaultExecutionOrder(int.MaxValue)]
    [RequireComponent(typeof(BehaviorDesigner.Runtime.BehaviorTree))]
    public class MonsterActivityComponentOpsiveBT : BaseMonsterActivityComponent
    {
        private BehaviorDesigner.Runtime.BehaviorTree bt;

        private void Start()
        {
            bt = GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
            bt.StartWhenEnabled = true;
            bt.PauseWhenDisabled = true;
            if (!Entity.IsServer)
                bt.enabled = false;
        }

        private void Update()
        {
            if (!Entity.IsServer || Entity.Identity.CountSubscribers() == 0 || CharacterDatabase == null)
            {
                bt.enabled = false;
                return;
            }
            bt.enabled = true;
        }
    }
}
