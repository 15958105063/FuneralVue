﻿using Funeral.Core.Common;
using Funeral.Core.Common.NpoiHelper;
using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Microsoft.AspNetCore.Hosting;
using NPOI.XWPF.UserModel;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Funeral.Core.Services
{
    public class NpoiWordExportService: INpoiWordExportServices
    {
        private readonly IAchFgpRepository _achFgpRepository;
        private readonly IAchFupRepository _achFupRepository;
        private readonly IAchFunRepository _achFunRepository;
        private readonly IAchBtnRepository _achBtnRepository;

        private readonly IAchOrgRepository _achOrgRepository;
        private readonly IRoleRepository _roleRepository;

        private static IHostingEnvironment _environment;


        public NpoiWordExportService(IAchFgpRepository achFgpRepository, IAchFupRepository achFupRepository, IAchFunRepository achFunRepository, IAchBtnRepository achBtnRepository, IAchOrgRepository achOrgRepository,IRoleRepository roleRepository,IHostingEnvironment iEnvironment)
        {
            this._achFgpRepository = achFgpRepository;
            this._achFupRepository = achFupRepository;
            this._achFunRepository = achFunRepository;
            this._achBtnRepository = achBtnRepository;

            this._achOrgRepository = achOrgRepository;
            this._roleRepository = roleRepository;
            _environment = iEnvironment;
        }

        #region 生成word模板

        /// <summary>
        ///  生成word文档,并保存静态资源文件夹（wwwroot)下的SaveWordFile文件夹中
        /// </summary>
        /// <param name="savePath">保存路径</param>
        public bool SaveWordFileDefault(string savePath)
        {
            savePath = "";
            try
            {
                string currentDate = DateTime.Now.ToString("yyyyMMdd");
                string checkTime = DateTime.Now.ToString("yyyy年MM月dd日");//检查时间
                //保存文件到静态资源wwwroot,使用绝对路径路径
                var uploadPath = _environment.WebRootPath + "/SaveWordFile/" + currentDate + "/";//>>>相当于HttpContext.Current.Server.MapPath("") 

                string workFileName = checkTime + "追逐时光企业员工培训考核统计记录表";
                string fileName = string.Format("{0}.docx", workFileName, System.Text.Encoding.UTF8);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                //TODO:使用FileStream文件流来写入数据（传入参数为：文件所在路径，对文件的操作方式，对文件内数据的操作）
                //通过使用文件流，创建文件流对象，向文件流中写入内容，并保存为Word文档格式
                using (var stream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create, FileAccess.Write))
                {
                    //创建document文档对象对象实例
                    XWPFDocument document = new XWPFDocument();

                    /**
                     *这里我通过设置公共的Word文档中SetParagraph（段落）实例创建和段落样式格式设置，大大减少了代码的冗余，
                     * 避免每使用一个段落而去创建一次段落实例和设置段落的基本样式
                     *(如下，ParagraphInstanceSetting为段落实例创建和样式设置，后面索引表示为当前是第几行段落,索引从0开始)
                     */
                    //文本标题
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, workFileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);

                    //TODO:这里一行需要显示两个文本
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"编号：20190927101120445887", false, 14, "宋体", ParagraphAlignment.LEFT, true, $"       检查时间：{checkTime}"), 1);


                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "登记机关：企业员工监督检查机构", false, 14, "宋体", ParagraphAlignment.LEFT), 2);


                    #region 文档第一个表格对象实例
                    //创建文档中的表格对象实例
                    XWPFTable firstXwpfTable = document.CreateTable(4, 4);//显示的行列数rows:3行,cols:4列
                    firstXwpfTable.Width = 5200;//总宽度
                    firstXwpfTable.SetColumnWidth(0, 1300); /* 设置列宽 */
                    firstXwpfTable.SetColumnWidth(1, 1100); /* 设置列宽 */
                    firstXwpfTable.SetColumnWidth(2, 1400); /* 设置列宽 */
                    firstXwpfTable.SetColumnWidth(3, 1400); /* 设置列宽 */

                    //Table 表格第一行展示...后面的都是一样，只改变GetRow中的行数
                    firstXwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "企业名称", ParagraphAlignment.CENTER, 24, true));
                    firstXwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "追逐时光", ParagraphAlignment.CENTER, 24, false));
                    firstXwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "企业地址", ParagraphAlignment.CENTER, 24, true));
                    firstXwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "湖南省-长沙市-岳麓区", ParagraphAlignment.CENTER, 24, false));

                    //Table 表格第二行
                    firstXwpfTable.GetRow(1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "联系人", ParagraphAlignment.CENTER, 24, true));
                    firstXwpfTable.GetRow(1).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "小明同学", ParagraphAlignment.CENTER, 24, false));
                    firstXwpfTable.GetRow(1).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "联系方式", ParagraphAlignment.CENTER, 24, true));
                    firstXwpfTable.GetRow(1).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "151****0456", ParagraphAlignment.CENTER, 24, false));


                    //Table 表格第三行
                    firstXwpfTable.GetRow(2).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "企业许可证号", ParagraphAlignment.CENTER, 24, true));
                    firstXwpfTable.GetRow(2).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "XXXXX-66666666", ParagraphAlignment.CENTER, 24, false));
                    firstXwpfTable.GetRow(2).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "检查次数", ParagraphAlignment.CENTER, 24, true));
                    firstXwpfTable.GetRow(2).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, $"本年度检查8次", ParagraphAlignment.CENTER, 24, false));


                    firstXwpfTable.GetRow(3).MergeCells(0, 3);//合并3列
                    firstXwpfTable.GetRow(3).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "", ParagraphAlignment.LEFT, 10, false));

                    #endregion

                    var checkPeopleNum = 0;//检查人数
                    var totalScore = 0;//总得分

                    #region 文档第二个表格对象实例（遍历表格项）
                    //创建文档中的表格对象实例
                    XWPFTable secoedXwpfTable = document.CreateTable(5, 4);//显示的行列数rows:8行,cols:4列
                    secoedXwpfTable.Width = 5200;//总宽度
                    secoedXwpfTable.SetColumnWidth(0, 1300); /* 设置列宽 */
                    secoedXwpfTable.SetColumnWidth(1, 1100); /* 设置列宽 */
                    secoedXwpfTable.SetColumnWidth(2, 1400); /* 设置列宽 */
                    secoedXwpfTable.SetColumnWidth(3, 1400); /* 设置列宽 */

                    //遍历表格标题
                    secoedXwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "员工姓名", ParagraphAlignment.CENTER, 24, true));
                    secoedXwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "性别", ParagraphAlignment.CENTER, 24, true));
                    secoedXwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "年龄", ParagraphAlignment.CENTER, 24, true));
                    secoedXwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "综合评分", ParagraphAlignment.CENTER, 24, true));

                    //遍历四条数据
                    for (var i = 1; i < 5; i++)
                    {
                        secoedXwpfTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "小明" + i + "号", ParagraphAlignment.CENTER, 24, false));
                        secoedXwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, "男", ParagraphAlignment.CENTER, 24, false));
                        secoedXwpfTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, 20 + i + "岁", ParagraphAlignment.CENTER, 24, false));
                        secoedXwpfTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, firstXwpfTable, 90 + i + "分", ParagraphAlignment.CENTER, 24, false));

                        checkPeopleNum++;
                        totalScore += 90 + i;
                    }

                    #endregion

                    #region 文档第三个表格对象实例
                    //创建文档中的表格对象实例
                    XWPFTable thirdXwpfTable = document.CreateTable(5, 4);//显示的行列数rows:5行,cols:4列
                    thirdXwpfTable.Width = 5200;//总宽度
                    thirdXwpfTable.SetColumnWidth(0, 1300); /* 设置列宽 */
                    thirdXwpfTable.SetColumnWidth(1, 1100); /* 设置列宽 */
                    thirdXwpfTable.SetColumnWidth(2, 1400); /* 设置列宽 */
                    thirdXwpfTable.SetColumnWidth(3, 1400); /* 设置列宽 */
                    //Table 表格第一行，后面的合并3列(注意关于表格中行合并问题，先合并，后填充内容)
                    thirdXwpfTable.GetRow(0).MergeCells(0, 3);//从第一列起,合并3列
                    thirdXwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "                                                                                         " + "检查内容: " +
                        $"于{checkTime}下午检查了追逐时光企业员工培训考核并对员工的相关信息进行了相关统计，统计结果如下：                                                                                                                                                                                                                " +
                        "-------------------------------------------------------------------------------------" +
                        $"共对该企业（{checkPeopleNum}）人进行了培训考核，培训考核总得分为（{totalScore}）分。 " + "", ParagraphAlignment.LEFT, 30, false));


                    //Table 表格第二行
                    thirdXwpfTable.GetRow(1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "检查结果: ", ParagraphAlignment.CENTER, 24, true));
                    thirdXwpfTable.GetRow(1).MergeCells(1, 3);//从第二列起，合并三列
                    thirdXwpfTable.GetRow(1).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "该企业非常优秀，坚持每天学习打卡，具有蓬勃向上的活力。", ParagraphAlignment.LEFT, 24, false));

                    //Table 表格第三行
                    thirdXwpfTable.GetRow(2).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "处理结果: ", ParagraphAlignment.CENTER, 24, true));
                    thirdXwpfTable.GetRow(2).MergeCells(1, 3);
                    thirdXwpfTable.GetRow(2).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "通过检查，评分为优秀！", ParagraphAlignment.LEFT, 24, false));

                    //Table 表格第四行，后面的合并3列(注意关于表格中行合并问题，先合并，后填充内容),额外说明
                    thirdXwpfTable.GetRow(3).MergeCells(0, 3);//合并3列
                    thirdXwpfTable.GetRow(3).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "备注说明: 记住，坚持就是胜利，永远保持一种求知，好问的心理！", ParagraphAlignment.LEFT, 24, false));

                    //Table 表格第五行
                    thirdXwpfTable.GetRow(4).MergeCells(0, 1);
                    thirdXwpfTable.GetRow(4).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "                                                                                                                                                                                                 检查人员签名：              年 月 日", ParagraphAlignment.LEFT, 30, false));
                    thirdXwpfTable.GetRow(4).MergeCells(1, 2);

                    thirdXwpfTable.GetRow(4).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, thirdXwpfTable, "                                                                                                                                                                                                 企业法人签名：              年 月 日", ParagraphAlignment.LEFT, 30, false));


                    #endregion

                    //向文档流中写入内容，生成word
                    document.Write(stream);

                    savePath = "/SaveWordFile/" + currentDate + "/" + fileName;

                    return true;
                }
            }
            catch (Exception ex)
            {
                //ignore
                savePath = ex.Message;
                return false;
            }
        }

        #endregion

        public async Task<string> SaveWordFile(string savePath, string tablename,string linktable1, int tid)
        {
            tablename = tablename.Replace("Ach", "Ful");
            savePath = "";
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string checkTime = DateTime.Now.ToString("yyyy年MM月dd日");//检查时间
                                                                    //保存文件到静态资源wwwroot,使用绝对路径路径
            var uploadPath = _environment.WebRootPath + "/SaveWordFile/" + currentDate + "/";//>>>相当于HttpContext.Current.Server.MapPath("") 
            try
            {


                string workFileName = checkTime + "SQL配置脚本";
                string fileName = string.Format("{0}.docx", workFileName, System.Text.Encoding.UTF8);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                //TODO:使用FileStream文件流来写入数据（传入参数为：文件所在路径，对文件的操作方式，对文件内数据的操作）
                //通过使用文件流，创建文件流对象，向文件流中写入内容，并保存为Word文档格式
                using (var stream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create, FileAccess.Write))
                {
                    //创建document文档对象对象实例
                    XWPFDocument document = new XWPFDocument();

                    /**
                     *这里我通过设置公共的Word文档中SetParagraph（段落）实例创建和段落样式格式设置，大大减少了代码的冗余，
                     * 避免每使用一个段落而去创建一次段落实例和设置段落的基本样式
                     *(如下，ParagraphInstanceSetting为段落实例创建和样式设置，后面索引表示为当前是第几行段落,索引从0开始)
                     */
                    //文本标题
                    //document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, workFileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);

                    //TODO:这里一行需要显示两个文本
                    //循环表格，生成对应的sql脚本
                    var list = await _achOrgRepository.Query(x => x.Tid == tid);

                    #region 写入文本
                    //先操作删除语句
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"DELETE FROM {tablename} ;", false, 6, "宋体", ParagraphAlignment.LEFT), 0);

                    int index = 0;
                    foreach (var item in list)
                    {
                        string insertstring = "";
                        string valuesstring = "";
                        Type t = item.GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            var value = pi.GetValue(item, null);//用pi.GetValue获得值
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            insertstring += $"{name},";
                            valuesstring += $"'{value}',";
                        }
                        //拼接sql语句
                        insertstring = insertstring.Substring(0, insertstring.Length - 1);
                        valuesstring = valuesstring.Substring(0, valuesstring.Length - 1);
                        index++;
                        document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"INSERT INTO {tablename} ({insertstring}) VALUES({valuesstring});", false, 6, "宋体", ParagraphAlignment.LEFT), index);
                    }
                    #endregion



                    // document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "登记机关：企业员工监督检查机构", false, 14, "宋体", ParagraphAlignment.LEFT), 2);

                    //向文档流中写入内容，生成word
                    document.Write(stream);

                    savePath = "/SaveWordFile/" + currentDate + "/" + fileName;
                    uploadPath += fileName;
                    return savePath;
                }
            }
            catch (Exception ex)
            {
                //ignore
                savePath = ex.Message;
                return "";
            }
        }


        /// <summary>
        /// 共公用--针对单独导出一个表的情况
        ///  生成word文档,并保存静态资源文件夹（wwwroot)下的SaveWordFile文件夹中
        /// </summary>
        /// <param name="savePath">保存路径</param>
        public async Task<string> SaveWordFile(string savePath, string tablename1, string tablename2, string tablename3, string tablename4, int tid)
        {
            tablename1 = tablename1.Replace("Ach", "Ful");
            tablename2 = tablename2.Replace("Ach", "Ful");
            tablename3 = tablename3.Replace("Ach", "Ful");
            tablename4 = tablename4.Replace("Ach", "Ful");
            savePath = Appsettings.app(new string[] { "WordDownLoadUrl", "Url" });//获取连接字符串

            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string checkTime = DateTime.Now.ToString("yyyy年MM月dd日");//检查时间
                                                                    //保存文件到静态资源wwwroot,使用绝对路径路径
            var uploadPath = _environment.WebRootPath + "/SaveWordFile/" + currentDate + "/";//>>>相当于HttpContext.Current.Server.MapPath("") 
            try
            {
                string workFileName = checkTime + "SQL配置脚本";
                string fileName = string.Format("{0}.docx", workFileName, System.Text.Encoding.UTF8);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                //TODO:使用FileStream文件流来写入数据（传入参数为：文件所在路径，对文件的操作方式，对文件内数据的操作）
                //通过使用文件流，创建文件流对象，向文件流中写入内容，并保存为Word文档格式
                using (var stream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create, FileAccess.Write))
                {
                    //创建document文档对象对象实例
                    XWPFDocument document = new XWPFDocument();

                    /**
                     *这里我通过设置公共的Word文档中SetParagraph（段落）实例创建和段落样式格式设置，大大减少了代码的冗余，
                     * 避免每使用一个段落而去创建一次段落实例和设置段落的基本样式
                     *(如下，ParagraphInstanceSetting为段落实例创建和样式设置，后面索引表示为当前是第几行段落,索引从0开始)
                     */
                    //文本标题
                    //document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, workFileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);

                    //TODO:这里一行需要显示两个文本
                    //循环表格，生成对应的sql脚本

                    #region 写入文档-四个表

                    var list1 = await _achFgpRepository.Query(x => x.Tid == tid);
                    var list2 = await _achFupRepository.Query(x => x.Tid == tid);
                    var list3 = await _achFunRepository.Query(x => x.Tid == tid);
                    var list4 = await _achBtnRepository.Query(x => x.Tid == tid);

                    int index = 0;//总的行数

                    #region 第一个表

                    #region 删除
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"DELETE FROM {tablename1};", false, 7, "宋体", ParagraphAlignment.LEFT), index);
                    #endregion

                    #region 插入属性
                    if (list1.Count > 0)
                    {
                        index++;
                        string insertstring = "";
                        Type t = list1[0].GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            insertstring += $"{name},";
                        }
                        insertstring = insertstring.Substring(0, insertstring.Length - 1);
                        document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"INSERT INTO {tablename1} ({insertstring}) VALUES", false, 8, "宋体", ParagraphAlignment.LEFT), index);
                    }
                    #endregion

                    #region 插入属性值
                    int index1 = 0;
                    foreach (var item in list1)
                    {
                        string valuesstring = "";
                        Type t = item.GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            var value = pi.GetValue(item, null);//用pi.GetValue获得值
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            valuesstring += $"'{value}',";
                        }
                        index1++;
                        index++;
                        //拼接sql语句
                        valuesstring = valuesstring.Substring(0, valuesstring.Length - 1);
                        if (list1.Count == index1)
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring});", false, 8, "宋体", ParagraphAlignment.LEFT), index);

                        }
                        else
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring}),", false, 8, "宋体", ParagraphAlignment.LEFT), index);

                        }

                    

                    }
                    #endregion

                    #endregion

                    index++;
                    #region 第二个表

                    #region 删除
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"DELETE FROM {tablename2};", false, 7, "宋体", ParagraphAlignment.LEFT), index);
                    #endregion

                    #region 插入属性
                    if (list2.Count > 0)
                    {
                        index++;
                        string insertstring = "";
                        Type t = list2[0].GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            insertstring += $"{name},";
                        }
                        insertstring = insertstring.Substring(0, insertstring.Length - 1);
                        document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"INSERT INTO {tablename2} ({insertstring}) VALUES", false, 8, "宋体", ParagraphAlignment.LEFT), index);
                    }
                    #endregion

                    #region 插入属性值
                    int index2 = 0;
                    foreach (var item in list2)
                    {
                        string valuesstring = "";
                        Type t = item.GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            var value = pi.GetValue(item, null);//用pi.GetValue获得值
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            valuesstring += $"'{value}',";
                        }
                        index2++;
                        index++;
                        //拼接sql语句
                        valuesstring = valuesstring.Substring(0, valuesstring.Length - 1);
                        if (list2.Count == index2)
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring});", false, 8, "宋体", ParagraphAlignment.LEFT), index);

                        }
                        else
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring}),", false, 8, "宋体", ParagraphAlignment.LEFT), index);

                        }
                      

                    }
                    #endregion

                    #endregion
                    index++;
                    #region 第三个表

                    #region 删除
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"DELETE FROM {tablename3};", false, 7, "宋体", ParagraphAlignment.LEFT), index);
                    #endregion

                    #region 插入属性
                    if (list3.Count > 0)
                    {
                        index++;
                        string insertstring = "";
                        Type t = list3[0].GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            insertstring += $"{name},";
                        }
                        insertstring = insertstring.Substring(0, insertstring.Length - 1);
                        document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"INSERT INTO {tablename3} ({insertstring}) VALUES", false, 8, "宋体", ParagraphAlignment.LEFT), index);
                    }
                    #endregion

                    #region 插入属性值
                    int index3 = 0;
                    foreach (var item in list3)
                    {
                        string valuesstring = "";
                        Type t = item.GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            var value = pi.GetValue(item, null);//用pi.GetValue获得值
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            valuesstring += $"'{value}',";
                        }
                        index3++;
                        index++;
                        //拼接sql语句
                        valuesstring = valuesstring.Substring(0, valuesstring.Length - 1);
                        if (list3.Count == index3)
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring});", false, 8, "宋体", ParagraphAlignment.LEFT), index);

                        }
                        else
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring}),", false, 8, "宋体", ParagraphAlignment.LEFT), index);

                        }
                       

                    }
                    #endregion

                    #endregion
                    index++;
                    #region 第四个表

                    #region 删除
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"DELETE FROM {tablename4};", false, 7, "宋体", ParagraphAlignment.LEFT), index);
                    #endregion

                    #region 插入属性
                    if (list4.Count > 0)
                    {
                        index++;
                        string insertstring = "";
                        Type t = list4[0].GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            insertstring += $"{name},";
                        }
                        insertstring = insertstring.Substring(0, insertstring.Length - 1);
                        document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"INSERT INTO {tablename4} ({insertstring}) VALUES", false, 8, "宋体", ParagraphAlignment.LEFT), index);
                    }
                    #endregion

                    #region 插入属性值
                    int index4 = 0;
                    foreach (var item in list4)
                    {
                        string valuesstring = "";
                        Type t = item.GetType();//获得该类的Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                            var value = pi.GetValue(item, null);//用pi.GetValue获得值
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            valuesstring += $"'{value}',";
                        }
                        index4++;
                        index++;
                        //拼接sql语句
                        valuesstring = valuesstring.Substring(0, valuesstring.Length - 1);
                 
                        if (list4.Count == index4)
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring});", false, 8, "宋体", ParagraphAlignment.LEFT), index);

                        }
                        else
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring}),", false, 8, "宋体", ParagraphAlignment.LEFT), index);
                          
                        }
                   

                    }
                    #endregion

                    #endregion
                    #endregion

                    //向文档流中写入内容，生成word
                    document.Write(stream);

                    savePath += "/SaveWordFile/" + currentDate + "/" + fileName;
                    uploadPath += fileName;
                    return savePath;
                }
            }
            catch (Exception ex)
            {
                //ignore
                uploadPath = ex.Message;
                return "";
            }
        }


    }
}
