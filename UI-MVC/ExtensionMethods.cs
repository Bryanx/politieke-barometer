using System.Linq;
using System.Reflection;

namespace BAR.UI.MVC {
    public static class ExtensionMethods {
        public static TEntity CopyTo<TEntity>(this TEntity originalEntity, TEntity newEntity) {
            PropertyInfo[] oProperties = originalEntity.GetType().GetProperties();

            foreach (PropertyInfo currentProperty in oProperties.Where(p => p.CanWrite)) {
                if (currentProperty.GetValue(newEntity, null) != null) {
                    currentProperty.SetValue(originalEntity, currentProperty.GetValue(newEntity, null), null);
                }
            }

            return originalEntity;
        }
    }
}