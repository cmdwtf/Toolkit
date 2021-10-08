
using System;

using Newtonsoft.Json;

namespace cmdwtf.Toolkit.History
{
	/// <summary>
	/// A small container to represent a state at a point in history.
	/// </summary>
	/// <typeparam name="T">The type of object the state represents.</typeparam>
	public class State<T> where T : class
	{
		/// <summary>
		/// Creates a state representing the given object, with it's associated
		/// annotation as a string.
		/// </summary>
		/// <param name="state">The object to store the state of.</param>
		/// <param name="info">The descriptive information about the object to store.</param>
		public State(T state, string info)
		{
			_state = JsonConvert.SerializeObject(state, SerializerSettings);
			Info = info;
		}

		private readonly string _state = null;

		private static readonly JsonSerializerSettings SerializerSettings = new()
		{
			Formatting = Formatting.None,
			//PreserveReferencesHandling = PreserveReferencesHandling.Objects
		};

		/// <summary>
		/// Information describing the state.
		/// </summary>
		public string Info { get; init; }

		/// <summary>
		/// The date/time the state was created.
		/// </summary>
		public DateTimeOffset Created { get; init; } = DateTimeOffset.Now;

		/// <summary>
		/// Gets the deserialzed object stored in the state.
		/// </summary>
		/// <returns>The object.</returns>
		public T GetState()
			=> JsonConvert.DeserializeObject<T>(_state, SerializerSettings);

		/// <summary>
		/// Gets the deserialized object, stored in the state, implicitly.
		/// </summary>
		/// <param name="historyState">The state to deserialize.</param>
		public static implicit operator T(State<T> historyState)
		{
			return historyState.GetState();
		}
	}
}
