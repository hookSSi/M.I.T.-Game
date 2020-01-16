using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace MIT.SamtleGame.Tools
{
    public struct GameEvnet
    {
        public string _eventName;
        public GameEvnet(string newName)
        {
            _eventName = newName;
        }
        static GameEvnet _event;
        public static void Trigger(string newName)
        {
            _event._eventName = newName;
            EventManager.TriggerEvent(_event);
        }
    }

    [ExecuteInEditMode]
    public static class EventManager
    {
        private static Dictionary<Type, List<EventListenerBase>> _subscribersList;

        static EventManager()
        {
            _subscribersList = new Dictionary<Type, List<EventListenerBase>>();
        }

		/// <summary>
	    /// Adds a new subscriber to a certain event.
	    /// </summary>
		/// <param name="listener">listener.</param>
	    /// <typeparam name="SamtleEvent">The event type.</typeparam>
		public static void AddListener<SamtleEvent>( EventListener<SamtleEvent> listener ) where SamtleEvent : struct
		{
			Type eventType = typeof(SamtleEvent);

			if(!_subscribersList.ContainsKey(eventType))
				_subscribersList[eventType] = new List<EventListenerBase>();

			if(!SubscriptionExists(eventType, listener))
				_subscribersList[eventType].Add(listener);
		}

		public static void RemoveListener<SamtleEvent>( EventListener<SamtleEvent> listener  ) where SamtleEvent : struct
		{
			Type eventType = typeof( SamtleEvent );

        	if( !_subscribersList.ContainsKey( eventType ) )
	        {
				#if EVENTROUTER_THROWEXCEPTIONS
					throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
				#else
					return;
				#endif
	        }

			List<EventListenerBase> subscriberList = _subscribersList[eventType];
	        bool listenerFound;
			listenerFound = false;

			if(listenerFound)
			{
				
			}

			for (int i = 0; i<subscriberList.Count; i++)
			{
				if( subscriberList[i] == listener )
				{
					subscriberList.Remove( subscriberList[i] );
					listenerFound = true;

					if( subscriberList.Count == 0 )
						_subscribersList.Remove( eventType );

					return;
				}
			}

			#if EVENTROUTER_THROWEXCEPTIONS
		        if( !listenerFound )
		        {
					throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		        }
			#endif
		}

		public static void TriggerEvent<SamtleEvent>( SamtleEvent newEvent ) where SamtleEvent : struct
	    {
	        List<EventListenerBase> list;
	        if( !_subscribersList.TryGetValue( typeof( SamtleEvent ), out list ) )
			#if EVENTROUTER_REQUIRELISTENER
			    throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( MMEvent ).ToString() ) );
			#else
			    return;
			#endif
			
			for (int i=0; i<list.Count; i++)
			{
				( list[i] as EventListener<SamtleEvent> ).OnEvent( newEvent );
			}
	    }

		private static bool SubscriptionExists( Type type, EventListenerBase receiver )
		{
			List<EventListenerBase> receivers;

			if( !_subscribersList.TryGetValue( type, out receivers ) ) return false;

			bool exists = false;

			for( int i = 0; i < receivers.Count; i++ )
			{
				if( receivers[i] == receiver )
				{
					exists = true;
					break;
				}
			}

			return exists;
		}
    }

    /// <summary>
	/// Static class that allows any class to start or stop listening to events
	/// </summary>
	public static class EventRegister
	{
	    public delegate void Delegate<T>( T eventType );

	    public static void EventStartListening<EventType>( this EventListener<EventType> caller ) where EventType : struct
	    {
			EventManager.AddListener<EventType>( caller );
	    }

		public static void EventStopListening<EventType>( this EventListener<EventType> caller ) where EventType : struct
	    {
			EventManager.RemoveListener<EventType>( caller );
	    }
	}

	/// <summary>
	/// Event listener basic interface
	/// </summary>
	public interface EventListenerBase { };

	/// <summary>
	/// A public interface you'll need to implement for each type of event you want to listen to.
	/// </summary>
	public interface EventListener<T> : EventListenerBase
	{
	    void OnEvent( T eventType );
	}
}

