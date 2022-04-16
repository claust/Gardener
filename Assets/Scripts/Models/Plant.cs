using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Plant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        private int _ticksPerTransition { get; set; }
        public int Stage { get; private set; }
        private GameObject _gameObject;
        private GameObject[] _stages;
        private GameObject CurrentStagePrefab
        {
            get
            {
                return _stages[Stage];
            }
        }
        public int Stages
        {
            get { return _stages?.Length ?? 0; }
        }
        private int _lastStageTransitionTick { get; set; }
        public GameObject GameObject
        {
            get
            {
                if (_gameObject == null)
                {
                    var rnd = new System.Random();
                    _gameObject = GameObject.Instantiate(CurrentStagePrefab, Vector3.zero, Quaternion.AngleAxis(rnd.Next(360), Vector3.up));
                }
                return _gameObject;
            }
        }

        public Plant(string name, string description, GameObject[] stages, int createdAtTick, int ticksPerTransition)
        {
            Name = name;
            Description = description;
            _stages = stages;
            Stage = 0;
            _ticksPerTransition = ticksPerTransition;
            _lastStageTransitionTick = createdAtTick;
        }
        public bool TransitionIfNeeded(int ticks)
        {
            if (Stage + 1 < Stages && _lastStageTransitionTick + _ticksPerTransition < ticks)
            {
                Stage += 1;
                var old = _gameObject;
                _gameObject = GameObject.Instantiate(CurrentStagePrefab, old?.transform.position ?? Vector3.zero, old?.transform.rotation ?? Quaternion.identity);
                _lastStageTransitionTick = ticks;
                if (old != null)
                {
                    GameObject.Destroy(old);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Plant Tomato(GameObject[] prefabs, int ticks)
        {
            return new Plant("Tomato", "A common red tomato", prefabs, ticks, 200);
        }
    }
}
