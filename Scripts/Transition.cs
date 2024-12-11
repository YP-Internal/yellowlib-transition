using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YellowPanda.Transition {
    public class Transition : MonoBehaviour {
        public static Transition Instance;

        public static Transition defaultTransitionPrefab;
        public static Transition overrideTransitionPrefab;

        // Parameters
        public bool autoFadeIn = true;

        // Components
        Animator _animator;

        // Internal
        bool sameScene, sameSceneAutoIn;
        AsyncOperation loadingScene;
        Action sameSceneCallback;

        [Obsolete("Goto is deprecated, please use LoadScene instead.")] //Deprecando porque o nome é feio
        public static void Goto(string scene, Transition transitionPrefab = null)
        {
            LoadScene(scene, transitionPrefab);
        }

        public static void LoadScene(string scene, Transition transitionPrefab = null) {
            if (defaultTransitionPrefab == null) {
                Debug.LogWarning("No default transition set, use Transition.defaultTransitionPrefab to set a default transition");
            }
            overrideTransitionPrefab = transitionPrefab;

            // Create if isn't instantiated already
            if (Instance == null) {
                if (overrideTransitionPrefab == null)
                    Instance = Instantiate(defaultTransitionPrefab);
                else
                    Instance = Instantiate(overrideTransitionPrefab);
            }
            // Disable auto fade in for this instance
            Instance.autoFadeIn = false;
            Instance.sameScene = false;

            // Load requested scene
            Instance.loadingScene = SceneManager.LoadSceneAsync(scene);
            Instance.loadingScene.allowSceneActivation = false;
            TransitionManager.Instance.PrepareSceneIntro(scene);

            // Fade out
            Instance._animator.Play("Out");
        }

        public static void SameSceneTransition(Action callback = null, bool autoIn = true) {
            // Create if isn't instantiated already
            if (Instance == null) {
                Instance = Instantiate(defaultTransitionPrefab);
            }

            // Disable auto fade in for this instance
            Instance.autoFadeIn = false;

            // Assign callback
            Instance.sameSceneCallback = callback;
            Instance.sameSceneAutoIn = autoIn;
            Instance.sameScene = true;

            // Fade out
            Instance._animator.Play("Out");
        }

        void OnDestroy() {
            // Release reference
            Instance = null;
        }

        public static void FadeIn() {
            if (Instance == null) return;

            Instance._animator.Play("In");
        }

        public void FadeInDone() {
            Destroy(gameObject);
        }

        public void FadeOutDone() {
            if (sameScene) {
                sameSceneCallback?.Invoke();
                if (sameSceneAutoIn) StartCoroutine(FadeInSkipFrame());
            }
            else {
                loadingScene.allowSceneActivation = true;
            }
        }

        IEnumerator FadeInSkipFrame() {
            // Skip 2 frames
            yield return null;
            yield return null;

            // Fade in
            _animator.Play("In");
        }

        void Start() {
            if (autoFadeIn) {
                _animator.Play("In", 0, 0);
            }
        }

        void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(Instance.gameObject);
            }

            Instance = this;

            // Components
            _animator = GetComponent<Animator>();
        }
    }
}