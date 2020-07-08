using Funeral.Core.Common;
using Funeral.Core.Common.NpoiHelper;
using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;
using Microsoft.AspNetCore.Hosting;
using NPOI.XWPF.UserModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Funeral.Core.Services
{
    /// <summary>
    /// AchOrgServices
    /// </summary>	
    public class AchOrgServices : BaseServices<AchOrg>,IAchOrgServices
    {

        private static IHostingEnvironment _environment;
        private readonly IAchOrgRepository _dal;
        public AchOrgServices(IAchOrgRepository dal, IHostingEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
            this._dal = dal;
            base.BaseDal = dal;
        }

        public async Task<string> SaveWordFile(string savePath, string tablename, int tid)
        {
            tablename = tablename.Replace("Ach", "Ful");
            savePath = Appsettings.app(new string[] { "WordDownLoadUrl", "Url" });//获取连接字符串
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string checkTime = DateTime.Now.ToString("yyyy年MM月dd日");//检查时间
                                                                    //保存文件到静态资源wwwroot,使用绝对路径路径
            var uploadPath = _environment.WebRootPath + "/SaveWordFile/" + currentDate + "/";//>>>相当于HttpContext.Current.Server.MapPath("") 
            try
            {
                string workFileName = checkTime + "机构信息SQL配置脚本";
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
                    var list = await _dal.Query(x => x.Tid == tid);

                    #region 写入文本

                    #region 删除语句
                    //先操作删除语句
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"DELETE FROM {tablename};", false, 8, "宋体", ParagraphAlignment.LEFT), 0);

                    #endregion

                    #region 插入语句属性
                    if (list.Count > 0)
                    {
                        string insertstring = "";

                        Type t = list[0].GetType();//获得该类的Type
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

                        document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"INSERT INTO {tablename} ({insertstring}) VALUES", false, 8, "宋体", ParagraphAlignment.LEFT), 1);
                    }
                    #endregion

                    #region 插入语句值
                    int index = 1;
                    foreach (var item in list)
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
                        //拼接sql语句
                        valuesstring = valuesstring.Substring(0, valuesstring.Length - 1);
                        index++;
                        if (list.Count == index - 1)
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
                savePath = ex.Message;
                return savePath;
            }
        }

        public bool SaveWordFileDefault(string savePath)
        {
            throw new NotImplementedException();
        }
    }
}
