using CommandList = MessageMicroCommand.MessageCommand;

public struct TextParameter
{
    /*
     コマンダーに持たせたい情報をここに追加せよ
    ゲームオブジェクトを持たせた場合、生成や削除も可能
    値は移動や待ち時間などに用いる。
     */
}

public class MessageMicroCommand :
    MicroCommander<TextParameter, CommandList>
{


    public class MessageCommand : Command
    {
        /*
         * 共通で使いたい関数をここに宣言
         */
    }

}

// 以下、CommandListを継承したコマンドクラスを追加