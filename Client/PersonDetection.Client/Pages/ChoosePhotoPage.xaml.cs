using MauiIcons.Core;
using PersonDetection.Client.ViewModels;

namespace PersonDetection.Client.Pages;

public partial class ChoosePhotoPage
{
    private readonly ChoosePhotoViewModel _viewModel;
    
    public ChoosePhotoPage(ChoosePhotoViewModel viewModel)
    {
        InitializeComponent();
        
        _viewModel = viewModel;
        BindingContext = _viewModel;
        // Temporary Workaround for url styled namespace in xaml
        _ = new MauiIcon();
    }
    
    protected override async void OnAppearing()
    {
        await _viewModel.LoadPhotosAsync();
    }
}