using System.Threading.Tasks;

namespace WpfApp1.Infrastructure
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
}
