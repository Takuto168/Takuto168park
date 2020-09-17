using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 双方向辞書
    /// キーから値へのマッピングと、その逆の値からキーへのマッピングを同時に保証するキーと値のコレクションを表します。
    /// </summary>
    /// <typeparam name="TKey">ディクショナリ内のキーの型。</typeparam>
    /// <typeparam name="TValue">ディクショナリ内の値の型。</typeparam>
    public class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region field

        /// <summary>
        /// キーから値へのマッピング。
        /// </summary>
        private Dictionary<TKey, TValue> _keyToValues;

        /// <summary>
        /// 値からキーへのマッピング。
        /// </summary>
        private Dictionary<TValue, TKey> _valueToKeys;

        #endregion field

        #region constructer

        /// <summary>
        /// 空で、既定の初期量を備え、キーの型と値型の既定の等値比較子を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public LinkedDictionary() : this(0, null, null) { }

        /// <summary>
        /// 空で、指定した初期量を備え、キーの型と値型の既定の等値比較子を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="capacity"><see cref="LinkedDictionary{TKey, TValue}"/> が格納できる要素数の初期値。</param>
        public LinkedDictionary(int capacity) : this(capacity, null, null) { }

        /// <summary>
        /// 空で、既定の初期量を備え、指定した <see cref="IEqualityComparer{TKey}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="comparerKey">キーの比較時に使用する <see cref="IEqualityComparer{TKey}"/> 実装。キーの型の既定の <see cref="EqualityComparer{TKey}"/> を使用する場合は null。</param>
        public LinkedDictionary(IEqualityComparer<TKey> comparerKey) : this(0, comparerKey, null) { }

        /// <summary>
        /// 空で、既定の初期量を備え、指定した <see cref="IEqualityComparer{TValue}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="comparerValue">値の比較時に使用する <see cref="IEqualityComparer{TValue}"/> 実装。値の型の既定の <see cref="EqualityComparer{TValue}"/> を使用する場合は null。</param>
        public LinkedDictionary(IEqualityComparer<TValue> comparerValue) : this(0, null, comparerValue) { }

        /// <summary>
        /// 空で、既定の初期量を備え、指定した <see cref="IEqualityComparer{TKey}"/> と <see cref="IEqualityComparer{TValue}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="comparerKey">キーの比較時に使用する <see cref="IEqualityComparer{TKey}"/> 実装。キーの型の既定の <see cref="EqualityComparer{TKey}"/> を使用する場合は null。</param>
        /// <param name="comparerValue">値の比較時に使用する <see cref="IEqualityComparer{TValue}"/> 実装。値の型の既定の <see cref="EqualityComparer{TValue}"/> を使用する場合は null。</param>
        public LinkedDictionary(IEqualityComparer<TKey> comparerKey, IEqualityComparer<TValue> comparerValue) : this(0, comparerKey, comparerValue) { }

        /// <summary>
        /// 空で、指定したの初期量を備え、指定した <see cref="IEqualityComparer{TKey}"/> と <see cref="IEqualityComparer{TValue}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="capacity"><see cref="LinkedDictionary{TKey, TValue}"/> が格納できる要素数の初期値。</param>
        /// <param name="comparerKey">キーの比較時に使用する <see cref="IEqualityComparer{TKey}"/> 実装。キーの型の既定の <see cref="EqualityComparer{TKey}"/> を使用する場合は null。</param>
        /// <param name="comparerValue">値の比較時に使用する <see cref="IEqualityComparer{TValue}"/> 実装。値の型の既定の <see cref="EqualityComparer{TValue}"/> を使用する場合は null。</param>
        public LinkedDictionary(int capacity, IEqualityComparer<TKey> comparerKey, IEqualityComparer<TValue> comparerValue)
        {
            if (capacity < 0) ThrowArgumentOutOfRangeException(nameof(capacity));

            this._keyToValues = new Dictionary<TKey, TValue>(capacity, comparerKey);
            this._valueToKeys = new Dictionary<TValue, TKey>(capacity, comparerValue);
        }

        /// <summary>
        /// 指定した <see cref="IDictionary{TKey, TValue}"/> から要素をコピーして格納し、キー型と値型の既定の等値比較子を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dictionary">新しい <see cref="LinkedDictionary{TKey, TValue}"/> に要素をコピーする <see cref="IDictionary{TKey, TValue}"/>。</param>
        public LinkedDictionary(IDictionary<TKey, TValue> dictionary) : this(dictionary, null, null) { }

        /// <summary>
        /// 指定した <see cref="IDictionary{TKey, TValue}"/> から要素をコピーして格納し、指定した <see cref="IEqualityComparer{TKey}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dictionary">新しい <see cref="LinkedDictionary{TKey, TValue}"/> に要素をコピーする <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="comparerKey">キーの比較時に使用する <see cref="IEqualityComparer{TKey}"/> 実装。キーの型の既定の <see cref="EqualityComparer{TKey}"/> を使用する場合は null。</param>
        public LinkedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparerKey) : this(dictionary, comparerKey, null) { }

        /// <summary>
        /// 指定した <see cref="IDictionary{TKey, TValue}"/> から要素をコピーして格納し、指定した <see cref="IEqualityComparer{TValue}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dictionary">新しい <see cref="LinkedDictionary{TKey, TValue}"/> に要素をコピーする <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="comparerValue">値の比較時に使用する <see cref="IEqualityComparer{TValue}"/> 実装。値の型の既定の <see cref="EqualityComparer{TValue}"/> を使用する場合は null。</param>
        public LinkedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TValue> comparerValue) : this(dictionary, null, comparerValue) { }

        /// <summary>
        /// 指定した <see cref="IDictionary{TKey, TValue}"/> から要素をコピーして格納し、指定した <see cref="IEqualityComparer{TKey}"/> と <see cref="IEqualityComparer{TValue}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dictionary">新しい <see cref="LinkedDictionary{TKey, TValue}"/> に要素をコピーする <see cref="IDictionary{TKey, TValue}"/>。</param>
        /// <param name="comparerKey">キーの比較時に使用する <see cref="IEqualityComparer{TKey}"/> 実装。キーの型の既定の <see cref="EqualityComparer{TKey}"/> を使用する場合は null。</param>
        /// <param name="comparerValue">値の比較時に使用する <see cref="IEqualityComparer{TValue}"/> 実装。値の型の既定の <see cref="EqualityComparer{TValue}"/> を使用する場合は null。</param>
        public LinkedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparerKey, IEqualityComparer<TValue> comparerValue) :
              this(dictionary != null ? dictionary.Count : 0, comparerKey, comparerValue)
        {
            if (dictionary == null) ThrowArgumentNullException(nameof(dictionary));
            this.AddRange(dictionary);
        }

        /// <summary>
        /// 指定した <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/> からコピーされた要素を格納する <see cref="LinkedDictionary{TKey, TValue}"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="collection">新しい <see cref="LinkedDictionary{TKey, TValue}"/> に要素をコピーする <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/>。</param>
        public LinkedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, null, null) { }

        /// <summary>
        /// 指定した <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/> から要素をコピーして格納し、指定した <see cref="IEqualityComparer{TKey}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="collection">新しい <see cref="LinkedDictionary{TKey, TValue}"/> に要素をコピーする <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/>。</param>
        /// <param name="comparerKey">キーの比較時に使用する <see cref="IEqualityComparer{TKey}"/> 実装。キーの型の既定の <see cref="EqualityComparer{TKey}"/> を使用する場合は null。</param>
        public LinkedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparerKey) : this(collection, comparerKey, null) { }

        /// <summary>
        /// 指定した <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/> から要素をコピーして格納し、指定した <see cref="IEqualityComparer{TValue}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="collection">新しい <see cref="LinkedDictionary{TKey, TValue}"/> に要素をコピーする <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/>。</param>
        /// <param name="comparerValue">値の比較時に使用する <see cref="IEqualityComparer{TValue}"/> 実装。値の型の既定の <see cref="EqualityComparer{TValue}"/> を使用する場合は null。</param>
        public LinkedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TValue> comparerValue) : this(collection, null, comparerValue) { }

        /// <summary>
        /// 指定した <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/> から要素をコピーして格納し、指定した <see cref="IEqualityComparer{TKey}"/> と <see cref="IEqualityComparer{TValue}"/> を使用する、<see cref="LinkedDictionary{TKey, TValue}"/>クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="collection">新しい <see cref="LinkedDictionary{TKey, TValue}"/> に要素をコピーする <see cref="IEnumerable{KeyValuePair{TKey, TValue}}"/>。</param>
        /// <param name="comparerKey">キーの比較時に使用する <see cref="IEqualityComparer{TKey}"/> 実装。キーの型の既定の <see cref="EqualityComparer{TKey}"/> を使用する場合は null。</param>
        /// <param name="comparerValue">値の比較時に使用する <see cref="IEqualityComparer{TValue}"/> 実装。値の型の既定の <see cref="EqualityComparer{TValue}"/> を使用する場合は null。</param>
        public LinkedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparerKey, IEqualityComparer<TValue> comparerValue) :
      this(collection != null ? collection.Count() : 0, comparerKey, comparerValue)
        {
            if (collection == null) ThrowArgumentNullException(nameof(collection));
            this.AddRange(collection);
        }

        #endregion constructer

        #region property

        /// <summary>
        /// ディクショナリのキーが等しいかどうかを確認するために使用する <see cref="IEqualityComparer{TKey}"/> を取得します。
        /// </summary>
        public IEqualityComparer<TKey> ComparerKey => this._keyToValues.Comparer;

        /// <summary>
        /// ディクショナリの値が等しいかどうかを確認するために使用する <see cref="IEqualityComparer{TValue}"/> を取得します。
        /// </summary>
        public IEqualityComparer<TValue> ComparerValue => this._valueToKeys.Comparer;

        /// <summary>
        /// <see cref="LinkedDictionary{TKey, TValue}"/> に格納されているキー/値ペアの数を取得します。
        /// </summary>
        public int Count => this._keyToValues.Count;

        /// <summary>
        /// 指定されたキーに関連付けられた値を取得または設定します。
        /// </summary>
        /// <param name="key">取得または設定する値のキー。</param>
        public TValue this[TKey key]
        {
            get => this.GetValue(key);
            set => this.SetValue(key, value);
        }

        /// <summary>
        /// 指定された値に関連付けられたキーを取得または設定します。
        /// </summary>
        /// <param name="argValue">取得または設定するキーの値。</param>
        public TKey this[TValue argValue]
        {
            get => this.GetKey(argValue);
            set => this.SetKey(argValue, value);
        }

        /// <summary>
        /// <see cref="LinkedDictionary{TKey, TValue}"/> 内のキーを格納しているコレクションを取得します。
        /// </summary>
        public ICollection<TKey> Keys => this._keyToValues.Keys;

        /// <summary>
        /// <see cref="LinkedDictionary{TKey, TValue}"/> 内の値を格納しているコレクションを取得します。
        /// </summary>
        public ICollection<TValue> Values => this._keyToValues.Values;

        /// <summary>
        /// IDictionary が読み取り専用かどうかを示す値を取得します。
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// <see cref="LinkedDictionary{TKey, TValue}"/> 内のキー/値ペアのコレクションを取得します。
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs
        {
            get
            {
                foreach (var item in this._keyToValues)
                {
                    yield return item;
                }
            }
        }

        #endregion property

        #region method[get]

        /// <summary>
        /// 指定されたキーに関連付けられた値を取得します。
        /// </summary>
        /// <param name="key">取得または設定する値のキー。</param>
        public TValue GetValue(TKey key)
        {
            if (!this.TryGetValue(key, out var value))
            {
                ThrowKeyNotFoundException();
            }
            return value;
        }

        /// <summary>
        /// 指定された値に関連付けられたキーを取得します。
        /// </summary>
        /// <param name="value">取得または設定するキーの値。</param>
        public TKey GetKey(TValue value)
        {
            if (!this.TryGetKey(value, out var key))
            {
                ThrowKeyNotFoundException();
            }
            return key;
        }

        /// <summary>
        /// 指定されたキーに関連付けられた値の取得を試みます。
        /// </summary>
        /// <param name="key">取得する値のキー。</param>
        /// <param name="value">キーが見つかった場合は、指定したキーに関連付けられている値が格納されます。それ以外の場合は value パラメーターの型に対する既定の値。</param>
        /// <returns>指定されたキーを持つ要素が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれている場合は true、含まれていない場合は false。</returns>
        public bool TryGetValue(TKey key, out TValue value) => this._keyToValues.TryGetValue(key, out value);

        /// <summary>
        /// 指定された値に関連付けられたキーの取得をを試みます。
        /// </summary>
        /// <param name="value">取得するキーの値。</param>
        /// <param name="key">値が見つかった場合は、指定した値に関連付けられているキーが格納されます。それ以外の場合は key パラメーターの型に対する既定の値。</param>
        /// <returns>指定された値を持つ要素が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれている場合は true、含まれていない場合は false。</returns>
        public bool TryGetKey(TValue value, out TKey key) => this._valueToKeys.TryGetValue(value, out key);

        /// <summary>
        /// 指定されたキーに関連付けられた値を取得します。
        /// </summary>
        /// <param name="key">取得する値のキー。</param>
        /// <returns>値が見つかった場合は、指定した値に関連付けられているキー。それ以外の場合は value パラメーターの型に対する既定の値。</returns>
        public TValue GetValueOrDefault(TKey key) => this.TryGetValue(key, out var value) ? value : default(TValue);

        /// <summary>
        /// 指定された値に関連付けられたキーを取得します。
        /// </summary>
        /// <param name="value">取得するキーの値。</param>
        /// <returns>キーが見つかった場合は、指定した値に関連付けられている値。それ以外の場合は key パラメーターの型に対する既定の値。</returns>
        public TKey GetKeyOrDefault(TValue value) => this.TryGetKey(value, out var key) ? key : default(TKey);

        /// <summary>
        /// 指定されたキーに関連付けられた値を取得し、値が見つからなかった場合は指定した value を追加します。
        /// </summary>
        /// <param name="key">取得する値のキー。</param>
        /// <param name="value">値が見つからなかった場合に追加する値。</param>
        /// <returns>値が見つかった場合は指定されたキーに関連付けられた値。見つからなかった場合は追加した値。</returns>
        public TValue GetValueOrAdd(TKey key, TValue value)
        {
            this.TryAdd(key, value);
            return this._keyToValues[key];
        }

        /// <summary>
        /// 指定された値に関連付けられたキーを取得し、キーが見つからなかった場合は指定した key を追加します。
        /// </summary>
        /// <param name="value">取得するキーの値。</param>
        /// <param name="key">キーが見つからなかった場合に追加するキー。</param>
        /// <returns>キーが見つかった場合は指定された値に関連付けられたキー。見つからなかった場合は追加したキー。</returns>
        public TKey GetKeyOrAdd(TValue value, TKey key)
        {
            this.TryAdd(key, value);
            return this._valueToKeys[value];
        }

        /// <summary>
        /// 指定されたキーに関連付けられた値を取得し、値が見つからなかった場合は指定した TValue 型に対する既定値を追加します。
        /// </summary>
        /// <param name="key">取得する値のキー。</param>
        /// <returns>値が見つかった場合は指定されたキーに関連付けられた値。見つからなかった場合は TValue 型の既定値。</returns>
        public TValue GetValueOrAddDefault(TKey key) => this.GetValueOrAdd(key, default(TValue));

        /// <summary>
        /// 指定された値に関連付けられたキーを取得し、キーが見つからなかった場合は指定した TKey 型に対する既定値を追加します。
        /// </summary>
        /// <param name="value">取得するキーの値。</param>
        /// <returns>キーが見つかった場合は指定された値に関連付けられたキー。見つからなかった場合は TKey 型の既定値。</returns>
        public TKey GetKeyOrAddDefault(TValue value) => this.GetKeyOrAdd(value, default(TKey));

        #endregion method[get]

        #region method[set]

        /// <summary>
        /// 指定されたキーに関連付けられた値を設定します。
        /// </summary>
        /// <param name="key">設定する値のキー。</param>
        /// <param name="value">設定する値。</param>
        public void SetValue(TKey key, TValue value)
        {
            if (!this.TrySetValue(key, value))
            {
                ThrowKeyNotFoundException();
            }
        }

        /// <summary>
        /// 指定された値に関連付けられたキーを設定します。
        /// </summary>
        /// <param name="value">設定するキーの値。</param>
        /// <param name="key">設定するキー。</param>
        public void SetKey(TValue value, TKey key)
        {
            if (!this.TrySetKey(value, key))
            {
                ThrowKeyNotFoundException();
            }
        }

        /// <summary>
        /// 指定されたキーに関連付けられた値の設定を試みます。
        /// </summary>
        /// <param name="key">設定する値のキー。</param>
        /// <param name="value">設定する値。</param>
        /// <returns>指定されたキーを持つ要素が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれている場合は true、含まれていない場合は false。</returns>
        public bool TrySetValue(TKey key, TValue value)
        {
            if (this._keyToValues.ContainsKey(key))
            {
                var currentValue = this._keyToValues[key];
                this._keyToValues[key] = value;
                this._valueToKeys.Remove(currentValue);
                this._valueToKeys.Add(value, key);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 指定された値に関連付けられたキーの設定を試みます。
        /// </summary>
        /// <param name="value">設定するキーの値。</param>
        /// <param name="key">設定するキー。</param>
        /// <returns>指定された値を持つ要素が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれている場合は true、含まれていない場合は false。</returns>
        public bool TrySetKey(TValue value, TKey key)
        {
            if (this._valueToKeys.ContainsKey(value))
            {
                var currentKey = this._valueToKeys[value];
                this._valueToKeys[value] = key;
                this._keyToValues.Remove(currentKey);
                this._keyToValues.Add(key, value);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 指定されたキーに関連付けられた値を設定または追加します。
        /// </summary>
        /// <param name="key">設定する値のキー。</param>
        /// <param name="value">設定する値。</param>
        public void SetValueOrAdd(TKey key, TValue value)
        {
            if (!this.TrySetValue(key, value)) this.Add(key, value);
        }

        /// <summary>
        /// 指定された値に関連付けられたキーを設定または追加します。
        /// </summary>
        /// <param name="key">設定するキーの値。</param>
        /// <param name="value">設定するキー。</param>
        public void SetKeyOrAdd(TValue value, TKey key)
        {
            if (!this.TrySetKey(value, key)) this.Add(key, value);
        }

        /// <summary>
        /// 指定されたキーに対して、 TValue 型の既定値を設定または追加します。
        /// </summary>
        /// <param name="key">設定する値のキー。</param>
        public void SetValueOrAddDefault(TKey key) => this.SetValueOrAdd(key, default(TValue));

        /// <summary>
        /// 指定された値に対して、 TKey 型の既定値を設定または追加します。
        /// </summary>
        /// <param name="value">設定する値のキー。</param>
        public void SeKeyOrAddDefault(TValue value) => this.SetValueOrAdd(default(TKey), value);

        #endregion method[set]

        #region method[add]

        /// <summary>
        /// 指定されたキーと値を <see cref="LinkedDictionary{TKey, TValue}"/> に追加します。
        /// </summary>
        /// <param name="key">追加する要素のキー。</param>
        /// <param name="value">追加する要素の値。</param>
        public void Add(TKey key, TValue value)
        {
            if (!this.TryAdd(key, value)) ThrowArgumentException();
        }

        /// <summary>
        /// 指定された <see cref="KeyValuePair{TKey, TValue}"/> を <see cref="LinkedDictionary{TKey, TValue}"/> に追加します。
        /// </summary>
        /// <param name="item">追加する <see cref="KeyValuePair{TKey, TValue}"/>。</param>
        public void Add(KeyValuePair<TKey, TValue> item) => this.Add(item.Key, item.Value);

        /// <summary>
        /// 指定された <see cref="ValueTuple{TKey, TValue}"/> を <see cref="LinkedDictionary{TKey, TValue}"/> に追加します。
        /// </summary>
        /// <param name="item">追加する <see cref="ValueTuple{TKey, TValue}"/>。</param>
        public void Add((TKey Key, TValue Value) item) => this.Add(item.Key, item.Value);

        /// <summary>
        /// <see cref="LinkedDictionary{TKey, TValue}"/> に対して、指定されたキーと値の追加を試みます。
        /// </summary>
        /// <param name="key">追加する要素のキー。</param>
        /// <param name="value">追加する要素の値。</param>
        /// <returns>キー/値ペアが <see cref="LinkedDictionary{TKey, TValue}"/> に追加された場合は true、それ以外の場合は false。</returns>
        public bool TryAdd(TKey key, TValue value)
        {
            if (key == null) ThrowArgumentNullException(nameof(key));
            if (value == null) ThrowArgumentNullException(nameof(value));
            if (this._keyToValues.ContainsKey(key) || this._valueToKeys.ContainsKey(value)) return false;
            this._keyToValues.Add(key, value);
            this._valueToKeys.Add(value, key);
            return true;
        }

        /// <summary>
        /// <see cref="LinkedDictionary{TKey, TValue}"/> に対して、指定された <see cref="KeyValuePair{TKey, TValue}"/> の追加を試みます。
        /// </summary>
        /// <param name="item">追加する <see cref="KeyValuePair{TKey, TValue}"/>。</param>
        /// <returns></returns>
        public bool TryAdd(KeyValuePair<TKey, TValue> item) => this.TryAdd(item.Key, item.Value);

        /// <summary>
        /// <see cref="LinkedDictionary{TKey, TValue}"/> に対して、指定された <see cref="ValueTuple{TKey, TValue}"/> の追加を試みます。
        /// </summary>
        /// <param name="item">追加する <see cref="ValueTuple{TKey, TValue}"/>。</param>
        /// <returns></returns>
        public bool TryAdd((TKey Key, TValue Value) item) => this.TryAdd(item.Key, item.Value);

        /// <summary>
        /// 指定した <see cref="KeyValuePair{TKey, TValue}"/> のコレクションを追加します。
        /// </summary>
        /// <param name="collection">追加する  <see cref="KeyValuePair{TKey, TValue}"/> のコレクション。</param>
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (var item in collection) this.Add(item);
        }

        /// <summary>
        /// 指定した <see cref="KeyValuePair{TKey, TValue}"/> のコレクションのうち、キーと値が重複しないもののみを追加します。
        /// </summary>
        /// <param name="collection">追加する  <see cref="KeyValuePair{TKey, TValue}"/> のコレクション。</param>
        public void TryAddRange(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            foreach (var item in collection) this.TryAdd(item);
        }

        #endregion method[add]

        #region method[remove]

        /// <summary>
        /// <see cref="LinkedDictionary{TKey, TValue}"/> からすべてのキーと値を削除します。
        /// </summary>
        public void Clear()
        {
            this._keyToValues.Clear();
            this._valueToKeys.Clear();
        }

        /// <summary>
        /// 指定したキーを持つ値を <see cref="LinkedDictionary{TKey, TValue}"/> から削除します。
        /// </summary>
        /// <param name="key">削除する要素のキー。</param>
        /// <returns>要素が見つかり削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(TKey key) => this.RemoveByKey(key);

        /// <summary>
        /// 指定したキーを持つ値を <see cref="LinkedDictionary{TKey, TValue}"/> から削除します。
        /// </summary>
        /// <param name="key">削除する要素のキー。</param>
        /// <returns>要素が見つかり削除された場合は true。それ以外の場合は false。</returns>
        public bool RemoveByKey(TKey key) => this.RemoveByKey(key, out _);

        /// <summary>
        /// 指定した値を持つキーを <see cref="LinkedDictionary{TKey, TValue}"/> から削除します。
        /// </summary>
        /// <param name="value">削除する要素の値。</param>
        /// <returns>要素が見つかり削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(TValue value) => this.RemoveByValue(value);

        /// <summary>
        /// 指定した値を持つキーを <see cref="LinkedDictionary{TKey, TValue}"/> から削除します。
        /// </summary>
        /// <param name="value">削除する要素の値。</param>
        /// <returns>要素が見つかり削除された場合は true。それ以外の場合は false。</returns>
        public bool RemoveByValue(TValue value) => this.RemoveByValue(value, out _);

        /// <summary>
        /// 指定されたキーを持つ値を <see cref="LinkedDictionary{TKey, TValue}"/> から削除し、その要素の値を value パラメーターにコピーします。
        /// </summary>
        /// <param name="key">削除する要素のキー。</param>
        /// <param name="value">削除された要素の値。</param>
        /// <returns>要素が見つかり削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(TKey key, out TValue value) => this.RemoveByKey(key, out value);

        /// <summary>
        /// 指定されたキーを持つ値を <see cref="LinkedDictionary{TKey, TValue}"/> から削除し、その要素の値を value パラメーターにコピーします。
        /// </summary>
        /// <param name="key">削除する要素のキー。</param>
        /// <param name="value">削除された要素の値。</param>
        public bool RemoveByKey(TKey key, out TValue value)
        {
            if (this._keyToValues.ContainsKey(key) && this._valueToKeys.ContainsKey(this._keyToValues[key]))
            {
                value = this._keyToValues[key];
                this._keyToValues.Remove(key);
                this._valueToKeys.Remove(value);
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// 指定された値を持つキーを <see cref="LinkedDictionary{TKey, TValue}"/> から削除し、その要素のキーを key パラメーターにコピーします。
        /// </summary>
        /// <param name="value">削除する要素の値。</param>
        /// <param name="key">削除する要素のキー。</param>
        /// <returns>要素が見つかり削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(TValue value, out TKey key) => this.RemoveByValue(value, out key);

        /// <summary>
        /// 指定された値を持つキーを <see cref="LinkedDictionary{TKey, TValue}"/> から削除し、その要素のキーを key パラメーターにコピーします。
        /// </summary>
        /// <param name="value">削除する要素の値。</param>
        /// <param name="key">削除する要素のキー。</param>
        /// <returns>要素が見つかり削除された場合は true。それ以外の場合は false。</returns>
        public bool RemoveByValue(TValue value, out TKey key)
        {
            if (this._valueToKeys.ContainsKey(value) && this._keyToValues.ContainsKey(this._valueToKeys[value]))
            {
                key = this._valueToKeys[value];
                this._valueToKeys.Remove(value);
                this._keyToValues.Remove(key);
                return true;
            }
            else
            {
                key = default(TKey);
                return false;
            }
        }

        /// <summary>
        /// 指定された <see cref="KeyValuePair{TKey, TValue}"/> を <see cref="LinkedDictionary{TKey, TValue}"/> から削除します。
        /// </summary>
        /// <param name="item">削除する <see cref="KeyValuePair{TKey, TValue}"/>。</param>
        /// <returns>要素が見つかり削除された場合は true。それ以外の場合は false。</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item) => this._keyToValues.ContainsKey(item.Key) && this._keyToValues[item.Key].Equals(item.Value) && this.Remove(item.Key);

        #endregion method[remove]

        #region method[determinate]

        /// <summary>
        /// 指定したキーを持つ要素が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれるかどうかを判断します。
        /// </summary>
        /// <param name="key">検索するキー。</param>
        /// <returns>指定されたキーを持つ要素が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれている場合は true、含まれていない場合は false。</returns>
        public bool ContainsKey(TKey key) => this._keyToValues.ContainsKey(key);

        /// <summary>
        /// 指定した値を持つ要素が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれるかどうかを判断します。
        /// </summary>
        /// <param name="value">検索する値。</param>
        /// <returns>指定された値を持つ要素が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれている場合は true、含まれていない場合は false。</returns>
        public bool ContainsValue(TValue value) => this._valueToKeys.ContainsKey(value);

        /// <summary>
        /// 指定した <see cref="KeyValuePair{TKey, TValue}"/> が <see cref="LinkedDictionary{TKey, TValue}"/> に含まれるかどうかを判断します。
        /// </summary>
        /// <param name="item">検索する <see cref="KeyValuePair{TKey, TValue}"/></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item) => this.KeyValuePairs.Contains(item);

        #endregion method[determinate]

        #region method [IEnumerator]

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>コレクションの繰り返し処理に使用できる列挙子。</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this._keyToValues.GetEnumerator();

        /// <summary>
        /// コレクションを反復処理する列挙子を返します。
        /// </summary>
        /// <returns>コレクションの反復処理に使用できる IEnumerator。</returns>
        IEnumerator IEnumerable.GetEnumerator() => this._keyToValues.GetEnumerator();

        #endregion method [IEnumerator]

        #region method[copy/convert]

        /// <summary>
        /// 指定した配列インデックスを開始位置として、配列に <see cref="ICollection{T}"/> の要素をコピーします。
        /// </summary>
        /// <param name="array"><see cref="ICollection{T}"/> から要素がコピーされる 1 次元の配列。 </param>
        /// <param name="index">array 内のコピーの開始位置を示すインデックス。</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if (array == null) ThrowArgumentNullException(nameof(array));
            if (index < 0 || index > array.Length) ThrowArgumentOutOfRangeException(nameof(index));
            if (array.Length - index < this.Count) ThrowArgumentException();
            foreach (var item in this._keyToValues)
            {
                array[index++] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
            }
        }

        /// <summary>
        /// シーケンスの各要素の値を <see cref="Func{TValue, TValueResult}"/>　によって新しいフォームに射影し、既存のキーと新しい値による <see cref="LinkedDictionary{TKey, TValueResult}"/> を生成します。
        /// </summary>
        /// <typeparam name="TValueResult">変換後の要素の値の型。</typeparam>
        /// <param name="valueSelector">要素の値の変換を表すデリゲート。</param>
        /// <returns>生成された <see cref="LinkedDictionary{TKey, TValueResult}"/>。</returns>
        public LinkedDictionary<TKey, TValueResult> SelectValue<TValueResult>(Func<TValue, TValueResult> valueSelector)
            => new LinkedDictionary<TKey, TValueResult>(this.KeyValuePairs.Select(item => new KeyValuePair<TKey, TValueResult>(item.Key, valueSelector(item.Value))));

        /// <summary>
        /// シーケンスの各要素の値を <see cref="Func{TKey, TKeyResult}"/>　によって新しいフォームに射影し、新しいキーと既存の値による <see cref="LinkedDictionary{TKeyResult, TValue}"/> を生成します。
        /// </summary>
        /// <typeparam name="TKeyResult">変換後の要素のキーの型。</typeparam>
        /// <param name="keySelector">要素のキーの変換を表すデリゲート。</param>
        /// <returns>生成された <see cref="LinkedDictionary{TKey, TValueResult}"/>。</returns>
        public LinkedDictionary<TKeyResult, TValue> SelectKey<TKeyResult>(Func<TKey, TKeyResult> keySelector)
            => new LinkedDictionary<TKeyResult, TValue>(this.KeyValuePairs.Select(item => new KeyValuePair<TKeyResult, TValue>(keySelector(item.Key), item.Value)));

        #endregion method[copy/convert]

        #region method[sort]

        /// <summary>
        /// 要素のキーの既定の比較子を使用して、<see cref="LinkedDictionary{TKey, TValue}"/> 全体内の要素をそのキーによって並べ替えます。
        /// </summary>
        public void SortByKey() => this.Sort(item => item.Key);

        /// <summary>
        /// 要素の値の既定の比較子を使用して、<see cref="LinkedDictionary{TKey, TValue}"/> 全体内の要素をその値によって並べ替えます。
        /// </summary>
        public void SortByValue() => this.Sort(item => item.Value);

        /// <summary>
        /// 指定した変換デリゲートを使用して、<see cref="LinkedDictionary{TKey, TValue}"/> 全体内の要素をその値によって並べ替えます。
        /// </summary>
        public void Sort<T>(Func<KeyValuePair<TKey, TValue>, T> Selecter)
        {
            var collection = this.KeyValuePairs.OrderBy(Selecter).ToArray();
            this.Clear();
            this.AddRange(collection);
        }

        #endregion method[sort]

        #region method[throw]

        /// <summary>
        /// <para><see cref="ArgumentOutOfRangeException"/>を投げる</para>
        /// <para>インライン化補助のためのメソッド</para>
        /// </summary>
        /// <param name="paramName"><see cref="ArgumentOutOfRangeException"/>の原因となった引数の名前。</param>
        private static void ThrowArgumentOutOfRangeException(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }

        /// <summary>
        /// <para><see cref="ArgumentException"/>を投げる</para>
        /// <para>インライン化補助のためのメソッド</para>
        /// </summary>
        private static void ThrowArgumentException()
        {
            throw new ArgumentException();
        }

        /// <summary>
        /// <para><see cref="ArgumentNullException"/>を投げる</para>
        /// <para>インライン化補助のためのメソッド</para>
        /// </summary>
        private static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// <para><see cref="KeyNotFoundException"/>を投げる</para>
        /// <para>インライン化補助のためのメソッド</para>
        /// </summary>
        private static void ThrowKeyNotFoundException()
        {
            throw new KeyNotFoundException();
        }

        #endregion
    }
}
