<a id="chinese">[English](#english)</a>

# Loom - Unity多线程工具

Loom是一个强大的Unity多线程工具，旨在简化多线程编程并实现线程间的数据同步。它提供了一种简单而有效的方式，让非主线程能够安全地操作Unity对象，避免了常见的线程同步问题。

## 特点与优势

- **数据同步**：Loom允许非主线程安全地访问和操作Unity对象，通过提供同步上下文来传递线程执行的结果。
- **易于使用**：使用Loom，您可以轻松地在多线程异步编程中执行任务，并通过简单的API将任务委托到主线程执行。
- **高效性能**：Loom通过合理地利用Unity的PlayerLoop System，在主线程的Update循环中执行任务，保证了最佳的性能和响应性。

## 安装和使用

1. 在Unity的Package Manager中，将以下链接复制并粘贴到输入框中以完成安装：``https://github.com/Bian-Sh/Loom.git?path=Packages/Loom``
   ![安装](./Documentation~/images/install.png)
2. 在需要在主线程或非主线程中执行的任务上使用Loom提供的API。
3. Loom会根据您选择的API自动将任务委托到相应的线程中执行，并确保数据同步的正确性。

```csharp
// 使用Loom执行任务的示例代码
// 到主线程执行
await Loom.ToMainThread;

// 到非主线程执行
await Loom.ToOtherThread;

// 使用Lambda表达式的方式
Loom.Post(() =>
{
    Debug.Log($"使用Post线程 ID={Thread.CurrentThread.ManagedThreadId}");
});
```

## 示例应用

Loom的应用场景广泛，特别适用于以下情况：

- 在多线程异步编程中需要访问和操作Unity对象。
- 需要在非主线程中执行耗时操作，并在执行完成后将结果传递到主线程。
- 需要实现高效的线程间数据同步和通信。

## 注意事项

- 请确保在使用Loom之前充分了解多线程编程的基本原理和最佳实践。
- 虽然Loom提供了方便的API来执行任务，但仍然需要谨慎处理多线程的相关问题，例如线程安全和数据一致性等。

Loom工具是提高多线程编程效率的绝佳选择，它简化了异步编程过程，减少了线程同步的复杂性，并提供了高效的数据同步机制。立即尝试Loom，提升您的Unity项目的性能和响应能力！

<a id="english">[简体中文](#chinese)</a>

# Loom - Unity Multithreading Tool

Loom is a powerful Unity tool designed to simplify multithreading programming and achieve data synchronization between threads. It provides a simple and

 efficient way for non-main threads to safely operate Unity objects, avoiding common thread synchronization issues.

## Features and Benefits

- **Data Synchronization**: Loom allows non-main threads to safely access and manipulate Unity objects by providing a synchronization context to pass the results of thread execution.
- **Ease of Use**: With Loom, you can easily execute tasks in multithreaded asynchronous programming and delegate tasks to the main thread for execution through a simple API.
- **High Performance**: Loom optimizes performance and responsiveness by leveraging Unity's PlayerLoop System to execute tasks within the main thread's Update loop.

## Installation and Usage

1. In Unity's Package Manager, copy and paste the following link into the input field to complete the installation: ``https://github.com/Bian-Sh/Loom.git?path=Packages/Loom``
   ![Installation](./Documentation~/images/install.png)
2. Use the provided Loom API on tasks that need to be executed on the main thread or non-main thread.
3. Loom automatically delegates the tasks to the corresponding thread for execution based on the chosen API and ensures the correctness of data synchronization.

```csharp
// Example code for executing a task using Loom
// Execute on the main thread
await Loom.ToMainThread;

// Execute on a non-main thread
await Loom.ToOtherThread;

// Using the lambda expression syntax
Loom.Post(() =>
{
    Debug.Log($"Use Post Thread ID ={Thread.CurrentThread.ManagedThreadId}");
});
```

## Example Applications

Loom has a wide range of applications and is particularly useful in the following scenarios:

- Accessing and manipulating Unity objects in multithreaded asynchronous programming.
- Performing time-consuming operations on non-main threads and passing the results back to the main thread upon completion.
- Implementing efficient synchronization and communication between threads.

## Considerations

- Make sure to have a solid understanding of multithreading programming principles and best practices before using Loom.
- While Loom provides convenient APIs for task execution, it is still important to handle multithreading-related issues such as thread safety and data consistency with care.

Loom is an excellent choice for improving the efficiency of multithreading programming. It simplifies the process of asynchronous programming, reduces the complexity of thread synchronization, and provides an efficient mechanism for data synchronization. Give Loom a try now to enhance the performance and responsiveness of your Unity projects!
