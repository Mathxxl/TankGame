using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Entity_Systems
{
    public class AudioController : EntitySystem
    {
        [SerializeField] private AudioElement[] audios;
        [SerializeField] private float smoothIn = 0.2f;
        [SerializeField] private float smoothOut = 0.4f;
        private Rigidbody _rb;
        private bool _moving;

        private float _initVolumeMove;
        private Coroutine _smoothInMoveRoutine;
        private Coroutine _smoothOutMoveRoutine;

        protected override void Awake()
        {
            base.Awake();
            SetClips();
            SetInitValue();
            if (entity.TryGetComponent(out Rigidbody rb))
            {
                _rb = rb;
            }
        }
        
        private void OnEnable()
        {
            entity.Events.OnStartMoving += Moving;
            entity.Events.OnStopMoving += StopMoving;
            entity.Events.OnPerformingAttack += Shoot;
        }

        private void OnDisable()
        {
            entity.Events.OnStartMoving -= Moving;
            entity.Events.OnStopMoving -= StopMoving;
            entity.Events.OnPerformingAttack -= Shoot;
        }

        private void Update()
        {
            if (_moving)
            {
                var elt = GetElementByName("MoveSound");
                if (elt == null) return;
                SetPitchToSpeed(elt.Value.source);
            }
        }

        private AudioElement? GetElementByName(string findName)
        {
            foreach (var a in audios)
            {
                if (a.name == findName) return a;
            }
            return null;
        }

        private void SetClips()
        {
            foreach (var a in audios)
            {
                if(a.source == null || a.clip == null) continue;
                a.source.clip = a.clip;
            }
        }

        private void Moving()
        {
            if (_moving) return;
            _moving = true;
            var elt = GetElementByName("MoveSound");
            if (elt == null) return;
            
            SmoothIn(elt.Value.source, _initVolumeMove);
        }

        private void StopMoving()
        {
            if (!_moving) return;
            _moving = false;
            var elt = GetElementByName("MoveSound");
            if (elt == null) return;
            
            SmoothOut(elt.Value.source, _initVolumeMove);
        }

        private void Shoot()
        {
            var elt = GetElementByName("ShootSound");
            elt?.source.Play();
        }

        private void SetPitchToSpeed(AudioSource source)
        {
            if (_rb == null)
            {
                return;
            }
            var speed = _rb.velocity.magnitude;
            
            source.pitch = 1 + speed / 200;
        }

        private IEnumerator Smooth(AudioSource source, bool goIn = true, float initVal = -1)
        {
            if(goIn) source.Play();
            
            var initVol = source.volume;
            var time = 0f;
            var smooth = goIn ? smoothIn : smoothOut;
            source.volume = goIn ? 0 : source.volume;
            var step = initVol / smooth;
            
            while (time < smooth)
            {
                time += Time.deltaTime;
                yield return null;
                source.volume += (goIn ? 1 : -1)* step * Time.deltaTime;
            }
            
            if(!goIn) source.Stop();

            source.volume = initVal > 0 ? initVal : initVol;
        }

        private void SmoothIn(AudioSource source, float init = -1)
        {
            StopMoveRoutines();
            _smoothInMoveRoutine = StartCoroutine(Smooth(source, true, init));
        }

        private void SmoothOut(AudioSource source, float init = -1)
        {
            StopMoveRoutines();
            _smoothOutMoveRoutine = StartCoroutine(Smooth(source, false, init));
        }

        private void SetInitValue()
        {
            var elt = GetElementByName("MoveSound");
            if (elt == null) return;

            _initVolumeMove = elt.Value.source.volume;
        }

        private void StopMoveRoutines()
        {
            if(_smoothInMoveRoutine != null) StopCoroutine(_smoothInMoveRoutine);
            if(_smoothOutMoveRoutine != null) StopCoroutine(_smoothOutMoveRoutine);
            
            var elt = GetElementByName("MoveSound");
            if (elt == null) return;
            elt.Value.source.volume = _initVolumeMove;
        }
    }
}