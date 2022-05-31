using FluxorExperiments.ClassLibrary;
using FluxorExperiments.ClassLibrary.Clipboard;
using FluxorExperiments.RazorClassLibrary.Clipboard;
using Microsoft.Extensions.DependencyInjection;

namespace FluxorExperiments.RazorClassLibrary;

public static class FluxorExperimentsRazorClassLibraryServiceProvider
{
	public static IServiceCollection AddFluxorExperimentsRazorClassLibraryServices(this IServiceCollection services)
	{
		return services
				.AddFluxorExperimentsClassLibraryServices()
				.AddClipboardProvider();
	}
	
	private static IServiceCollection AddClipboardProvider(this IServiceCollection services)
	{
		return services.AddScoped<IClipboardProvider, ClipboardProvider>();
	}
}