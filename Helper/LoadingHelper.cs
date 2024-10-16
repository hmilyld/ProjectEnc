using DevExpress.XtraSplashScreen;
using ProjectEnc.Form;
using System;

namespace ProjectEnc.Helper
{
    public class LoadingHelper
    {
        public static void Show()
        {
            SplashScreenManager.ShowForm(typeof(LoadingForm));
        }

        public static void Show(Type type)
        {
            SplashScreenManager.ShowForm(type);
        }

        public static void Show(System.Windows.Forms.Form parent)
        {
            SplashScreenManager.ShowForm(parent, typeof(LoadingForm));
        }

        public static void Hide()
        {
            SplashScreenManager.CloseForm();
        }

        public static void Set(string str)
        {
            SplashScreenManager.Default.SetWaitFormDescription(str);
        }
    }
}