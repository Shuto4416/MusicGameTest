using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Notes {
    public interface IBaseNote
    {
        public void Initialize(float lifeSpan, int laneNum);
    }
}
