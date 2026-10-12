namespace ProjectEnc.Helper
{
    /// <summary>
    /// 全局常量：集中管理目录名、文件名、工具路径与扩展名，避免魔法字符串散落各处。
    /// </summary>
    public static class AppConstants
    {
        public const string ProjectsFolder = "Projects";

        public const string ToolsFolder = "Tools";

        public const string TemplateFolder = "Template";

        /// <summary>工程数据文件名。</summary>
        public const string ProjectFileName = "工程.project";

        /// <summary>ConfuserEx 加密模板文件名。</summary>
        public const string ConfuserTemplateFile = "工程加密.crproj";

        /// <summary>Inno Setup 打包模板文件名。</summary>
        public const string InnoTemplateFile = "工程打包.iss";

        /// <summary>主程序备份扩展名。</summary>
        public const string BackupExtension = ".backup";

        /// <summary>调试符号文件扩展名。</summary>
        public const string PdbExtension = ".pdb";

        /// <summary>ILMerge 可执行文件相对 Tools 的路径。</summary>
        public const string ILMergeRelativePath = @"ILMerge\ILMerge.exe";

        /// <summary>ConfuserEx 可执行文件相对 Tools 的路径。</summary>
        public const string ConfuserExeRelativePath = @"ConfuserEx\Confuser.CLI.exe";

        /// <summary>Inno Setup 编译器相对 Tools 的路径。</summary>
        public const string InnoRelativePath = @"Inno\iscc.exe";
    }
}
