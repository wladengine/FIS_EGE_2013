using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace FIS_EGE_2013
{
    public partial class EgeForm : Form
    {
        private BackgroundWorker bw;
        private string defaultEmptyCertificateNumber = "нет свидетельства";
        public EgeForm()
        {
            InitializeComponent();
            FillGrid();
        }

        private void FillGrid()
        {
            using (PriemEntities context = new PriemEntities())
            {
                var data =
                    (from pers in context.extPerson
                     join Abit in context.Abiturient on pers.Id equals Abit.PersonId
                     join forApps in context.qAbiturientForeignApplicationsOnly on Abit.Id equals forApps.Id into forApps2
                     from forApps in forApps2.DefaultIfEmpty()

                     join cer in context.EgeCertificate on pers.Id equals cer.PersonId into cer2
                     from cer in cer2.DefaultIfEmpty()

                     where /*(cer.Id != null || cer.FBSStatusId == 0) && cer.Year != "2014" &&*/ pers.SchoolTypeId != 4 && pers.PassportTypeId == 1 && forApps.Id == null
                     select new
                     {
                         pers.Id,
                         pers.PersonNum,
                         pers.FIO
                     }).Distinct().OrderBy(x => x.FIO).ToList();

                dgvAbits.DataSource = data;
                dgvAbits.Columns["Id"].Visible = false;
                dgvAbits.Columns["PersonNum"].HeaderText = "ИД номер";
                dgvAbits.Columns["FIO"].HeaderText = "ФИО";
                dgvAbits.Columns["FIO"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                lblCount.Text = dgvAbits.Rows.Count.ToString();
            }
        }

        private void GetDataAsync(Guid PersonId)
        {
            string queryXML = GetPersonSingleCheckXML(PersonId);

            //bw = new BackgroundWorker();
            //bw.DoWork += bw_DoWork;
            //bw.RunWorkerCompleted += bw_GetAnswerAndUpdateCerts;

            //var rData = new
            //{
            //    queryXML = queryXML
            //};

            //tbReq.Text = queryXML;

            //bw.RunWorkerAsync(rData);

            FisEgeService.WSChecksSoapClient client = new FisEgeService.WSChecksSoapClient();
            string outXml = client.SingleCheck(new FisEgeService.UserCredentials() { Login = "p.karpenko@spbu.ru", Password = "E0k02II", Client = "p.karpenko@spbu.ru" }, queryXML);

            bwSaveAll.ReportProgress(0, new { inXml = queryXML, outXml = outXml });
            var certs = GetCertsFromXML(outXml);
            foreach (var c in certs)
                SaveEgeCertificate(c);
        }
        private void GetData(Guid PersonId)
        {
            string queryXML = GetPersonSingleCheckXML(PersonId);
            tbReq.Text = queryXML;
            FisEgeService.WSChecksSoapClient client = new FisEgeService.WSChecksSoapClient();
            string outXml = client.SingleCheck(new FisEgeService.UserCredentials() { Login = "p.karpenko@spbu.ru", Password = "E0k02II", Client = "p.karpenko@spbu.ru" }, queryXML);
            tbAns.Text = outXml;
            var certs = GetCertsFromXML(outXml);
            foreach (var c in certs)
                SaveEgeCertificate(c);
        }
        private void GetCertDataAsync(Guid EgeCertificateId)
        {
            string queryXML = GetCertSingleCheckXML(EgeCertificateId);

            //bw = new BackgroundWorker();
            //bw.DoWork += bw_DoWork;
            //bw.RunWorkerCompleted += bw_GetAnswerAndUpdateCerts;

            //var rData = new
            //{
            //    queryXML = queryXML
            //};

            //tbReq.Text = queryXML;

            //bw.RunWorkerAsync(rData);

            FisEgeService.WSChecksSoapClient client = new FisEgeService.WSChecksSoapClient();
            string outXml = client.SingleCheck(new FisEgeService.UserCredentials() { Login = "p.karpenko@spbu.ru", Password = "E0k02II", Client = "p.karpenko@spbu.ru" }, queryXML);

            bwSaveAll.ReportProgress(0, new { inXml = queryXML, outXml = outXml });
            var certs = GetCertsFromXML(outXml);
            foreach (var c in certs)
                SaveEgeCertificate(c);
        }

        void bw_GetAnswerAndUpdateCerts(object sender, RunWorkerCompletedEventArgs e)
        {
            string outXml = e.Result.ToString();
            tbAns.Text = outXml;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_doWork_2;
            bw.RunWorkerCompleted += bw_FinishAndUpdateGrid;

            var rData = new
            {
                queryXML = outXml
            };

            bw.RunWorkerAsync(rData);
        }
        void bw_FinishAndUpdateGrid(object sender, RunWorkerCompletedEventArgs e)
        {
            //FillGrid();
        }

        void bw_doWork_2(object sender, DoWorkEventArgs e)
        {
            string outXml = ((dynamic)e.Argument).queryXML;
            var certs = GetCertsFromXML(outXml);
            foreach (var c in certs)
                SaveEgeCertificate(c);
        }

        List<EgeCertificateClass> GetCertsFromXML(string outXml)
        {
            XmlDocument doc = new XmlDocument(); 
            List<EgeCertificateClass> lstCerts = new List<EgeCertificateClass>();
            doc.LoadXml(outXml);

            XmlElement node = doc["checkResults"];
            if (node.HasChildNodes)
            {
                var innerNodeList = node.GetElementsByTagName("certificate");
                foreach (XmlNode innerNode in innerNodeList)
                {
                    EgeCertificateClass cert = new EgeCertificateClass();
                    string Surname = innerNode["lastName"].InnerText;
                    string Name = innerNode["firstName"].InnerText;
                    string SecondName = innerNode["patronymicName"].InnerText;
                    string PassportSeries = innerNode["passportSeria"].InnerText;
                    string PassportNumber = innerNode["passportNumber"].InnerText;
                    string CertificateNumber = innerNode["certificateNumber"].InnerText;
                    if (string.IsNullOrEmpty(CertificateNumber))
                        CertificateNumber = defaultEmptyCertificateNumber;

                    string TypographicNumber = innerNode["typographicNumber"].InnerText;
                    string Year = innerNode["year"].InnerText;
                    string Status = innerNode["status"].InnerText;
                    string CertificateDeny = innerNode["certificateDeny"].InnerText;
                    string DenyComment = "", CertificateNewNumber = "";

                    cert.Status = EgeStatus.IsOk;
                    if (CertificateDeny == "1" || Status.IndexOf("Действующий", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        if (((XmlElement)innerNode).GetElementsByTagName("certificateDenyComment").Count > 0)
                        {
                            DenyComment = innerNode["certificateDenyComment"].InnerText;
                            CertificateNewNumber = innerNode["certificateNewNumber"].InnerText;
                        }
                        else
                            DenyComment = Status;
                        
                        if (!string.IsNullOrEmpty(CertificateNewNumber))
                        {
                            DenyComment += ". Новый сертификат: " + CertificateNewNumber;
                        }
                        cert.Status = EgeStatus.IsDeny;
                    }

                    cert.Surname = Surname;
                    cert.Name = Name;
                    cert.SecondName = SecondName;
                    cert.FBSComment = DenyComment;
                    cert.Number = CertificateNumber;
                    cert.PassportSeries = PassportSeries;
                    cert.PassportNumber = PassportNumber;
                    cert.TypographicNumber = TypographicNumber;

                    int iYear = 0;
                    int.TryParse(Year, out iYear);
                    cert.Year = iYear;

                    cert.Marks = new List<EgeMarkClass>();
                    foreach (XmlNode markNode in ((XmlElement)innerNode).GetElementsByTagName("mark"))
                    {
                        string EgeExamName = markNode["subjectName"].InnerText;
                        string Value = markNode["subjectMark"].InnerText;
                        double dValue;
                        double.TryParse(Value, out dValue);
                        int iValue;
                        int.TryParse(Value, out iValue);
                        string IsAppeal = markNode["subjectAppeal"].InnerText;
                        bool bIsAppeal = (IsAppeal == "1");

                        cert.Marks.Add(new EgeMarkClass() { IsAppeal = bIsAppeal, Value = (int)dValue, EgeExamName = EgeExamName });
                    }

                    lstCerts.Add(cert);
                }
            }
            return lstCerts;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            string queryXML = ((dynamic)e.Argument).queryXML;
            FisEgeService.WSChecksSoapClient client = new FisEgeService.WSChecksSoapClient();
            e.Result = client.SingleCheck(new FisEgeService.UserCredentials() { Login = "p.karpenko@spbu.ru", Password = "E0k02II", Client = "p.karpenko@spbu.ru" }, queryXML);
        }

        private string GetPersonSingleCheckXML(Guid PersonId)
        {
            string retString = "";

            XmlDocument doc = new XmlDocument();
            //XmlImplementation imp = new XmlImplementation();

            string fname = PersonId.ToString() + ".xml";
            using (PriemEntities context = new PriemEntities())
            {
                var Person = context.Person.Where(x => x.Id == PersonId).FirstOrDefault();
                if (Person != null)
                {
                    //создаём корневой элемент
                    doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "items", ""));

                    //создаём элементы AuthData и PackageData
                    XmlNode root = doc["items"];

                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "query", ""));

                    //заполняем данные AuthData
                    root = root["query"];
                    
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "lastName", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "firstName", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "patronymicName", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "passportSeria", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "passportNumber", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "certificateNumber", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "typographicNumber", ""));

                    root["lastName"].InnerText = Person.Surname;
                    root["firstName"].InnerText = Person.Name;
                    root["firstName"].InnerText = Person.Name;
                    root["patronymicName"].InnerText = Person.SecondName;
                    root["passportSeria"].InnerText = Person.PassportSeries;
                    root["passportNumber"].InnerText = Person.PassportNumber;

                    var declaration = doc.CreateXmlDeclaration("1.0", "utf-8", "");

                    retString = declaration.OuterXml + doc.InnerXml;
                }
            }

            return retString;
        }
        private string GetCertSingleCheckXML(Guid EgeCertificateId)
        {
            string retString = "";

            XmlDocument doc = new XmlDocument();
            using (PriemEntities context = new PriemEntities())
            {
                var EgeCertificate = context.EgeCertificate.Where(x => x.Id == EgeCertificateId).FirstOrDefault();
                if (EgeCertificate != null && EgeCertificate.Person != null)
                {
                    //создаём корневой элемент
                    doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "items", ""));

                    //создаём элементы AuthData и PackageData
                    XmlNode root = doc["items"];

                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "query", ""));

                    //заполняем данные AuthData
                    root = root["query"];

                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "lastName", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "firstName", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "patronymicName", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "passportSeria", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "passportNumber", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "certificateNumber", ""));
                    root.AppendChild(doc.CreateNode(XmlNodeType.Element, "typographicNumber", ""));

                    root["lastName"].InnerText = EgeCertificate.Person.Surname;
                    root["firstName"].InnerText = EgeCertificate.Person.Name;
                    root["patronymicName"].InnerText = EgeCertificate.Person.SecondName;
                    root["certificateNumber"].InnerText = EgeCertificate.Number;
                    root["typographicNumber"].InnerText = EgeCertificate.PrintNumber;

                    var declaration = doc.CreateXmlDeclaration("1.0", "utf-8", "");

                    retString = declaration.OuterXml + doc.InnerXml;
                }
            }

            return retString;
        }

        private static void SaveEgeCertificate(EgeCertificateClass cert)
        {
            using (PriemEntities context = new PriemEntities())
            {
                //для SQL-запроса регистр (UpperCase/LowerCase) не важен
                var Person = context.Person.Where(x => x.PassportSeries == cert.PassportSeries && x.PassportNumber == cert.PassportNumber && x.Surname == cert.Surname).FirstOrDefault();
                if (Person == null)
                    Person = context.EgeCertificate.Where(x => x.Number == cert.Number && x.Person.Surname == cert.Surname && x.Person.Name == cert.Name && x.Year != "2014").Select(x => x.Person).FirstOrDefault();

                if (Person != null)
                {
                    string sYear = cert.Year.ToString();
                    var CertList = Person.EgeCertificate.Where(x => x.Year == sYear && x.Number == cert.Number && x.FBSStatusId != 2).Select(x => new { x.Id, x.FBSStatusId }).ToList();
                    //if (CertList.Count == 1)
                    //{
                    //    if (CertList[0].FBSStatusId == 0)
                    //    {
                    //        context.EgeCertificate_Delete(CertList[0].Id);
                    //        CertList.Clear();
                    //    }
                    //}
                    if (CertList.Count == 0 || (CertList.Count == 1 && CertList[0].FBSStatusId != 4))
                    {
                        bool isNew = (CertList.Count == 0 || cert.Status == EgeStatus.IsDeny);
                        cert.Id = isNew ? Guid.NewGuid() : CertList[0].Id;

                        if (isNew)
                        {
                            context.EgeCertificate.AddObject(new EgeCertificate()
                            {
                                PersonId = Person.Id,
                                Id = cert.Id,
                                FBSStatusId = cert.Status == EgeStatus.IsOk ? 1 : 2,
                                FBSComment = cert.FBSComment,
                                IsImported = true,
                                Number = cert.Number,
                                PrintNumber = cert.TypographicNumber,
                                Year = cert.Year.ToString(),
                                ImportDate = DateTime.Now
                            });

                            context.SaveChanges();
                        }
                        
                        foreach (var mrk in cert.Marks)
                        {
                            int EgeExamNameId = context.EgeExamName.Where(x => x.Name == mrk.EgeExamName).FirstOrDefault().Id;
                            context.EgeMark_Insert(mrk.Value, EgeExamNameId, cert.Id, mrk.IsAppeal, false);
                        }

                        context.EgeCertificate_UpdateFBSStatus(cert.Status == EgeStatus.IsOk ? 1 : 2, cert.FBSComment, cert.Id);

                        //MessageBox.Show("Сертификат добавлен");
                    }
                    else
                    {
                        //MessageBox.Show("Сертификат уже имеется");
                    }
                }
            }
            //MessageBox.Show("Done!");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvAbits.SelectedCells.Count == 0)
                return;

            List<int> lstRwIds = new List<int>();
            for (int i = 0; i < dgvAbits.SelectedCells.Count; i++)
            {
                if (!lstRwIds.Contains(dgvAbits.SelectedCells[i].RowIndex))
                    lstRwIds.Add(dgvAbits.SelectedCells[i].RowIndex);
            }

            foreach (int rwInd in lstRwIds)
            {
                //int rwInd = dgvAbits.SelectedCells[0].RowIndex;
                if (rwInd < 0)
                    return;

                Guid PersonId;
                Guid.TryParse(dgvAbits["Id", rwInd].Value.ToString(), out PersonId);

                GetData(PersonId);
            }
            //var certs = GetCertsFromXML(tbOutXML.Text);
            //foreach (var c in certs)
            //    SaveEgeCertificate(c);
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            string outXml = tbAns.Text;
            var certs = GetCertsFromXML(outXml);
            foreach (var c in certs)
                SaveEgeCertificate(c); 
        }

        BackgroundWorker bwBatchRequestWorker;
        private void btnPacketImport_Click(object sender, EventArgs e)
        {
            using (PriemEntities context = new PriemEntities())
            {
                var data =
                    (from pers in context.Person
                     join Abit in context.Abiturient on pers.Id equals Abit.PersonId
                     join forApps in context.qAbiturientForeignApplicationsOnly on Abit.Id equals forApps.Id into forApps2
                     from forApps in forApps2.DefaultIfEmpty()

                     join cer in context.EgeCertificate on pers.Id equals cer.PersonId into cer2
                     from cer in cer2.DefaultIfEmpty()

                     where cer.Id == null && pers.Person_EducationInfo.SchoolTypeId != 4 && pers.PassportTypeId == 1 && forApps.Id == null
                     select pers.Id).Distinct().ToList();

                bwBatchRequestWorker = new BackgroundWorker();
                bwBatchRequestWorker.ProgressChanged += SetProgressInBatchRequest;
                bwBatchRequestWorker.DoWork += bw_DoGetCertsAndUpdate;
                bwBatchRequestWorker.RunWorkerCompleted += bwBatchRequestWorker_RunWorkerCompleted;
                string outXml = GetPacketCheckXML(data);
                tbReq.Text = outXml;
                MessageBox.Show("Now starting async");
                bwBatchRequestWorker.RunWorkerAsync(outXml);
                //GetPacketEgeCerts(outXml);
            }
        }
        void bwBatchRequestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }
        void bw_DoGetCertsAndUpdate(object sender, DoWorkEventArgs e)
        {
            string outXml = e.Argument.ToString();
            GetPacketEgeCerts(outXml);
        }
        void SetProgressInBatchRequest(object sender, ProgressChangedEventArgs e)
        {
            tbAns.Text = "[" + DateTime.Now.ToShortTimeString() + "] - " + e.UserState.ToString() + "\r\n";
        }
        private string GetPacketCheckXML(List<Guid> lstPersonIds)
        {
            string retString = "";

            XmlDocument doc = new XmlDocument();
            //XmlImplementation imp = new XmlImplementation();

            //создаём корневой элемент
            doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "items", ""));

            //создаём элементы AuthData и PackageData
            XmlNode root = doc["items"];

            using (PriemEntities context = new PriemEntities())
            {
                //не более 5000 записей
                foreach (Guid PersonId in lstPersonIds.Take(5000))
                {
                    var Person = context.Person.Where(x => x.Id == PersonId).FirstOrDefault();
                    if (Person != null)
                    {
                        //root.AppendChild(doc.CreateNode(XmlNodeType.Element, "query", ""));

                        ////заполняем данные AuthData
                        //XmlNode queryNode = root["query"];

                        XmlNode queryNode = doc.CreateNode(XmlNodeType.Element, "query", "");

                        queryNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "lastName", ""));
                        queryNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "firstName", ""));
                        queryNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "patronymicName", ""));
                        queryNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "passportSeria", ""));
                        queryNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "passportNumber", ""));
                        queryNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "certificateNumber", ""));
                        queryNode.AppendChild(doc.CreateNode(XmlNodeType.Element, "typographicNumber", ""));

                        queryNode["lastName"].InnerText = Person.Surname;
                        queryNode["firstName"].InnerText = Person.Name;
                        queryNode["firstName"].InnerText = Person.Name;
                        queryNode["patronymicName"].InnerText = Person.SecondName;
                        queryNode["passportSeria"].InnerText = Person.PassportSeries;
                        queryNode["passportNumber"].InnerText = Person.PassportNumber;

                        root.AppendChild(queryNode);
                    }
                }
            }

            var declaration = doc.CreateXmlDeclaration("1.0", "utf-8", "");

            retString = declaration.OuterXml + doc.InnerXml;

            return retString;
        }
        private void GetPacketEgeCerts(string queryXML)
        {
            MessageBox.Show("Starting...");
            bwBatchRequestWorker.ReportProgress(0, "Start....");
            FisEgeService.WSChecksSoapClient client = new FisEgeService.WSChecksSoapClient();
            string outXML = client.BatchCheck(new FisEgeService.UserCredentials() { Login = "p.karpenko@spbu.ru", Password = "E0k02II", Client = "p.karpenko@spbu.ru" }, queryXML);
            string ErrorMessage = "";
            Guid BatchId = GetBatchId(outXML, out ErrorMessage);
            MessageBox.Show("Answered...");
            if (BatchId == Guid.Empty)
            {
                MessageBox.Show("Error: " + ErrorMessage);
                bwBatchRequestWorker.ReportProgress(0, "Error: " + ErrorMessage);
            }
            else
            {
                bwBatchRequestWorker.ReportProgress(0, "BatchId=" + BatchId.ToString());
                GetCertificatesFromBatch(BatchId);
            }
        }
        private Guid GetBatchId(string outXml, out string ErrorMessage)
        {
            ErrorMessage = "";
            Guid gBatchId = Guid.Empty;
            XmlDocument doc = new XmlDocument(); 
            List<EgeCertificateClass> lstCerts = new List<EgeCertificateClass>();
            doc.LoadXml(outXml);

            XmlElement node = doc["checkResults"];
            if (node.HasChildNodes)
            {
                if (node.GetElementsByTagName("batchId").Count > 0)
                {
                    string BatchId = node["batchId"].InnerText;
                    Guid.TryParse(BatchId, out gBatchId);
                }
                else if (node.GetElementsByTagName("errors").Count > 0)
                {
                    foreach (XmlNode inNode in node.GetElementsByTagName("error"))
                    {
                        ErrorMessage += inNode.InnerText + "\n";
                    }
                }
            }

            return gBatchId;
        }
        private void GetCertificatesFromBatch(Guid BatchId)
        {
            FisEgeService.WSChecksSoapClient client = new FisEgeService.WSChecksSoapClient();
            string queryXML = GetXmlQueryForBatchRequest(BatchId);
            string outXml = "";
            BatchCheckStatus stat = BatchCheckStatus.InProgress;
            int cntr = 0;
            while (stat == BatchCheckStatus.InProgress && cntr < 100)
            {
                outXml = client.GetBatchCheckResult(new FisEgeService.UserCredentials() { Login = "p.karpenko@spbu.ru", Password = "E0k02II", Client = "p.karpenko@spbu.ru" }, queryXML);
                stat = GetBatchRequestStatus(outXml);
                //5 sec delay
                bwBatchRequestWorker.ReportProgress(cntr, " BatchId=" + BatchId.ToString() + " InProgress... ");
                System.Threading.Thread.Sleep(5 * 1000);
                cntr++;
            }

            bwBatchRequestWorker.ReportProgress(100, outXml);

            if (stat == BatchCheckStatus.Finished)
            {
                bwBatchRequestWorker.ReportProgress(cntr, " Finished... ");
                var lstCerts = GetCertsFromXML(outXml);
                //foreach (var Cert in lstCerts)
                //    SaveEgeCertificate(Cert);

                MessageBox.Show("Done! " + lstCerts.Count + " certs founded!");
            }
            else
                MessageBox.Show("Done! No certs founded");
        }
        private string GetXmlQueryForBatchRequest(Guid BatchId)
        {
            string retString = "";

            XmlDocument doc = new XmlDocument();
            //XmlImplementation imp = new XmlImplementation();

            //создаём корневой элемент
            doc.AppendChild(doc.CreateNode(XmlNodeType.Element, "items", ""));

            //создаём элементы AuthData и PackageData
            XmlNode root = doc["items"];

            XmlNode queryNode = doc.CreateNode(XmlNodeType.Element, "batchId", "");
            queryNode.InnerText = BatchId.ToString();
            root.AppendChild(queryNode);

            var declaration = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            retString = declaration.OuterXml + doc.InnerXml;
            return retString;
        }
        private BatchCheckStatus GetBatchRequestStatus(string outXml)
        {
            XmlDocument doc = new XmlDocument(); 
            List<EgeCertificateClass> lstCerts = new List<EgeCertificateClass>();
            doc.LoadXml(outXml);

            XmlElement node = doc["checkResults"];
            if (node.HasChildNodes)
            {
                if (node.GetElementsByTagName("statusCode").Count > 0)
                {
                    switch (node["statusCode"].InnerText)
                    {
                        case "0": return BatchCheckStatus.NotFound;
                        case "1": return BatchCheckStatus.InProgress;
                        case "2": return BatchCheckStatus.Finished;
                        default: return BatchCheckStatus.NotFound;
                    }
                }
                else
                    return BatchCheckStatus.NotFound;
            }
            else
                return BatchCheckStatus.NotFound;
        }

        private void btnUpdateExists_Click(object sender, EventArgs e)
        {
            bwSaveAll = new BackgroundWorker();
            bwSaveAll.DoWork += bw_DoWorksss2;
            bwSaveAll.RunWorkerCompleted += bw_RunWorkerCompletedssss;
            bwSaveAll.ProgressChanged += bw_ProgressChangedsss;
            bwSaveAll.WorkerReportsProgress = true;

            bwSaveAll.RunWorkerAsync();
        }
        void bw_DoWorksss2(object sender, DoWorkEventArgs e)
        {
            using (PriemEntities context = new PriemEntities())
            {
                var certs = context.EgeCertificate.Where(x => x.FBSStatusId == 0 && x.Year != "2014").Select(x => x.Id).Distinct().ToList();
                int iMax = certs.Count;
                int iCntr = 0;
                int iPerc = 0;
                int iPercOld = 0;
                foreach (var c in certs)
                {
                    iPerc = Convert.ToInt32(((double)++iCntr / (double)iMax) * 100d);

                    if (iPerc != iPercOld)
                    {
                        iPercOld = iPerc;
                        bwSaveAll.ReportProgress(iPerc);
                    }

                    GetCertDataAsync(c);
                }
            }
        }

        private void btnGetExists_Click(object sender, EventArgs e)
        {
            if (dgvAbits.SelectedCells.Count == 0)
                return;

            List<int> lstRwIds = new List<int>();
            for (int i = 0; i < dgvAbits.SelectedCells.Count; i++)
            {
                if (!lstRwIds.Contains(dgvAbits.SelectedCells[i].RowIndex))
                    lstRwIds.Add(dgvAbits.SelectedCells[i].RowIndex);
            }

            foreach (int rwInd in lstRwIds)
            {
                //int rwInd = dgvAbits.SelectedCells[0].RowIndex;
                if (rwInd < 0)
                    return;

                Guid PersonId;
                Guid.TryParse(dgvAbits["Id", rwInd].Value.ToString(), out PersonId);

                using (PriemEntities context = new PriemEntities())
                {
                    var certs = context.EgeCertificate.Where(x => x.FBSStatusId == 0 && x.Year != "2014" && (x.PrintNumber != "" || x.Number != "") && x.PersonId == PersonId).Select(x => x.Id).ToList();

                    foreach (var c in certs)
                        GetCertDataAsync(c);
                }
            }
        }

        BackgroundWorker bwSaveAll;
        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            bwSaveAll = new BackgroundWorker();
            bwSaveAll.DoWork += bw_DoWorksss;
            bwSaveAll.RunWorkerCompleted += bw_RunWorkerCompletedssss;
            bwSaveAll.ProgressChanged += bw_ProgressChangedsss;
            bwSaveAll.WorkerReportsProgress = true;

            bwSaveAll.RunWorkerAsync();
        }

        void bw_ProgressChangedsss(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 0)
                pbProgress.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
                tbReq.Text = ((dynamic)e.UserState).inXml;
                tbAns.Text = ((dynamic)e.UserState).outXml;
            }
        }
        void bw_RunWorkerCompletedssss(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Done!");
        }
        void bw_DoWorksss(object sender, DoWorkEventArgs e)
        {
            using (PriemEntities context = new PriemEntities())
            {
                var certs = context.Abiturient.Where(x => x.Person.Person_EducationInfo.SchoolTypeId != 4 && x.Entry.StudyLevel.StudyLevelGroup.Id == 1).Select(x => x.PersonId).Distinct().ToList();
                int iMax = certs.Count;
                int iCntr = 0;
                int iPerc = 0;
                int iPercOld = 0;
                foreach (var c in certs)
                {
                    iPerc = Convert.ToInt32(((double)++iCntr / (double)iMax) * 100d);

                    if (iPerc != iPercOld)
                    {
                        iPercOld = iPerc;
                        bwSaveAll.ReportProgress(iPerc);
                    }

                    GetDataAsync(c);
                }
                //MessageBox.Show("Done!");
            }
        }

        private void tbRegNumSearch_TextChanged(object sender, EventArgs e)
        {
            Search(dgvAbits, "PersonNum", tbRegNumSearch.Text);
        }
        private void tbFIOSearch_TextChanged(object sender, EventArgs e)
        {
            Search(dgvAbits, "FIO", tbFIOSearch.Text);
        }

        public void Search(DataGridView dgv, string sColumnName, string sPattern)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                object cellValue = dgv.Rows[i].Cells[sColumnName].Value;
                // Если ячейка грида соответствует полю таблицы имеющему значение NULL,
                // то значение ячейки (объект "Value") становится null,
                // чтобы избежать "null reference exception" в момент вызова метода ToString(),
                // присваиваем Value объект string.Empty
                cellValue = (cellValue == null ? string.Empty : cellValue);

                if (cellValue.ToString().StartsWith(sPattern, true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    //dgv.FirstDisplayedScrollingRowIndex = i;
                    //dgv.Rows[i].Selected = true;
                    dgv.CurrentCell = dgv[sColumnName, i];
                    break;
                }
            }
        }
    }

    public enum BatchCheckStatus { NotFound, InProgress, Finished }
}
