namespace ConfigTool.Model
{
    /// <summary>
    /// 建筑功能
    /// </summary>
    public class BuildFeture
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int BuildID { get; set; }

        public int Workers { get; set; }

        public string Description { get; set; }

        public string ResourceNeed { get; set; }

        public long RequiredTime { get; set; }

        /// <summary>
        /// 这里表示结果
        /// </summary>
        public string Result { get; set; }
    }
}