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
        /// Справочник № 6 - Основание для оценки
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
        private Dictionary<string, string> dic19_Olympics = new Dictionary<string, string>();
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
                var vals = context.SP_Faculty.OrderBy(x => x.Id).
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
        private void ExportXML_Part1()
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

            UpdateDictionaries();
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
                string CampaignId = new Guid("00000000-0000-0000-2014-000000000001").ToString();
                wc.Show();
                //--------------------------------------------------------------------------------------------------------------
                //заполняем данные AdmissionInfo
                //объём приёма (КЦ)
                root = doc["Root"]["PackageData"];
                root["AdmissionInfo"].AppendChild(doc.CreateNode(XmlNodeType.Element, "AdmissionVolume", ""));
                //Конкурсные группы
                root["AdmissionInfo"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroups", ""));

                //Заполняем объём приёма (КЦ)
                root = root["AdmissionInfo"]["AdmissionVolume"];
                
                //объём для СПО не грузим совсем
                var LPs = context.extEntry.Where(x => x.StudyLevelGroupId <= 3 && (FacultyId.HasValue ? x.FacultyId == FacultyId.Value : true)
                    && (LicenseProgramId.HasValue ? x.LicenseProgramId == LicenseProgramId.Value : true)).Select(x => x.LicenseProgramId).Distinct();

                var PriemHelps = context.hlpLicenseProgramKCP.Where(x => LPs.Contains(x.LicenseProgramId) /*&& x.LevelGroupId == StudyLevelGroupId*/).Distinct().ToList();

                wcMax = PriemHelps.Count();
                wc.SetText("Заполняем объём приёма (КЦ)");
                wc.SetMax(wcMax);
                wc.ZeroCount();

                foreach (var p in PriemHelps.Select(x => new { x.QualificationCode, x.LicenseProgramCode, x.LicenseProgramName, x.StudyLevelId, x.KC_OB, x.KC_OP, x.KC_OZB, x.KC_OZP, x.KC_ZB, x.KC_ZP, x.KC_O_CEL, x.KC_OZ_CEL, x.KC_Z_CEL }).Distinct())
                {
                    wc.PerformStep();
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Item", ""));
                    //идентификатор объёма приёма по направлению подготовки в приёме
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    root.LastChild["UID"].InnerXml = p.LicenseProgramCode.ToString() + "_" + p.QualificationCode;
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CampaignUID", ""));
                    root.LastChild["CampaignUID"].InnerXml = CampaignId;
                    //Справочник №2 - Уровни образования
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationLevelID", ""));
                    string StudyLevelFISName = context.extEntry.Where(x => x.StudyLevelId == p.StudyLevelId).Select(x => x.StudyLevelFISName).FirstOrDefault();
                    root.LastChild["EducationLevelID"].InnerXml = SearchInDictionary(dic02_StudyLevel, StudyLevelFISName); //dic02_StudyLevel[p.StudyLevelId.ToString()];
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Course", ""));
                    root.LastChild["Course"].InnerXml = "1";
                    //Справочник №10 - направления подготовки
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DirectionID", ""));
                    string qual_code = p.QualificationCode;
                    root.LastChild["DirectionID"].InnerXml = SearchInDictionary(dic10_Direction, p.LicenseProgramName, p.LicenseProgramCode, qual_code);
                    if (p.KC_OB.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetO", ""));
                        root.LastChild["NumberBudgetO"].InnerXml = p.KC_OB.ToString();
                    }
                    if (p.KC_OZB.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetOZ", ""));
                        root.LastChild["NumberBudgetOZ"].InnerXml = p.KC_OZB.ToString();
                    }
                    if (p.KC_ZB.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetZ", ""));
                        root.LastChild["NumberBudgetZ"].InnerXml = p.KC_ZB.ToString();
                    }
                    if (p.KC_OP.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidO", ""));
                        root.LastChild["NumberPaidO"].InnerXml = p.KC_OP.ToString();
                    }
                    if (p.KC_OZP.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidOZ", ""));
                        root.LastChild["NumberPaidOZ"].InnerXml = p.KC_OZP.ToString();
                    }
                    if (p.KC_ZP.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidZ", ""));
                        root.LastChild["NumberPaidZ"].InnerXml = p.KC_ZP.ToString();
                    }
                    if (p.KC_O_CEL.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetO", ""));
                        root.LastChild["NumberTargetO"].InnerXml = p.KC_O_CEL.ToString();
                    }
                    if (p.KC_OZ_CEL.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetOZ", ""));
                        root.LastChild["NumberTargetOZ"].InnerXml = p.KC_OZ_CEL.ToString();
                    }
                    if (p.KC_Z_CEL.HasValue)
                    {
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetZ", ""));
                        root.LastChild["NumberTargetZ"].InnerXml = p.KC_Z_CEL.ToString();
                    }
                }
                dt2 = DateTime.Now - dt;
                dt = DateTime.Now;
                //Заполняем конкурсные группы
                List<int> lstStudLev = new List<int>();
                if (StudyLevelGroupId == 1)
                {
                    lstStudLev.Add(16);
                    lstStudLev.Add(18);
                }
                if (StudyLevelGroupId == 2)
                {
                    lstStudLev.Add(17);
                }
                if (StudyLevelGroupId == 3)
                {
                    lstStudLev.Add(8);
                    lstStudLev.Add(10);
                }

                root = root.ParentNode["CompetitiveGroups"];
                var CompetitionGroups = context.extCompetitiveGroup.Where(x =>
                    (LicenseProgramId.HasValue ? x.LicenseProgramId == LicenseProgramId.Value : true)
                    && (FacultyId.HasValue ? x.FacultyId == FacultyId.Value : true)
                    /*&& (lstStudLev.Contains(x.StudyLevelId))*/)
                    .Select(x => new { x.Id, x.CompetitiveGroupItemId, x.StudyLevelFISName, x.Name, x.QualificationCode, x.LicenseProgramName, x.LicenseProgramCode, 
                        x.KCP_ZB, x.KCP_ZP, x.KCP_Cel_B, x.KCP_Cel_P, x.KCP_OB, x.KCP_OP, x.KCP_OZB, x.KCP_OZP } )
                    .Distinct();

                wcMax = CompetitionGroups.Count();
                wc.SetText("Заполняем конкурсные группы");
                wc.SetMax(wcMax);
                wc.ZeroCount();

                foreach (var CompetitionGroup in CompetitionGroups)
                {
                    wc.PerformStep();
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroup", ""));

                    //идентификатор конкурсной группы в приёме
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    root.LastChild["UID"].InnerXml = CompetitionGroup.Id.ToString();
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CampaignUID", ""));
                    root.LastChild["CampaignUID"].InnerXml = CampaignId;
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Course", ""));
                    root.LastChild["Course"].InnerXml = "1";
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Name", ""));
                    root.LastChild["Name"].InnerXml = CompetitionGroup.Name;

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Items", ""));
                    //root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "TargetOrganizations", ""));



                    //добавляем направление подготовки конкурсной группы
                    root.LastChild["Items"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupItem", ""));
                    //root.LastChild["Items"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupItem", ""));

                    //заполняем данные о направлении подготовки для конкурсной группы
                    //это сделано для ВУЗов, где ведётся один конкурс на НЕСКОЛЬКО направлений
                    root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    //поскольку у нас нет одного конкурса на несколько направлений, то мы просто возьмём тот же UID, что и для всей группы
                    root.LastChild["Items"]["CompetitiveGroupItem"]["UID"].InnerXml = CompetitionGroup.CompetitiveGroupItemId.ToString();
                    //Справочник №2 - Уровни образования
                    root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationLevelID", ""));
                    root.LastChild["Items"]["CompetitiveGroupItem"]["EducationLevelID"].InnerXml = SearchInDictionary(dic02_StudyLevel, CompetitionGroup.StudyLevelFISName);
                    //id направления подготовки из справочника №10
                    root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DirectionID", ""));
                    root.LastChild["Items"]["CompetitiveGroupItem"]["DirectionID"].InnerXml = SearchInDictionary(dic10_Direction, CompetitionGroup.LicenseProgramName, CompetitionGroup.LicenseProgramCode, CompetitionGroup.QualificationCode);
                    //CompetitionGroup.LicenseProgramCode; //пока пусть будет код направления
                    if (CompetitionGroup.KCP_OB.HasValue)
                    {
                        root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetO", ""));
                        root.LastChild["Items"]["CompetitiveGroupItem"]["NumberBudgetO"].InnerXml = CompetitionGroup.KCP_OB.ToString();
                    }
                    if (CompetitionGroup.KCP_OZB.HasValue)
                    {
                        root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetOZ", ""));
                        root.LastChild["Items"]["CompetitiveGroupItem"]["NumberBudgetOZ"].InnerXml = CompetitionGroup.KCP_OZB.ToString();
                    }
                    if (CompetitionGroup.KCP_ZB.HasValue)
                    {
                        root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberBudgetZ", ""));
                        root.LastChild["Items"]["CompetitiveGroupItem"]["NumberBudgetZ"].InnerXml = CompetitionGroup.KCP_ZB.ToString();
                    }
                    if (CompetitionGroup.KCP_OP.HasValue)
                    {
                        root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidO", ""));
                        root.LastChild["Items"]["CompetitiveGroupItem"]["NumberPaidO"].InnerXml = CompetitionGroup.KCP_OP.ToString();
                    }
                    if (CompetitionGroup.KCP_OZP.HasValue)
                    {
                        root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidOZ", ""));
                        root.LastChild["Items"]["CompetitiveGroupItem"]["NumberPaidOZ"].InnerXml = CompetitionGroup.KCP_OZP.ToString();
                    }
                    if (CompetitionGroup.KCP_ZP.HasValue)
                    {
                        root.LastChild["Items"]["CompetitiveGroupItem"].AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberPaidZ", ""));
                        root.LastChild["Items"]["CompetitiveGroupItem"]["NumberPaidZ"].InnerXml = CompetitionGroup.KCP_ZP.ToString();
                    }

                    /*
                    if ((CompetitionGroup.KCP_Cel_B ?? 0) > 0)
                    {
                        //добавляем сведения о целевом наборе (необяз)
                        root.LastChild["TargetOrganizations"].AppendChild(doc.CreateNode(XmlNodeType.Element, "TargetOrganization", ""));

                        root.LastChild["TargetOrganizations"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                        root.LastChild["TargetOrganizations"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "TargetOrganizationName", ""));
                        root.LastChild["TargetOrganizations"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Items", ""));

                        //направления подготовки целевого приёма
                        root.LastChild["TargetOrganizations"].LastChild["Items"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupTargetItem", ""));

                        root.LastChild["TargetOrganizations"].LastChild["Items"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                        root.LastChild["TargetOrganizations"].LastChild["Items"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EducationLevelID", ""));
                        root.LastChild["TargetOrganizations"].LastChild["Items"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetO", ""));
                        root.LastChild["TargetOrganizations"].LastChild["Items"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetOZ", ""));
                        root.LastChild["TargetOrganizations"].LastChild["Items"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NumberTargetZ", ""));
                        //id направления подготовки из справочника №10
                        root.LastChild["TargetOrganizations"].LastChild["Items"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DirectionID", ""));
                    }
                     */

                    //добавляем "условия предоставления общей льготы" (б/э)
                    /*
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CommonBenefit", ""));
                    root.LastChild["CommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CommonBenefitItem", ""));

                    root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    //типы дипломов олимпиад
                    root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDiplomTypes", ""));
                    root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitKindID", ""));
                    root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "IsForAllOlympics", ""));
                    //перечень олимпиад, для которых действует льгота
                    root.LastChild["CommonBenefit"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Olympics", ""));

                    //заполняем список типов дипломов олимпиад
                    //id типа диплома (справочник №18)
                    root.LastChild["CommonBenefit"].LastChild["OlympicDiplomTypes"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlimpicDiplomTypeID", ""));

                    //заполняем перечень олимпиад, для которых действует льгота
                    //id олимпиады (справочник №20 "Названия олимпиад")
                    root.LastChild["CommonBenefit"].LastChild["Olympics"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicID", ""));
                    */
                    var EntryExams = (from ExInEnt in context.extExamInEntry
                                      join qComp in context.qEntryToCompetitiveGroup on ExInEnt.EntryId equals qComp.EntryId
                                      join Ent in context.extEntry on ExInEnt.EntryId equals Ent.Id
                                      where qComp.CompetitiveGroupId == CompetitionGroup.Id
                                      select new
                                      {
                                          UID = ExInEnt.ExamInEntryToCompetitiveGroupId,
                                          TestType = ExInEnt.IsAdditional ? 2 : /*ExInEnt.IsProfil ? 3 :*/ 1,
                                          MinScore = ExInEnt.EgeMin,
                                          SubjectName = ExInEnt.ExamName
                                      }).Where(x => x.MinScore.HasValue && x.MinScore > 0).Distinct().GroupBy(x => new { x.UID, x.TestType, x.SubjectName })
                                      .Select(x => new { x.Key.UID, x.Key.SubjectName, x.Key.TestType, MinScore = x.Select(z => z.MinScore).Min() }).Distinct().ToList();

                    if (EntryExams.Count() > 0)
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestItems", ""));

                    foreach (var EntryExam in EntryExams)
                    {
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
                        root.LastChild["EntranceTestItems"].LastChild["MinScore"].InnerXml = EntryExam.MinScore.ToString();

                        //название вступительного испытания
                        root.LastChild["EntranceTestItems"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestSubject", ""));
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestSubject"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectName", ""));
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestSubject"]["SubjectName"].InnerXml = EntryExam.SubjectName;

                        //условия предоставления льгот
                        /*
                        root.LastChild["EntranceTestItems"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestBenefits", ""));

                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestBenefitItem", ""));
                        //заносим условие предоставления льгот
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDiplomTypes", ""));
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitKindID", ""));
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "IsForAllOlympics", ""));
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Olympics", ""));
                        //заполняем дипломы (по справочнику №18 "Тип диплома")
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["OlympicDiplomTypes"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDiplomTypeID", ""));
                        //заполняем олимпиады (по справочнику №20 "Олимпиады")
                        root.LastChild["EntranceTestItems"].LastChild["EntranceTestBenefits"].LastChild["Olympics"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicID", ""));
                        
                         */
                    }
                }
                wc.Close();
                doc.Save(fname);

                MessageBox.Show("OK");
            }
        }
        //заполняет только раздел о заявлениях
        private void ExportXML_Part3(string fname)
        {
            XmlDocument doc = new XmlDocument();
            //XmlImplementation imp = new XmlImplementation();

            UpdateDictionaries();
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
                            join person in context.Person on abit.PersonId equals person.Id
                            join compGroup in context.qEntryToCompetitiveGroup on abit.EntryId equals compGroup.EntryId
                            join extCompGroup in context.extCompetitiveGroup on compGroup.CompetitiveGroupId equals extCompGroup.Id
                            where (FacultyId.HasValue ? abit.FacultyId == FacultyId.Value : true)
                            && (LicenseProgramId.HasValue ? abit.LicenseProgramId == LicenseProgramId.Value : true)
                            && (abit.StudyLevelGroupId == StudyLevelGroupId)
                            select new
                            {
                                AppUID = abit.Id,
                                PersonId = abit.PersonId,
                                ApplicationNumber = abit.RegNum,
                                RegistrationDate = abit.DocInsertDate,
                                LastDenyDate = abit.BackDocDate,
                                NeedHostel = person.Person_AdditionalInfo.HostelEduc,
                                EntrantUID = abit.PersonId,
                                EntrantSurname = person.Surname,
                                EntrantName = person.Name,
                                EntrantMiddleName = person.SecondName,
                                abit.StudyBasisFISName,
                                abit.StudyFormFISName,
                                AddInfo = person.Person_AdditionalInfo.ExtraInfo,
                                EgeDocOrigin = abit.HasOriginals,
                                person.PassportSeries,
                                person.PassportNumber,
                                person.PassportAuthor,
                                PassportDate = person.PassportDate,
                                abit.HasOriginals,
                                NationalityFISName = person.Nationality.FISName != null ? person.Nationality.FISName : context.ForeignCountry.Where(x => x.Id == person.ForeignNationalityId).Select(x => x.Name).FirstOrDefault(),
                                PassportTypeFISName = person.PassportType.FISName,
                                person.BirthDate,
                                person.BirthPlace,
                                person.Person_EducationInfo.SchoolTypeId,
                                person.Sex,
                                EducDocSeries = person.Person_EducationInfo.SchoolTypeId == 1 ? person.Person_EducationInfo.AttestatSeries : person.Person_EducationInfo.DiplomSeries,
                                EducDocRegion = person.Person_EducationInfo.SchoolTypeId == 1 ? person.Person_EducationInfo.AttestatRegion : "",
                                EducDocNum = person.Person_EducationInfo.SchoolTypeId == 1 ? person.Person_EducationInfo.AttestatNum : person.Person_EducationInfo.DiplomNum,
                                compGroup.CompetitiveGroupId,
                                compGroup.CompetitiveGroupName,
                                extCompGroup.CompetitiveGroupItemId,
                                NotEnabled = abit.NotEnabled,
                                abit.BackDoc
                            });

                root = doc["Root"]["PackageData"]["Applications"];

                int wcMax = apps.Count();
                wc.SetText(fname + " - Заполняем данные Applications");
                wc.SetMax(wcMax);
                wc.ZeroCount();


                dt4_allAbitList = DateTime.Now - dt;

                dt = DateTime.Now;
                foreach (var app in apps)
                {
                    wc.PerformStep();
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "Application", ""));

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    root.LastChild["UID"].InnerText = app.AppUID.ToString();

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ApplicationNumber", ""));
                    root.LastChild["ApplicationNumber"].InnerText = app.ApplicationNumber.ToString();

                    //данные о человеке
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Entrant", ""));
                    //дата регистрации заявления в ИС
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "RegistrationDate", ""));
                    root.LastChild["RegistrationDate"].InnerText = app.RegistrationDate.ToString("yyyy-MM-ddTHH:mm:ss");

                    //дата отзыва заявления (если была)
                    if (app.LastDenyDate.HasValue)
                    {
                        DateTime LastDenyDate = app.LastDenyDate.Value < app.RegistrationDate ? app.RegistrationDate.AddHours(1) : app.LastDenyDate.Value;
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "LastDenyDate", ""));
                        root.LastChild["LastDenyDate"].InnerText = LastDenyDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "NeedHostel", ""));
                    root.LastChild["NeedHostel"].InnerText = app.NeedHostel.ToString().ToLower();

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
                    if (app.BackDoc)
                        AppStatus = "Отозвано";
                    else
                        AppStatus = "Принято";

                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "StatusID", ""));
                    root.LastChild["StatusID"].InnerXml = SearchInDictionary(dic04_ApplicationStatus, AppStatus);

                    //конкурсные группы для заявления
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SelectedCompetitiveGroups", ""));
                    root.LastChild["SelectedCompetitiveGroups"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                    root.LastChild["SelectedCompetitiveGroups"]["CompetitiveGroupID"].InnerXml = app.CompetitiveGroupId.ToString();

                    //элементы конкурсных групп для заявления
                    root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SelectedCompetitiveGroupItems", ""));
                    root.LastChild["SelectedCompetitiveGroupItems"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupItemID", ""));
                    root.LastChild["SelectedCompetitiveGroupItems"]["CompetitiveGroupItemID"].InnerXml = app.CompetitiveGroupItemId.ToString();

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
                    //UID организации
                    root.LastChild["FinSourceAndEduForms"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "TargetOrganizationUID", ""));

                    //добавляем сведения о льготе, предоставленной абитуриенту
                    /*
                    root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                    //id типа документа-основания (Справочник №31 - "Тип документа") - необяз
                    root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentTypeID", ""));
                    //документ-основание - необяз
                    root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentReason", ""));
                    //id вида льготы (Справочник №30 - "Вид льготы")
                    root.LastChild["ApplicationCommonBenefit"].AppendChild(doc.CreateNode(XmlNodeType.Element, "BenefitKindID", ""));
                    
                    //вводим документ-основание (если есть) - один из четырёх
                    List<int?> regards = new List<int?>() { 5, 6, 7 };//победител/призёр
                    var OlympDocs = context.extOlympiads.Where(x => x.AbiturientId == app.AppUID && regards.Contains(x.OlympValueId));
                    if (OlympDocs.Where(x => x.OlympTypeId == 2).Count() > 0)
                        //диплом победителя/призёра всероссийской олимпиады школьников
                        root.LastChild["ApplicationCommonBenefit"]["DocumentReason"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicTotalDocument", ""));
                    else if (OlympDocs.Where(x => x.OlympTypeId != 2).Count() > 0)
                        //диплом победителя/призёра олимпиады школьников
                        root.LastChild["ApplicationCommonBenefit"]["DocumentReason"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDocument", ""));

                    //основание для льготы по медицинским показаниям - ПОКА ЧТО В БАЗЕ НЕТ
                    //root.LastChild["ApplicationCommonBenefit"]["DocumentReason"].AppendChild(doc.CreateNode(XmlNodeType.Element, "MedicalDocument", ""));
                    //прочее - ПОКА ЧТО В БАЗЕ НЕТ
                    //root.LastChild["ApplicationCommonBenefit"]["DocumentReason"].AppendChild(doc.CreateNode(XmlNodeType.Element, "CustomDocument", ""));
                    */
                    dt = DateTime.Now;
                    //вводим данные об оценках на вступительных испытаниях
                    var abitMarks = context.qMark.Where(x => x.AbiturientId == app.AppUID && !x.IsFromEge);

                    //рез-ты вступительных испытаний
                    if (abitMarks.Count() > 0)
                        root.LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestResults", ""));

                    foreach (var mrk in abitMarks)
                    {
                        root.LastChild["EntranceTestResults"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestResult", ""));

                        root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                        root.LastChild["EntranceTestResults"].LastChild["UID"].InnerXml = mrk.Id.ToString();
                        root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultValue", ""));
                        root.LastChild["EntranceTestResults"].LastChild["ResultValue"].InnerXml = mrk.Value.ToString();
                        //Тип основания для оценки (документа-основания)

                        string ResultSourceTypeName = "Экзаменационная ведомость";
                        if (mrk.IsFromEge || mrk.IsFromOlymp /*|| mrk.IsManual*/)
                            ResultSourceTypeName = "Экзаменационный лист";
                        else
                        {

                            if (context.ExamsVedHistory.Where(x => x.ExamsVedId == mrk.ExamVedId && x.PersonId == app.PersonId && (x.AppealMark != null || x.OralAppealMark != null)).Count() > 0)
                                ResultSourceTypeName = "Апелляционная ведомость";
                        }

                        root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultSourceTypeID", ""));
                        root.LastChild["EntranceTestResults"].LastChild["ResultSourceTypeID"].InnerXml = SearchInDictionary(dic33_, ResultSourceTypeName);

                        //вносим предмет (название), Id не заносим, их ФИС сама найдёт и придумает
                        root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestSubject", ""));
                        root.LastChild["EntranceTestResults"].LastChild["EntranceTestSubject"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectName", ""));
                        root.LastChild["EntranceTestResults"].LastChild["EntranceTestSubject"]["SubjectName"].InnerXml = mrk.ExamName;

                        //ИД типа конкурсного испытания (справочник №11)
                        root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "EntranceTestTypeID", ""));
                        root.LastChild["EntranceTestResults"].LastChild["EntranceTestTypeID"].InnerXml = SearchInDictionary(dic11_EntranceTestType, mrk.EntranceTestType);
                        //ИД конкурсной группы
                        root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "CompetitiveGroupID", ""));
                        root.LastChild["EntranceTestResults"].LastChild["CompetitiveGroupID"].InnerXml = app.CompetitiveGroupId.ToString();

                        //вносим сведения об основании для оценки (OlympicDocument или OlympicTotalDocument или InstitutionDocument или EgeDocumentID) - необяз
                        root.LastChild["EntranceTestResults"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "ResultDocument", ""));
                        //string ResultDocumentId = "";
                        //if (mrk.IsFromOlymp)
                        //{
                        //    var Olymp = context.extOlympiads.Where(x => x.Id == mrk.OlympiadId).FirstOrDefault();
                        //    //тут надо посмотреть, какой из олимпийских сертификатов давать
                        //    if (Olymp == null)
                        //        continue;
                        //    if (Olymp.OlympTypeId == 2)
                        //    {
                        //        root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicTotalDocument", ""));
                        //    }
                        //    root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OlympicDocument", ""));

                        //}
                        //else 
                        if (mrk.IsFromEge)
                        {
                            //id свидетельства о рез-тах ЕГЭ, которое было приложено к заявлению
                            root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EgeDocumentID", ""));
                            root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.InnerXml = mrk.EgeCertificateId.ToString();
                        }
                        else if (mrk.IsManual)
                        {
                            //самостоятельное испытание (ведомость ручного ввода от какой-то даты)
                            root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "InstitutionDocument", ""));
                            root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                            root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild["DocumentNumber"].InnerXml = "Ведомость ручного ввода от " + mrk.PassDate.Value.ToShortDateString();
                        }
                        else
                        {
                            //самостоятельное испытание (ведомость вступительного испытания)
                            root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "InstitutionDocument", ""));
                            root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));

                            root.LastChild["EntranceTestResults"].LastChild["ResultDocument"].LastChild["DocumentNumber"].InnerXml = "Ведомость ручного ввода от " + mrk.PassDate.Value.ToShortDateString();
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

                    var egeDocs = context.extEgeMark.Where(x => x.PersonId == app.EntrantUID).Select(x => new { x.Id, x.EgeCertificateId, x.Number, x.Year, x.FISName, x.Value });
                    //Свидетельства о результатах ЕГЭ - необяз
                    if (egeDocs.Count() > 0)
                        root.LastChild["ApplicationDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EgeDocuments", ""));

                    foreach (var cert in egeDocs.Select(x => new { x.EgeCertificateId, x.Year, x.Number }).Distinct())
                    {
                        //Заполняем данные о сертификатах
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EgeDocument", ""));

                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["UID"].InnerXml = cert.EgeCertificateId.ToString();
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();
                        //root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["DocumentNumber"].InnerXml = cert.Number;
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentYear", ""));
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["DocumentYear"].InnerXml = cert.Year;
                        root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Subjects", ""));

                        var exams = egeDocs.Where(x => x.EgeCertificateId == cert.EgeCertificateId).Select(x => new { x.FISName, x.Value });
                        foreach (var exam in exams)
                        {
                            //заполняем оценки в сертификате ЕГЭ
                            root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["Subjects"].AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectData", ""));

                            //id дисциплины (справочник №1 - "Общеобразовательные предметы")
                            root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["Subjects"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "SubjectID", ""));
                            root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["Subjects"].LastChild["SubjectID"].InnerXml = SearchInDictionary(dic01_Subject, exam.FISName);
                            root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["Subjects"].LastChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "Value", ""));
                            root.LastChild["ApplicationDocuments"]["EgeDocuments"].LastChild["Subjects"].LastChild["Value"].InnerXml = exam.Value.ToString();
                        }
                    }

                    dt4_EgeMarks += DateTime.Now - dt;

                    //заполняем данные о док-те, удостоверяющем личность
                    //UID необяз
                    //root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "UID", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();
                    //root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceivedDate", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentSeries", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["DocumentSeries"].InnerXml = string.IsNullOrEmpty(app.PassportSeries) ? "-" : app.PassportSeries;
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["DocumentNumber"].InnerXml = app.PassportNumber;
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentDate", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["DocumentDate"].InnerXml = app.PassportDate.HasValue ? app.PassportDate.Value.ToString("yyyy-MM-dd") : "";
                    //кем выдан - необяз
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentOrganization", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["DocumentOrganization"].InnerXml = app.PassportAuthor;
                    //ID типа документа, удостовер личность (Справочник №22)
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "IdentityDocumentTypeID", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["IdentityDocumentTypeID"].InnerXml = SearchInDictionary(dic22_IdentityDocumentType, app.PassportTypeFISName);
                    //Список стран (справочник №7)
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "NationalityTypeID", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["NationalityTypeID"].InnerXml = SearchInDictionary(dic07_Country, app.NationalityFISName);
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "BirthDate", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["BirthDate"].InnerXml = app.BirthDate.ToString("yyyy-MM-dd");
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"].AppendChild(doc.CreateNode(XmlNodeType.Element, "BirthPlace", ""));
                    root.LastChild["ApplicationDocuments"]["IdentityDocument"]["BirthPlace"].InnerXml = app.BirthPlace;

                    //вносим документы об образовании
                    root.LastChild["ApplicationDocuments"]["EduDocuments"].AppendChild(doc.CreateNode(XmlNodeType.Element, "EduDocument", ""));
                    //на выбор - один из
                    //все их объединяет три поля - Серия, Номер, Оригинал(да/нет)
                    XmlNode rootChild = root;
                    switch (app.SchoolTypeId ?? 1)
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
                    string series = (app.EducDocRegion != "" ? (app.EducDocRegion + " ") : "") + app.EducDocSeries;
                    rootChild["DocumentSeries"].InnerXml = string.IsNullOrEmpty(series) ? "-" : series;
                    rootChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "DocumentNumber", ""));
                    rootChild["DocumentNumber"].InnerXml = string.IsNullOrEmpty(app.EducDocNum) ? "-" : app.EducDocNum;
                    rootChild.AppendChild(doc.CreateNode(XmlNodeType.Element, "OriginalReceived", ""));
                    rootChild["OriginalReceived"].InnerXml = app.HasOriginals.ToString().ToLower();

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

        private void WCF_Import(XmlDocument docToImport)
        {

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
        private void UpdateDictionaries()
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

            List<int> list_codes = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 10, 11, 12, 13, 14, 15, 17, 18, 19, 22, 23, 30, 31, 33, 34 };

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
            if (dicCode != 10)
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
            else
            {
                try
                {
                    foreach (XmlNode node in xmlData["DictionaryData"]["DictionaryItems"].ChildNodes)
                        dic10_Direction.Add(new Dictionary10Direction() 
                        { 
                            Id = node["ID"].InnerXml, 
                            Name = node["Name"].InnerXml,
                            Code = node["NewCode"].InnerXml,
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
                case 19: { dic19_Olympics = dic; break; }
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
            FillComboLicenseProgram();
        }
        private void btnExportXML_part1_Click(object sender, EventArgs e)
        {
            ExportXML_Part1();
        }
        private void btnExportXML_part3_Click(object sender, EventArgs e)
        {
            if (!chbFullImport.Checked)
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Filter = "XML File|*.xml";
                if (sf.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                ExportXML_Part3(sf.FileName);
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
                        ExportXML_Part3(folderPath + (x.Id < 10 ? "0" : "") + x.Id.ToString() + "_" + x.Acronym + ".xml");
                    }
                    MessageBox.Show("OK");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                            Guid CompGroupId = context.qEntryToCompetitiveGroup.Where(x => x.EntryId == EntryID).FirstOrDefault().CompetitiveGroupId;
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
    }
}
