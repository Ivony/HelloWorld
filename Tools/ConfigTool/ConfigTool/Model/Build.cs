namespace ConfigTool.Model
{
    /// <summary>
    /// 建筑
    /// </summary>
    public class Build
    {
        public string ID { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 建造时间
        /// </summary>
        public long RequiredTime { get; set; }

        /// <summary>
        /// 需要劳动力
        /// </summary>
        public int Workers { get; set; }

        /// <summary>
        /// 建造依赖（为0无依赖，否则是升级）
        /// </summary>
        public string DependentBuild { get; set; }

        /// <summary>
        /// 资源需求
        /// 采用 {"木头":100,"石头":20} json方式编码
        /// </summary>
        public string ResourceNeed { get; set; }

        /// <summary>
        /// 血
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// 攻击力
        /// </summary>
        public int AP { get; set; }
    }
}