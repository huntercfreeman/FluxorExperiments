using FluxorExperiments.ClassLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace FluxorExperiments.RazorClassLibrary;

public static class FluxorExperimentsRazorClassLibraryServiceProvider
{
	public static IServiceCollection AddFluxorExperimentsRazorClassLibraryServices(this IServiceCollection services)
	{
		return services.AddFluxorExperimentsClassLibraryServices();
	}
}