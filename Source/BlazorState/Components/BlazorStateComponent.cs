namespace BlazorState
{
  using MediatR;
  using Microsoft.AspNetCore.Components;
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Linq;
  using System.Linq.Expressions;

  /// <summary>
  /// A non required Base Class that injects Mediator and Store.
  /// And exposes StateHasChanged
  /// </summary>
  /// <remarks>Implements IBlazorStateComponent by Injecting</remarks>
  public class BlazorStateComponent : ComponentBase, IDisposable, IBlazorStateComponent
  {
    static readonly ConcurrentDictionary<string, int> s_InstanceCounts = new();

    public BlazorStateComponent()
    {
      string name = GetType().Name;
      int count = s_InstanceCounts.AddOrUpdate(name, 1, (aKey, aValue) => aValue + 1);

      Id = $"{name}-{count}";
    }

    /// <summary>
    /// A generated unique Id based on the Class name and number of times they have been created
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Allows for the Assigning of a value one can use to select an element during automated testing.
    /// </summary>
    [Parameter] public string TestId { get; set; }

    [Inject] public IMediator Mediator { get; set; }
    [Inject] public IStore Store { get; set; }

    /// <summary>
    /// Maintains all components that subscribe to a State.
    /// Is updated by using the GetState method
    /// </summary>
    [Inject] public Subscriptions Subscriptions { get; set; }

    /// <summary>
    /// Exposes StateHasChanged
    /// </summary>
    public void ReRender() => base.InvokeAsync(StateHasChanged);

    /// <summary>
    /// Place a Subscription for the calling component
    /// And returns the requested state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected T GetState<T>()
    {
      Type stateType = typeof(T);
      Subscriptions.Add(stateType, this);
      return Store.GetState<T>();
    }

    /// <summary>
    /// Place a Subscription for the calling component so that individual subscriptions can be added piecemeal
    /// And returns the requested state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected T GetStateWithPropertySubscriptions<T>(params Expression<Func<T,object>>[] propertySelectors) where T : State<T>
    {
      Type stateType = typeof(T);
      Subscriptions.Add(stateType, this, propertySelectors);
      return Store.GetState<T>();
    }


    // TODO idea: can we enable modifying a state that already exists via extension method?
    //public static State<TState> ClearPropertySubscriptions<TState>(this State<TState> state)
    //{
    //  Subscriptions.
    //}

    private List<ParameterExpression> SubscribedProperties { get; } = new();


    public virtual void Dispose()
    {
      Subscriptions.Remove(this);
      GC.SuppressFinalize(this);
    }
  }
}