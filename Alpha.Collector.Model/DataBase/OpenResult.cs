﻿using Alpha.Collector.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Collector.Model
{
    /// <summary>
    /// 彩票开奖结果
    /// </summary>
    public class OpenResult
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// 采集时间戳
        /// </summary>
        public long create_timestamp
        {
            get
            {
                return this.create_time.ToTimestamp();
            }
        }

        /// <summary>
        /// 开奖时间
        /// </summary>
        public DateTime open_time { get; set; }

        /// <summary>
        /// 开奖时间戳
        /// </summary>
        public long open_timestamp
        {
            get
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return (long)(this.open_time - startTime).TotalMilliseconds;
            }
        }

        /// <summary>
        /// 彩种代码
        /// </summary>
        public string lottery_code { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public long issue_number { get; set; }

        private string _open_data;
        /// <summary>
        /// 开奖号码
        /// </summary>
        public string open_data
        {
            get
            {
                List<string> numList = this._open_data.Split(',').ToList();
                return numList.Aggregate(string.Empty, (c, r) =>
                {
                    if (r.Trim().TrimStart('0') == "")
                    {
                        return c + r.Trim() + ',';
                    }
                    else
                    {
                        return c + r.Trim().TrimStart('0') + ',';
                    }
                }).TrimEnd(',');
            }
            set
            {
                this._open_data = value;
            }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public string data_source { get; set; }

        /// <summary>
        /// 数据源文字表示
        /// </summary>
        public string DataSourceText
        {
            get
            {
                return ModelFunction.GetDataSourceName(this.data_source);
            }
        }

        /// <summary>
        /// 开奖号码是否合法
        /// </summary>
        public int is_legal
        {
            get
            {
                return this.CheckLegal() ? 1 : 0;
            }
        }

        /// <summary>
        /// 校验当前开奖号码是否合法
        /// </summary>
        /// <returns></returns>
        private bool CheckLegal()
        {
            switch (this.lottery_code)
            {
                case LotteryEnum.BJK3:
                    return this.CheckBJK3Legal();

                case LotteryEnum.AHK3:
                case LotteryEnum.FJK3:
                case LotteryEnum.GSK3:
                case LotteryEnum.GXK3:
                case LotteryEnum.GZK3:
                case LotteryEnum.HeBK3:
                case LotteryEnum.HuBK3:
                case LotteryEnum.JLK3:
                case LotteryEnum.JSK3:
                case LotteryEnum.NMGK3:
                case LotteryEnum.SHK3:
                    return this.CheckK3Legal();

                case LotteryEnum.AH11X5:
                case LotteryEnum.GD11X5:
                case LotteryEnum.GX11X5:
                case LotteryEnum.HB11X5:
                case LotteryEnum.JL11X5:
                case LotteryEnum.JS11X5:
                case LotteryEnum.JX11X5:
                case LotteryEnum.LN11X5:
                case LotteryEnum.NMG11X5:
                case LotteryEnum.SD11X5:
                case LotteryEnum.SH11X5:
                case LotteryEnum.ZJ11X5:
                    return this.Check11X5Legal();

                case LotteryEnum.CQXYNC:
                case LotteryEnum.GDKLSF:
                case LotteryEnum.TJKLSF:
                case LotteryEnum.HNKLSF:
                    return this.CheckKLSFLegal();

                case LotteryEnum.GXKLSF://TODO:广西快乐十分的期号样式待定
                    return true;

                case LotteryEnum.CQSSC:
                case LotteryEnum.TJSSC:
                case LotteryEnum.XJSSC:
                    return this.CheckSSCLegal();

                case LotteryEnum.FC3D:
                case LotteryEnum.TCPL3:
                    return this.CheckFC3DLegal();

                case LotteryEnum.PCDD:
                    return this.CheckPCDDLegal();

                case LotteryEnum.BJKC:
                    return this.CheckKCLegal();

                case LotteryEnum.BJKL8:
                    return this.CheckBJKL8Legal();

                case LotteryEnum.XGLHC:
                    return this.CheckLHCLegal();

                default: return false;
            }
        }

        /// <summary>
        /// 校验快3开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckBJK3Legal()
        {
            return this.Check(false, 3, 1, 6, true);
        }

        /// <summary>
        /// 校验快3开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckK3Legal()
        {
            return this.Check(true, 3, 1, 6, true);
        }


        /// <summary>
        /// 校验11选5开奖号码
        /// </summary>
        /// <returns></returns>
        private bool Check11X5Legal()
        {
            return this.Check(true, 5, 1, 11, true);
        }

        /// <summary>
        /// 校验快乐十分开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckKLSFLegal()
        {
            return this.Check(true, 8, 1, 20, true);
        }

        /// <summary>
        /// 校验时时彩开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckSSCLegal()
        {
            return this.Check(true, 5, 0, 9, true);
        }

        /// <summary>
        /// 校验福彩3D及排列3开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckFC3DLegal()
        {
            return this.Check(true, 3, 0, 9, true);
        }

        /// <summary>
        /// 校验PCDD开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckPCDDLegal()
        {
            List<string> numList = this.open_data.Split(',').ToList();
            if (numList.Sum(o => Convert.ToInt32(o)) < 0 || numList.Sum(o => Convert.ToInt32(o)) > 27)
            {
                return false;
            }

            return this.Check(false, 3, 0, 9, true);
        }

        /// <summary>
        /// 校验赛车开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckKCLegal()
        {
            return this.Check(false, 10, 1, 10, false);
        }

        /// <summary>
        /// 校验北京快乐8开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckBJKL8Legal()
        {
            return this.Check(false, 20, 1, 80, false);
        }

        /// <summary>
        /// 校验六合彩开奖号码
        /// </summary>
        /// <returns></returns>
        private bool CheckLHCLegal()
        {
            return this.Check(true, 7, 1, 49, false);
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="issueNoContainsDate">期号是否包含日期</param>
        /// <param name="numCount">开奖号码位数</param>
        /// <param name="minNum">开奖号码允许的最小值</param>
        /// <param name="maxNum">开奖号码允许的最大值</param>
        /// <param name="canSameNum">开奖号码是否允许重复</param>
        /// <returns></returns>
        private bool Check(bool issueNoContainsDate, int numCount, int minNum, int maxNum, bool canSameNum)
        {
            if (issueNoContainsDate && !this.issue_number.ToString().Contains(this.open_time.ToString("yyyyMMdd")) && !this.issue_number.ToString().Contains(this.open_time.AddDays(-1).ToString("yyyyMMdd")))
            {
                return false;
            }

            List<string> numList = this.open_data.Split(',').ToList();
            if (numList.Count != numCount)
            {
                return false;
            }

            if (!numList.All(n => Convert.ToInt32(n) <= maxNum && Convert.ToInt32(n) >= minNum))
            {
                return false;
            }

            if (!canSameNum && numList.Count != numList.Distinct().ToList().Count)
            {
                return false;
            }

            return true;
        }
    }
}
