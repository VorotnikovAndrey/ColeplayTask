﻿using System;
using UnityEngine;
using Zenject;

namespace PlayVibe
{
    public class PoolView : MonoBehaviour
    {
        [HideInInspector] public string PoolId;
        [SerializeField] protected PoolCategory poolCategory;
        [SerializeField] protected PopupSceneChangeBehavior sceneChangeBehavior;

        [Inject] protected ObjectPoolService objectPoolService;
        [Inject] protected EventAggregator eventAggregator;

        public PoolCategory PoolCategory => poolCategory;

        protected void Start()
        {
            if (eventAggregator == null)
            {
                return;
            }
            
            eventAggregator.Add<SceneLoadedEvent>(OnSceneLoadedEvent);
            eventAggregator.Add<SceneUnloadedEvent>(OnSceneUnloadedEvent);
        }

        private void OnDestroy()
        {
            if (eventAggregator == null)
            {
                return;
            }
            
            eventAggregator.Remove<SceneLoadedEvent>(OnSceneLoadedEvent);
            eventAggregator.Remove<SceneUnloadedEvent>(OnSceneUnloadedEvent);
        }

        public PopupSceneChangeBehavior PopupSceneChangeBehavior => sceneChangeBehavior;

        private void OnSceneLoadedEvent(SceneLoadedEvent sender) => OnSceneChanged();
        private void OnSceneUnloadedEvent(SceneUnloadedEvent sender) => OnSceneChanged();

        protected virtual void OnSceneChanged()
        {
            switch (sceneChangeBehavior)
            {
                case PopupSceneChangeBehavior.Release:
                    objectPoolService.ReleaseView(this);
                    break;
                case PopupSceneChangeBehavior.ReturnToPool:
                    objectPoolService.ReturnToPool(this);
                    break;
                case PopupSceneChangeBehavior.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void OnReturnToPool()
        {

        }
    }
}