namespace ConfigTool.Model
{
    /// <summary>
    /// 建筑
    /// </summary>
    public class Build
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long RequiredTime { get; set; }

        public int Workers { get; set; }

        public int DependentBuild { get; set; }

        public string ResourceNeed { get; set; }
    }
}