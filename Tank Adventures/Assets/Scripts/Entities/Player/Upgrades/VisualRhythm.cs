using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Player.Upgrades
{
    public class VisualRhythm : MonoBehaviour
    {
        [Serializable]
        private struct RhythmVisualRepresentation
        {
            public RhythmDescriptor descriptor;
            public Color rColor;
        }
        
        [SerializeField] private Image image;
        [SerializeField] private List<RhythmVisualRepresentation> colors;
        
        [HideInInspector] public RhythmUpgrade01 rUpgrade;
        private Dictionary<RhythmDescriptor, Color> _dicColors;

        private void Awake()
        {
            _dicColors = new Dictionary<RhythmDescriptor, Color>();
            SetDict();
        }

        private void SetDict()
        {
            foreach (var rHolder in colors)
            {
                _dicColors.Add(rHolder.descriptor, rHolder.rColor);
            }
        }

        private void Start()
        {
            rUpgrade.OnRhythmChange += RhythmUpdated;
        }

        private void OnDisable()
        {
            if(rUpgrade!=null) rUpgrade.OnRhythmChange -= RhythmUpdated;
        }

        private void RhythmUpdated(RhythmDescriptor newDesc)
        {
            if (_dicColors.ContainsKey(newDesc))
            {
                image.color = _dicColors[newDesc];
            }
        }
    }
}