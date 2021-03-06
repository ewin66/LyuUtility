﻿

using System.Collections.Generic;

namespace Lyu.Core.Application.Services.Dto
{
    public class QueryResultOutput<T>
    {
        /// <summary>
        /// 上面提到了，Datatables发送的draw是多少那么服务器就返回多少。
        /// 这里注意，作者出于安全的考虑，强烈要求把这个转换为整数后再返回，而不是纯粹的接受然后返回，这是 为了防止跨站脚本（XSS）攻击。
        /// </summary>
        public int Draw { get; set; }
        /// <summary>
        /// 即没有过滤的记录数（数据库里总共记录数）
        /// </summary>
        public int RecordsTotal { get; set; }
        /// <summary>
        /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
        /// </summary>
        public int RecordsFiltered { get; set; }
        /// <summary>
        /// 表中中需要显示的数据。这是一个对象数组，也可以只是数组，区别在于 纯数组前台就不需要用 columns绑定数据，会自动按照顺序去显示 ，
        /// 而对象数组则需要使用 columns绑定数据才能正常显示。 
        /// 注意这个 data的名称可以由 ajaxDT 的 ajax.dataSrcDT 控制
        /// </summary>
        public IEnumerable<T> Data { get; set; }
        /// <summary>
        /// 可选。你可以定义一个错误来描述服务器出了问题后的友好提示
        /// </summary>
        public string Error { get; set; }
    }

}
