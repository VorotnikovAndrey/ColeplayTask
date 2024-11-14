using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace PlayVibe
{
    public abstract class AbstractStageBase : IStage
    {
        protected abstract string StageType { get; }

        private Dictionary<object, IStage> SubStages { get; } = new ();

        [Inject] protected EventAggregator eventAggregator;
        [Inject] protected PopupService popupService;

        public virtual UniTask Initialize(object data = null)
        {
            Debug.Log($"{StageType.AddColorTag(Color.yellow)} Initialized".AddColorTag(Color.cyan));

            foreach (var value in SubStages.Values)
            {
                value.Initialize(data);
            }

            Subscribes();
            
            return UniTask.CompletedTask;
        }

        public virtual UniTask DeInitialize()
        {
            Debug.Log($"{StageType.AddColorTag(Color.yellow)} DeInitialized".AddColorTag(Color.cyan));

            foreach (var value in SubStages.Values)
            {
                value.DeInitialize();
            }

            Unsubscribes();
            
            return UniTask.CompletedTask;
        }
        
        private void Subscribes()
        {
            OnSubscribes();
        }

        private void Unsubscribes()
        {
            OnUnsubscribes();
        }

        protected abstract void OnSubscribes();
        protected abstract void OnUnsubscribes();
    }
}
