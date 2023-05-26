using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LoadingScreen
{
    public class Fading : MonoBehaviour
    {
        #region Attributes

        [SerializeField] private Button button;
        [SerializeField] private GameObject splashScreen;
        [SerializeField] private GameObject splashScreenToGo; 
        [SerializeField] private float duration = 1.2f;

        private List<Image> _splashScreenImage;
        private List<Image> _splashScreenToGoImage;
        private bool _fading;

        #endregion

        #region Properties

        public bool IsFading => _fading;

        public GameObject SplashScreen => splashScreen;

        public GameObject SplashScreenToGo => splashScreenToGo;

        public float Duration
        {
            get => duration;
            set => duration = value;
        }

        #endregion
    
        #region Mono Behaviours

        private void Start()
        {
            //Setting the listeners of the button
            if (button)
            {
                button.onClick.AddListener(Fade);
            }
            Init();
        }
    
        #endregion
    

        #region Public Methods
    
        public void Fade()
        {
            if (!_fading) StartCoroutine(ToFade());
        }

        public void InverseImg()
        {
            (splashScreen, splashScreenToGo) = (splashScreenToGo, splashScreen);
            Init();
        }

        public void Set(GameObject img1, GameObject img2)
        {
            splashScreen = img1;
            splashScreenToGo = img2;
            Init();
        }

        public void ResetFade()
        {
            StopAllCoroutines();
            SetAllAlpha(_splashScreenImage, 1);
            SetAllAlpha(_splashScreenToGoImage, 0);
        }
    
        #endregion

        #region Private Methods
    
        private void Init()
        {
            //Setting the lists
            _splashScreenImage = new List<Image>();
            _splashScreenToGoImage = new List<Image>();
        
            //Setting the image list (in order to change the alpha of the children too
            if(splashScreen) SetImageList(_splashScreenImage, splashScreen);
            if(splashScreenToGo) SetImageList(_splashScreenToGoImage, splashScreenToGo);
        
            //Set first to visible and second to invisible
            SetAllAlpha(_splashScreenImage, 1);
            SetAllAlpha(_splashScreenToGoImage, 0);
        }

    
        private void SetImageList(ICollection<Image> l, GameObject splash)
        {
            if(splash) l.Add(splash.GetComponent<Image>());
            var childImages = splash.GetComponentsInChildren<Image>();
            foreach (var img in childImages){ l.Add(img); }
        }

        /// <summary>
        /// Performs a fade in of the splash screen in duration time
        /// </summary>
        /// <returns></returns>
        private IEnumerator ToFade()
        {
            _fading = true;
        
            float time = 0;
            while (time <= duration)
            {
                time += Time.deltaTime;
                var alphaValue = (time / duration);
                SetAllAlpha(_splashScreenImage, 1 - alphaValue);
                SetAllAlpha(_splashScreenToGoImage, alphaValue);
                yield return null;
            }

            _fading = false;
            OnFadeEnd?.Invoke();
        }

        //Set the alpha of the image in paramerter
        private void SetAlpha(Graphic image, float value)
        {
            var tmp = image.color;
            tmp.a = value;
            image.color = tmp;
        }

        //Set the alpha of each images in the parameter list
        private void SetAllAlpha(List<Image> limage, float value)
        {
            foreach (var image in limage)
            {
                SetAlpha(image, value);
            }
        }
    
        #endregion

        #region Events

        public delegate void OnFadeEndEventHandler();

        public OnFadeEndEventHandler OnFadeEnd;

        #endregion
    }
}
