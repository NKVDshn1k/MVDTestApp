using Microsoft.Extensions.DependencyInjection;
using MVDTestApp.ViewModel;

namespace MVDTestApp.Locator;

class ViewModelLocator
{
    public MainWindowViewModel MainWindowModel => 
        App.Services.GetRequiredService<MainWindowViewModel>();
    public WorkTaskCreateOrEditeViewModel WorkTaskCreateOrEditeModel => 
        App.Services.GetRequiredService<WorkTaskCreateOrEditeViewModel>();
    public WorkTaskDetailsViewModel WorkTaskDetailsModel => 
        App.Services.GetRequiredService<WorkTaskDetailsViewModel>();
}
