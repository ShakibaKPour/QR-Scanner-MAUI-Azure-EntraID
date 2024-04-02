namespace RepRepair.Models;

public class EventAggregator
{
    private readonly Dictionary<Type, List<WeakReference>> _subscriptions = new();

    public void Subscribe<TMessage>(Action<TMessage> action)
    {
        if (!_subscriptions.ContainsKey(typeof(TMessage)))
        {
            _subscriptions[typeof(TMessage)] = new List<WeakReference>();
        }

        _subscriptions[typeof(TMessage)].Add(new WeakReference(action));
    }

    public void Publish<TMessage>(TMessage message)
    {
        if (!_subscriptions.ContainsKey(typeof(TMessage))) return;

        var toRemove = new List<WeakReference>();
        foreach (var weakReference in _subscriptions[typeof(TMessage)])
        {
            if (weakReference.Target is Action<TMessage> action)
            {
                action.Invoke(message);
            }
            else
            {
                toRemove.Add(weakReference);
            }
        }

        foreach (var remove in toRemove)
        {
            _subscriptions[typeof(TMessage)].Remove(remove);
        }
    }
}
