using System.Collections;

namespace easy_core;

/// <summary>
/// Defines properties and methods of a lexicon. A lexicon is a dictionary that can contain multiple items with the same key.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface ILexicon<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, ICollection, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	/// <inheritdoc cref="Dictionary{TKey, TValue}.Keys"/>
	ICollection<TKey> Keys { get; }

	/// <summary>
	/// Gets a collection containing the values in the <see cref="Lexicon{TKey, TValue}"/> grouped by key.
	/// </summary>
	ICollection<List<TValue>> GroupedValues { get; }

	/// <inheritdoc cref="Dictionary{TKey, TValue}.Values"/>
	ICollection<TValue> Values { get; }

	/// <inheritdoc cref="Dictionary{TKey, TValue}.this"/>
	List<TValue> this[TKey key] { get; set; }

	/// <inheritdoc cref="Dictionary{TKey, TValue}.this"/>
	TValue this[TKey key, int index] { get; set; }

	/// <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
	void Add(TKey key, TValue value);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
	void Add(TKey key, IEnumerable<TValue> values);

	/// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
	void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items);

	/// <summary>
	/// Updates the provided value to a new value.
	/// </summary>
	/// <param name="key">The key that contains the value.</param>
	/// <param name="oldvalue">The value to change.</param>
	/// <param name="newValue">The value to assign.</param>
	bool ChangeValue(TKey key, TValue oldvalue, TValue newValue);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.ContainsValue(TValue)"/>
	bool Contains(TKey key, TValue value);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.ContainsKey(TKey)"/>
	bool ContainsKey(TKey key);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.ContainsValue(TValue)"/>
	bool ContainsValue(TValue value);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
	bool Remove(TKey key, TValue value);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.Remove(TKey)"/>
	bool Remove(TKey key);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
	bool TryGetValue(TKey key, int index, out TValue value);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
	bool TryGetValue(TKey key, out List<TValue> values);

	/// <inheritdoc cref="Dictionary{TKey, TValue}.TrimExcess()"/>
	void TrimExcess();

	/// <inheritdoc cref="Dictionary{TKey, TValue}.TrimExcess(int)"/>
	void TrimExcess(int capacity);
}
