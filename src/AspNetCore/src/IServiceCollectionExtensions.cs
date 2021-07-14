using System;
using Microsoft.Extensions.DependencyInjection;

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage
{

    /// <summary> Extensions to <see cref="IServiceCollection"/>. </summary>
    public static class IServiceCollectionExtensions
    {

        /// <summary> Adds services required for Static Web Assets Storage. </summary>
        /// <param name="configure"> An (optional) delegate method that configures the default <see cref="StaticWebAssetsStorageOptions"/>. </param>
        public static IServiceCollection AddStaticWebAssetsStorage( this IServiceCollection services, Action<StaticWebAssetsStorageOptions> configure = null )
        {
            if( services == null )
            {
                throw new ArgumentNullException( nameof( services ) );
            }

            services.AddOptions<StaticWebAssetsStorageOptions>()
                .Configure( options => configure?.Invoke( options ) );

            return services;
        }

    }

}
