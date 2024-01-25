using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using PersonDetection.Client.ViewModels;

namespace PersonDetection.Client.Pages;

public partial class PhotoPage : ContentPage
{
    public PhotoPage(PhotoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}