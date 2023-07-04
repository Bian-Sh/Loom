using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using static zFramework.Internal.Loom;
namespace zFramework.Example.Loom
{
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(Demo))]
    class DemoEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Click~~"))
            {
                _ = (target as Demo).FooAsync();
            }
        }
    }
#endif
    public class Demo : MonoBehaviour
    {
        internal async Task FooAsync()
        {
            Debug.Log($"1. Thread ID ={Thread.CurrentThread.ManagedThreadId}");
            await ToOtherThread;
            Debug.Log($"2. Thread ID ={Thread.CurrentThread.ManagedThreadId}");

            Post(() =>
            {
                Debug.Log($"Use Post Thread ID ={Thread.CurrentThread.ManagedThreadId}");
            });
            
            await ToMainThread;
            Debug.Log($"3. Thread ID ={Thread.CurrentThread.ManagedThreadId}");
        }
    }
}