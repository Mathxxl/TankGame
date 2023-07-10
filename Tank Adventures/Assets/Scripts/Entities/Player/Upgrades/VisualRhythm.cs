using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Entities.Player.Upgrades
{
    public class VisualRhythm : MonoBehaviour
    {
        [Serializable]
        private struct RhythmVisualRepresentation
        {
            public RhythmDescriptor descriptor;
            public Color rColor;
            public string text;
        }
        
        [SerializeField] private TextMeshProUGUI visualTM;
        [SerializeField] private List<RhythmVisualRepresentation> visuals;
        [SerializeField] private float displayDuration = 0.5f;
        [HideInInspector] public GameObject metronomePivot;

        [HideInInspector] public RhythmUpgrade01 rUpgrade;
        private Dictionary<RhythmDescriptor, RhythmVisualRepresentation> _dicColors;
        private RhythmDescriptor _currentDescriptor;

        private Coroutine _currentVisualRoutine;
        private Coroutine _currentMetronomeRoutine;

        private void Awake()
        {
            _dicColors = new Dictionary<RhythmDescriptor, RhythmVisualRepresentation>();
            SetDict();
        }

        private void SetDict()
        {
            foreach (var rHolder in visuals)
            {
                _dicColors.Add(rHolder.descriptor, rHolder);
            }
        }

        private void Start()
        {
            rUpgrade.OnRhythmChange += SetCurrentDescriptor;
            
            visualTM.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if(rUpgrade!=null) rUpgrade.OnRhythmChange -= SetCurrentDescriptor;
        }

        private void SetCurrentDescriptor(RhythmDescriptor newDesc)
        {
            _currentDescriptor = newDesc;
            
            Debug.Log($"SetCurrentDescriptor to {newDesc}");
            
            //Metronome
            if(metronomePivot.transform.GetChild(0).TryGetComponent(out RawImage image))
            {
                RhythmUpdated(_currentDescriptor);
                image.color = visualTM.color;
            }
        }

        private void RhythmUpdated(RhythmDescriptor newDesc)
        {
            if (!_dicColors.ContainsKey(newDesc)) return;
            
            var elt = _dicColors[newDesc];
            visualTM.color = elt.rColor;
            visualTM.text = elt.text;
        }

        public void AttackPerformed()
        {
            if(_currentVisualRoutine != null) StopCoroutine(_currentVisualRoutine);
            _currentVisualRoutine = StartCoroutine(TextLifetime());
        }
        
        private IEnumerator TextLifetime()
        {
            yield return null;
            
            visualTM.gameObject.SetActive(true);
            visualTM.transform.localScale = Vector3.one;

            RhythmUpdated(_currentDescriptor);
            
            var time = 0f;
            var i = 0;
            while (time < displayDuration/2 && i < 3600)
            {
                time += Time.deltaTime;
                i++;
                yield return null;

                visualTM.transform.localScale += 2 * Time.deltaTime * Vector3.one;
                
                //Debug.Log($"Increasing : time = {time}, scale = {visualTM.transform.localScale.x}");
            }
            
            time = 0f;
            i = 0;
            
            while (time < displayDuration/2 && i < 3600)
            {
                time += Time.deltaTime;
                i++;
                yield return null;

                visualTM.transform.localScale -= 2 * Time.deltaTime * Vector3.one;
            }
            
            
            visualTM.gameObject.SetActive(false);
        }

        public void StartMetronome(float bpm)
        {
            Debug.Log($"start metronome at bpm {bpm}");
            if(_currentMetronomeRoutine != null) StopCoroutine(_currentMetronomeRoutine);
            _currentMetronomeRoutine = StartCoroutine(Metronome(bpm));
        }
        
        private IEnumerator Metronome(float currentBpm)
        {
            if (metronomePivot == null) yield break;

            var goal = (60f / (currentBpm * 2f)) + rUpgrade.Floor / 2;
            var dir = 1;
            var step = 80 / goal;

            Debug.Log($"Metronome : goal = {goal}, step = {step}, rotation = {metronomePivot.transform.rotation.eulerAngles}");

            Debug.Break();
            
            while (true)
            {
                metronomePivot.transform.rotation = Quaternion.Euler(0, 0, dir*80);
                
                //Debug.Log($"Metronome : goal = {goal}, step = {step}, rotation = {metronomePivot.transform.rotation.eulerAngles}");
                dir = -dir;
                
                var time = 0f;
                while (time < goal)
                {
                    yield return null;
                    
                    metronomePivot.transform.Rotate(0, 0, step * dir * Time.deltaTime);
                    
                    time += Time.deltaTime;
                }

                metronomePivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log($"change, time = {time}");

                yield return new WaitForSeconds(time - goal);

                time = 0f;

                while (time < goal)
                {
                    yield return null;
                    
                    metronomePivot.transform.Rotate(0, 0, step * dir * Time.deltaTime);
                    
                    time += Time.deltaTime;
                }
                
                yield return new WaitForSeconds(time - goal);

            }
        }
    }
}