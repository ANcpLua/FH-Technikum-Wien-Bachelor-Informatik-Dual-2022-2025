using Blazored.Toast.Services;
using UI.Service.Interface;

namespace UI.Service;

public class ToastService : IToastServiceWrapper
{
    private readonly IToastService _toastService;

    public ToastService(IToastService toastService)
    {
        _toastService = toastService;
    }

    public void ShowSuccess(string message) => _toastService.ShowSuccess(message);

    public void ShowError(string message) => _toastService.ShowError(message);
}
