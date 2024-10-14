using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObservableCollections;

public class Invoker : MonoBehaviour
{
    public IObservableCollection<ILongCommand> UndoLongCommandsBuffer => _undoLongCommandsBuffer;
    public IObservableCollection<ILongCommand> RedoLongCommandsBuffer => _redoLongCommandsBuffer;
    
    private const int CACHE_SIZE = 30;
    private readonly Queue<ILongCommand> _longCommands = new();
    // private readonly ObservableStack<ILongCommand> _longCommandsBuffer = new(CACHE_SIZE);
    private readonly ObservableFixedSizeRingBuffer<ILongCommand> _undoLongCommandsBuffer = new(CACHE_SIZE);
    private readonly ObservableFixedSizeRingBuffer<ILongCommand> _redoLongCommandsBuffer = new(CACHE_SIZE);

    public void AddLongCommand(ILongCommand longCommand)
    {
        _longCommands.Enqueue(longCommand);
    }

    public void ExecuteFirstLongCommand(Action callback = null)
    {
        if (_longCommands.Count > 0)
        {
            if (_redoLongCommandsBuffer.Count > 0)
                _redoLongCommandsBuffer.Clear();
            StartCoroutine(ExecuteFirstLongCommandCoroutine(callback));
        }
    }

    public void UndoLastLongCommand(Action callback = null)
    {
        if (_undoLongCommandsBuffer.Count > 0)
        {
            StartCoroutine(UndoLastLongCommandCoroutine(callback));
        }
    }

    public void RedoLastLongCommand(Action callback = null)
    {
        if (_redoLongCommandsBuffer.Count > 0)
        {
            StartCoroutine(RedoLastLongCommandCoroutine(callback));
        }
    }

    public void Clear()
    {
        StopAllCoroutines();
        _undoLongCommandsBuffer.Clear();
        _redoLongCommandsBuffer.Clear();
        _longCommands.Clear();
    }

    private IEnumerator ExecuteFirstLongCommandCoroutine(Action callback = null)
    {
        var longCommandToExecute = _longCommands.Dequeue();
        if (longCommandToExecute != null)
        {
            yield return longCommandToExecute.Execute();
            //_longCommandsBuffer.Push(longCommandToExecute);
            PushLongCommandsUndoBuffer(longCommandToExecute);
        } 
        callback?.Invoke();
    }

    private IEnumerator UndoLastLongCommandCoroutine(Action callback = null)
    {
        var longCommandToUndo = _undoLongCommandsBuffer.RemoveLast();
        PushLongCommandsRedoBuffer(longCommandToUndo);
        yield return longCommandToUndo?.Undo();
        callback?.Invoke();
    }

    private IEnumerator RedoLastLongCommandCoroutine(Action callback = null)
    {
        var longCommandToRedo = _redoLongCommandsBuffer.RemoveLast();
        PushLongCommandsUndoBuffer(longCommandToRedo);
        yield return longCommandToRedo?.Execute();
        callback?.Invoke();
    }

    private void PushLongCommandsUndoBuffer(ILongCommand longCommand)
    {
        if (_undoLongCommandsBuffer.Count >= CACHE_SIZE)
        {
            _undoLongCommandsBuffer.RemoveFirst();
        }

        _undoLongCommandsBuffer.AddLast(longCommand);
    }

    private void PushLongCommandsRedoBuffer(ILongCommand longCommand)
    {
        if (_redoLongCommandsBuffer.Count >= CACHE_SIZE)
        {
            _redoLongCommandsBuffer.RemoveFirst();
        }

        _redoLongCommandsBuffer.AddLast(longCommand);
    }
}
