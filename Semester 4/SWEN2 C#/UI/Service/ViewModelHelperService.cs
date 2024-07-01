using UI.Service.Interface;

namespace UI.Service;

public class ViewModelHelperService : IViewModelHelperService
{
    public bool ToggleVisibility(ref bool visibilityFlag)
    {
        visibilityFlag = !visibilityFlag;
        return visibilityFlag;
    }

    public void ShowForm(ref bool formVisibilityFlag) => formVisibilityFlag = true;

    public void ResetForm<T>(ref T formModel, Func<T> defaultValueFactory)
        where T : class => formModel = defaultValueFactory();
}
