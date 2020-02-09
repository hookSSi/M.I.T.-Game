using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Tools
{
    public class Node : MonoBehaviour
    {
        [Header("Gizmo 설정")]
        [ColorUsage(false)]
        public Color _gizmoColor = Color.blue;
        public float _gizmoRadius = 0.3f;

        private void OnDrawGizmosSelected() 
        {
            Gizmos.color =_gizmoColor;
            Gizmos.DrawSphere(transform.position, _gizmoRadius);
        }
    }
}

