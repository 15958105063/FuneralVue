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
            savePath = Appsettings.app(new string[] { "WordDownLoadUrl", "Url" });//��ȡ�����ַ���
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string checkTime = DateTime.Now.ToString("yyyy��MM��dd��");//���ʱ��
                                                                    //�����ļ�����̬��Դwwwroot,ʹ�þ���·��·��
            var uploadPath = _environment.WebRootPath + "/SaveWordFile/" + currentDate + "/";//>>>�൱��HttpContext.Current.Server.MapPath("") 
            try
            {
                string workFileName = checkTime + "������ϢSQL���ýű�";
                string fileName = string.Format("{0}.docx", workFileName, System.Text.Encoding.UTF8);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                //TODO:ʹ��FileStream�ļ�����д�����ݣ��������Ϊ���ļ�����·�������ļ��Ĳ�����ʽ�����ļ������ݵĲ�����
                //ͨ��ʹ���ļ����������ļ����������ļ�����д�����ݣ�������ΪWord�ĵ���ʽ
                using (var stream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create, FileAccess.Write))
                {
                    //����document�ĵ��������ʵ��
                    XWPFDocument document = new XWPFDocument();

                    /**
                     *������ͨ�����ù�����Word�ĵ���SetParagraph�����䣩ʵ�������Ͷ�����ʽ��ʽ���ã��������˴�������࣬
                     * ����ÿʹ��һ�������ȥ����һ�ζ���ʵ�������ö���Ļ�����ʽ
                     *(���£�ParagraphInstanceSettingΪ����ʵ����������ʽ���ã�����������ʾΪ��ǰ�ǵڼ��ж���,������0��ʼ)
                     */
                    //�ı�����
                    //document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, workFileName, true, 19, "����", ParagraphAlignment.CENTER), 0);

                    //TODO:����һ����Ҫ��ʾ�����ı�
                    //ѭ��������ɶ�Ӧ��sql�ű�
                    var list = await _dal.Query(x => x.Tid == tid);

                    #region д���ı�

                    #region ɾ�����
                    //�Ȳ���ɾ�����
                    document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"DELETE FROM {tablename};", false, 8, "����", ParagraphAlignment.LEFT), 0);

                    #endregion

                    #region �����������
                    if (list.Count > 0)
                    {
                        string insertstring = "";

                        Type t = list[0].GetType();//��ø����Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//������Ե�����,����Ϳ��Ը��������ж�������Щ�Լ���Ҫ�Ĳ���
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            insertstring += $"{name},";
                        }
                        insertstring = insertstring.Substring(0, insertstring.Length - 1);

                        document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"INSERT INTO {tablename} ({insertstring}) VALUES", false, 8, "����", ParagraphAlignment.LEFT), 1);
                    }
                    #endregion

                    #region �������ֵ
                    int index = 1;
                    foreach (var item in list)
                    {
                        string valuesstring = "";
                        Type t = item.GetType();//��ø����Type
                        foreach (var pi in t.GetProperties())
                        {
                            var name = pi.Name;//������Ե�����,����Ϳ��Ը��������ж�������Щ�Լ���Ҫ�Ĳ���
                            var value = pi.GetValue(item, null);//��pi.GetValue���ֵ
                            if (name == "Id" || name == "Tid" || name == "CreateId" || name == "CreateBy" || name == "CreateTime" || name == "ModifyId" || name == "ModifyBy" || name == "ModifyTime")
                            {
                                continue;
                            }
                            valuesstring += $"'{value}',";
                        }
                        //ƴ��sql���
                        valuesstring = valuesstring.Substring(0, valuesstring.Length - 1);
                        index++;
                        if (list.Count == index - 1)
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring});", false, 8, "����", ParagraphAlignment.LEFT), index);

                        }
                        else
                        {
                            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"({valuesstring}),", false, 8, "����", ParagraphAlignment.LEFT), index);

                        }
                    }
                    #endregion

                    #endregion

                    //���ĵ�����д�����ݣ�����word
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
