// java version : "13-ea"

import java.util.*;
import java.util.Scanner;

public class Main {
      
    /**
     * N人のキリバンゲッターが次に狙うアクセスカウンタの値を出力します。
     */
    public static void main(String[] args) {
        
        var nums = new ArrayList<Integer>();
        
        // 入力数値を取得
        var scan = new Scanner(System.in);
        while (scan.hasNextLine()) {
            nums.add(scan.nextInt());
        }
        scan.close();
        
        // 各キリバンゲッターが次に狙うアクセスカウンタの値を出力
        for (var i = 1; i <= nums.get(0); i++){
            System.out.println(GetNextKiriban(nums.get(i)));
        }
        
    }
    
    /**
     * キリバンゲッターが次に狙うアクセスカウンターの値を取得します。
     * @param cnt 現在のアクセスカウンタの値
     * @return 次のキリバン
     */
    private static int GetNextKiriban(int cnt) {
        
        // 次のキリバンが見つかるまでインクリメント
        do {
            cnt++;
        } while (!(IsKiriban(cnt)));
        
        return cnt;
        
    }
    
    /**
     * アクセスカウンターの値がキリバンかどうか判定します。
     * @param cnt アクセスカウンタの値
     * @return キリバンならtrue, そうでなければfalse
     */
    private static boolean IsKiriban(int cnt) {
        
        // 文字列型に変換し、各文字が先頭の文字と全て一致するか判定
        String s = String.valueOf(cnt);
        for (var i = 1; i < s.length(); i++){
            if (s.charAt(0) != s.charAt(i)) {
                return false;
            }
        }
        
        return true;
        
    }
    
}
