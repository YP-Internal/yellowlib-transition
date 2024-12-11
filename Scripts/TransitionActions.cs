using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YellowPanda.Transition {
    public class TransitionActions : MonoBehaviour {
        public void LoadScene(string scene) {
            Transition.LoadScene(scene);
        }

        public void FadeIn() {
            Transition.FadeIn();
        }
    }

}