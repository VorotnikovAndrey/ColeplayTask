using Cysharp.Threading.Tasks;
using Zenject;

namespace PlayVibe
{
    public class GameplayStage : AbstractStageBase
    {
        protected override string StageType => Constants.Stages.GameplayStage;

        [Inject] private ObjectPoolService objectPoolService;

        public override async UniTask Initialize(object data = null)
        {
            base.Initialize(data);
        }

        public override UniTask DeInitialize()
        {
            base.DeInitialize();

            return UniTask.CompletedTask;
        }

        protected override void OnSubscribes()
        {
        }

        protected override void OnUnsubscribes()
        {
        }
    }
}