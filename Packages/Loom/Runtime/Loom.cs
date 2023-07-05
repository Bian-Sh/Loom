// Copyright (c) https://github.com/Bian-Sh
// Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.LowLevel;
namespace zFramework.Internal
{
    public static class Loom
    {
        static int mainThreadId;
        public static bool IsMainThread => Thread.CurrentThread.ManagedThreadId == mainThreadId;
        static readonly ConcurrentQueue<Action> tasks = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Install()
        {
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            #region 使用 PlayerLoop 在 Unity 主线程的 Update 中更新本任务同步器
            // 为了 ref 而 ref
            static ref PlayerLoopSystem FindSubSystem(PlayerLoopSystem root, Predicate<PlayerLoopSystem> predicate)
            {
                for (int i = 0; i < root.subSystemList.Length; i++)
                {
                    if (predicate.Invoke(root.subSystemList[i]))
                    {
                        // 可以关注 ref 配合 return 的用法，这样可以直接修改 root.subSystemList[i] 的值
                        // https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/ref#ref-returns
                        return ref root.subSystemList[i];
                    }
                }
                throw new Exception("Not Found!");
            }
            var rootLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
            ref var sub_pls = ref FindSubSystem(rootLoopSystem, v => v.type == typeof(UnityEngine.PlayerLoop.Update));
            Array.Resize(ref sub_pls.subSystemList, sub_pls.subSystemList.Length + 1);
            sub_pls.subSystemList[^1] = new PlayerLoopSystem { type = typeof(Loom), updateDelegate = Update };
            PlayerLoop.SetPlayerLoop(rootLoopSystem);

#if UNITY_EDITOR
            //因为编辑器停止播放后上面插入的 loopsystem 依旧会触发，进入或退出Play 模式先清空 tasks
            EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
            static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
            {
                if (obj == PlayModeStateChange.ExitingEditMode || obj == PlayModeStateChange.ExitingPlayMode)
                {
                    while (tasks.TryDequeue(out _)) { }//清空任务列表
                }
            }
#endif
            #endregion
        }

#if UNITY_EDITOR
        // 确保编辑器下推送的事件也能被执行
        [InitializeOnLoadMethod]
        static void EditorForceUpdate()
        {
            Install();
            EditorApplication.update -= ForceEditorPlayerLoopUpdate;
            EditorApplication.update += ForceEditorPlayerLoopUpdate;
            static void ForceEditorPlayerLoopUpdate()
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
                {
                    return;   // Not in Edit mode, don't interfere
                }
                Update();
            }
        }
#endif

        /// <summary>
        ///  在主线程中执行
        /// </summary>
        /// <param name="task">要执行的委托</param>
        public static void Post(Action task)
        {
            if (IsMainThread)
            {
                task?.Invoke();
            }
            else
            {
                tasks.Enqueue(task);
            }
        }

        static void Update()
        {
            while (tasks.TryDequeue(out var task))
            {
                try
                {
                    task?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.Log($"{nameof(Loom)}:  封送的任务执行过程中发现异常，请确认: {e}");
                }
            }
        }
        /// <summary>
        /// 切换到主线程中执行
        /// </summary>
        public static SwitchToUnityThreadAwaitable ToMainThread => new();
        /// <summary>
        /// 切换到线程池中执行
        /// </summary>
        public static SwitchToThreadPoolAwaitable ToOtherThread => new();
        public struct SwitchToUnityThreadAwaitable
        {
            public Awaiter GetAwaiter() => new();
            public struct Awaiter : INotifyCompletion
            {
                public bool IsCompleted => IsMainThread;
                public void GetResult() { }
                public void OnCompleted(Action continuation) => Post(continuation);
            }
        }
        public struct SwitchToThreadPoolAwaitable
        {
            public Awaiter GetAwaiter() => new();
            public struct Awaiter : ICriticalNotifyCompletion
            {
                static readonly WaitCallback switchToCallback = state => ((Action)state).Invoke();
                public bool IsCompleted => false;
                public void GetResult() { }
                public void OnCompleted(Action continuation) => ThreadPool.UnsafeQueueUserWorkItem(switchToCallback, continuation);
                public void UnsafeOnCompleted(Action continuation) => ThreadPool.UnsafeQueueUserWorkItem(switchToCallback, continuation);
            }
        }
    }
}
