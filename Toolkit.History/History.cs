using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace cmdwtf.Toolkit.History
{

	/// <summary>
	/// History. Provides an interface for saving past versions of an object.
	/// </summary>
	/// <exception cref='System.NotSupportedException'>
	/// Is thrown when the a method is called at an unsupported time. Example,
	/// calling merge recent history when the current state isn't the latest.
	/// </exception>
	/// <exception cref='System.InvalidOperationException'>
	/// Is thrown when the operations are called at times when they're not applicable.
	/// Example, calling redo with no history to redo.
	/// </exception>
	/// <remarks>
	/// This is not a particularly efficent or performant way of doing a
	/// history or undo/redo system. But it's relatively simple enough to use for many cases.
	/// </remarks>
	public partial class History<T> where T : class
	{
		private readonly List<State<T>> _states;
		private int _maxStates = 5;
		private int _currentState = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="History{T}"/> class.
		/// </summary>
		/// <remarks>
		/// Should not be called externally. Should be called by all other constructors.
		/// </remarks>
		private History()
		{
			if (typeof(T).IsDefined(typeof(JsonObjectAttribute), true) == false)
			{
				throw new NotSupportedException($"History is only usable with types marked with {nameof(JsonObjectAttribute)}.");
			}

			_states = new List<State<T>>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="History{T}"/> class.
		/// </summary>
		/// <param name='max_states'>
		/// Max_states.
		/// </param>
		public History(int max_states) : this()
		{
			_maxStates = max_states;
		}

		/// <summary>
		/// Gets a value indicating whether this instance can undo.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can undo; otherwise, <c>false</c>.
		/// </value>
		public bool CanUndo => (_states.Count > (_currentState + 1));

		/// <summary>
		/// Gets a value indicating whether this instance can redo.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance can redo; otherwise, <c>false</c>.
		/// </value>
		public bool CanRedo => (_currentState > 0);

		/// <summary>
		/// Gets the history info.
		/// </summary>
		/// <value>
		/// The history info.
		/// </value>
		public HistoryDescription[] HistoryInfo
		{
			get
			{
				var s = new HistoryDescription[_states.Count];

				for (int scan = 0; scan < _states.Count; scan++)
				{
					int delta = _currentState - scan;
					s[scan] = new HistoryDescription(_states[scan].Info, delta);
				}

				return s;
			}
		}

		/// <summary>
		/// Gets the state of the current.
		/// </summary>
		/// <value>
		/// The state of the current.
		/// </value>
		/// <remarks>
		/// This function creates a copy of the stored state.
		/// Be careful calling this often.
		/// </remarks>
		public T? CurrentState
		{
			get
			{
				if (_currentState == -1)
				{
					return null;
				}

				if (_currentState >= _states.Count)
				{
					return null;
				}

				return _states[_currentState];
			}
		}

		/// <summary>
		/// Pushes the state given, with the info.
		/// </summary>
		/// <returns>
		/// True if successful.
		/// </returns>
		/// <param name='state'>
		/// The state to store.
		/// </param>
		/// <param name='info'>
		/// Information describing the state.
		/// </param>
		public bool PushCurrentState(T state, string info)
		{
			if (state == null)
			{
				return false;
			}

			if (_currentState > 0)
			{
				ClearFromCurrentForward();
			}

			var hs = new State<T>(state, info);

			_states.Insert(0, hs);

			if (_currentState == -1 && _states.Count == 1)
			{
				_currentState = 0;
			}

			EnforceStateLimit();

			return true;
		}

		/// <summary>
		/// Replaces the last states with the given one. This will effectively 'merge'
		/// those states into the new one. For example if you had a 'remove' then 'add',
		/// and you wanted to show those up as 'moved', you'd call this with your recent state
		/// and tell it to merge the last 2.
		/// </summary>
		/// <returns>
		/// True if successful.
		/// </returns>
		/// <param name='state'>
		/// The state to store.
		/// </param>
		/// <param name='info'>
		/// Information describing the state.
		/// </param>
		/// <param name='number_to_merge'>
		/// The number of previous states to merge.
		/// </param>
		/// <exception cref='System.NotSupportedException'>
		/// Is thrown if the current state isn't the latest. You must be on the latest
		/// state to merge and replace states.
		/// </exception>
		public bool ReplaceLastStates(T state, string info, int number_to_merge)
		{
			if (state == null)
			{
				return false;
			}

			if (_currentState > 0)
			{
				throw new NotSupportedException("States can only be merged if the current state is the latest.");
			}

			var merge = new State<T>(state, info);

			_states.RemoveRange(0, Math.Min(number_to_merge, _states.Count));

			_states.Insert(0, merge);

			if (_currentState == -1 && _states.Count == 1)
			{
				_currentState = 0;
			}

			EnforceStateLimit();

			return true;
		}

		/// <summary>
		/// Goes one step back, if possible.
		/// Then returns that state.
		/// </summary>
		/// <param name='steps'>
		/// The number of steps to undo, defaulting to one.
		/// </param>
		public State<T> Undo(int steps = 1)
		{
			while (CanUndo && steps-- > 0)
			{
				_currentState += 1;
			}

			return _states[_currentState];
		}

		/// <summary>
		/// Goes forward one step if possible. Then returns that state.
		/// </summary>
		/// <param name='steps'>
		/// The number of steps to redo, defaulting to one.
		/// </param>
		/// <exception cref='System.InvalidOperationException'>
		/// Is thrown if there is no history.
		/// </exception>
		public State<T> Redo(int steps = 0)
		{
			if (_currentState < 0)
			{
				throw new System.InvalidOperationException("Cannot redo with no history.");
			}

			if (_currentState == 0)
			{
				return _states[_currentState];
			}

			while (CanRedo && steps-- > 0)
			{
				_currentState--;
			}

			// this shouldn't happen.
			System.Diagnostics.Debug.Assert(_currentState < _states.Count);

			return _states[_currentState];
		}

		/// <summary>
		/// Clears all history.
		/// </summary>
		public void Clear() => Clear(-1);

		/// <summary>
		/// Clears all history, sets new max state limit.
		/// </summary>
		/// <param name='newMaxStates'>
		/// New_max_states.
		/// </param>
		public void Clear(int newMaxStates)
		{
			SetMaxStates(newMaxStates);

			_currentState = -1;
			_states.Clear();
		}

		/// <summary>
		/// Sets the max states to store.
		/// </summary>
		/// <param name='newMaxStates'>
		/// New_max_states.
		/// </param>
		public void SetMaxStates(int newMaxStates)
		{
			if (newMaxStates > 0)
			{
				_maxStates = newMaxStates;
			}

			EnforceStateLimit();
		}

		/// <summary>
		/// Clears from current state forward.
		/// Used internally when a new state is
		/// added when not at the most recent.
		/// </summary>
		public void TrimLaterStates() => ClearFromCurrentForward();

		/// <summary>
		/// Clears from current state forward.
		/// Used internally when a new state is
		/// added when not at the most recent.
		/// </summary>
		private void ClearFromCurrentForward()
		{
			_states.RemoveRange(0, _currentState);
			_currentState = 0;
		}

		/// <summary>
		/// Enforces the state limit. Trims the
		/// history list down to the max size.
		/// </summary>
		private void EnforceStateLimit()
		{
			if (_states.Count > _maxStates)
			{
				_states.RemoveRange(_maxStates, _states.Count - _maxStates);
			}
		}
	}
}
