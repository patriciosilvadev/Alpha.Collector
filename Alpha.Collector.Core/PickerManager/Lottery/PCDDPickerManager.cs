﻿using Alpha.Collector.Model;
using System;

namespace Alpha.Collector.Core
{
    /// <summary>
    /// PC蛋蛋采集器管理器
    /// </summary>
    public class PCDDPickerManager : PickerManager
    {
        /// <summary>
        /// 获取采集器
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public override IPicker GetPicker(string dataSource)
        {
            switch (dataSource)
            {
                case DataSource._168:
                    return (IPicker)Activator.CreateInstance(typeof(_168PCDDPicker));
                case DataSource.KCZX:
                    return (IPicker)Activator.CreateInstance(typeof(KCPCDDPicker));
                case DataSource.ROBO:
                    return (IPicker)Activator.CreateInstance(typeof(RoboPCDDPicker));
                default:
                    return null;
            }
        }
    }
}