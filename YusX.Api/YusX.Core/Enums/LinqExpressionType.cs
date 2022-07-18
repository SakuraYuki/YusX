namespace YusX.Core.Enums
{
    /// <summary>
    /// LINQ 表达式类型
    /// </summary>
    public enum LinqExpressionType
    {
        /// <summary>
        /// =，等于
        /// </summary>
        Equal = 0,
        /// <summary>
        /// !=，不等于
        /// </summary>
        NotEqual = 1,
        /// <summary>
        /// >，大于
        /// </summary>
        GreaterThan = 2,
        /// <summary>
        /// <，小于
        /// </summary>
        LessThan = 3,
        /// <summary>
        /// >=，大于等于
        /// </summary>
        ThanOrEqual = 4,
        /// <summary>
        /// <=，小于等于
        /// </summary>
        LessThanOrEqual = 5,
        /// <summary>
        /// In
        /// </summary>
        In = 6,
        /// <summary>
        /// Contains
        /// </summary>
        Contains = 7,
        /// <summary>
        /// NotContains
        /// </summary>
        NotContains = 8,
    }
}
