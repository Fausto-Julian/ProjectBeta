using System;
using System.Collections.Generic;
using _ProjectBeta.Scripts.ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.Assertions;

namespace _ProjectBeta.Scripts.PlayerScrips
{
    public class KillStreakSystem : MonoBehaviour
    {
        private PlayerModel _model;
        private KillStreakData _data;

        private float _currentPoints;
        private readonly List<PlayerEffect> _effectsApplications = new();

        private bool _isActive;
        private float _waitTime;

        private int _index;

        public Action<int> OnChangeLevel;
        public Action<float, float> OnChangePoints;

        public void Initialize(PlayerModel model)
        {
            _model = model;
            _data = _model.GetData().KillStreakData;
            Assert.IsNotNull(_data);

            var statisticsController = _model.GetStatisticsController();
            statisticsController.OnKillChange += OnKillChangeHandler;
            statisticsController.OnAssistanceChange += OnAssistanceChangeHandler;
            
            _model.GetHealthController().OnDie += OnDieHandler;
        }

        private void Update()
        {
            if (!_isActive)
                return;
            
            if (_waitTime >= Time.time)
                return;

            _currentPoints -= _data.PointToRemoveInactiveTime * Time.deltaTime;
            
            var index = _index >= _data.ExperiencePointNeededToLevelUp.Count ? _data.ExperiencePointNeededToLevelUp.Count - 1 : _index;
            OnChangePoints?.Invoke(_currentPoints, _data.ExperiencePointNeededToLevelUp[index]);

            if (CheckIsRemoveLevelUp(index))
                ResetEffect();
            
            if (_currentPoints > 0)
                return;
            
            ResetAll();
        }

        private void OnDieHandler()
        {
            ResetAll();
        }

        private void OnAssistanceChangeHandler(int obj)
        {
            AddPoint();
        }

        private void OnKillChangeHandler(int obj)
        {
            AddPoint();
        }

        private void AddPoint()
        {
            if (_index >= _data.ExperiencePointNeededToLevelUp.Count)
                return;
            
            _currentPoints += _data.PointToAddAssistance;
            
            OnChangePoints?.Invoke(_currentPoints, _data.ExperiencePointNeededToLevelUp[_index]);
            
            _waitTime = Time.time + _data.InactiveTime;
            _isActive = true;

            if (!CheckIsLevelUp())
                return;
            
            LevelUp();
        }

        private bool CheckIsLevelUp()
        {
            return _currentPoints <= _data.ExperiencePointNeededToLevelUp[_index];
        }

        private void LevelUp()
        {
            var effect = _data.Effects[_index];
            effect.ActivateEffect(_model);
            
            _effectsApplications.Add(effect);
            _index++;
            OnChangeLevel?.Invoke(_index);
        }

        private bool CheckIsRemoveLevelUp(int index)
        {
            return !(_currentPoints >= _data.ExperiencePointNeededToLevelUp[index]);
        }

        private void ResetEffect()
        {
            var maxLevel = _index >= _data.ExperiencePointNeededToLevelUp.Count;
            var index = maxLevel ? _data.ExperiencePointNeededToLevelUp.Count - 1 : _index;
            var effect = _data.Effects[index];
            
            effect.RemoveEffect(_model);

            if (_effectsApplications.Contains(effect))
                _effectsApplications.Remove(effect);

            _index = maxLevel ? _index - 2 : _index--;
            OnChangeLevel?.Invoke(_index);
        }
        
        private void ResetAll()
        {
            for (var i = 0; i < _effectsApplications.Count; i++)
            {
                _effectsApplications[i].RemoveEffect(_model);
            }

            _effectsApplications.Clear();
            _currentPoints = 0;
            _isActive = false;
            _index = 0;
            
            OnChangeLevel?.Invoke(_index);
            OnChangePoints?.Invoke(_currentPoints, _data.ExperiencePointNeededToLevelUp[_index]);
        }
    }
}