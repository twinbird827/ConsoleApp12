using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp12
{
    /*
     *  野球のボールカウント・アウトカウントの遷移を計算する。（得点・ランナー・イニング の計算は不要）
     *  ただし、ストライク・ボール・ファウル・ヒット・ピッチャーフライしかない。
     *  細かいルールは下記の通り：
     *
     *  ストライクが３つになったらアウトが増え、ストライクとボールがゼロになる。
     *  ボールが4つになったらフォアボールになり、ストライクとボールがゼロになる。アウトは増えない。
     *  ヒットを打ったらストライクとボールがゼロになる。アウトは増えない。
     *  ピッチャーフライを打ったらストライクとボールがゼロになり、アウトが増える。
     *  アウトが3つになったら、アウト・ストライク・ボール全てゼロになる。
     *  ファウルの場合、もともとストライクが1以下の場合はストライクが増え、ストライクが2の場合には変化なし。
     *  入力は "sbsfbhsshssbbffbbssbs" のように、ひとつながりの文字列として与えられる。
     *  s, b, f, h, p がそれぞれ ストライク、ボール、ファウル、ヒット、ピッチャーフライ を意味する。
     *  出力は、アウト・ストライク・ボールの順にカウントをつなげたものをコンマで区切る。例を参照。
     *  不正入力には対処しなくてよい。
     *  最終回を超えることも考慮しなくてよい。
     *
     *  以下、入力 -> 出力 の形式で例を示す。
     *
     *  s -> 010
     *  sss -> 010,020,100
     *  bbbb -> 001,002,003,000
     *  ssbbbb -> 010,020,021,022,023,000
     *  hsbhfhbh -> 000,010,011,000,010,000,001,000
     *  psbpfpbp -> 100,110,111,200,210,000,001,100
     *  ppp -> 100,200,000
     *  ffffs -> 010,020,020,020,100
     *  ssspfffs -> 010,020,100,200,210,220,220,000
     *  bbbsfbppp -> 001,002,003,013,023,000,100,200,000
     *  sssbbbbsbhsbppp -> 010,020,100,101,102,103,100,110,111,100,110,111,200,000,100
     *  ssffpffssp -> 010,020,020,020,100,110,120,200,210,000
     *
     *  ※ 解答例をコメント欄に（ソースではなく）リンクの形で書いてくださるとありがたいです。
     *  ※ そのうち ruby か groovy で解答例を投稿するつもり。
     */
    class Program
    {
        static void Main(string[] args)
        {
            Test("s");
            Test("sss");
            Test("bbbb");
            Test("ssbbbb");
            Test("hsbhfhbh");
            Test("psbpfpbp");
            Test("ppp");
            Test("ffffs");
            Test("ssspfffs");
            Test("bbbsfbppp");
            Test("sssbbbbsbhsbppp");
            Test("ssffpffssp");
            Console.ReadLine();
        }

        static void Test(string target)
        {
            Console.WriteLine($"{target.PadRight(15)}:{Proc(target)}");
        }

        static string Proc(string target)
        {
            // 開始時の o, s, b
            var src = new[] { 0, 0, 0 };

            // s, b, f, h, p の処理定義
            var dic = new Dictionary<char, Func<int[]>>();
            // 3<=++sなら'p'の処理
            dic['s'] = () => ++src[1] < 3 ? src : dic['p']();
            // 4<=++bなら'h'の処理
            dic['b'] = () => ++src[2] < 4 ? src : dic['h']();
            // s<2なら's'の処理
            dic['f'] = () => src[1] < 2 ? dic['s']() : src;
            // sとbをﾘｾｯﾄ
            dic['h'] = () => src = new[] { src[0], 0, 0 };
            // ++o<=3ならo=0, それ以外ならo=oのまま、s=0, b=0する
            dic['p'] = () => src = new[] { 3 <= ++src[0] ? 0 : src[0], 0, 0 };

            return string.Join(",", target
                .Select(chr => dic[chr]())
                .Select(arr => string.Concat(arr))
            );
        }
    }
}
