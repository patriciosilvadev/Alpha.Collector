﻿using Alpha.Collector.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Collector.Core
{
    /// <summary>
    /// 快彩在线采集福彩3D
    /// </summary>
    public class KCFC3DPicker : BasePicker, IPicker, IFC3DPicker
    {
        private KCPicker _kcPicker;

        public KCFC3DPicker()
        {
            this._kcPicker = new KCPicker(LotteryEnum.FC3D);
        }

        /// <summary>
        /// 执行抓取
        /// </summary>
        /// <returns></returns>
        List<OpenResult> IPicker.Run()
        {
            try
            {
                List<OpenResult> dataList = this._kcPicker.Pick();
                return (from o in dataList
                        select new OpenResult
                        {
                            create_time = o.create_time,
                            open_time = o.open_time,
                            lottery_code = LotteryEnum.FC3D,
                            issue_number = Convert.ToInt64(o.open_time.ToString("yyyyMMdd") + o.issue_number.ToString().Replace(o.open_time.ToString("yyyy"), "")),
                            open_data = o.open_data,
                            data_source = o.data_source
                        }).OrderBy(o => o.issue_number).ToList();
            }
            catch (Exception ex)
            {
                AppLog appLog = new AppLog
                {
                    create_time = DateTime.Now,
                    log_type = LogTypeEnum.ERROR,
                    lottery_code = LotteryEnum.FC3D,
                    data_source = DataSourceEnum.KC,
                    log_message = ex.ToString()
                };
                AlphaLogManager.Error(appLog);

                return new List<OpenResult>();
            }
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return base.LotteryList.Contains(LotteryEnum.FC3D) && base.DataSourceList.Contains(DataSourceEnum.KC);
            }
        }
    }
}
