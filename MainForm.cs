using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using EducServLib;

namespace FIS_EGE_2013
{
    public partial class MainForm : Form
    {
        #region Dics

        /// <summary>
        /// Справочник № 1 - Общеобразовательные предметы
        /// </summary>
        private Dictionary<string, string> dic01_Subject = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 2 - Уровень образования
        /// </summary>
        private Dictionary<string, string> dic02_StudyLevel = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 3 - Уровень олимпиады
        /// </summary>
        private Dictionary<string, string> dic03_OlympLevel = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 4 - Статус заявления
        /// </summary>
        private Dictionary<string, string> dic04_ApplicationStatus = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 5 - Пол
        /// </summary>
        private Dictionary<string, string> dic05_Sex = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 6 - ИД основания для оценки
        /// </summary>
        private Dictionary<string, string> dic06_MarkDocument = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 7 - Страна
        /// </summary>
        private Dictionary<string, string> dic07_Country = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 10 - Направления подготовки
        /// </summary>
        private List<Dictionary10Direction> dic10_Direction = new List<Dictionary10Direction>();
        /// <summary>
        /// Справочник № 11 - Тип вступительных испытаний
        /// </summary>
        private Dictionary<string, string> dic11_EntranceTestType = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 12 - Статус проверки заявлений
        /// </summary>
        private Dictionary<string, string> dic12_ApplicationCheckStatus = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 13 - Статус проверки документа
        /// </summary>
        private Dictionary<string, string> dic13_DocumentCheckStatus = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 14 - Форма обучения
        /// </summary>
        private Dictionary<string, string> dic14_EducationForm = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 15 - Источник финансирования
        /// </summary>
        private Dictionary<string, string> dic15_FinSource = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 17 - Сообщения об ошибках
        /// </summary>
        private Dictionary<string, string> dic17_Errors = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 18 - Тип диплома
        /// </summary>
        private Dictionary<string, string> dic18_DiplomaType = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 19 - Олимпиады
        /// </summary>
        private List<DictionaryOlympiad> dic19_Olympics = new List<DictionaryOlympiad>();
        /// <summary>
        /// Справочник № 22 - Тип документа, удостоверяющего личность
        /// </summary>
        private Dictionary<string, string> dic22_IdentityDocumentType = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 23 - Группа инвалидности
        /// </summary>
        private Dictionary<string, string> dic23_DisabilityType = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 30 - Вид льготы
        /// </summary>
        private Dictionary<string, string> dic30_BenefitKind = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 31 - Тип документа
        /// </summary>
        private Dictionary<string, string> dic31_DocumentType = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 33 - Тип документа для вступительного испытания ОУ
        /// </summary>
        private Dictionary<string, string> dic33_ = new Dictionary<string, string>();
        /// <summary>
        /// Справочник № 34 - Статус приемной кампании
        /// </summary>
        private Dictionary<string, string> dic34_CampaignStatus = new Dictionary<string, string>();

        #endregion

        public MainForm()
        {
            InitializeComponent();
            FillComboStudyLevelGroup();
            FillComboFaculty();
        }

        private void FillComboStudyLevelGroup()
        {
            List<KeyValuePair<string, string>> lst = new List<KeyValuePair<string, string>>();
            lst.Add(new KeyValuePair<string, string>("1", "1 к."));
            lst.Add(new KeyValuePair<string, string>("2", "маг."));
            lst.Add(new KeyValuePair<string, string>("3", "СПО"));
            ComboServ.FillCombo(cbStudyLevelGroup, lst, false, false);
        }
        public int StudyLevelGroupId
        {
            get { return ComboServ.GetComboIdInt(cbStudyLevelGroup).Value; }
        }
        public int? FacultyId
        {
            get
            {
                if (cbFaculty.Text == ComboServ.DISPLAY_ALL_VALUE)
                    return null;
                else
                    return ComboServ.GetComboIdInt(cbFaculty);
            }
            set
            {
                ComboServ.SetComboId(cbFaculty, value);
            }
        }
        private void FillComboFaculty()
        {
            using (PriemEntities context = new PriemEntities())
            {
                var vals = context.extEntry.Where(x => x.StudyLevelGroupId == StudyLevelGroupId).Select(x => new { Id = x.FacultyId, Name = x.FacultyName }).Distinct().OrderBy(x => x.Id).
                    Select(x => new { x.Id, x.Name }).ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name)).ToList();
                ComboServ.FillCombo(cbFaculty, vals, false, true);
            }
        }

        public int? LicenseProgramId
        {
            get
            {
                if (cbLicenseProgram.Text == ComboServ.DISPLAY_ALL_VALUE)
                    return null;
                else
                    return ComboServ.GetComboIdInt(cbLicenseProgram);
            }
        }
        private void FillComboLicenseProgram()
        {
            using (PriemEntities context = new PriemEntities())
            {
                var vals = context.extEntry.Where(x => (FacultyId.HasValue ? x.FacultyId == FacultyId.Value : true) && x.StudyLevelGroupId == StudyLevelGroupId).
                    OrderBy(x => x.LicenseProgramCode).Select(x => new { x.LicenseProgramId, x.LicenseProgramCode, x.LicenseProgramName }).Distinct().ToList().
                    Select(x => new KeyValuePair<string, string>(x.LicenseProgramId.ToString(), x.LicenseProgramCode + " " + x.LicenseProgramName)).ToList();
                ComboServ.FillCombo(cbLicenseProgram, vals, false, true);
            }
        }
        private void cbFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillComboLicenseProgram();
        }
        private void cbLicenseProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }
        private void btnExportXML_Click(object sender, EventArgs e)
        {
            //ExportXML();
        }

        //заполняет только первые два раздела
        private void ExportXML_Part1(bool isCrimea)
        {
            XmlDocument doc = new XmlDocument();
            //XmlImplementation imp = new XmlImplementation();

            string fname = "";

            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "XML File|*.xml";
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                fname = sf.FileName;
            else
                return;

            UpdateDictionaries_Local();
            List<int?> BeneficiaryCompetitions = new List<int?>() { 1, 2, 7, 8 };

            using (PriemEntities context = new PriemEntities())
            {
                DateTime dt = DateTime.Now;
                TimeSpan dt1, dt2, dt3, dt4_allAbitList;
                TimeSpan dt4_EgeMarks = TimeSpan.MinValue, dt4_Marks = TimeSpan.MinValue;

                //создаём корневой элемент
                doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "Root", ""));

                //создаём элементы AuthData и PackageData
                XmlNode root = doc["Root"];

                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "AuthData", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "PackageData", ""));

                //заполняем данные AuthData
                root = root["AuthData"];
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Login", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Pass", ""));

                bool isDefault = true;
                root["Login"].InnerText = isDefault ? "p.karpenko@spbu.ru" : tbLogin.Text;
                root["Pass"].InnerText = isDefault ? "E0k02II" : tbPassword.Text;

                //----------------------------------------------------------------------------------------------------------
                //заполняем данные PackageData
                root = doc["Root"];
                //root["PackageData"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CampaignInfo", ""));
                root["PackageData"].AppendChild(doc.CreateNode(XmlNodeType.Element, "AdmissionInfo", ""));

                NewWatch wc = new NewWatch();
                int wcMax = 0;

                ////заполняем данные CampaignInfo
                //root = root["PackageData"]["CampaignInfo"];
                //root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Campaigns", ""));
                ////вкладываем внутрь текущую приёмную кампанию
                ////в дальнейшем нужно вводить ВСЕ кампании за ВСЕ годы???
                //root["Campaigns"].AppendChild(doc.CreateNode(XmlNodeType.Element, "Campaign", ""));
                ////заполняем сведения о текущей приёмной кампании
                ////Guid рандомно придуманный
                string CampaignId = new Guid("00000000-0000-0000-2015-00000000000" + (isCrimea ? "2" : "1")).ToString();
                wc.Show();
                //--------------------------------------------------------------------------------------------------------------
                //заполняем данные AdmissionInfo
                //объём приёма (КЦ)
                root = doc["Root"]["PackageData"];
                root["AdmissionInfo"].AppendChild(doc.CreateNode(XmlNodeType.Element, "AdmissionVolume", ""));
                //распределение по уровням бюджета
                root["AdmissionInfo"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DistributedAdmissionVolume", ""));

                //Конкурсные группы
                root["AdmissionInfo"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroups", ""));

                //Заполняем объём приёма (КЦ)
                root = root["AdmissionInfo"]["AdmissionVolume"];
                
                //объём для СПО не грузим совсем
                var LPs = context.extEntry.Where(x => x.StudyLevelGroupId <= 4 && (FacultyId.HasValue ? x.FacultyId == FacultyId.Value : true)
                    && (LicenseProgramId.HasValue ? x.LicenseProgramId == LicenseProgramId.Value : true)).Select(x => x.LicenseProgramId).Distinct();

                var PriemHelps = context.hlpLicenseProgramKCP.Where(x => x.LevelGroupId <= 4).ToList()
                    .Select(x => new 
                    { 
                        x.QualificationCode, 
                        x.LicenseProgramCode, 
                        x.LicenseProgramName, 
                        x.StudyLevelId, 
                        x.IsReduced, 
                        x.KC_OB, 
                        x.KC_OP, 
                        x.KC_OZB, 
                        x.KC_OZP, 
                        x.KC_ZB, 
                        x.KC_ZP, 
                        x.KC_O_CEL, 
                        x.KC_OZ_CEL, 
                        x.KC_Z_CEL,
                        x.KC_O_QUOTA,
                        x.KC_OZ_QUOTA,
                        x.KC_Z_QUOTA
                    })
                    .Distinct().ToList();

                wcMax = PriemHelps.Count();
                wc.SetText("Заполняем объём приёма (КЦ)");
                wc.SetMax(wcMax);
                wc.ZeroCount();

                foreach (var p in PriemHelps)
                {
                    wc.PerformStep();
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Item", ""));
                    //идентификатор объёма приёма по направлению подготовки в приёме
                    string StudyLevelFISName = context.extEntry.Where(x => x.StudyLevelId == p.StudyLevelId).Select(x => x.StudyLevelFISName).FirstOrDefault();
                    if (StudyLevelFISName == "Бакалавриат" && p.IsReduced)
                        StudyLevelFISName += " (сокращ.)";

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    root.LastChild["UID"].InnerXml = DateTime.Now.Year.ToString() + "_" + p.LicenseProgramCode.ToString() + "_" + p.QualificationCode + "_" + (StudyLevelFISName == "Бакалавриат (сокращ.)" ? "1" : "0");
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CampaignUID", ""));
                    root.LastChild["CampaignUID"].InnerXml = CampaignId;
                    //Справочник №2 - Уровни образования
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationLevelID", ""));
                    root.LastChild["EducationLevelID"].InnerXml = SearchInDictionary(dic02_StudyLevel, StudyLevelFISName); //dic02_StudyLevel[p.StudyLevelId.ToString()];
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Course", ""));
                    root.LastChild["Course"].InnerXml = "1";
                    //Справочник №10 - направления подготовки
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DirectionID", ""));
                    string qual_code = p.QualificationCode;
                    root.LastChild["DirectionID"].InnerXml = SearchInDictionary(dic10_Direction, p.LicenseProgramName, p.LicenseProgramCode, qual_code);
                    if (p.KC_OB.HasValue && p.KC_OB > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetO", ""));
                        root.LastChild["NumberBudgetO"].InnerXml = p.KC_OB.ToString();
                    }
                    if (p.KC_OZB.HasValue && p.KC_OZB > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetOZ", ""));
                        root.LastChild["NumberBudgetOZ"].InnerXml = p.KC_OZB.ToString();
                    }
                    if (p.KC_ZB.HasValue && p.KC_ZB > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetZ", ""));
                        root.LastChild["NumberBudgetZ"].InnerXml = p.KC_ZB.ToString();
                    }
                    if (p.KC_OP.HasValue && p.KC_OP > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidO", ""));
                        root.LastChild["NumberPaidO"].InnerXml = p.KC_OP.ToString();
                    }
                    if (p.KC_OZP.HasValue && p.KC_OZP > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidOZ", ""));
                        root.LastChild["NumberPaidOZ"].InnerXml = p.KC_OZP.ToString();
                    }
                    if (p.KC_ZP.HasValue && p.KC_ZP > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidZ", ""));
                        root.LastChild["NumberPaidZ"].InnerXml = p.KC_ZP.ToString();
                    }
                    if (p.KC_O_CEL.HasValue && p.KC_O_CEL > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetO", ""));
                        root.LastChild["NumberTargetO"].InnerXml = p.KC_O_CEL.ToString();
                    }
                    if (p.KC_OZ_CEL.HasValue && p.KC_OZ_CEL > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetOZ", ""));
                        root.LastChild["NumberTargetOZ"].InnerXml = p.KC_OZ_CEL.ToString();
                    }
                    if (p.KC_Z_CEL.HasValue && p.KC_Z_CEL > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetZ", ""));
                        root.LastChild["NumberTargetZ"].InnerXml = p.KC_Z_CEL.ToString();
                    }
                    //квотники - дополнение 2015 года
                    if (p.KC_O_QUOTA.HasValue && p.KC_O_QUOTA > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaO", ""));
                        root.LastChild["NumberQuotaO"].InnerXml = p.KC_O_QUOTA.ToString();
                    }
                    if (p.KC_OZ_QUOTA.HasValue && p.KC_OZ_QUOTA > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaOZ", ""));
                        root.LastChild["NumberQuotaOZ"].InnerXml = p.KC_OZ_QUOTA.ToString();
                    }
                    if (p.KC_Z_QUOTA.HasValue && p.KC_Z_QUOTA > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaZ", ""));
                        root.LastChild["NumberQuotaZ"].InnerXml = p.KC_Z_QUOTA.ToString();
                    }
                }

                //повторяем операцию для распределения по уровням бюджета
                root = root.ParentNode["DistributedAdmissionVolume"];
                foreach (var p in PriemHelps)
                {
                    wc.PerformStep();
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Item", ""));
                    //идентификатор объёма приёма по направлению подготовки в приёме
                    string StudyLevelFISName = context.extEntry.Where(x => x.StudyLevelId == p.StudyLevelId).Select(x => x.StudyLevelFISName).FirstOrDefault();
                    if (StudyLevelFISName == "Бакалавриат" && p.IsReduced)
                        StudyLevelFISName += " (сокращ.)";

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "AdmissionVolumeUID", ""));
                    root.LastChild["AdmissionVolumeUID"].InnerXml = DateTime.Now.Year.ToString() + "_" + p.LicenseProgramCode.ToString() + "_" + p.QualificationCode + "_" + 
                        (StudyLevelFISName == "Бакалавриат (сокращ.)" ? "1" : "0");
                    //Справочник №35 - Уровни бюджета
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "LevelBudget", ""));
                    root.LastChild["LevelBudget"].InnerXml = "1";
                    if (p.KC_OB.HasValue && p.KC_OB > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetO", ""));
                        root.LastChild["NumberBudgetO"].InnerXml = p.KC_OB.ToString();
                    }
                    if (p.KC_OZB.HasValue && p.KC_OZB > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetOZ", ""));
                        root.LastChild["NumberBudgetOZ"].InnerXml = p.KC_OZB.ToString();
                    }
                    if (p.KC_ZB.HasValue && p.KC_ZB > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetZ", ""));
                        root.LastChild["NumberBudgetZ"].InnerXml = p.KC_ZB.ToString();
                    }
                    if (p.KC_O_CEL.HasValue && p.KC_O_CEL > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetO", ""));
                        root.LastChild["NumberTargetO"].InnerXml = p.KC_O_CEL.ToString();
                    }
                    if (p.KC_OZ_CEL.HasValue && p.KC_OZ_CEL > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetOZ", ""));
                        root.LastChild["NumberTargetOZ"].InnerXml = p.KC_OZ_CEL.ToString();
                    }
                    if (p.KC_Z_CEL.HasValue && p.KC_Z_CEL > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetZ", ""));
                        root.LastChild["NumberTargetZ"].InnerXml = p.KC_Z_CEL.ToString();
                    }
                    //квотники - дополнение 2015 года
                    if (p.KC_O_QUOTA.HasValue && p.KC_O_QUOTA > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaO", ""));
                        root.LastChild["NumberQuotaO"].InnerXml = p.KC_O_QUOTA.ToString();
                    }
                    if (p.KC_OZ_QUOTA.HasValue && p.KC_OZ_QUOTA > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaOZ", ""));
                        root.LastChild["NumberQuotaOZ"].InnerXml = p.KC_OZ_QUOTA.ToString();
                    }
                    if (p.KC_Z_QUOTA.HasValue && p.KC_Z_QUOTA > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaZ", ""));
                        root.LastChild["NumberQuotaZ"].InnerXml = p.KC_Z_QUOTA.ToString();
                    }
                }

                dt2 = DateTime.Now - dt;
                dt = DateTime.Now;
                //Заполняем конкурсные группы
                List<int> lstStudLev = new List<int>();
                
                lstStudLev.Add(16);
                lstStudLev.Add(18);
                lstStudLev.Add(17);
                lstStudLev.Add(15);
                lstStudLev.Add(8);
                lstStudLev.Add(10);

                root = root.ParentNode["CompetitiveGroups"];
                var CompetitionGroups = context.extCompetitiveGroup.Where(x => x.IsForeign == false && x.IsCrimea == isCrimea &&
                    (LicenseProgramId.HasValue ? x.LicenseProgramId == LicenseProgramId.Value : true)
                    && (FacultyId.HasValue ? x.FacultyId == FacultyId.Value : true)
                    && (lstStudLev.Contains(x.StudyLevelId)))
                    .Select(x => new { x.Id, x.StudyLevelFISName, x.Name, x.QualificationCode, x.LicenseProgramName, x.LicenseProgramCode, 
                        x.KCP_ZB, x.KCP_ZP, x.KCP_Cel_B, x.KCP_Cel_P, x.KCP_OB, x.KCP_OP, x.KCP_OZB, x.KCP_OZP, x.KCP_QuotaO, x.KCP_QuotaOZ, x.KCP_QuotaZ, x.StudyLevelId } )
                    .Distinct();

                wcMax = CompetitionGroups.Count();
                wc.SetText("Заполняем конкурсные группы");
                wc.SetMax(wcMax);
                wc.ZeroCount();

                var Benefits =
                            (from OlBenefit in context.OlympResultToCommonBenefit
                             join compGroup in context.qEntryToCompetitiveGroup on OlBenefit.EntryId equals compGroup.EntryId
                             join extOl in context.extOlympiadsAll on OlBenefit.OlympSubjectId equals extOl.OlympSubjectId
                             join OlympSubjToExam in context.OlympSubjectToExam on extOl.OlympSubjectId equals OlympSubjToExam.OlympSubjectId
                             join Ex in context.Exam on OlympSubjToExam.ExamId equals Ex.Id
                             join ExName in context.ExamName on Ex.ExamNameId equals ExName.Id
                             where OlBenefit.OlympTypeId > 2 //только олимпиады школьников, всероссы-международные пропускаем
                             && extOl.OlympTypeId > 2 //только олимпиады школьников, всероссы-международные пропускаем
                             && extOl.OlympLevelId == OlBenefit.OlympLevelId
                             && extOl.OlympTypeId == OlBenefit.OlympTypeId
                             select new
                             {
                                 OlBenefit.Id,
                                 compGroup.CompetitiveGroupId,
                                 extOl.OlympSubjectName,
                                 extOl.OlympName,
                                 OlympValue = (OlBenefit.OlympValueId == 6 ? 1 : 2),
                                 ExamName = ExName.Name
                             }).Distinct().ToList();

                foreach (var CompetitionGroup in CompetitionGroups)
                {
                    wc.PerformStep();
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroup", ""));

                    var CompGroupNode = root.LastChild;

                    //идентификатор конкурсной группы в приёме
                    CompGroupNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    CompGroupNode["UID"].InnerXml = CompetitionGroup.Id.ToString();
                    CompGroupNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "CampaignUID", ""));
                    CompGroupNode["CampaignUID"].InnerXml = CampaignId;
                    CompGroupNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "Course", ""));
                    CompGroupNode["Course"].InnerXml = "1";
                    CompGroupNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                    CompGroupNode["Name"].InnerXml = CompetitionGroup.Name;

                    CompGroupNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "Items", ""));

                    //добавляем направление подготовки конкурсной группы
                    CompGroupNode["Items"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupItem", ""));

                    var node = CompGroupNode["Items"]["CompetitiveGroupItem"];

                    //заполняем данные о направлении подготовки для конкурсной группы
                    //это сделано для ВУЗов, где ведётся один конкурс на НЕСКОЛЬКО направлений
                    //поскольку у нас нет одного конкурса на несколько направлений, то мы просто возьмём тот же UID, что и для всей группы
                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    node["UID"].InnerXml = CompetitionGroup.Id.ToString();
                    
                    //Справочник №2 - Уровни образования
                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationLevelID", ""));
                    string sEducationLevelID = SearchInDictionary(dic02_StudyLevel, CompetitionGroup.StudyLevelFISName);
                    node["EducationLevelID"].InnerXml = sEducationLevelID;
                    
                    //id направления подготовки из справочника №10
                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DirectionID", ""));
                    string sDirectionID = SearchInDictionary(dic10_Direction, CompetitionGroup.LicenseProgramName, CompetitionGroup.LicenseProgramCode, CompetitionGroup.QualificationCode);
                    node["DirectionID"].InnerXml = sDirectionID;

                    if (CompetitionGroup.KCP_OB.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetO", ""));
                        node["NumberBudgetO"].InnerXml = CompetitionGroup.KCP_OB.ToString();
                    }
                    if (CompetitionGroup.KCP_OZB.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetOZ", ""));
                        node["NumberBudgetOZ"].InnerXml = CompetitionGroup.KCP_OZB.ToString();
                    }
                    if (CompetitionGroup.KCP_ZB.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetZ", ""));
                        node["NumberBudgetZ"].InnerXml = CompetitionGroup.KCP_ZB.ToString();
                    }
                    if (CompetitionGroup.KCP_OP.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidO", ""));
                        node["NumberPaidO"].InnerXml = CompetitionGroup.KCP_OP.ToString();
                    }
                    if (CompetitionGroup.KCP_OZP.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidOZ", ""));
                        node["NumberPaidOZ"].InnerXml = CompetitionGroup.KCP_OZP.ToString();
                    }
                    if (CompetitionGroup.KCP_ZP.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidZ", ""));
                        node["NumberPaidZ"].InnerXml = CompetitionGroup.KCP_ZP.ToString();
                    }

                    if (CompetitionGroup.KCP_QuotaO.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaO", ""));
                        node["NumberQuotaO"].InnerXml = CompetitionGroup.KCP_QuotaO.ToString();
                    }
                    if (CompetitionGroup.KCP_QuotaOZ.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaOZ", ""));
                        node["NumberQuotaOZ"].InnerXml = CompetitionGroup.KCP_QuotaOZ.ToString();
                    }
                    if (CompetitionGroup.KCP_QuotaZ.HasValue)
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberQuotaZ", ""));
                        node["NumberQuotaZ"].InnerXml = CompetitionGroup.KCP_QuotaZ.ToString();
                    }

                    if ((CompetitionGroup.KCP_Cel_B ?? 0) > 0)
                    {
                        var TargetOrganizations_RAW =
                            (from CelComp in context.CelCompetitionInEntry
                             join Ent in context.Entry on CelComp.EntryId equals Ent.Id
                             join CompGroups in context.qEntryToCompetitiveGroup on Ent.Id equals CompGroups.EntryId
                             where CompGroups.CompetitiveGroupId == CompetitionGroup.Id
                             select new
                             {
                                 TargetOrganizationUID = CelComp.CelCompetitionId,
                                 TargetOrganizationName = CelComp.CelCompetition.Name,
                                 Ent.StudyFormId,
                                 CelComp.KCP
                             }).Distinct().ToList();

                        if (TargetOrganizations_RAW.Count > 0)
                        {
                            var TargetOrganizationsList = TargetOrganizations_RAW.Select(x => new
                            {
                                x.TargetOrganizationUID,
                                x.TargetOrganizationName,
                                KCP_O = TargetOrganizations_RAW.Where(y => y.TargetOrganizationUID == x.TargetOrganizationUID && y.StudyFormId == 1).Select(y => y.KCP).DefaultIfEmpty(0).Sum(),
                                KCP_OZ = TargetOrganizations_RAW.Where(y => y.TargetOrganizationUID == x.TargetOrganizationUID && y.StudyFormId == 2).Select(y => y.KCP).DefaultIfEmpty(0).Sum(),
                                KCP_Z = TargetOrganizations_RAW.Where(y => y.TargetOrganizationUID == x.TargetOrganizationUID && y.StudyFormId == 3).Select(y => y.KCP).DefaultIfEmpty(0).Sum(),
                            }).Distinct()
                            .ToList();

                            CompGroupNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "TargetOrganizations", ""));

                            foreach (var TargetOrganization in TargetOrganizationsList)
                            {
                                //добавляем сведения о целевом наборе (необяз)
                                CompGroupNode["TargetOrganizations"].AppendChild(doc.CreateNode(XmlNodeType.Element, "TargetOrganization", ""));

                                var TargetOrgNode = CompGroupNode["TargetOrganizations"].LastChild;

                                TargetOrgNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                TargetOrgNode["UID"].InnerXml = string.Format("{0}_{1}", DateTime.Now.Year, TargetOrganization.TargetOrganizationUID);
                                TargetOrgNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "TargetOrganizationName", ""));
                                TargetOrgNode["TargetOrganizationName"].InnerXml = TargetOrganization.TargetOrganizationName;

                                TargetOrgNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "Items", ""));

                                //направления подготовки целевого приёма
                                TargetOrgNode["Items"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupTargetItem", ""));

                                var t_node = TargetOrgNode["Items"].LastChild;

                                t_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                t_node["UID"].InnerXml = CompetitionGroup.Id.ToString();
                                t_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationLevelID", ""));
                                t_node["EducationLevelID"].InnerXml = sEducationLevelID;
                                if (TargetOrganization.KCP_O > 0)
                                {
                                    t_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetO", ""));
                                    t_node["NumberTargetO"].InnerXml = TargetOrganization.KCP_O.ToString();
                                }
                                if (TargetOrganization.KCP_OZ > 0)
                                {
                                    t_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetOZ", ""));
                                    t_node["NumberTargetOZ"].InnerXml = TargetOrganization.KCP_OZ.ToString();
                                }
                                if (TargetOrganization.KCP_Z > 0)
                                {
                                    t_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetZ", ""));
                                    t_node["NumberTargetZ"].InnerXml = TargetOrganization.KCP_Z.ToString();
                                }
                                //id направления подготовки из справочника №10
                                t_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DirectionID", ""));
                                t_node["DirectionID"].InnerXml = sDirectionID;
                            }
                        }
                    }

                    var EntryExams = (from ExInEnt in context.extExamInEntry
                                      join qComp in context.qEntryToCompetitiveGroup on ExInEnt.EntryId equals qComp.EntryId
                                      join Ent in context.extEntry on ExInEnt.EntryId equals Ent.Id
                                      where qComp.CompetitiveGroupId == CompetitionGroup.Id
                                      && ExInEnt.ParentExamInEntryId == null
                                      select new
                                      {
                                          UID = ExInEnt.ExamInEntryToCompetitiveGroupId,
                                          TestType = ExInEnt.IsAdditional ? 2 : /*ExInEnt.IsProfil ? 3 :*/ 1,
                                          MinScore = ExInEnt.EgeMin,
                                          SubjectName = ExInEnt.ExamName,
                                      }).Where(x => x.MinScore.HasValue && x.MinScore > 0)
                                      .Distinct()
                                      .GroupBy(x => new { x.UID, x.TestType, x.SubjectName })
                                      .Select(x => new { x.Key.UID, x.Key.SubjectName, x.Key.TestType, MinScore = x.Select(z => z.MinScore).Min() })
                                      .Distinct().ToList();

                    if (CompetitionGroup.StudyLevelId == 16 || CompetitionGroup.StudyLevelId == 18)
                    {
                        var BenList = Benefits.Where(x => x.CompetitiveGroupId == CompetitionGroup.Id).ToList();
                        if (BenList.Count > 0)
                        {
                            //добавляем "условия предоставления общей льготы" (б/э)
                            root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CommonBenefit", ""));

                            root.LastChild["CommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CommonBenefitItem", ""));
                            root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                            root.LastChild["CommonBenefit"].LastChild["UID"].InnerXml = CompetitionGroup.Id.ToString();

                            root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitKindID", ""));
                            root.LastChild["CommonBenefit"].LastChild["BenefitKindID"].InnerXml = "1";

                            List<string> lstOlympics = new List<string>();
                            Dictionary<string, string> dicMinEgeMarks = new Dictionary<string, string>();
                            foreach (var BenefitItem in BenList.Select(x => new { x.OlympName, x.OlympSubjectName }).Distinct())
                            {
                                //высчитываем олимпиады
                                for (int year = DateTime.Now.Year - 4; year <= DateTime.Now.Year; year++)
                                {
                                    string olId = SearchInDictionary(dic19_Olympics, BenefitItem.OlympName, year.ToString(), BenefitItem.OlympSubjectName);
                                    if (!string.IsNullOrEmpty(olId))
                                        lstOlympics.Add(olId);
                                }

                                //и минимальные баллы
                                var lstExams = BenList.Where(x => x.OlympName == BenefitItem.OlympName && x.OlympSubjectName == BenefitItem.OlympSubjectName)
                                    .Select(x => SearchInDictionary(dic01_Subject, x.ExamName)).Distinct();
                                foreach (string EntryExam in lstExams)
                                {
                                    if (!string.IsNullOrEmpty(EntryExam))
                                    {
                                        if (!dicMinEgeMarks.ContainsKey(EntryExam))
                                            dicMinEgeMarks.Add(EntryExam, "65");
                                    }
                                }
                            }

                            root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "IsForAllOlympics", ""));
                            root.LastChild["CommonBenefit"].LastChild["IsForAllOlympics"].InnerXml = lstOlympics.Count == 0 ? "true" : "false";

                            //типы дипломов олимпиад
                            root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDiplomTypes", ""));
                            foreach (int OlympValueId in BenList.Select(x => x.OlympValue).Distinct())
                            {
                                root.LastChild["CommonBenefit"].LastChild["OlympicDiplomTypes"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDiplomTypeID", ""));
                                root.LastChild["CommonBenefit"].LastChild["OlympicDiplomTypes"].LastChild.InnerXml = OlympValueId.ToString();
                            }

                            if (lstOlympics.Count > 0)
                            {
                                //перечень олимпиад, для которых действует льгота
                                root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Olympics", ""));
                                foreach (string olId in lstOlympics)
                                {
                                    //заполняем перечень олимпиад, для которых действует льгота
                                    //id олимпиады (справочник №20 "Названия олимпиад")
                                    root.LastChild["CommonBenefit"].LastChild["Olympics"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicID", ""));
                                    root.LastChild["CommonBenefit"].LastChild["Olympics"].LastChild.InnerXml = olId;
                                }
                            }

                            if (dicMinEgeMarks.Count > 0)
                            {
                                root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "MinEgeMarks", ""));
                                foreach (var kvp in dicMinEgeMarks)
                                {
                                    root.LastChild["CommonBenefit"].LastChild["MinEgeMarks"].AppendChild(doc.CreateNode(XmlNodeType.Element, "MinMarks", ""));
                                    root.LastChild["CommonBenefit"].LastChild["MinEgeMarks"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectID", ""));
                                    root.LastChild["CommonBenefit"].LastChild["MinEgeMarks"].LastChild["SubjectID"].InnerXml = kvp.Key;
                                    root.LastChild["CommonBenefit"].LastChild["MinEgeMarks"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "MinMark", ""));
                                    root.LastChild["CommonBenefit"].LastChild["MinEgeMarks"].LastChild["MinMark"].InnerXml = kvp.Value;
                                }
                            }
                        }
                    }

                    if (EntryExams.Count() > 0)
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestItems", ""));

                    foreach (var EntryExam in EntryExams)
                    {
                        if (!EntryExam.UID.HasValue)
                        {
                            MessageBox.Show("нет EntranceTestItem UID!");
                            return;
                        }
                        //добавляем вступительные испытания для конкурсной группы
                        root.LastChild["EntranceTestItems"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestItem", ""));

                        root.LastChild["EntranceTestItems"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                        root.LastChild["EntranceTestItems"].LastChild["UID"].InnerXml = EntryExam.UID.ToString();
                        //вид вступительного испытания (справочник №11 "Тип вступительных испытаний")
                        root.LastChild["EntranceTestItems"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestTypeID", ""));
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestTypeID"].InnerXml = EntryExam.TestType.ToString();
                        //форма вступительного испытания
                        root.LastChild["EntranceTestItems"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Form", ""));
                        root.LastChild["EntranceTestItems"].LastChild["Form"].InnerXml = EntryExam.TestType == 2 ? "Творческий конкурс" : (CompetitionGroup.QualificationCode == "68" ? "Вступительные испытания в магистратуру" : "Зачёт результатов ЕГЭ");
                        root.LastChild["EntranceTestItems"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "MinScore", ""));
                        root.LastChild["EntranceTestItems"].LastChild["MinScore"].InnerXml = CompetitionGroup.QualificationCode == "68" ? "1" : (EntryExam.MinScore ?? 1).ToString();

                        //название вступительного испытания
                        root.LastChild["EntranceTestItems"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestSubject", ""));

                        string subject_name = EntryExam.SubjectName;
                        List<int> lst = new List<int>() { 16, 18, 8, 10 };

                        if (lst.Contains(CompetitionGroup.StudyLevelId) && dic01_Subject.ContainsValue(subject_name))
                        {
                            root.LastChild["EntranceTestItems"].LastChild["EntranceTestSubject"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectID", ""));
                            root.LastChild["EntranceTestItems"].LastChild["EntranceTestSubject"]["SubjectID"].InnerXml = SearchInDictionary(dic01_Subject, EntryExam.SubjectName);
                        }
                        else
                        {
                            root.LastChild["EntranceTestItems"].LastChild["EntranceTestSubject"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectName", ""));
                            root.LastChild["EntranceTestItems"].LastChild["EntranceTestSubject"]["SubjectName"].InnerXml = EntryExam.SubjectName;
                        }

                        var lstBenefits =
                            (from mrk in context.qMark
                             join exinent in context.extExamInEntry on mrk.ExamInEntryId equals exinent.Id
                             join olympiads in context.extOlympiadsAll on mrk.OlympiadId equals olympiads.Id
                             where exinent.ExamInEntryToCompetitiveGroupId == EntryExam.UID && exinent.StudyLevelGroupId == 1
                             select new { olympiads.Number, olympiads.OlympLevelName, olympiads.OlympNameId, olympiads.OlympSubjectId, olympiads.OlympName, olympiads.OlympTypeId, olympiads.OlympValueId }).Distinct().ToList();

                        if (lstBenefits.Count > 0)
                        {
                            //условия предоставления льгот
                            root.LastChild["EntranceTestItems"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestBenefits", ""));
                            //у нас льгота лишь одна: 100 баллов за ЕГЭ (№3 в справочнике №30)
                            //foreach (var Benefit in lstBenefits)
                            {
                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestBenefitItem", ""));
                                //заносим условие предоставления льгот
                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                //UID
                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["UID"].InnerXml = EntryExam.UID.ToString() + "_3";

                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDiplomTypes", ""));
                                foreach (var xxx in lstBenefits.Select(x => x.OlympValueId == 7 ? "1" : "2").Distinct().ToList())
                                {
                                    //заполняем дипломы (по справочнику №18 "Тип диплома")
                                    root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["OlympicDiplomTypes"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDiplomTypeID", ""));
                                    root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["OlympicDiplomTypes"].LastChild.InnerXml = xxx;
                                }

                                //Вид льготы (справочник №30)
                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitKindID", ""));
                                //у нас льгота лишь одна: 100 баллов за ЕГЭ (3 в справочнике №30)
                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["BenefitKindID"].InnerXml = "3";

                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "IsForAllOlympics", ""));
                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["IsForAllOlympics"].InnerXml = "true";
                                //root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Olympics", ""));
                                ////заполняем олимпиады (по справочнику №20 "Олимпиады")
                                //root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["Olympics"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicID", ""));

                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "MinEgeMark", ""));
                                root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["MinEgeMark"].InnerXml = "65";
                            }
                        }
                    }
                }
                wc.Close();
                doc.Save(fname);

                MessageBox.Show("OK");
            }
        }
        //заполняет только раздел о заявлениях
        private void ExportXML_Part3(string fname, bool isCrimea)
        {
            XmlDocument doc = new XmlDocument();
            //XmlImplementation imp = new XmlImplementation();

            UpdateDictionaries_Local();
            List<int?> BeneficiaryCompetitions = new List<int?>() { 1, 2, 7, 8 };

            using (PriemEntities context = new PriemEntities())
            {
                DateTime dt = DateTime.Now;
                TimeSpan dt1, dt2, dt3, dt4_allAbitList;
                TimeSpan dt4_EgeMarks = TimeSpan.MinValue, dt4_Marks = TimeSpan.MinValue;

                //создаём корневой элемент
                doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "Root", ""));

                //создаём элементы AuthData и PackageData
                XmlNode root = doc["Root"];

                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "AuthData", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "PackageData", ""));

                //заполняем данные AuthData
                root = root["AuthData"];
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Login", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Pass", ""));

                bool isDefault = true;
                root["Login"].InnerText = isDefault ? "p.karpenko@spbu.ru" : tbLogin.Text;
                root["Pass"].InnerText = isDefault ? "E0k02II" : tbPassword.Text;

                //----------------------------------------------------------------------------------------------------------
                //заполняем данные PackageData
                root = doc["Root"];
                root["PackageData"].AppendChild(doc.CreateNode(XmlNodeType.Element, "Applications", ""));

                NewWatch wc = new NewWatch();
                
                wc.Show();
                //--------------------------------------------------------------------------------------------------------------
                //заполняем данные Applications
                var apps = (from abit in context.qAbiturient
                            join person in context.extPerson on abit.PersonId equals person.Id
                            join CurrEduc in context.extPerson_EducationInfo_Current on person.Id equals CurrEduc.PersonId
                            join compGroup in context.qEntryToCompetitiveGroup on abit.EntryId equals compGroup.EntryId
                            //join extCompGroup in context.extCompetitiveGroup on compGroup.CompetitiveGroupId equals extCompGroup.Id
                            join Nationality in context.Country on person.NationalityId equals Nationality.Id
                            join ForNat in context.ForeignCountry on person.ForeignNationalityId equals ForNat.Id into ForNat2
                            from ForNat in ForNat2.DefaultIfEmpty()
                            where (FacultyId.HasValue ? abit.FacultyId == FacultyId.Value : true)
                            && (abit.StudyLevelGroupId == StudyLevelGroupId)
                            &&  !abit.IsForeign && abit.IsCrimea == isCrimea
                            && abit.CompetitionId == 6 //&& !abit.BackDoc && !abit.NotEnabled
                            select new
                            {
                                AppUID = abit.Id,
                                PersonId = abit.PersonId,
                                ApplicationNumber = abit.RegNum,
                                abit.CompetitionId,
                                RegistrationDate = abit.DocInsertDate,
                                LastDenyDate = abit.BackDocDate,
                                NeedHostel = person.HostelEduc,
                                EntrantUID = abit.PersonId,
                                EntrantSurname = person.Surname,
                                EntrantName = person.Name,
                                EntrantMiddleName = person.SecondName,
                                abit.StudyBasisFISName,
                                abit.StudyFormFISName,
                                AddInfo = person.ExtraInfo,
                                EgeDocOrigin = person.HasOriginals,
                                person.PassportSeries,
                                person.PassportNumber,
                                person.PassportAuthor,
                                PassportDate = person.PassportDate,
                                HasOriginals = person.HasOriginals,
                                NationalityFISName = Nationality.FISName != null ? 
                                    Nationality.FISName : 
                                    context.ForeignCountry.Where(x => x.Id == person.ForeignNationalityId).Select(x => x.Name).FirstOrDefault(),
                                PassportTypeFISName = person.PassportTypeFISName,
                                person.BirthDate,
                                person.BirthPlace,
                                SchoolTypeId = CurrEduc.SchoolTypeId,
                                person.Sex,
                                EducDocSeries = CurrEduc.SchoolTypeId == 1 ? CurrEduc.AttestatSeries : CurrEduc.DiplomSeries,
                                EducDocNum = CurrEduc.SchoolTypeId == 1 ? CurrEduc.AttestatNum : CurrEduc.DiplomNum,
                                compGroup.CompetitiveGroupId,
                                compGroup.CompetitiveGroupName,
                                NotEnabled = abit.NotEnabled,
                                abit.BackDoc,
                                TargetOrganizationUID = abit.CelCompetitionId,
                            }).ToList();

                var otherPassports = context.PersonOtherPassport.Where(x => x.PassportTypeId == 1).Select(x => new { x.Id, x.PersonId, x.PassportSeries, x.PassportNumber, x.PassportDate }).ToList();

                root = doc["Root"]["PackageData"]["Applications"];

                int wcMax = apps.Count;
                wc.SetText(fname + " - Заполняем данные Applications");
                wc.SetMax(wcMax);
                wc.ZeroCount();


                dt4_allAbitList = DateTime.Now - dt;

                dt = DateTime.Now;
                foreach (var app in apps)
                {
                    wc.PerformStep();

                    //if (app.NationalityFISName.IndexOf("лицо без гражданства", StringComparison.CurrentCultureIgnoreCase) > -1)
                    //    continue;

                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Application", ""));

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    root.LastChild["UID"].InnerText = app.AppUID.ToString();

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationNumber", ""));
                    root.LastChild["ApplicationNumber"].InnerText = string.Format("{0}_{1}", DateTime.Now.Year, app.ApplicationNumber.ToString());

                    //данные о человеке
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Entrant", ""));
                    //дата регистрации заявления в ИС
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "RegistrationDate", ""));
                    root.LastChild["RegistrationDate"].InnerText = app.RegistrationDate.ToString("yyyy-MM-ddTHH:mm:ss");

                    //статус заявления (справочник №4)
                    //для начала следует определить статус заявления:
                    //В приказе - есть в extEntryView и OrderNumbers
                    //Новое - ещё не в протоколе о допуске
                    //Отклонено == "Не допущен"
                    //Отозвано == "Забрал документы"
                    //Принято == внесён в протокол о допуске
                    //Редактируется == в настоящий момент IsOpen (бессмысленно)
                    //Рекомендованное к зачислению - есть в extEntryView и ещё нет в OrderNumbers
                    string AppStatus = "";
                    /*
                    if (app.NotEnabled)
                        AppStatus = "Отклонено";
                    else if (app.BackDoc)
                        AppStatus = "Отозвано";
                    else if (context.extEntryView.Where(x => x.AbiturientId == app.AppUID && (x.OrderNum != null || x.OrderNumFor != null)).Count() > 0)
                        AppStatus = "В приказе";
                    else if (context.extEntryView.Where(x => x.AbiturientId == app.AppUID && x.OrderNum == null && x.OrderNumFor == null).Count() > 0)
                        AppStatus = "Рекомендованное к зачислению";
                    else if (context.extEnableProtocol.Where(x => x.AbiturientId == app.AppUID && x.Excluded == false).Count() > 0)
                        AppStatus = "Принято";
                    else
                        AppStatus = "Новое";
                     */
                    if (app.BackDoc || app.NotEnabled)
                        AppStatus = "Отозвано";
                    else
                        AppStatus = "Принято";

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "StatusID", ""));
                    string sStatusID = SearchInDictionary(dic04_ApplicationStatus, AppStatus);
                    root.LastChild["StatusID"].InnerXml = sStatusID;

                    if (sStatusID == "6")
                    {
                        //дата отзыва заявления (если была)
                        if (app.LastDenyDate.HasValue)
                        {
                            DateTime LastDenyDate = app.LastDenyDate.Value < app.RegistrationDate ? app.RegistrationDate.AddHours(1) : app.LastDenyDate.Value;
                            root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "LastDenyDate", ""));
                            root.LastChild["LastDenyDate"].InnerText = LastDenyDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        else
                        {
                            DateTime LastDenyDate = app.RegistrationDate.AddHours(1);
                            root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "LastDenyDate", ""));
                            root.LastChild["LastDenyDate"].InnerText = LastDenyDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                    }

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NeedHostel", ""));
                    root.LastChild["NeedHostel"].InnerText = app.NeedHostel.ToString().ToLower();

                    //конкурсные группы для заявления
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SelectedCompetitiveGroups", ""));
                    root.LastChild["SelectedCompetitiveGroups"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                    root.LastChild["SelectedCompetitiveGroups"]["CompetitiveGroupID"].InnerXml = app.CompetitiveGroupId.ToString();

                    //элементы конкурсных групп для заявления
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SelectedCompetitiveGroupItems", ""));
                    root.LastChild["SelectedCompetitiveGroupItems"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupItemID", ""));
                    root.LastChild["SelectedCompetitiveGroupItems"]["CompetitiveGroupItemID"].InnerXml = app.CompetitiveGroupId.ToString();

                    //формы обучения и источники финансирования выбранные абитуриентом
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "FinSourceAndEduForms", ""));
                    //общая льгота, предоставленная абитуриенту (необяз)
                    //root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationCommonBenefit", ""));

                    //док-ты, приложенные к заявлению
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationDocuments", ""));

                    //заполняем данные об абитуриенте
                    root.LastChild["Entrant"].AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    root.LastChild["Entrant"]["UID"].InnerText = app.EntrantUID.ToString();
                    //Имя
                    root.LastChild["Entrant"].AppendChild(doc.CreateNode(XmlNodeType.Element, "FirstName", ""));
                    root.LastChild["Entrant"]["FirstName"].InnerText = app.EntrantName;
                    //Отчество
                    root.LastChild["Entrant"].AppendChild(doc.CreateNode(XmlNodeType.Element, "MiddleName", ""));
                    root.LastChild["Entrant"]["MiddleName"].InnerText = app.EntrantMiddleName;
                    //Фамилия
                    root.LastChild["Entrant"].AppendChild(doc.CreateNode(XmlNodeType.Element, "LastName", ""));
                    root.LastChild["Entrant"]["LastName"].InnerText = app.EntrantSurname;
                    //Пол (справочник №5)
                    root.LastChild["Entrant"].AppendChild(doc.CreateNode(XmlNodeType.Element, "GenderID", ""));
                    root.LastChild["Entrant"]["GenderID"].InnerXml = SearchInDictionary(dic05_Sex, (app.Sex ? "Мужской" : "Женский"));

                    //доп. сведения, представленные абитуриентом
                    root.LastChild["Entrant"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CustomInformation", ""));
                    root.LastChild["Entrant"]["CustomInformation"].InnerText = app.AddInfo;

                    //заполняем формы обучения и источники финансирования выбранные абитуриентом (FinSourceAndEduForms)
                    root.LastChild["FinSourceAndEduForms"].AppendChild(doc.CreateNode(XmlNodeType.Element, "FinSourceEduForm", ""));

                    //id источника финансирования (справочник №15 "Источники финансирования")
                    root.LastChild["FinSourceAndEduForms"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "FinanceSourceID", ""));
                    root.LastChild["FinSourceAndEduForms"].LastChild["FinanceSourceID"].InnerXml = SearchInDictionary(dic15_FinSource, app.StudyBasisFISName);
                    //id формы обучения (справочник №14 "Формы обучения")
                    root.LastChild["FinSourceAndEduForms"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationFormID", ""));
                    root.LastChild["FinSourceAndEduForms"].LastChild["EducationFormID"].InnerXml = SearchInDictionary(dic14_EducationForm, app.StudyFormFISName);
                    
                    ////UID организации
                    root.LastChild["FinSourceAndEduForms"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "TargetOrganizationUID", ""));
                    root.LastChild["FinSourceAndEduForms"].LastChild["TargetOrganizationUID"].InnerXml = string.Format("{0}_{1}", DateTime.Now.Year, app.TargetOrganizationUID);
                    
                    List<int?> BEs = new List<int?>() { 1, 8 };//сведения о б/э
                    
                    List<int?> regards = new List<int?>() { 5, 6, 7 };//победител/призёр
                    List<int?> OlympTypeList = new List<int?>() { 1, 2, 3, 4 };
                    //добавляем сведения о льготе, предоставленной абитуриенту
                    if (BEs.Contains(app.CompetitionId) && context.extOlympiadsAll.Where(x => x.AbiturientId == app.AppUID && OlympTypeList.Contains(x.OlympTypeId) && regards.Contains(x.OlympValueId)).Count() > 0)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationCommonBenefit", ""));
                        root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                        root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = app.AppUID.ToString();
                        root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                        root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = app.CompetitiveGroupId.ToString();
                        //id типа документа-основания (Справочник №31 - "Тип документа") - необяз
                        //root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentTypeID", ""));
                        
                        //id вида льготы (Справочник №30 - "Вид льготы")
                        root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitKindID", ""));
                        root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = "1";

                        //вводим документ-основание (если есть) - один из четырёх
                        var OlympDocs = context.extOlympiadsAll.Where(x => x.AbiturientId == app.AppUID && regards.Contains(x.OlympValueId) && OlympTypeList.Contains(x.OlympTypeId));
                        if (OlympDocs.Count() > 0)
                        {
                            if (OlympDocs.Where(x => x.OlympTypeId == 2).Count() > 0)
                            {
                                //документ-основание - необяз
                                root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentReason", ""));
                                var Ol = OlympDocs.Where(x => x.OlympTypeId == 2).OrderBy(x => x.OlympValueSortOrder).FirstOrDefault();
                                if (Ol != null)
                                {
                                    string OlSubjId = SearchInDictionary(dic01_Subject, Ol.OlympSubjectName);
                                    if (!string.IsNullOrEmpty(OlSubjId))
                                    {
                                        var node = root.LastChild["ApplicationCommonBenefit"]["DocumentReason"];

                                        //диплом победителя/призёра всероссийской олимпиады школьников
                                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicTotalDocument", ""));
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                        node.LastChild["UID"].InnerXml = Ol.Id.ToString();
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                                        node.LastChild["OriginalReceived"].InnerXml = app.HasOriginals ?? false ? "true" : "false";

                                        if (app.HasOriginals ?? false)
                                        {
                                            node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                                            node.LastChild["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                                        }

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                                        node.LastChild["DocumentSeries"].InnerXml = string.IsNullOrEmpty(Ol.DocumentSeries) ? "-" : Ol.DocumentSeries;
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                                        node.LastChild["DocumentNumber"].InnerXml = string.IsNullOrEmpty(Ol.DocumentNumber) ? "---" : Ol.DocumentNumber;

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DiplomaTypeID", ""));
                                        node.LastChild["DiplomaTypeID"].InnerXml = Ol.OlympValueId == 7 ? "1" : "2";
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Subjects", ""));
                                        node.LastChild["Subjects"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectBriefData", ""));
                                        node.LastChild["Subjects"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectID", ""));
                                        node.LastChild["Subjects"].LastChild["SubjectID"].InnerXml = OlSubjId;
                                    }
                                }
                            }
                            else if (OlympDocs.Where(x => x.OlympTypeId != 2).Count() > 0)
                            {
                                //документ-основание - необяз
                                root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentReason", ""));

                                //диплом победителя/призёра олимпиады школьников
                                var Ol = OlympDocs.Where(x => x.OlympTypeId != 2).OrderBy(x => x.OlympValueSortOrder).FirstOrDefault();
                                if (Ol != null)
                                {
                                    string Subject = Ol.OlympSubjectName;
                                    string Year = Ol.OlympYear.ToString();
                                    string Name = Ol.OlympName;

                                    string OlympicID = SearchInDictionary(dic19_Olympics, Name, Year, Subject);
                                    if (!string.IsNullOrEmpty(OlympicID))
                                    {
                                        //диплом победителя/призёра всероссийской олимпиады школьников
                                        var node = root.LastChild["ApplicationCommonBenefit"]["DocumentReason"];
                                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDocument", ""));
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                        node.LastChild["UID"].InnerXml = Ol.Id.ToString();
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                                        node.LastChild["OriginalReceived"].InnerXml = app.HasOriginals ?? false ? "true" : "false";

                                        if (app.HasOriginals ?? false)
                                        {
                                            node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                                            node.LastChild["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                                        }

                                        if (!string.IsNullOrEmpty(Ol.DocumentSeries))
                                        {
                                            node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                                            node.LastChild["DocumentSeries"].InnerXml = Ol.DocumentSeries;
                                        }

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                                        node.LastChild["DocumentNumber"].InnerXml = string.IsNullOrEmpty(Ol.DocumentNumber) ? "---" : Ol.DocumentNumber;

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DiplomaTypeID", ""));
                                        node.LastChild["DiplomaTypeID"].InnerXml = Ol.OlympValueId == 7 ? "1" : "2";
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicID", ""));
                                        string OlNumber = (Ol.Number ?? 44).ToString();
                                        node.LastChild["OlympicID"].InnerXml = OlympicID;
                                    }
                                }
                            }
                        }
                    }

                    List<int?> VKs = new List<int?>() { 2, 7 };//сведения о в/к
                    if (VKs.Contains(app.CompetitionId))
                    {
                        var MedDocs = context.PersonBenefitDocument.Where(x => x.PersonId == app.PersonId && x.BenefitDocumentTypeId < 4)//только основные типы документов
                            .Select(x => new { x.Id, x.BenefitDocumentType.Name, x.BenefitDocumentTypeId, x.Series, x.Number, x.Date, x.DisabilityTypeId }).ToList();

                        if (MedDocs.Count > 0)
                        {
                            root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationCommonBenefit", ""));
                            root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                            root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = app.AppUID.ToString();
                            root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                            root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = app.CompetitiveGroupId.ToString();
                            //id типа документа-основания (Справочник №31 - "Тип документа") - необяз
                            //root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentTypeID", ""));

                            //id вида льготы (Справочник №30 - "Вид льготы")
                            root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitKindID", ""));
                            root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = "4";

                            root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentReason", ""));
                            var node = root.LastChild["ApplicationCommonBenefit"]["DocumentReason"];
                            //медицинские документы
                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "MedicalDocuments", ""));
                            var par_node = node["MedicalDocuments"];
                            foreach (var Doc in MedDocs)
                            {
                                switch (Doc.BenefitDocumentTypeId)
                                {
                                    //1	Справка об установлении инвалидности
                                    case 1:
                                        {
                                            if (par_node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "BenefitDocument").Count() == 0)
                                                par_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitDocument", ""));
                                            
                                            if (par_node["BenefitDocument"].ChildNodes.Cast<XmlNode>().Where(x => x.Name == "DisabilityDocument").Count() == 1
                                                || par_node["BenefitDocument"].ChildNodes.Cast<XmlNode>().Where(x => x.Name == "MedicalDocument").Count() == 1)
                                                continue;

                                            par_node["BenefitDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DisabilityDocument", ""));
                                            node = par_node["BenefitDocument"]["DisabilityDocument"];
                                            break;
                                        }
                                    //2	Заключение психолого-медико-педагогической комиссии
                                    case 2:
                                        {
                                            if (par_node.ChildNodes.Cast<XmlNode>().Where(x => x.Name == "BenefitDocument").Count() == 0)
                                                par_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitDocument", ""));
                                            
                                            if (par_node["BenefitDocument"].ChildNodes.Cast<XmlNode>().Where(x => x.Name == "DisabilityDocument").Count() == 1
                                                || par_node["BenefitDocument"].ChildNodes.Cast<XmlNode>().Where(x => x.Name == "MedicalDocument").Count() == 1)
                                                continue;
                                            
                                            par_node["BenefitDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "MedicalDocument", ""));
                                            node = par_node["BenefitDocument"]["MedicalDocument"];
                                            break;
                                        }
                                    //3 Заключение об отсутствии противопоказаний для обучения
                                    case 3:
                                        {
                                            par_node.AppendChild(doc.CreateNode(XmlNodeType.Element, "AllowEducationDocument", ""));
                                            node = par_node["AllowEducationDocument"];
                                            break;
                                        }
                                }

                                node.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                node["UID"].InnerXml = Doc.Id.ToString();
                                node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                                node["OriginalReceived"].InnerXml = app.HasOriginals ?? false ? "true" : "false";

                                if (app.HasOriginals ?? false)
                                {
                                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                                    node["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                                }

                                if (Doc.BenefitDocumentTypeId != 3)
                                {
                                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                                    node["DocumentSeries"].InnerXml = string.IsNullOrEmpty(Doc.Series) ? "-" : Doc.Series;
                                }

                                node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                                node["DocumentNumber"].InnerXml = string.IsNullOrEmpty(Doc.Number) ? "---" : Doc.Number;

                                if (Doc.Date.HasValue)
                                {
                                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                                    node["DocumentDate"].InnerXml = Doc.Date.Value.ToString("yyyy-MM-dd");
                                }

                                if (Doc.BenefitDocumentTypeId == 1)
                                {
                                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DisabilityTypeID", ""));
                                    node["DisabilityTypeID"].InnerXml = (Doc.DisabilityTypeId ?? 1).ToString();
                                }
                            }
                        }
                        else
                        {
                            var OtherDocs = context.PersonBenefitDocument.Where(x => x.PersonId == app.PersonId && x.BenefitDocumentTypeId == 4)//только доп
                                .Select(x => new { x.Id, x.BenefitDocumentType.Name, x.Series, x.Number, x.Date }).ToList();

                            if (OtherDocs.Count > 0)
                            {
                                root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationCommonBenefit", ""));
                                root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = app.AppUID.ToString();
                                root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                                root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = app.CompetitiveGroupId.ToString();

                                //id вида льготы (Справочник №30 - "Вид льготы")
                                root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitKindID", ""));
                                root.LastChild["ApplicationCommonBenefit"].LastChild.InnerXml = "4";

                                root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentReason", ""));
                                var node = root.LastChild["ApplicationCommonBenefit"]["DocumentReason"];
                                //медицинские документы
                                node.AppendChild(doc.CreateNode(XmlNodeType.Element, "CustomDocument", ""));
                                var Doc = OtherDocs.First();

                                node = node["CustomDocument"];

                                node.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                node["UID"].InnerXml = Doc.Id.ToString();
                                node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                                node["OriginalReceived"].InnerXml = app.HasOriginals ?? false ? "true" : "false";

                                if (app.HasOriginals ?? false)
                                {
                                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                                    node["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                                }

                                node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentTypeNameText", ""));
                                node["DocumentTypeNameText"].InnerXml = "Справка";

                                if (!string.IsNullOrEmpty(Doc.Series))
                                {
                                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                                    node["DocumentSeries"].InnerXml = string.IsNullOrEmpty(Doc.Series) ? "-" : Doc.Series;
                                }

                                node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                                node["DocumentNumber"].InnerXml = string.IsNullOrEmpty(Doc.Number) ? "---" : Doc.Number;

                                if (Doc.Date.HasValue)
                                {
                                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                                    node["DocumentDate"].InnerXml = Doc.Date.Value.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                    }
                    

                    /*
                    //основание для льготы по медицинским показаниям - ПОКА ЧТО В БАЗЕ НЕТ
                    //root.LastChild["ApplicationCommonBenefit"]["DocumentReason"].AppendChild(doc.CreateNode(XmlNodeType.Element, "MedicalDocument", ""));
                    //прочее - ПОКА ЧТО В БАЗЕ НЕТ
                    //root.LastChild["ApplicationCommonBenefit"]["DocumentReason"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CustomDocument", ""));
                    */

                    dt = DateTime.Now;

                    if (app.CompetitionId != 1 && app.CompetitionId != 8)
                    {
                        //вводим данные об оценках на вступительных испытаниях
                        var abitMarks =
                            (from qM in context.qMark
                             join exVed in context.extExamsVed on qM.ExamVedId equals exVed.Id into exVed2
                             from exVed in exVed2.DefaultIfEmpty()
                             where qM.AbiturientId == app.AppUID
                             select new
                             {
                                 qM.Id,
                                 qM.IsFromEge,
                                 qM.IsFromOlymp,
                                 qM.IsManual,
                                 qM.EgeCertificateId,
                                 qM.ExamId,
                                 qM.ExamName,
                                 qM.EntranceTestType,
                                 qM.ExamVedId,
                                 qM.OlympiadId,
                                 qM.PassDate,
                                 qM.Value,
                                 VedNum = (int?)exVed.Number,
                                 VedDate = (DateTime?)exVed.Date
                             }).ToList();

                        //рез-ты вступительных испытаний
                        if (abitMarks.Count() > 0)
                            root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestResults", ""));
                        //List<string> badValues = new List<string>() { "2014", "2015" };
                        foreach (var mrk in abitMarks)
                        {
                            root.LastChild["EntranceTestResults"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestResult", ""));

                            root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                            root.LastChild["EntranceTestResults"].LastChild["UID"].InnerXml = mrk.Id.ToString();
                            root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultValue", ""));

                            if (mrk.IsFromOlymp)
                            {
                                List<int> lstEgeExamNameIds = context.EgeToExam.Where(x => x.ExamId == mrk.ExamId).Select(x => x.EgeExamNameId).ToList();
                                int EgeVal = context.hlpEgeMarkMaxApprovedValue
                                    .Where(x => x.PersonId == app.PersonId && lstEgeExamNameIds.Contains(x.EgeExamNameId))
                                    .Select(x => x.EgeMarkValue)
                                    .DefaultIfEmpty(100)
                                    .Max(x => x ?? 0);

                                root.LastChild["EntranceTestResults"].LastChild["ResultValue"].InnerXml = EgeVal.ToString();
                            }
                            else
                            {
                                root.LastChild["EntranceTestResults"].LastChild["ResultValue"].InnerXml = mrk.Value.ToString();
                            }
                            //Тип основания для оценки (документа-основания)

                            string ResultSourceTypeName = "Вступительное испытание ОО";
                            if (mrk.IsFromEge) /*||*/
                                ResultSourceTypeName = "Свидетельство ЕГЭ";
                            else if (mrk.IsFromOlymp)
                                ResultSourceTypeName = "Диплом победителя/призера олимпиады";
                            else if (mrk.IsManual)
                                ResultSourceTypeName = "Вступительное испытание ОО";

                            root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultSourceTypeID", ""));
                            root.LastChild["EntranceTestResults"].LastChild["ResultSourceTypeID"].InnerXml = SearchInDictionary(dic06_MarkDocument, ResultSourceTypeName);

                            //вносим предмет
                            root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestSubject", ""));
                            if ((StudyLevelGroupId == 1 || StudyLevelGroupId == 3) && dic01_Subject.ContainsValue(mrk.ExamName))
                            {
                                root.LastChild["EntranceTestResults"].LastChild["EntranceTestSubject"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectID", ""));
                                root.LastChild["EntranceTestResults"].LastChild["EntranceTestSubject"]["SubjectID"].InnerXml = SearchInDictionary(dic01_Subject, mrk.ExamName);
                            }
                            else
                            {
                                root.LastChild["EntranceTestResults"].LastChild["EntranceTestSubject"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectName", ""));
                                root.LastChild["EntranceTestResults"].LastChild["EntranceTestSubject"]["SubjectName"].InnerXml = mrk.ExamName;
                            }

                            //ИД типа конкурсного испытания (справочник №11)
                            root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestTypeID", ""));
                            root.LastChild["EntranceTestResults"].LastChild["EntranceTestTypeID"].InnerXml = SearchInDictionary(dic11_EntranceTestType, mrk.EntranceTestType);
                            //ИД конкурсной группы
                            root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                            root.LastChild["EntranceTestResults"].LastChild["CompetitiveGroupID"].InnerXml = app.CompetitiveGroupId.ToString();

                            if (mrk.IsFromEge)
                            {
                                var EgeCert = context.EgeCertificate.Where(x => x.Id == mrk.EgeCertificateId).FirstOrDefault();
                                root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultDocument", ""));
                                //id свидетельства о рез-тах ЕГЭ, которое было приложено к заявлению
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EgeDocumentID", ""));
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.InnerXml = mrk.EgeCertificateId.ToString();
                            }
                            else if (mrk.IsManual)
                            {
                                root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultDocument", ""));
                                //самостоятельное испытание (ведомость ручного ввода от какой-то даты)
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "InstitutionDocument", ""));
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild["DocumentNumber"].InnerXml = "Ведомость ручного ввода от " + mrk.PassDate.Value.ToShortDateString();
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild["DocumentDate"].InnerXml = mrk.PassDate.Value.ToString("yyyy-MM-dd");
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentTypeID", ""));
                                root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild["DocumentTypeID"].InnerXml = "1";
                            }
                            else if (mrk.IsFromOlymp)
                            {
                                var olymp = context.extOlympiadsAll.Where(x => x.Id == mrk.OlympiadId && OlympTypeList.Contains(x.OlympTypeId)).FirstOrDefault();
                                if (olymp != null)
                                {
                                    root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultDocument", ""));
                                    var node = root.LastChild["EntranceTestResults"].LastChild["ResultDocument"];

                                    if (olymp.OlympTypeId == 2)//vseross
                                    {
                                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicTotalDocument", ""));
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                        node.LastChild["UID"].InnerXml = olymp.Id.ToString();
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                                        node.LastChild["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();

                                        if (app.HasOriginals ?? false)
                                        {
                                            node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                                            node.LastChild["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                                        }

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                                        string DocSeries = olymp.DocumentSeries;
                                        if (string.IsNullOrEmpty(DocSeries))
                                            DocSeries = "нет";
                                        if (DocSeries.Length > 4)
                                            DocSeries = DocSeries.Substring(0, 4);

                                        node.LastChild["DocumentSeries"].InnerXml = DocSeries;

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                                        string DocNum = olymp.DocumentNumber;
                                        if (string.IsNullOrEmpty(DocNum))
                                            DocNum = "не указан";
                                        node.LastChild["DocumentNumber"].InnerXml = DocNum;

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DiplomaTypeID", ""));
                                        node.LastChild["DiplomaTypeID"].InnerXml = olymp.OlympValueId == 6 ? "1" : "2";

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Subjects", ""));
                                        node.LastChild["Subjects"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectBriefData", ""));
                                        node.LastChild["Subjects"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectID", ""));
                                        node.LastChild["Subjects"].LastChild["SubjectID"].InnerXml = SearchInDictionary(dic01_Subject, olymp.OlympSubjectName);

                                    }
                                    else //all other
                                    {
                                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDocument", ""));
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                                        node.LastChild["UID"].InnerXml = olymp.Id.ToString();
                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                                        node.LastChild["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();
                                        
                                        if (app.HasOriginals ?? false)
                                        {
                                            node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                                            node.LastChild["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                                        }

                                        if (!string.IsNullOrEmpty(olymp.DocumentSeries))
                                        {
                                            string DocSeries = olymp.DocumentSeries;
                                            if (DocSeries.Length > 4)
                                                DocSeries = DocSeries.Substring(0, 4);

                                            node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                                            node.LastChild["DocumentSeries"].InnerXml = DocSeries;
                                        }

                                        string DocNum = olymp.DocumentNumber;
                                        if (string.IsNullOrEmpty(DocNum))
                                            DocNum = "не указан";

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                                        node.LastChild["DocumentNumber"].InnerXml = DocNum;

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DiplomaTypeID", ""));
                                        node.LastChild["DiplomaTypeID"].InnerXml = olymp.OlympValueId == 6 ? "1" : "2";

                                        node.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicID", ""));
                                        
                                        string OlympName = olymp.OlympName;
                                        string Year = olymp.OlympYear.ToString();
                                        string Subject = olymp.OlympSubjectName;

                                        node.LastChild["OlympicID"].InnerXml = SearchInDictionary(dic19_Olympics, OlympName, Year, Subject);
                                    }
                                }
                            }
                            else
                            {
                                if (mrk.ExamVedId.HasValue)
                                {

                                    root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultDocument", ""));
                                    //самостоятельное испытание (ведомость вступительного испытания)
                                    root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "InstitutionDocument", ""));

                                    root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                                    root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild["DocumentNumber"].InnerXml = "Ведомость №" + mrk.VedNum;

                                    root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                                    root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild["DocumentDate"].InnerXml = mrk.VedDate.Value.ToString("yyyy-MM-dd");

                                    root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentTypeID", ""));
                                    root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild["DocumentTypeID"].InnerXml = "1";
                                }
                            }
                        }
                    }

                    dt4_Marks += DateTime.Now - dt;

                    //заносим документы, приложенные к заявлению

                    //док-т удостоверяющий личность
                    root.LastChild["ApplicationDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "IdentityDocument", ""));
                    //док-т об образовании
                    root.LastChild["ApplicationDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EduDocuments", ""));
                    //военный билет - необяз
                    //root.LastChild["ApplicationDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "MilitaryCardDocument", ""));
                    //иные документы - необяз
                    //root.LastChild["ApplicationDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CustomDocuments", ""));
                    //справка ГИА - необяз
                    //root.LastChild["ApplicationDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "GiaDocuments", ""));

                    dt = DateTime.Now;
                    
                    var egeDocs = context.extEgeMark
                        .Where(x => x.PersonId == app.EntrantUID /*&& !badValues.Contains(x.Year)*/ && (x.FBSStatusId == 1 || x.FBSStatusId == 4) && x.Year != "" && x.Year != null
                            && x.FISName != null)
                        .Select(x => new { x.Id, x.EgeCertificateId, x.Number, x.Year, x.FISName, x.Value });
                    //Свидетельства о результатах ЕГЭ - необяз
                    if (egeDocs.Count() > 0)
                        root.LastChild["ApplicationDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EgeDocuments", ""));

                    foreach (var cert in egeDocs.Select(x => new { x.EgeCertificateId, x.Year, x.Number }).Distinct())
                    {
                        //Заполняем данные о сертификатах
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EgeDocument", ""));

                        var node = root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild;

                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                        node["UID"].InnerXml = cert.EgeCertificateId.ToString();
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                        node["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();

                        if (app.HasOriginals ?? false)
                        {
                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                            node["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                        }

                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                        node["DocumentNumber"].InnerXml = cert.Number;
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentYear", ""));
                        node["DocumentYear"].InnerXml = cert.Year;
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Subjects", ""));

                        var exams = egeDocs.Where(x => x.EgeCertificateId == cert.EgeCertificateId).Select(x => new { x.FISName, x.Value });
                        foreach (var exam in exams)
                        {
                            //заполняем оценки в сертификате ЕГЭ
                            node["Subjects"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectData", ""));

                            var subjNode = node["Subjects"].LastChild;

                            //id дисциплины (справочник №1 - "Общеобразовательные предметы")
                            subjNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectID", ""));
                            subjNode["SubjectID"].InnerXml = SearchInDictionary(dic01_Subject, exam.FISName);
                            subjNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "Value", ""));
                            subjNode["Value"].InnerXml = exam.Value.ToString();
                        }
                    }

                    dt4_EgeMarks += DateTime.Now - dt;

                    //заполняем данные о док-те, удостоверяющем личность
                    //UID необяз
                    //root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    //root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                    //root.LastChild["ApplicationDocuments"]["IdentityDocument"]["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();

                    //if (app.HasOriginals ?? false)
                    //{
                    //    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                    //    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                    //}

                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["DocumentSeries"].InnerXml = string.IsNullOrEmpty(app.PassportSeries) ? "-" : app.PassportSeries;
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["DocumentNumber"].InnerXml = app.PassportNumber;
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["DocumentDate"].InnerXml = app.PassportDate.HasValue ? app.PassportDate.Value.ToString("yyyy-MM-dd") : "";
                    //кем выдан - необяз
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentOrganization", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["DocumentOrganization"].InnerXml = (app.PassportAuthor ?? "").Replace("&", " AND ");
                    //ID типа документа, удостовер личность (Справочник №22)
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "IdentityDocumentTypeID", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["IdentityDocumentTypeID"].InnerXml = SearchInDictionary(dic22_IdentityDocumentType, app.PassportTypeFISName);
                    //Список стран (справочник №7)
                    string Nationality = app.NationalityFISName;
                    if (Nationality == "лицо без гражданства")
                        Nationality = "Не определено";

                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "NationalityTypeID", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["NationalityTypeID"].InnerXml = SearchInDictionary(dic07_Country, Nationality);
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "BirthDate", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["BirthDate"].InnerXml = app.BirthDate.ToString("yyyy-MM-dd");
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "BirthPlace", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["BirthPlace"].InnerXml = app.BirthPlace;

                    var otherPassList = otherPassports.Where(x => x.PersonId == app.PersonId).ToList();
                    if (otherPassList.Count > 0)
                    {
                        root.LastChild["ApplicationDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OtherIdentityDocuments", ""));
                        foreach (var OPP in otherPassList)
                        {
                            root.LastChild["ApplicationDocuments"]["OtherIdentityDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "IdentityDocument", ""));
                            var node = root.LastChild["ApplicationDocuments"]["OtherIdentityDocuments"].LastChild;
                            //заполняем данные о док-те, удостоверяющем личность
                            //UID необяз
                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                            node["UID"].InnerXml = string.Format("{0}_{1}", DateTime.Now.Year, OPP.Id);
                            //node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                            //node["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();

                            //if (app.HasOriginals ?? false)
                            //{
                            //    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                            //    node["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                            //}

                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                            node["DocumentSeries"].InnerXml = string.IsNullOrEmpty(OPP.PassportSeries) ? "-" : OPP.PassportSeries;
                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                            node["DocumentNumber"].InnerXml = OPP.PassportNumber;
                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                            node["DocumentDate"].InnerXml = OPP.PassportDate.HasValue ? OPP.PassportDate.Value.ToString("yyyy-MM-dd") : "";
                            //ID типа документа, удостовер личность (Справочник №22)
                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "IdentityDocumentTypeID", ""));
                            node["IdentityDocumentTypeID"].InnerXml = "1";

                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "NationalityTypeID", ""));
                            node["NationalityTypeID"].InnerXml = "1";

                            node.AppendChild(doc.CreateNode(XmlNodeType.Element, "BirthDate", ""));
                            node["BirthDate"].InnerXml = app.BirthDate.ToString("yyyy-MM-dd");
                            //node.AppendChild(doc.CreateNode(XmlNodeType.Element, "BirthPlace", ""));
                            //node["BirthPlace"].InnerXml = app.BirthPlace;
                        }
                    }

                    //вносим документы об образовании
                    root.LastChild["ApplicationDocuments"]["EduDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EduDocument", ""));
                    //на выбор - один из
                    //все их объединяет три поля - Серия, Номер, Оригинал(да/нет)
                    XmlNode rootChild = root;
                    switch (app.SchoolTypeId)
                    {
                        case 1://Школа
                            {
                                //SchoolCertificateDocument - аттестат за 11 класс
                                root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SchoolCertificateDocument", ""));
                                rootChild = root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild["SchoolCertificateDocument"];
                                break;
                            }
                        case 2://ССУЗ
                            {
                                //MiddleEduDiplomaDocument - диплом СПО
                                root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "MiddleEduDiplomaDocument", ""));
                                rootChild = root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild["MiddleEduDiplomaDocument"];
                                break;
                            }
                        case 3://НПО
                            {
                                //BasicDiplomaDocument - диплом НПО
                                root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "BasicDiplomaDocument", ""));
                                rootChild = root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild["BasicDiplomaDocument"];
                                break;
                            }
                        case 4://ВУЗ
                            {
                                //HighEduDiplomaDocument - диплом ВПО
                                root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "HighEduDiplomaDocument", ""));
                                rootChild = root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild["HighEduDiplomaDocument"];
                                break;
                            }
                        case 5://СПО
                            {
                                //MiddleEduDiplomaDocument - диплом СПО
                                root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "MiddleEduDiplomaDocument", ""));
                                rootChild = root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild["MiddleEduDiplomaDocument"];
                                break;
                            }
                        ////IncomplHighEduDiplomaDocument - диплом о неполном ВПО
                        //root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "IncomplHighEduDiplomaDocument", ""));
                        ////AcademicDiplomaDocument - академ. справка
                        //root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "AcademicDiplomaDocument", ""));
                        ////SchoolCertificateBasicDocument - аттестат за 9 класс
                        //root.LastChild["ApplicationDocuments"]["EduDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SchoolCertificateBasicDocument", ""));
                    }

                    rootChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                    string series = app.EducDocSeries;
                    rootChild["DocumentSeries"].InnerXml = string.IsNullOrEmpty(series) ? "-" : series;
                    rootChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                    rootChild["DocumentNumber"].InnerXml = string.IsNullOrEmpty(app.EducDocNum) ? "-" : app.EducDocNum;
                    rootChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                    rootChild["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();

                    if (app.HasOriginals ?? false)
                    {
                        rootChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                        rootChild["OriginalReceivedDate"].InnerXml = app.RegistrationDate.ToString("yyyy-MM-dd");
                    }

                    ////вносим прочие документы
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CustomDocument", ""));

                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                    ////организация, выдавшая документ
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentOrganization", ""));
                    ////Доп. сведения
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "AdditionalInfo", ""));
                    ////тип документа (справочник №31 "Тип документа")
                    //root.LastChild["ApplicationDocuments"]["CustomDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentTypeNameText", ""));

                    ////вносим справки ГИА
                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "GiaDocument", ""));

                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentOrganization", ""));
                    ////предметы
                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Subjects", ""));

                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild["Subjects"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectData", ""));

                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild["Subjects"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectID", ""));
                    //root.LastChild["ApplicationDocuments"]["GiaDocuments"].LastChild["Subjects"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Value", ""));
                }

                wc.Close();
                doc.Save(fname);


                if (!chbFullImport.Checked)
                    MessageBox.Show("OK");
            }
        }

        //удаляет заявления
        //private void ExportXML_DataForDelete_Applications(string fname)
        //{
        //    XmlDocument doc = new XmlDocument();

        //    UpdateDictionaries_Local();
        //    List<int?> BeneficiaryCompetitions = new List<int?>() { 1, 2, 7, 8 };

        //    using (PriemEntities context = new PriemEntities())
        //    {
        //        DateTime dt = DateTime.Now;
        //        TimeSpan dt4_EgeMarks = TimeSpan.MinValue, dt4_Marks = TimeSpan.MinValue;

        //        //создаём корневой элемент
        //        doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "Root", ""));

        //        //создаём элементы AuthData и PackageData
        //        XmlNode root = doc["Root"];

        //        root.AppendChild(doc.CreateNode(XmlNodeType.Element, "AuthData", ""));
        //        root.AppendChild(doc.CreateNode(XmlNodeType.Element, "DataForDelete", ""));

        //        //заполняем данные AuthData
        //        root = root["AuthData"];
        //        root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Login", ""));
        //        root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Pass", ""));

        //        bool isDefault = true;
        //        root["Login"].InnerText = isDefault ? "p.karpenko@spbu.ru" : tbLogin.Text;
        //        root["Pass"].InnerText = isDefault ? "E0k02II" : tbPassword.Text;

        //        //----------------------------------------------------------------------------------------------------------
        //        //заполняем данные PackageData
        //        root = doc["Root"];
        //        root["DataForDelete"].AppendChild(doc.CreateNode(XmlNodeType.Element, "Applications", ""));

        //        NewWatch wc = new NewWatch();

        //        wc.Show();
        //        //--------------------------------------------------------------------------------------------------------------
        //        //заполняем данные Applications
        //        var apps = (from abit in context.qAbiturient
        //                    join compGroup in context.qEntryToCompetitiveGroup on abit.EntryId equals compGroup.EntryId
        //                    join extCompGroup in context.extCompetitiveGroup on compGroup.CompetitiveGroupId equals extCompGroup.Id
        //                    join SL in context.StudyLevel on abit.StudyLevelId equals SL.Id
        //                    where (FacultyId.HasValue ? abit.FacultyId == FacultyId.Value : true)
        //                    && (LicenseProgramId.HasValue ? abit.LicenseProgramId == LicenseProgramId.Value : true)
        //                    && (abit.StudyLevelGroupId == StudyLevelGroupId)
        //                    select new
        //                    {
        //                        ApplicationNumber = abit.RegNum,
        //                        RegistrationDate = abit.DocInsertDate,
        //                    });

        //        root = doc["Root"]["DataForDelete"]["Applications"];

        //        int wcMax = apps.Count();
        //        wc.SetText(fname + " - Заполняем данные DataForDelete - Applications");
        //        wc.SetMax(wcMax);
        //        wc.ZeroCount();

        //        dt = DateTime.Now;
        //        foreach (var app in apps)
        //        {
        //            wc.PerformStep();
        //            root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Application", ""));

        //            //Номер заявления
        //            root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationNumber", ""));
        //            root.LastChild["ApplicationNumber"].InnerText = DateTime.Now.Year.ToString() + "_" + app.ApplicationNumber.ToString();
                    
        //            //Дата регистрации заявления
        //            root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "RegistrationDate", ""));
        //            root.LastChild["RegistrationDate"].InnerText = app.RegistrationDate.ToString("yyyy-MM-ddTHH:mm:ss");
        //        }
      
        //        wc.Close();
        //        doc.Save(fname);

        //        if (!chbFullImport.Checked)
        //            MessageBox.Show("OK");
        //    }
        //}

        //удаляет приказы
        private void DeleteOrders(string fname)
        {
            XmlDocument doc = new XmlDocument();

            UpdateDictionaries_Local();

            using (PriemEntities context = new PriemEntities())
            {
                DateTime dt = DateTime.Now;
                TimeSpan dt1, dt2, dt3, dt4_allAbitList;
                TimeSpan dt4_EgeMarks = TimeSpan.MinValue, dt4_Marks = TimeSpan.MinValue;

                //создаём корневой элемент
                doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "Root", ""));

                //создаём элементы AuthData и PackageData
                XmlNode root = doc["Root"];

                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "AuthData", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "DataForDelete", ""));

                //заполняем данные AuthData
                root = root["AuthData"];
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Login", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Pass", ""));

                bool isDefault = true;
                root["Login"].InnerText = isDefault ? "p.karpenko@spbu.ru" : tbLogin.Text;
                root["Pass"].InnerText = isDefault ? "E0k02II" : tbPassword.Text;

                //----------------------------------------------------------------------------------------------------------
                //заполняем данные PackageData
                root = doc["Root"];
                root["DataForDelete"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OrdersOfAdmission", ""));

                NewWatch wc = new NewWatch();

                wc.Show();
                //--------------------------------------------------------------------------------------------------------------
                var apps = (from abit in context.qAbiturient
                            join person in context.Person on abit.PersonId equals person.Id
                            join extEv in context.extEntryView on abit.Id equals extEv.AbiturientId
                            join compGroup in context.qEntryToCompetitiveGroup on abit.EntryId equals compGroup.EntryId
                            join extCompGroup in context.extCompetitiveGroup on compGroup.CompetitiveGroupId equals extCompGroup.Id
                            join SL in context.StudyLevel on abit.StudyLevelId equals SL.Id
                            where SL.LevelGroupId <= 4
                            select new
                            {
                                OrderId = extEv.Id,
                                extEv.Date,
                                AppUID = abit.Id,
                                CompetitionId = abit.CompetitionId,
                                ApplicationNumber = abit.RegNum,
                                RegistrationDate = abit.DocInsertDate,
                                compGroup.CompetitiveGroupId,
                                extCompGroup.LicenseProgramCode,
                                extCompGroup.LicenseProgramName,
                                extCompGroup.QualificationCode,
                                abit.StudyLevelGroupId,
                            }).ToList();

                root = doc["Root"]["DataForDelete"]["OrdersOfAdmission"];

                int wcMax = apps.Count();
                wc.SetText(fname + " - Заполняем данные OrdersOfAdmission");
                wc.SetMax(wcMax);
                wc.ZeroCount();

                dt4_allAbitList = DateTime.Now - dt;

                dt = DateTime.Now;

                foreach (var eOrder in apps.OrderBy(x => x.QualificationCode).ThenBy(x => x.LicenseProgramCode))
                {
                    //wc.SetText(string.Format("{0} {1}", eOrder.LicenseProgramCode, eOrder.LicenseProgramName));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Application", ""));

                    var node = root.LastChild;

                    wc.PerformStep();

                    //Номер заявления
                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationNumber", ""));
                    node["ApplicationNumber"].InnerText = string.Format("{0}_{1}", DateTime.Now.Year, eOrder.ApplicationNumber);

                    //Дата регистрации заявления
                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "RegistrationDate", ""));
                    node["RegistrationDate"].InnerText = eOrder.RegistrationDate.ToString("yyyy-MM-ddTHH:mm:ss");
                }

                wc.Close();
                doc.Save(fname);

                if (!chbFullImport.Checked)
                    MessageBox.Show("OK");
            }
        }

        //заполняет только раздел о заявлениях. включенные в приказ
        private void ExportXML_Part4(string fname, bool isCrimea)
        {
            XmlDocument doc = new XmlDocument();
            //XmlImplementation imp = new XmlImplementation();

            UpdateDictionaries_Local();
            List<int?> BeneficiaryCompetitions = new List<int?>() { 2, 7 };

            using (PriemEntities context = new PriemEntities())
            {
                DateTime dt = DateTime.Now;
                TimeSpan dt1, dt2, dt3, dt4_allAbitList;
                TimeSpan dt4_EgeMarks = TimeSpan.MinValue, dt4_Marks = TimeSpan.MinValue;

                //создаём корневой элемент
                doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "Root", ""));

                //создаём элементы AuthData и PackageData
                XmlNode root = doc["Root"];

                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "AuthData", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "PackageData", ""));

                //заполняем данные AuthData
                root = root["AuthData"];
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Login", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Pass", ""));

                bool isDefault = true;
                root["Login"].InnerText = isDefault ? "p.karpenko@spbu.ru" : tbLogin.Text;
                root["Pass"].InnerText = isDefault ? "E0k02II" : tbPassword.Text;

                //----------------------------------------------------------------------------------------------------------
                //заполняем данные PackageData
                root = doc["Root"];
                root["PackageData"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OrdersOfAdmission", ""));

                NewWatch wc = new NewWatch();

                wc.Show();
                //--------------------------------------------------------------------------------------------------------------
                //заполняем данные Applications
                bool bIsOnlyZeroWave = true;

                DateTime dtZeroWaveLastTime = new DateTime(2015, 7, 31);
                var apps = (from abit in context.qAbiturient
                            join person in context.Person on abit.PersonId equals person.Id
                            join extEv in context.extEntryView on abit.Id equals extEv.AbiturientId
                            join compGroup in context.qEntryToCompetitiveGroup on abit.EntryId equals compGroup.EntryId
                            join extCompGroup in context.extCompetitiveGroup on compGroup.CompetitiveGroupId equals extCompGroup.Id
                            join SL in context.StudyLevel on abit.StudyLevelId equals SL.Id
                            where SL.LevelGroupId <= 4
                            && abit.IsCrimea == isCrimea
                            && !abit.IsForeign
                            //нулевая волна
                            && (bIsOnlyZeroWave ? extEv.Date <= dtZeroWaveLastTime : extEv.Date > dtZeroWaveLastTime)
                            select new
                            {
                                OrderId = extEv.Id,
                                extEv.OrderNum,
                                extEv.OrderNumFor,
                                extEv.OrderDate,
                                extEv.OrderDateFor,
                                extEv.Date,
                                person.NationalityId,
                                AppUID = abit.Id,
                                CompetitionId = abit.CompetitionId,
                                IsForeign = abit.IsForeign,
                                ApplicationNumber = abit.RegNum,
                                RegistrationDate = abit.DocInsertDate,
                                abit.StudyBasisFISName,
                                abit.StudyBasisId,
                                abit.StudyFormFISName,
                                StudyLevelFISName = SL.FISName + (abit.IsReduced ? " (сокращ.)" : ""),
                                compGroup.CompetitiveGroupId,
                                extCompGroup.LicenseProgramCode,
                                extCompGroup.LicenseProgramName,
                                extCompGroup.QualificationCode,
                                abit.StudyLevelGroupId,
                            }).ToList();

                root = doc["Root"]["PackageData"]["OrdersOfAdmission"];

                int wcMax = apps.Count();
                wc.SetText(fname + " - Заполняем данные OrdersOfAdmission");
                wc.SetMax(wcMax);
                wc.ZeroCount();

                dt4_allAbitList = DateTime.Now - dt;

                dt = DateTime.Now;

                foreach (var eOrder in apps.OrderBy(x => x.QualificationCode).ThenBy(x => x.LicenseProgramCode))
                {
                    wc.SetText(string.Format("{0} {1}", eOrder.LicenseProgramCode, eOrder.LicenseProgramName));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "OrderOfAdmission", ""));

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Application", ""));

                    wc.PerformStep();

                    //Номер заявления
                    root.LastChild["Application"].AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationNumber", ""));
                    root.LastChild["Application"]["ApplicationNumber"].InnerText = string.Format("{0}_{1}", DateTime.Now.Year, eOrder.ApplicationNumber);

                    //Дата регистрации заявления
                    root.LastChild["Application"].AppendChild(doc.CreateNode(XmlNodeType.Element, "RegistrationDate", ""));
                    root.LastChild["Application"]["RegistrationDate"].InnerText = eOrder.RegistrationDate.ToString("yyyy-MM-ddTHH:mm:ss");

                    if (eOrder.StudyBasisId == 1)
                    {
                        //Дата регистрации заявления
                        root.LastChild["Application"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OrderIdLevelBudget", ""));
                        root.LastChild["Application"]["OrderIdLevelBudget"].InnerText = "1";
                    }

                    //Идентификатор приказа в ИС
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OrderOfAdmissionUID", ""));
                    root.LastChild["OrderOfAdmissionUID"].InnerXml = eOrder.OrderId.ToString() + "_" + (eOrder.NationalityId == 1 ? "0" : "1");

                    if (eOrder.OrderDate.HasValue || eOrder.OrderDateFor.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OrderNumber", ""));
                        root.LastChild["OrderNumber"].InnerXml = eOrder.NationalityId == 1 ? eOrder.OrderNum : eOrder.OrderNumFor;

                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OrderDate", ""));
                        root.LastChild["OrderDate"].InnerXml = eOrder.NationalityId == 1 ? eOrder.OrderDate.Value.ToString("yyyy-MM-dd") : eOrder.OrderDateFor.Value.ToString("yyyy-MM-dd");
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OrderDatePublished", ""));
                        root.LastChild["OrderDatePublished"].InnerXml = eOrder.NationalityId == 1 ? eOrder.OrderDate.Value.ToString("yyyy-MM-dd") : eOrder.OrderDateFor.Value.ToString("yyyy-MM-dd");
                    }
                    //id направления подготовки (справочник №14 "Направления подготовки")
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DirectionID", ""));
                    root.LastChild["DirectionID"].InnerXml = SearchInDictionary(dic10_Direction, eOrder.LicenseProgramName, eOrder.LicenseProgramCode, eOrder.QualificationCode);

                    //id формы обучения (справочник №14 "Формы обучения")
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationFormID", ""));
                    root.LastChild["EducationFormID"].InnerXml = SearchInDictionary(dic14_EducationForm, eOrder.StudyFormFISName);

                    //id источника финансирования (справочник №15 "Источники финансирования")
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "FinanceSourceID", ""));
                    root.LastChild["FinanceSourceID"].InnerXml = SearchInDictionary(dic15_FinSource, eOrder.StudyBasisFISName);

                    //id Уровня образования (справочник №2 "Уровни Образования")
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationLevelID", ""));
                    root.LastChild["EducationLevelID"].InnerXml = SearchInDictionary(dic02_StudyLevel, eOrder.StudyLevelFISName);

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupUID", ""));
                    root.LastChild["CompetitiveGroupUID"].InnerXml = eOrder.CompetitiveGroupId.ToString();

                    if (BeneficiaryCompetitions.Contains(eOrder.CompetitionId))
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "IsBeneficiary", ""));
                        root.LastChild["IsBeneficiary"].InnerText = "true";
                    }

                    if (eOrder.IsForeign)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "IsForeigner", ""));
                        root.LastChild["IsForeigner"].InnerText = "true";
                    }

                    DateTime dtFirstWaveLastDate = new DateTime(2015, 8, 5);
                    DateTime dtSecondWaveLastDate = new DateTime(2015, 8, 8);

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Stage", ""));

                    if (eOrder.StudyLevelGroupId == 1 && eOrder.StudyBasisId == 1)
                    {
                        DateTime dtOrder = eOrder.Date;
                        if (dtOrder < dtFirstWaveLastDate && dtOrder > dtZeroWaveLastTime)
                            root.LastChild["Stage"].InnerText = "1";
                        else if (dtOrder > dtFirstWaveLastDate && dtOrder < dtSecondWaveLastDate)
                            root.LastChild["Stage"].InnerText = "2";
                        else
                            root.LastChild["Stage"].InnerText = "0";
                    }
                    else
                        root.LastChild["Stage"].InnerText = "0";

                }
                wc.Close();
                doc.Save(fname);

                if (!chbFullImport.Checked)
                    MessageBox.Show("OK");
            }
        }

        private void ExportXML_RecommendedList(string fname, bool isCrimea)
        {
            XmlDocument doc = new XmlDocument();
            //XmlImplementation imp = new XmlImplementation();

            UpdateDictionaries_Local();
            List<int?> BeneficiaryCompetitions = new List<int?>() { 2, 7 };

            using (PriemEntities context = new PriemEntities())
            {
                DateTime dt = DateTime.Now;
                TimeSpan dt1, dt2, dt3, dt4_allAbitList;
                TimeSpan dt4_EgeMarks = TimeSpan.MinValue, dt4_Marks = TimeSpan.MinValue;

                //создаём корневой элемент
                doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "Root", ""));

                //создаём элементы AuthData и PackageData
                XmlNode root = doc["Root"];

                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "AuthData", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "PackageData", ""));

                //заполняем данные AuthData
                root = root["AuthData"];
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Login", ""));
                root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Pass", ""));

                bool isDefault = true;
                root["Login"].InnerText = isDefault ? "p.karpenko@spbu.ru" : tbLogin.Text;
                root["Pass"].InnerText = isDefault ? "E0k02II" : tbPassword.Text;

                //----------------------------------------------------------------------------------------------------------
                //заполняем данные PackageData
                root = doc["Root"];
                root["PackageData"].AppendChild(doc.CreateNode(XmlNodeType.Element, "RecommendedLists", ""));

                NewWatch wc = new NewWatch();

                wc.Show();
                //--------------------------------------------------------------------------------------------------------------
                //заполняем данные Applications
                //bool bIsOnlyZeroWave = true;

                DateTime dtZeroWaveLastTime = new DateTime(2015, 7, 31);
                DateTime dtFirstWaveLastDate = new DateTime(2015, 8, 5);
                DateTime dtSecondWaveLastDate = new DateTime(2015, 8, 8);

                var apps = (from abit in context.qAbiturient
                            join person in context.Person on abit.PersonId equals person.Id
                            join extEv in context.extEntryView on abit.Id equals extEv.AbiturientId
                            join compGroup in context.qEntryToCompetitiveGroup on abit.EntryId equals compGroup.EntryId
                            join extCompGroup in context.extCompetitiveGroup on compGroup.CompetitiveGroupId equals extCompGroup.Id
                            join SL in context.StudyLevel on abit.StudyLevelId equals SL.Id
                            where SL.LevelGroupId <= 4
                            && abit.IsCrimea == isCrimea
                            && !abit.IsForeign
                            //нулевая волна
                            && extEv.Date > dtZeroWaveLastTime
                            && extEv.StudyLevelGroupId == 1 && extEv.StudyBasisId == 1
                            select new
                            {
                                extEv.OrderNum,
                                extEv.OrderNumFor,
                                extEv.OrderDate,
                                extEv.OrderDateFor,
                                extEv.Date,
                                AppUID = abit.Id,
                                CompetitionId = abit.CompetitionId,
                                ApplicationNumber = abit.RegNum,
                                RegistrationDate = abit.DocInsertDate,
                                abit.StudyBasisFISName,
                                abit.StudyBasisId,
                                abit.StudyFormFISName,
                                StudyLevelFISName = SL.FISName + (abit.IsReduced ? " (сокращ.)" : ""),
                                compGroup.CompetitiveGroupId,
                                extCompGroup.LicenseProgramCode,
                                extCompGroup.LicenseProgramName,
                                extCompGroup.QualificationCode,
                                abit.StudyLevelGroupId,
                            }).ToList()
                            .Select(x => new {
                                x.ApplicationNumber,
                                x.AppUID,
                                x.CompetitiveGroupId,
                                x.Date,
                                x.LicenseProgramCode,
                                x.LicenseProgramName,
                                x.QualificationCode,
                                x.RegistrationDate,
                                x.StudyBasisId,
                                x.StudyLevelGroupId,
                                x.StudyBasisFISName,
                                x.StudyFormFISName,
                                x.StudyLevelFISName,
                                Stage = 
                                (
                                    (x.Date < dtFirstWaveLastDate && x.Date > dtZeroWaveLastTime) ?
                                        (1) : (x.Date > dtFirstWaveLastDate && x.Date < dtSecondWaveLastDate ? 2 : 0)
                                )
                            }).ToList();

                root = doc["Root"]["PackageData"]["RecommendedLists"];

                int wcMax = apps.Count();
                wc.SetText(fname + " - Заполняем данные RecommendedLists");
                wc.SetMax(wcMax);
                wc.ZeroCount();

                dt4_allAbitList = DateTime.Now - dt;

                dt = DateTime.Now;
                foreach (var eStage in apps.Select(x => x.Stage).Distinct())
                {
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "RecommendedList", ""));
                    var node = root.LastChild;
                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "Stage", ""));
                    node["Stage"].InnerXml = eStage.ToString();
                    node.AppendChild(doc.CreateNode(XmlNodeType.Element, "RecLists", ""));

                    node = node["RecLists"];

                    foreach (var eOrder in apps.Where(x => x.Stage == eStage).OrderBy(x => x.LicenseProgramCode))
                    {
                        node.AppendChild(doc.CreateNode(XmlNodeType.Element, "RecList", ""));
                        var InnerNode = node.LastChild;
                        wc.SetText(string.Format("{0} {1}", eOrder.LicenseProgramCode, eOrder.LicenseProgramName));

                        InnerNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "Application", ""));

                        wc.PerformStep();

                        //Номер заявления
                        InnerNode["Application"].AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationNumber", ""));
                        InnerNode["Application"]["ApplicationNumber"].InnerText = string.Format("{0}_{1}", DateTime.Now.Year, eOrder.ApplicationNumber);

                        //Дата регистрации заявления
                        InnerNode["Application"].AppendChild(doc.CreateNode(XmlNodeType.Element, "RegistrationDate", ""));
                        InnerNode["Application"]["RegistrationDate"].InnerText = eOrder.RegistrationDate.ToString("yyyy-MM-ddTHH:mm:ss");

                        InnerNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "FinSourceAndEduForms", ""));
                        InnerNode["FinSourceAndEduForms"].AppendChild(doc.CreateNode(XmlNodeType.Element, "FinSourceEduForm", ""));

                        InnerNode = InnerNode["FinSourceAndEduForms"]["FinSourceEduForm"];
                        //id направления подготовки (справочник №14 "Направления подготовки")
                        InnerNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "DirectionID", ""));
                        InnerNode["DirectionID"].InnerXml = SearchInDictionary(dic10_Direction, eOrder.LicenseProgramName, eOrder.LicenseProgramCode, eOrder.QualificationCode);

                        //id формы обучения (справочник №14 "Формы обучения")
                        InnerNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationFormID", ""));
                        InnerNode["EducationFormID"].InnerXml = SearchInDictionary(dic14_EducationForm, eOrder.StudyFormFISName);

                        //id Уровня образования (справочник №2 "Уровни Образования")
                        InnerNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationalLevelID", ""));
                        InnerNode["EducationalLevelID"].InnerXml = SearchInDictionary(dic02_StudyLevel, eOrder.StudyLevelFISName);

                        InnerNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                        InnerNode["CompetitiveGroupID"].InnerXml = eOrder.CompetitiveGroupId.ToString();
                    }
                }
                wc.Close();
                doc.Save(fname);

                if (!chbFullImport.Checked)
                    MessageBox.Show("OK");
            }
        }

        private void UpdateGrid()
        {
            using (PriemEntities context = new PriemEntities())
            {
                var src = from c in context.extCompetitiveGroup
                          where FacultyId.HasValue ? c.FacultyId == FacultyId : true
                          && LicenseProgramId.HasValue ? c.LicenseProgramId == LicenseProgramId : true
                          select new
                          {
                              c.Name,
                              c.KCP_OB,
                              c.KCP_OP,
                              c.KCP_OZB,
                              c.KCP_OZP
                          };
                dgvCompGroup.DataSource = src.Distinct();
            }
        }
        private void UpdateDictionaries_Local()
        {
            //обновляем по очереди словари
            XmlDocument result_doc = new XmlDocument();
            List<int> list_codes = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 10, 11, 12, 13, 14, 15, 17, 18, 19, 22, 23, 30, 31, 33, 34, 35, 36 };
            foreach (int code in list_codes)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(System.Windows.Forms.Application.StartupPath + "/dics/dic" + code + ".xml"))
                {
                    result_doc.LoadXml(sr.ReadToEnd());
                }

                UpdateDictionary(code, ref result_doc);
            }
        }
        private void UpdateDictionaries_Web()
        {
            //обновляем по очереди словари
            XmlDocument query_doc = new XmlDocument();
            XmlDocument result_doc = new XmlDocument();
            query_doc.AppendChild(query_doc.CreateNode(XmlNodeType.Element, "Root", ""));
            query_doc["Root"].AppendChild(query_doc.CreateNode(XmlNodeType.Element, "AuthData", ""));
            query_doc["Root"].AppendChild(query_doc.CreateNode(XmlNodeType.Element, "GetDictionaryContent", ""));

            bool isDefault = true;

            query_doc["Root"]["AuthData"].AppendChild(query_doc.CreateNode(XmlNodeType.Element, "Login", ""));
            query_doc["Root"]["AuthData"]["Login"].InnerXml = isDefault ? "p.karpenko@spbu.ru" : tbLogin.Text;
            query_doc["Root"]["AuthData"].AppendChild(query_doc.CreateNode(XmlNodeType.Element, "Pass", ""));
            query_doc["Root"]["AuthData"]["Pass"].InnerXml = isDefault ? "E0k02II" : tbPassword.Text;

            query_doc["Root"]["GetDictionaryContent"].AppendChild(query_doc.CreateNode(XmlNodeType.Element, "DictionaryCode", ""));
            XmlNode DictionaryCodeNode = query_doc["Root"]["GetDictionaryContent"]["DictionaryCode"];

            List<int> list_codes = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 10, 11, 12, 13, 14, 15, 17, 18, 19, 22, 23, 30, 31, 33, 34, 35, 36 };

            foreach (int code in list_codes)
            {
                //result_doc.InnerXml = "";
                DictionaryCodeNode.InnerXml = code.ToString();
                string q_str = query_doc.InnerXml;
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                client.Headers["Content-Type"] = "text/xml";
                try
                {
                    string address = tbServiceAddress.Text;
                    if (string.IsNullOrEmpty(address))
                        address = "http://10.0.3.1:8080/import/importservice.svc";

                    if (System.Environment.UserName != "v.chikhira")
                    {
                        result_doc.LoadXml(client.UploadString(address + "/dictionarydetails", q_str));
                        result_doc.Save(System.Windows.Forms.Application.StartupPath + "/dics/dic" + code + ".xml");
                    }
                    else
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(System.Windows.Forms.Application.StartupPath + "/dics/dic" + code + ".xml"))
                        {
                            result_doc.LoadXml(sr.ReadToEnd());
                        }
                    }

                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                UpdateDictionary(code, ref result_doc);
            }
        }
        private void UpdateDictionary(int dicCode, ref XmlDocument xmlData)
        {
            if (string.IsNullOrEmpty(xmlData.InnerXml))
                return;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (dicCode != 10 && dicCode != 19)
            {
                try
                {
                    foreach (XmlNode node in xmlData["DictionaryData"]["DictionaryItems"].ChildNodes)
                        dic.Add(node["ID"].InnerXml, node["Name"].InnerXml);
                }
                catch (Exception)
                {
                    return;
                }
            }
            else if (dicCode == 19)
            {
                try
                {
                    foreach (XmlNode node in xmlData["DictionaryData"]["DictionaryItems"].ChildNodes)
                    {
                        dic19_Olympics.Add(new DictionaryOlympiad()
                        {
                            Id = node["OlympicID"].InnerXml,
                            Year = node["Year"].InnerXml,
                            Name = node["OlympicName"].InnerXml,
                        });
                    }
                        //dic.Add(node["ID"].InnerXml, node["Name"].InnerXml);
                }
                catch (Exception)
                {
                    return;
                }
            }
            else
            {
                try
                {
                    foreach (XmlNode node in xmlData["DictionaryData"]["DictionaryItems"].ChildNodes)
                        dic10_Direction.Add(new Dictionary10Direction() 
                        { 
                            Id = node["ID"].InnerXml, 
                            Name = node["Name"].InnerXml,
                            Code = node["NewCode"] == null ? node["Code"].InnerXml : node["NewCode"].InnerXml,
                            QualificationCode = node["QualificationCode"].InnerXml 
                        });
                }
                catch (Exception)
                {
                    return;
                }
            }

            switch (dicCode)
            {
                case 1: { dic01_Subject = dic; break; }
                case 2: { dic02_StudyLevel = dic; break; }
                case 3: { dic03_OlympLevel = dic; break; }
                case 4: { dic04_ApplicationStatus = dic; break; }
                case 5: { dic05_Sex = dic; break; }
                case 6: { dic06_MarkDocument = dic; break; }
                case 7: { dic07_Country = dic; break; }
                case 11: { dic11_EntranceTestType = dic; break; }
                case 12: { dic12_ApplicationCheckStatus = dic; break; }
                case 13: { dic13_DocumentCheckStatus = dic; break; }
                case 14: { dic14_EducationForm = dic; break; }
                case 15: { dic15_FinSource = dic; break; }
                case 17: { dic17_Errors = dic; break; }
                case 18: { dic18_DiplomaType = dic; break; }
                //case 19: { dic19_Olympics = dic; break; }
                case 22: { dic22_IdentityDocumentType = dic; break; }
                case 23: { dic23_DisabilityType = dic; break; }
                case 30: { dic30_BenefitKind = dic; break; }
                case 31: { dic31_DocumentType = dic; break; }
                case 33: { dic33_ = dic; break; }
                case 34: { dic34_CampaignStatus = dic; break; }
            }
        }
        
        private string SearchInDictionary(Dictionary<string, string> dic, string pattern)
        {
            var res = dic.Where(x => x.Value.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) == 0).Select(x => x.Key);
            if (res.Count() == 0)
                return null;
            else
                return res.First();
        }

        private string SearchInDictionary(List<DictionaryOlympiad> dic, string name, string year, string subject)
        {
            var res = dic.Where(x => x.Name.ToUpper() == name.Replace("ё", "е").ToUpper() && x.Year == year
                //&& (x.Subject == null || x.Subject.ToUpper() == subject.ToUpper())
                ).Select(x => x.Id);
            if (res.Count() == 0)
                return null;
            else
                return res.First();
        }

        private string SearchInDictionary(List<Dictionary10Direction> dic, string name, string code, string qual_code)
        {
            var res = dic.Where(x => x.Name.ToUpper() == name.Replace("ё", "е").ToUpper() && x.Code == code 
                && x.QualificationCode.ToUpper() == qual_code.ToUpper()).Select(x => x.Id);
            if (res.Count() == 0)
                return null;
            else
                return res.First();
        }

        private void cbStudyLevelGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillComboFaculty();
            //FillComboLicenseProgram();
        }
        private void btnExportXML_part1_Click(object sender, EventArgs e)
        {
            ExportXML_Part1(chbIsCrimea.Checked);
        }
        private void btnExportXML_part3_Click(object sender, EventArgs e)
        {
            if (!chbFullImport.Checked)
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "XML File|*.xml";
                if (sf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                ExportXML_Part3(sf.FileName, chbIsCrimea.Checked);
            }
            else
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                string folderPath = dlg.SelectedPath + "\\";
                using (PriemEntities context = new PriemEntities())
                {
                    var facs = context.extEntry.Where(x => x.StudyLevelGroupId == StudyLevelGroupId).Select(x => new { Id = x.FacultyId, Acronym = x.FacultyAcr })
                        .Distinct().ToList().Select(x => new { x.Acronym, x.Id }).ToList();
                    foreach (var x in facs)
                    {
                        FacultyId = x.Id;
                        ExportXML_Part3(folderPath + (x.Id < 10 ? "0" : "") + x.Id.ToString() + "_" + x.Acronym + ".xml", chbIsCrimea.Checked);
                    }
                    MessageBox.Show("OK");
                }
            }
        }

        private void btnUpdateExanInCompetitiveGroup_Click(object sender, EventArgs e)
        {
            using (PriemEntities ctx = new PriemEntities())
            {
                var ExamInEntryEntities = ctx.ExamInEntry.Where(x => x.ExamInEntryToCompetitiveGroupId == null);
                NewWatch wc = new NewWatch(ExamInEntryEntities.Count() + 1);
                wc.Show();
                try
                {
                    foreach (var E in ExamInEntryEntities)
                    {
                        using (PriemEntities context = new PriemEntities())
                        {
                            Guid EntryID = E.EntryId;
                            Guid CompGroupId = context.qEntryToCompetitiveGroup.Where(x => x.EntryId == EntryID).Select(z => z.CompetitiveGroupId).DefaultIfEmpty(Guid.Empty).FirstOrDefault();
                            if (CompGroupId == Guid.Empty)
                                continue;

                            var EntryList = context.qEntryToCompetitiveGroup.Where(x => x.CompetitiveGroupId == CompGroupId).Select(x => x.EntryId);
                            Guid? gExamInEntryToCompetitiveGroupId = context.ExamInEntry.Where(x => x.ExamId == E.ExamId && EntryList.Contains(x.EntryId) && x.ExamInEntryToCompetitiveGroupId != null).Select(x => x.ExamInEntryToCompetitiveGroupId).FirstOrDefault();
                            if (!gExamInEntryToCompetitiveGroupId.HasValue || gExamInEntryToCompetitiveGroupId.Value == Guid.Empty)
                                gExamInEntryToCompetitiveGroupId = Guid.NewGuid();

                            context.ExamInEntry_UpdateExamInCompetitiveGroupId(gExamInEntryToCompetitiveGroupId, E.Id);
                            context.SaveChanges();
                            //System.Threading.Thread.Sleep(100);
                            wc.PerformStep();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    wc.Close();
                    MessageBox.Show("Done!");
                }
            }
        }

        private void btnImportOrdersOfAdmission_Click(object sender, EventArgs e)
        {
            //if (!chbFullImport.Checked)
            //{
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "XML File|*.xml";
            if (sf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            ExportXML_Part4(sf.FileName, chbIsCrimea.Checked);
            
        }
        private void btnDeleteApplications_Click(object sender, EventArgs e)
        {
            if (!chbFullImport.Checked)
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "XML File|*.xml";
                if (sf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                DeleteOrders(sf.FileName);
                //ExportXML_DataForDelete_Applications(sf.FileName);
            }
            else
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                string folderPath = dlg.SelectedPath + "\\";
                using (PriemEntities context = new PriemEntities())
                {
                    var facs = context.SP_Faculty.Select(x => new { x.Acronym, x.Id });
                    foreach (var x in facs)
                    {
                        FacultyId = x.Id;
                        DeleteOrders(folderPath + (x.Id < 10 ? "0" : "") + x.Id.ToString() + "_" + x.Acronym + ".xml");
                        //ExportXML_DataForDelete_Applications(folderPath + (x.Id < 10 ? "0" : "") + x.Id.ToString() + "_" + x.Acronym + ".xml");
                    }
                    MessageBox.Show("OK");
                }
            }

        }

        private void btnUpdateDics_Click(object sender, EventArgs e)
        {
            UpdateDictionaries_Web();
        }

        private void btnExportRecommendedList_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "XML File|*.xml";
            if (sf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            ExportXML_RecommendedList(sf.FileName, chbIsCrimea.Checked);
        }
    }
}
