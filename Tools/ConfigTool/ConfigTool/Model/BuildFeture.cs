namespace ConfigTool.Model
{
    /// <summary>
    /// 建筑功能
    /// </summary>
    public class BuildFeture
    {
        public string ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 建筑ID
        /// </summary>
        public int BuildID { get; set; }

        /// <summary>
        /// 需要劳动力
        /// </summary>
        public int Workers { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 资源所需
        /// </summary>
        public string ResourceNeed { get; set; }

        /// <summary>
        /// 需求时间
        /// </summary>
        public long RequiredTime { get; set; }

        /// <summary>
        /// 这里表示结果
        /// </summary>
        public string Result { get; set; }
    }
}