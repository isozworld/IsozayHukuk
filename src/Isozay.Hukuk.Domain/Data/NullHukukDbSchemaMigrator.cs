using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Isozay.Hukuk.Data
{

    /* This is used if database provider does't define
     * IHukukDbSchemaMigrator implementation.
     */
    public class NullHukukDbSchemaMigrator : IHukukDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}