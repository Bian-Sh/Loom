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
        private void Start()
        {
            _ = FooAsync();
        }

        internal async Task FooAsync()
        {
            Debug.LogError($"1. Thread ID ={Thread.CurrentThread.ManagedThreadId}");
            await ToOtherThread;
            Debug.LogError($"2. Thread ID ={Thread.CurrentThread.ManagedThreadId}");

            Post(() =>
            {
                Debug.LogError($"Use Post Thread ID ={Thread.CurrentThread.ManagedThreadId}");
            });

            await ToMainThread;
            Debug.LogError($"3. Thread ID ={Thread.CurrentThread.ManagedThreadId}");
        }
    }
}