using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YellowPanda.Transition {
    public class TransitionActions : MonoBehaviour {
        public void Goto(string scene) {
            Transition.Goto(scene);
        }

        public void FadeIn() {
            Transition.FadeIn();
        }
    }

}