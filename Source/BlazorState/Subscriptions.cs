namespace BlazorState
{
  using Microsoft.Extensions.Logging;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;

  public class Subscriptions
  {
    private readonly ILogger Logger;

    private readonly List<Subscription> BlazorStateComponentReferencesList;

    public Subscriptions(ILogger<Subscriptions> aLogger)
    {
      Logger = aLogger;
      BlazorStateComponentReferencesList = new List<Subscription>();
    }

    public Subscriptions Add<T>(IBlazorStateComponent aBlazorStateComponent)
    {
      Type type = typeof(T);

      return Add(type, aBlazorStateComponent);
    }

    public Subscriptions Add(Type aType, IBlazorStateComponent aBlazorStateComponent)
    {
      // Add only once.
      if (!BlazorStateComponentReferencesList.Any(aSubscription => aSubscription.StateType == aType && aSubscription.ComponentId == aBlazorStateComponent.Id))
      {
        var subscription = new Subscription(
          aType,
          aBlazorStateComponent.Id,
          new WeakReference<IBlazorStateComponent>(aBlazorStateComponent));

        BlazorStateComponentReferencesList.Add(subscription);
      }

      return this;
    }

    /// <summary>
    /// Add subscriptions, but only on the specified properties.
    /// </summary>
    public Subscriptions Add<T>(Type aType, IBlazorStateComponent aBlazorStateComponent, params Expression<Func<T, object>>[] propertySelectors) where T:State<T>
    {
      // Add only once.
      if (!BlazorStateComponentReferencesList.Any(aSubscription => aSubscription.StateType == aType && aSubscription.ComponentId == aBlazorStateComponent.Id))
      {
        string[] parameterNames = propertySelectors
          .Select(tree => (tree.Body as MemberExpression)?.Member.Name)
          .Where(n => n is not null)
          .ToArray();

        Console.WriteLine($"Subscriptions.Add {string.Join(", ", parameterNames)}");

        var subscription = new Subscription(
          aType,
          aBlazorStateComponent.Id,
          new WeakReference<IBlazorStateComponent>(aBlazorStateComponent),
          parameterNames.ToArray()
        );
        BlazorStateComponentReferencesList.Add(subscription);

      }

      return this;
    }


    public override bool Equals(object aObject) =>
      aObject is Subscriptions subscriptions &&
      EqualityComparer<ILogger>.Default.Equals(Logger, subscriptions.Logger) &&
      EqualityComparer<List<Subscription>>.Default.Equals(BlazorStateComponentReferencesList, subscriptions.BlazorStateComponentReferencesList);

    public override int GetHashCode() => HashCode.Combine(Logger, BlazorStateComponentReferencesList);

    public Subscriptions Remove(IBlazorStateComponent aBlazorStateComponent)
    {
      Logger.LogDebug($"Removing Subscription for {aBlazorStateComponent.Id}");
      BlazorStateComponentReferencesList.RemoveAll(aRecord => aRecord.ComponentId == aBlazorStateComponent.Id);

      return this;
    }

    /// <summary>
    /// Will iterate over all subscriptions for the given type and call ReRender on each.
    /// If the target component no longer exists it will remove its subscription.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void ReRenderSubscribers<T>()
    {
      Type type = typeof(T);

      ReRenderSubscribers(type);
    }

    /// <summary>
    /// Will iterate over all subscriptions for the given type and call ReRender on each.
    /// If the target component no longer exists it will remove its subscription.
    /// </summary>
    /// <param name="aType"></param>
    public void ReRenderSubscribers(Type aType)
    {
      IEnumerable<Subscription> subscriptions = BlazorStateComponentReferencesList.Where(aRecord => aRecord.StateType == aType);
      Console.WriteLine($"ReRenderSubscribers");

      foreach (Subscription subscription in subscriptions.ToList())
      {
        Console.WriteLine($"ReRenderSubscribers {subscription.GetType().Name}");

        if (subscription.BlazorStateComponentReference.TryGetTarget(out IBlazorStateComponent target))
        {
          Logger.LogDebug($"ReRender ComponentId:{subscription.ComponentId} StateType.Name:{subscription.StateType.Name}");
          Console.WriteLine($"ReRenderSubscribers {subscription.GetType().Name} target {target.GetType().Name} props {string.Join(", ", subscription.PropertyNames)}");
          target.ReRender();
        }
        else
        {
          // If Dispose is called will I ever have items in this list that got Garbage collected?
          // Maybe for those that don't inherit from our BaseComponent?
          Logger.LogDebug($"Removing Subscription for ComponentId:{subscription.ComponentId} StateType.Name:{subscription.StateType.Name}");
          BlazorStateComponentReferencesList.Remove(subscription);
        }
      }
    }

    private readonly struct Subscription : IEquatable<Subscription>
    {
      public WeakReference<IBlazorStateComponent> BlazorStateComponentReference { get; }
      public string[] PropertyNames { get; }
      public string ComponentId { get; }

      public Type StateType { get; }

      public Subscription(
        Type aStateType,
        string aComponentId,
        WeakReference<IBlazorStateComponent> aBlazorStateComponentReference,
        string[] propertyNames = null)
      {
        StateType = aStateType;
        ComponentId = aComponentId;
        BlazorStateComponentReference = aBlazorStateComponentReference;
        PropertyNames = propertyNames ?? new string[0];
      }

      public static bool operator !=(Subscription aLeftSubscription, Subscription aRightSubscription) => !(aLeftSubscription == aRightSubscription);

      public static bool operator ==(Subscription aLeftSubscription, Subscription aRightSubscription) => aLeftSubscription.Equals(aRightSubscription);

      public bool Equals(Subscription aSubscription) =>
                    EqualityComparer<Type>.Default.Equals(StateType, aSubscription.StateType) &&
        ComponentId == aSubscription.ComponentId &&
        EqualityComparer<WeakReference<IBlazorStateComponent>>.Default.Equals(BlazorStateComponentReference, aSubscription.BlazorStateComponentReference);

      public override bool Equals(object aObject) => this.Equals((Subscription)aObject);

      public override int GetHashCode() => ComponentId.GetHashCode();
    }
  }
}