using System.Threading.Tasks;

namespace Isozay.Hukuk.Data
{

    public interface IHukukDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}