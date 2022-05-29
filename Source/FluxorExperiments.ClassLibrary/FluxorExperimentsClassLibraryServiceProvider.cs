using Fluxor;
using Microsoft.Extensions.DependencyInjection;

namespace FluxorExperiments.ClassLibrary;

public static class FluxorExperimentsClassLibraryServiceProvider
{
	public static IServiceCollection AddFluxorExperimentsClassLibraryServices(this IServiceCollection services)
	{
		return services.AddFluxor(options => options
			.ScanAssemblies(typeof(FluxorExperimentsClassLibraryServiceProvider).Assembly));
	}
}