using DevExpress.XtraSplashScreen;
using ProjectEnc.Form;

namespace ProjectEnc.Helper
{
    /// <summary>
    /// 加载遮罩（启动画面）辅助类。
    /// </summary>
    public static class LoadingHelper
    {
        public static void Show()
        {
            SplashScreenManager.ShowForm(typeof(LoadingForm));
        }

        public static void Hide()
        {
            SplashScreenManager.CloseForm();
        }
    }
}
