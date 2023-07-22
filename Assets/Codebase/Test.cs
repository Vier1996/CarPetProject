using System.Drawing;
using Codebase.Extension.String.Cast;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Codebase
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private int size = 10;
        
        [Button]
        private void SetArray()
        {
            string der = "";
            
            for (int i = 0; i < size; i++)
            {
                der = i.ToStringNonAlloc();
                
                Debug.Log(der);
            }
            
        }
    }
}
