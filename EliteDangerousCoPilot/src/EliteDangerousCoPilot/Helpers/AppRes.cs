using System.Windows;

namespace NSW.EliteDangerous.Copilot.Helpers
{
    public static class AppRes
    {
        public static T GetResource<T>(string key)
        {
            var result = Application.Current.TryFindResource(key);
            if (result != null)
                return (T)result;
            return default;
        }
    }
}