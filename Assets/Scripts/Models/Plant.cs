﻿using System;
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
        public InventoryItemType HarvestableType { get; set; }
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

        public Plant(string name, string description, InventoryItemType harvestableType, GameObject[] stages, int createdAtTick, int ticksPerTransition)
        {
            Name = name;
            Description = description;
            HarvestableType = harvestableType;
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
                Type = HarvestableType,
                Quantity = 1,
            };
            return item;
        }

        public static Plant Tomato(GameObject[] prefabs, int ticks)
        {
            return new Plant("Tomato", "A common red tomato", InventoryItemType.Tomato, prefabs, ticks, 200);
        }
    }
}
