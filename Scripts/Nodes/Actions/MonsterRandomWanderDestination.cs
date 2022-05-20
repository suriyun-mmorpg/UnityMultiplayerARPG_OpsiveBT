using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace MultiplayerARPG.OpsiveBT
{
    public class MonsterRandomWanderDestination : MonsterActionNode
    {
        public SharedVector3 moveToPosition;
        public float randomWanderDistance = 2f;

        public override void OnStart()
        {

        }

        public override TaskStatus OnUpdate()
        {
            // Random position around summoner or around spawn point
            if (Entity.Summoner != null)
            {
                // Random position around summoner
                moveToPosition.Value = CurrentGameInstance.GameplayRule.GetSummonPosition(Entity.Summoner);
            }
            else
            {
                // Random position around spawn point
                Vector2 randomCircle = Random.insideUnitCircle * randomWanderDistance;
                if (CurrentGameInstance.DimensionType == DimensionType.Dimension2D)
                    moveToPosition.Value = Entity.SpawnPosition + new Vector3(randomCircle.x, randomCircle.y);
                else
                    moveToPosition.Value = Entity.SpawnPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
            }
            return TaskStatus.Success;
        }
    }
}
