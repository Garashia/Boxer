using CommandList = MessageMicroCommand.MessageCommand;

public struct TextParameter
{
    /*
     �R�}���_�[�Ɏ������������������ɒǉ�����
    �Q�[���I�u�W�F�N�g�����������ꍇ�A������폜���\
    �l�͈ړ���҂����ԂȂǂɗp����B
     */
}

public class MessageMicroCommand :
    MicroCommander<TextParameter, CommandList>
{


    public class MessageCommand : Command
    {
        /*
         * ���ʂŎg�������֐��������ɐ錾
         */
    }

}

// �ȉ��ACommandList���p�������R�}���h�N���X��ǉ�