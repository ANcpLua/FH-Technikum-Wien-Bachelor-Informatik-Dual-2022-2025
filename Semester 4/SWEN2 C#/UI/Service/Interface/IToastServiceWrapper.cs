namespace UI.Service.Interface;

public interface IToastServiceWrapper
{
    void ShowSuccess(string message);
    void ShowError(string message);
}
