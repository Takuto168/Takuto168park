using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RpnCalculator
{
    /// <summary>
    /// 逆ポーランド記法を計算する機能を提供します。
    /// </summary>
    public static class RpnCalculator<N> where N : struct
    {
        /// <summary>
        /// 算術四則演算と文字列からの変換を行うためのジェネリック数値型クラスです。
        /// </summary>
        private static class NumericalGeneric
        {
            /// <summary>
            /// 算術加算演算を行います。
            /// </summary>
            public static Func<N, N, N> Add { get; }

            /// <summary>
            /// 算術減算演算を行います。
            /// </summary>
            public static Func<N, N, N> Subtract { get; }

            /// <summary>
            /// 算術乗算演算を行います。
            /// </summary>
            public static Func<N, N, N> Multiply { get; }

            /// <summary>
            /// 算術除算演算を行います。
            /// </summary>
            public static Func<N, N, N> Divide { get; }

            /// <summary>
            /// 指定したジェネリック型を取得します。
            /// </summary>
            private static Type _type => typeof(N);

            /// <summary>
            /// 指定した数値型における文字列からの変換メソッドを取得します。
            /// </summary>
            private static MethodInfo _tryParseInvoker => _type.GetMethod("TryParse", new[] { typeof(string), _type.MakeByRefType() });

            /// <summary>
            /// 静的クラスの生成時に、算術四則演算デリゲートを作成します。
            /// </summary>
            static NumericalGeneric()
            {
                var p1 = Expression.Parameter(typeof(N));
                var p2 = Expression.Parameter(typeof(N));
                Add = Expression.Lambda<Func<N, N, N>>(Expression.Add(p1, p2), p1, p2).Compile();
                Subtract = Expression.Lambda<Func<N, N, N>>(Expression.Subtract(p1, p2), p1, p2).Compile();
                Multiply = Expression.Lambda<Func<N, N, N>>(Expression.Multiply(p1, p2), p1, p2).Compile();
                Divide = Expression.Lambda<Func<N, N, N>>(Expression.Divide(p1, p2), p1, p2).Compile();
            }

            /// <summary>
            /// 数値の文字列形式を、それと等価なジェネリック数値型に変換します。 戻り値は、変換が成功したかどうかを示します。
            /// </summary>
            /// <param name="s">変換する数値を格納する文字列。</param>
            /// <param name="result">変換が成功した場合、このメソッドが返されるときに、s に格納された数値と等価のジェネリック数値を格納します。変換に失敗した場合は 0 を格納します。</param>
            /// <returns>s が正常に変換された場合は true。それ以外の場合は false。</returns>
            public static bool TryParse(string s, out N result)
            {
                if (_tryParseInvoker == null)
                {
                    // Reflection で N.TryParse メソッドを取得できなかった場合
                    result = default(N);
                    return false;
                }
                var args = new object[] { s, null };
                if (!(bool)_tryParseInvoker.Invoke(null, args))
                {
                    // 変換失敗
                    result = default(N);
                    return false;
                }
                result = (N)args[1];
                return true;
            }
        }

        /// <summary>
        /// 逆ポーランド記法における1つのトークンを表す構造体です。
        /// </summary>
        private struct Token
        {
            /// <summary>
            /// 演算子を表す文字列とその実行処理のマッピング。
            /// </summary>
            private static readonly Dictionary<string, Func<N, N, N>> _operaters
                = new Dictionary<string, Func<N, N, N>>() {
                                                              { "+", (d1, d2) => NumericalGeneric.Add(d1, d2) },
                                                              { "-", (d1, d2) => NumericalGeneric.Subtract(d1, d2) },
                                                              { "*", (d1, d2) => NumericalGeneric.Multiply(d1, d2) },
                                                              { "/", (d1, d2) => NumericalGeneric.Divide(d1, d2) }
                                                          };

            /// <summary>
            /// トークンが演算子であるかどうかを取得します。
            /// </summary>
            public bool IsOperater => !string.IsNullOrEmpty(this.Operater);

            /// <summary>
            /// トークンが演算子であるとき、その文字列を取得します。
            /// </summary>
            public string Operater { get; }

            /// <summary>
            /// トークンが数値であるとき、その値を取得します。
            /// </summary>
            public N Value { get; }

            /// <summary>
            /// 逆ポーランド記法の文字列からトークンを生成します。
            /// </summary>
            /// <param name="s">逆ポーランド記法のトークンを表す文字列。</param>
            /// <param name="replacePrams">指定したトークン文字列を数値に置き換えるためのマッピング。</param>
            public Token(string s, Dictionary<string, N> replacePrams)
            {
                if (_operaters.ContainsKey(s))
                {
                    // 演算子の場合
                    this.Value = default(N);
                    this.Operater = s;
                }
                else
                {
                    // 数値の場合
                    if (replacePrams?.ContainsKey(s) ?? false)
                        this.Value = replacePrams[s];  // 指定したトークン文字列を数値に置き換え
                    else if (NumericalGeneric.TryParse(s, out var t))
                        this.Value = t;                // N.TryParse によって変換に成功
                    else
                        throw new FormatException();   // 認識できない文字列
                    this.Operater = null;
                }
            }

            /// <summary>
            /// 2つのトークンに対してトークンの示す算術演算を行い、その結果から新たなトークンを作成します。
            /// 引数は、Stack<T>から取り出されることを想定して順序が判定していることに留意してください。
            /// </summary>
            /// <param name="d2">2つ目の数値。</param>
            /// <param name="d1">1つ目の数値。</param>
            /// <returns></returns>
            public Token Operate(N d2, N d1) => new Token(_operaters[this.Operater](d1, d2));

            /// <summary>
            /// 数値型のトークンを生成します。
            /// </summary>
            /// <param name="value">数値。</param>
            private Token(N value) => (this.Value, this.Operater) = (value, null);
        }

        /// <summary>
        /// 逆ポーランド記法の演算に対応し得る型のリスト。
        /// </summary>
        private static Type[] _availableTypes => new[] { typeof(int),
                                                         typeof(uint),
                                                         typeof(short),
                                                         typeof(ushort),
                                                         typeof(long),
                                                         typeof(ulong),
                                                         typeof(decimal),
                                                         typeof(double),
                                                         typeof(float)};

        /// <summary>
        /// 静的クラスの生成時に、指定した数値型が演算に対応しているか判定します。
        /// </summary>
        static RpnCalculator()
        {
            if (!_availableTypes.Contains(typeof(N))) throw new NotSupportedException();
        }

        /// <summary>
        /// 逆ポーランド記法の演算を行います。
        /// </summary>
        /// <param name="exp">式。</param>
        /// <returns>結果値。</returns>
        public static N Calculate(string exp) => CalculateInvoker(exp, null);

        /// <summary>
        /// 逆ポーランド記法の演算を行います。
        /// </summary>
        /// <param name="exp">式。</param>
        /// <param name="replaceParam">指定したトークン文字列を数値に置き換えるためのマッピング。</param>
        /// <param name="replaceParams">指定したトークン文字列を数値に置き換えるためのマッピング。</param>
        /// <returns>結果値。</returns>
        public static N Calculate(string exp, (string Key, N Value) replaceParam, params (string Key, N Value)[] replaceParams)
        {
            var valueList = new Dictionary<string, N>(replaceParams.Length + 1);
            valueList.Add(replaceParam.Key, replaceParam.Value);
            foreach (var item in replaceParams) valueList.Add(item.Key, item.Value);
            return CalculateInvoker(exp, valueList);
        }

        /// <summary>
        /// 逆ポーランド記法の演算を行います。
        /// </summary>
        /// <param name="exp">式。</param>
        /// <param name="replaceParams">指定したトークン文字列を数値に置き換えるためのマッピング。</param>
        /// <returns>結果値。</returns>
        public static N Calculate(string exp, IEnumerable<(string Key, N Value)> replaceParams) => CalculateInvoker(exp, replaceParams.ToDictionary(t => t.Key, t => t.Value));

        /// <summary>
        /// 逆ポーランド記法の演算を行います。
        /// </summary>
        /// <param name="exp">式。</param>
        /// <param name="replaceParam">指定したトークン文字列を数値に置き換えるためのマッピング。</param>
        /// <param name="replaceParams">指定したトークン文字列を数値に置き換えるためのマッピング。</param>
        /// <returns>結果値。</returns>
        public static N Calculate(string exp, N replaceParam, params N[] replaceParams)
        {
            var valueList = new List<N>(replaceParams.Length + 1);
            valueList.Add(replaceParam);
            valueList.AddRange(replaceParams);
            return Calculate(exp, valueList);
        }

        /// <summary>
        /// 逆ポーランド記法の演算を行います。
        /// </summary>
        /// <param name="exp">式。</param>
        /// <param name="replaceParams">指定したトークン文字列を数値に置き換えるためのマッピング。</param>
        /// <returns>結果値。</returns>
        public static N Calculate(string exp, IEnumerable<N> replaceParams) => CalculateInvoker(exp, replaceParams.Select((Item, Index) => new { Item, Index }).ToDictionary(v => v.Index.ToString("{0}"), v => v.Item));

        /// <summary>
        /// 逆ポーランド記法の演算を行います。
        /// </summary>
        /// <param name="exp">式。</param>
        /// <param name="replaceParams">指定したトークン文字列を数値に置き換えるためのマッピング。</param>
        /// <returns>結果値。</returns>
        private static N CalculateInvoker(string exp, Dictionary<string, N> replaceParams)
        {
            var stack = new Stack<Token>();
            foreach (var s in exp.Split(' ').Where(s => !string.IsNullOrEmpty(s)))
            {
                var token = new Token(s, replaceParams);
                stack.Push(token.IsOperater ? token.Operate(stack.Pop().Value, stack.Pop().Value) : token);
            }

            return stack.Pop().Value;
        }
    }
}