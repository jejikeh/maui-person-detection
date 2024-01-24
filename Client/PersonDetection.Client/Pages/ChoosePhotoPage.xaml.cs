using PersonDetection.Client.Application.ViewModels;

namespace PersonDetection.Client.Pages;

public partial class ChoosePhotoPage
{
    public ChoosePhotoPage(ChoosePhotoViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}