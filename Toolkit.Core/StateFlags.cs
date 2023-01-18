#if NET5_0_OR_GREATER || NET47_OR_GREATER
using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace cmdwtf.Toolkit
{
	/// <summary>
	/// Provides a simple light bit vector with easy integer/enum or Boolean access to
	/// a 32 bit storage.
	/// </summary>
	/// <remarks>
	/// This struct based on <see cref="BitVector32"/>.
	/// Licensed under the MIT license.
	/// Copyright (c) 2021 Chris Marc Dailey (nitz)
	/// Copyright (c) .NET Foundation and Contributors
	/// <see href="https://github.com/dotnet/runtime/blob/8f12df6f3f39ea2bedf6c222664720a48c03c599/src/libraries/System.Collections.Specialized/src/System/Collections/Specialized/StateFlags.cs">StateFlags.cs</see>
	/// </remarks>
	public struct StateFlags<T> where T : Enum
	{
		private uint _data;

		/// <summary>
		/// Initializes a new instance of the StateFlags structure with the specified internal data.
		/// </summary>
		public StateFlags(int data)
		{
			_data = unchecked((uint)data);
			CheckEnumWidth();
		}

		/// <summary>
		/// Initializes a new instance of the StateFlags structure with the information in the specified
		/// value.
		/// </summary>
		public StateFlags(StateFlags<T> value)
		{
			_data = value._data;
			CheckEnumWidth();
		}

		/// <summary>
		/// Initializes a new instance of the StateFlags structure with the information in the specified
		/// value.
		/// </summary>
		public StateFlags(BitVector32 value)
		{
			_data = unchecked((uint)value.Data);
			CheckEnumWidth();
		}

		/// <summary>
		///	Gets or sets a value indicating whether all the specified bits are set.
		/// </summary>
		public bool this[int bit]
		{
			get => (_data & bit) == unchecked((uint)bit);
			set
			{
				unchecked
				{
					if (value)
					{
						_data |= (uint)bit;
					}
					else
					{
						_data &= ~(uint)bit;
					}
				}
			}
		}

		/// <summary>
		///	Gets or sets a value indicating whether all the specified bits are set.
		/// </summary>
		public bool this[T bit]
		{
			get
			{
				uint bitVal =
#if NET5_0_OR_GREATER
					Unsafe.As<T, uint>(ref bit);
#else
					Convert.ToUInt32(bit);
#endif // NET5_0_OR_GREATER
				return (_data & bitVal) == unchecked(bitVal);
			}
			set
			{
				unchecked
				{
					uint bitVal =
#if NET5_0_OR_GREATER
						Unsafe.As<T, uint>(ref bit);
#else
						Convert.ToUInt32(bit);
#endif // NET5_0_OR_GREATER
					if (value)
					{
						_data |= bitVal;
					}
					else
					{
						_data &= ~bitVal;
					}
				}
			}
		}

		/// <summary>
		/// returns the raw data stored in this bit vector...
		/// </summary>
		public int Data => unchecked((int)_data);

		/// <summary>
		/// Creates the first mask in a series.
		/// </summary>
		public static int CreateMask() => CreateMask(0);

		/// <summary>
		/// Creates the next mask in a series.
		/// </summary>
		public static int CreateMask(int previous)
		{
			if (previous == 0)
			{
				return 1;
			}

			if (previous == unchecked((int)0x80000000))
			{
				throw new InvalidOperationException($"{nameof(StateFlags<T>)} full, no more masks can be created.");
			}

			return previous << 1;
		}

		/// <inheritdoc cref="object.ToString()"/>
		public static string ToString(StateFlags<T> value)
		{
#if NET5_0_OR_GREATER
			string prefixStr = $"StateFlags<{typeof(T).Name}>{{";
			return string.Create(prefixStr.Length + /*32 bits*/32 + /*"}".Length"*/1, value, (dst, v) =>
			{
				ReadOnlySpan<char> prefix = prefixStr;
				prefix.CopyTo(dst);
				dst[^1] = '}';

				int locdata = unchecked((int)v._data);
				dst = dst.Slice(prefix.Length, 32);
				for (int i = 0; i < dst.Length; i++)
				{
					dst[i] = (locdata & 0x80000000) != 0 ? '1' : '0';
					locdata <<= 1;
				}
			});
#else
			System.Text.StringBuilder sb = new(/*"BitVector32{".Length*/12 + /*32 bits*/32 + /*"}".Length"*/1);
			sb.Append("BitVector32{");
			int locdata = unchecked((int)value._data);
			for (int i = 0; i < 32; i++)
			{
				if ((locdata & 0x80000000) != 0)
				{
					sb.Append('1');
				}
				else
				{
					sb.Append('0');
				}
				locdata <<= 1;
			}
			sb.Append('}');
			return sb.ToString();
#endif // NET5_0_OR_GREATER
		}

		#region object Overrides

		/// <inheritdoc cref="object.Equals(object?)"/>
		public override bool Equals(object? obj)
			=> obj switch
			{
				StateFlags<T> other => _data == other._data,
				BitVector32 other => _data == other.Data,
				_ => false,
			};

		/// <inheritdoc cref="object.GetHashCode"/>
		public override int GetHashCode() => _data.GetHashCode();

		/// <inheritdoc cref="object.ToString"/>
		public override string ToString() => ToString(this);

		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="left">The lefthand <see cref="StateFlags{T}"/> operand to compare.</param>
		/// <param name="right">The righthand <see cref="StateFlags{T}"/> operand to compare.</param>
		/// <returns>true, if the flags are equal; otherwise, false.</returns>
		public static bool operator ==(StateFlags<T> left, StateFlags<T> right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Compares <paramref name="left"/> to <paramref name="right"/>.
		/// </summary>
		/// <param name="left">The lefthand <see cref="StateFlags{T}"/> operand to compare.</param>
		/// <param name="right">The righthand <see cref="StateFlags{T}"/> operand to compare.</param>
		/// <returns>true, if the flags are not equal; otherwise, false.</returns>
		public static bool operator !=(StateFlags<T> left, StateFlags<T> right)
		{
			return !(left == right);
		}

		#endregion Object Overrides

		/// <summary>
		/// Ensures that <see cref="StateFlags{T}"/> is wide enough to represent <typeparamref name="T"/>.
		/// </summary>
		private void CheckEnumWidth()
		{
			_data = 0;

			int tSize = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
			int dataSize = Marshal.SizeOf(_data.GetType());
			if (dataSize < tSize)
			{
				throw new ArgumentException($"{nameof(StateFlags<T>)} can not represent flags for enum of type {typeof(T).FullName}. " +
					$"It is {tSize} bytes wide, and {nameof(StateFlags<T>)} is only {dataSize} bytes long.", nameof(T));
			}
		}
	}
}
#endif // NET5_0_OR_GREATER
