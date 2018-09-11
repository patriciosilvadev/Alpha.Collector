﻿using Alpha.Collector.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Collector.Core
{
    /// <summary>
    /// 168开奖网抓取北京赛车
    /// </summary>
    internal class _168BJKCPicker : IPicker
    {
        /// <summary>
        /// 采集地址
        /// </summary>
        private const string URL = "https://api.api68.com/CQShiCai/getBaseCQShiCaiList.do?date=&lotCode=10002";

        /// <summary>
        /// 执行抓取
        /// </summary>
        List<OpenResult> IPicker.Run()
        {
            try
            {
                _168Picker<_168SSCData> picker = new _168Picker<_168SSCData>(URL, LotteryType.BJKC);
                List<_168SSCData> dataList = picker.Pick();

                return (from o in dataList
                        select new OpenResult
                        {
                            create_time = DateTime.Now,
                            open_time = DateTime.Parse(o.preDrawTime),
                            lottery_code = LotteryType.BJKC,
                            issue_number = Convert.ToInt64(o.preDrawIssue),
                            open_data = o.preDrawCode,
                            data_source = DataSource._168
                        }).OrderBy(o => o.issue_number).ToList();
            }
            catch (Exception ex)
            {
                AppLog appLog = new AppLog
                {
                    create_time = DateTime.Now,
                    log_type = LogType.ERROR,
                    lottery_code = LotteryType.BJKC,
                    data_source = DataSource._168,
                    log_message = ex.ToString()
                };
                AlphaLogManager.Error(appLog);

                return new List<OpenResult>();
            }
        }
    }
}
