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
        private PlantData _plantData;
        public string Name { get { return _plantData.Name; } }
        public string Description { get { return _plantData.Description; } }
        public InventoryItemType HarvestableType { get { return _plantData.HarvestableType; } }
        private int _ticksPerTransition { get { return _plantData.ticksPerTransition; } }
        public int Stage { get; private set; }
        private GameObject _gameObject;
        private GameObject[] _stages { get { return _plantData.StagePrefabs.ToArray(); } }
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

        private Transform _parent
        {
            get
            {
                return GameObject.FindObjectsOfType<GameObject>().Where(go => go.name == "Tiles").First().transform;
            }
        }
        public GameObject GameObject
        {
            get
            {
                if (_gameObject == null)
                {
                    var rnd = new System.Random();
                    _gameObject = GameObject.Instantiate(CurrentStagePrefab, Vector3.zero, Quaternion.AngleAxis(rnd.Next(360), Vector3.up));
                    _gameObject.transform.parent = _parent;
                }
                return _gameObject;
            }
        }

        public Plant(PlantData plantData, int createdAtTick)
        {
            _plantData = plantData;
            Stage = 0;
            _lastStageTransitionTick = createdAtTick;
        }

        public bool TransitionIfNeeded(int ticks)
        {
            if (Stage + 1 < Stages && _lastStageTransitionTick + _ticksPerTransition < ticks)
            {
                Stage += 1;
                var old = _gameObject;
                _gameObject = GameObject.Instantiate(CurrentStagePrefab, old?.transform.position ?? Vector3.zero, old?.transform.rotation ?? Quaternion.identity);
                _gameObject.transform.parent = _parent;
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

        public bool IsHarvestable()
        {
            return Stage == Stages - 1;
        }

        public bool Harvest(int ticks)
        {
            if (Stage == Stages - 1)
            {
                Debug.Log($"Harvesting {Name}");
                Stage -= 2;
                _lastStageTransitionTick = 0;
                return true;
            }
            Debug.Log($"Nothing to harvest on {Name}");
            return false;
        }

        public InventoryItem ToInventoryItem()
        {
            var item = new InventoryItem()
            {
                Name = Name,
                Description = Description,
                Icon = _plantData.Icon,
                Type = HarvestableType,
                Quantity = 1,
            };
            return item;
        }
    }
}
