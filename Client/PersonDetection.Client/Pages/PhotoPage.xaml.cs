using PersonDetection.Client.ViewModels;

namespace PersonDetection.Client.Pages;

public partial class PhotoPage
{
    public PhotoPage(PhotoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}