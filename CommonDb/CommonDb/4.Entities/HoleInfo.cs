using SqlSugar;

namespace CommonDb;

///<summary>
///被测孔信息
///</summary>
[SugarTable("HoleInfo_{year}{month}{day}")]
[SplitTable(SplitType.Month)]
public partial class HoleInfo
{
    /// <summary>
    /// 主键，自动写入雪花ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    public long Id { get; set; }

    /// <summary>
    /// 分表字段
    /// </summary>
    [SplitField]
    public DateTime CreateTime { get; set; }


    [SugarColumn(IsNullable = true)]
    public int pcbNo { get; set; }


    [SugarColumn(IsNullable = true)]
    public int Index { get; set; }


    [SugarColumn(IsNullable = true)]
    public int? AreaIndex { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? PointFCenterX { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? PointFCenterY { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? Z { get; set; }


    [SugarColumn(IsNullable = true)]
    public int? ProgramT { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? Diameter { get; set; }


    [SugarColumn(IsNullable = true)]
    public int? RowIndex { get; set; }


    [SugarColumn(IsNullable = true)]
    public int? ColIndex { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? MeasAreaCenterX { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? MeasAreaCenterY { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? MeasAreaHoleOffsetX { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? MeasAreaHoleOffsetY { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? MeasAreaHoleCenterX { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? MeasAreaHoleCenterY { get; set; }


    [SugarColumn(IsNullable = true)]
    public float? MeasAreaHoleRadius { get; set; }

}