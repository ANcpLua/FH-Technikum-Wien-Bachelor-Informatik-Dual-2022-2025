namespace UI.Service.Interface;

public interface IViewModelHelperService
{
    bool ToggleVisibility(ref bool visibilityFlag);
    void ShowForm(ref bool formVisibilityFlag);
    void ResetForm<T>(ref T formModel, Func<T> defaultValueFactory)
        where T : class;
}
