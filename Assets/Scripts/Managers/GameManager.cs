using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }

        private void Awake()
        {
            instance = this;//overriding is intentional
        }
        
    }
}
