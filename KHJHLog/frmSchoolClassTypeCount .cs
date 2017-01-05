using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using FISCA.DSAClient;
using System.Xml.Linq;
using System.IO;
using Aspose.Cells;

namespace KHJHLog
{
    public partial class frmSchoolClassTypeCount : BaseForm
    {
        BackgroundWorker _bgWorker;
        Dictionary<string, string> _SchoolDict;
        List<SchoolClassTypeStudentCount> _SchoolClassTypeStudentCountList;

        Dictionary<string,SchoolClassFinalData> _SchoolClassTypeCountDict;
        
        List<string> _SelectSchoolList;

        Campus.Configuration.ConfigData cd = Campus.Configuration.Config.User["SchoolClassCountOption"];
        public frmSchoolClassTypeCount()
        {
            InitializeComponent();
            
            _SelectSchoolList = new List<string>();
            _SchoolDict = new Dictionary<string, string>();
            _SchoolClassTypeCountDict = new Dictionary<string,SchoolClassFinalData>();
            _SchoolClassTypeStudentCountList = new List<SchoolClassTypeStudentCount>();
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;
            _bgWorker.ProgressChanged += _bgWorker_ProgressChanged;
            _bgWorker.WorkerReportsProgress = true;
        }

        void _bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("各校人數班級類別統計產生中..", e.ProgressPercentage);
        }

        void _bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnQuery.Enabled = true;

            FISCA.Presentation.MotherForm.SetStatusBarMessage("各校人數班級類別統計產生完成");

            #region 列印

            Workbook wb = new Workbook();
            wb.Open(new MemoryStream(Properties.Resources.各校班級類別統計樣版));

            int rowIdx = 1;
            foreach (var School in _SchoolClassTypeCountDict.Keys)
            {
                //第一張 Sheet 各校班級類別班級數量統計
                wb.Worksheets[0].Cells[rowIdx, 0].PutValue(_SchoolClassTypeCountDict[School].SchoolName);
                wb.Worksheets[0].Cells[rowIdx, 1].PutValue(_SchoolClassTypeCountDict[School].NormalClassCount1);
                wb.Worksheets[0].Cells[rowIdx, 2].PutValue(_SchoolClassTypeCountDict[School].SportClassCount1);
                wb.Worksheets[0].Cells[rowIdx, 3].PutValue(_SchoolClassTypeCountDict[School].SkillClassCount1);
                wb.Worksheets[0].Cells[rowIdx, 4].PutValue(_SchoolClassTypeCountDict[School].IepClassCount1);
                wb.Worksheets[0].Cells[rowIdx, 5].PutValue(_SchoolClassTypeCountDict[School].NormalClassCount2);
                wb.Worksheets[0].Cells[rowIdx, 6].PutValue(_SchoolClassTypeCountDict[School].SportClassCount2);
                wb.Worksheets[0].Cells[rowIdx, 7].PutValue(_SchoolClassTypeCountDict[School].SkillClassCount2);
                wb.Worksheets[0].Cells[rowIdx, 8].PutValue(_SchoolClassTypeCountDict[School].IepClassCount2);
                wb.Worksheets[0].Cells[rowIdx, 9].PutValue(_SchoolClassTypeCountDict[School].NormalClassCount3);
                wb.Worksheets[0].Cells[rowIdx, 10].PutValue(_SchoolClassTypeCountDict[School].SportClassCount3);
                wb.Worksheets[0].Cells[rowIdx, 11].PutValue(_SchoolClassTypeCountDict[School].SkillClassCount3);
                wb.Worksheets[0].Cells[rowIdx, 12].PutValue(_SchoolClassTypeCountDict[School].IepClassCount3);

                //第二張 Sheet 各校班級類別班級人數統計
                wb.Worksheets[1].Cells[rowIdx, 0].PutValue(_SchoolClassTypeCountDict[School].SchoolName);
                wb.Worksheets[1].Cells[rowIdx, 1].PutValue(_SchoolClassTypeCountDict[School].NormalClassStudentCount1);
                wb.Worksheets[1].Cells[rowIdx, 2].PutValue(_SchoolClassTypeCountDict[School].SportClassStudentCount1);
                wb.Worksheets[1].Cells[rowIdx, 3].PutValue(_SchoolClassTypeCountDict[School].SkillClassStudentCount1);
                wb.Worksheets[1].Cells[rowIdx, 4].PutValue(_SchoolClassTypeCountDict[School].IepClassStudentCount1);
                wb.Worksheets[1].Cells[rowIdx, 5].PutValue(_SchoolClassTypeCountDict[School].NormalClassStudentCount2);
                wb.Worksheets[1].Cells[rowIdx, 6].PutValue(_SchoolClassTypeCountDict[School].SportClassStudentCount2);
                wb.Worksheets[1].Cells[rowIdx, 7].PutValue(_SchoolClassTypeCountDict[School].SkillClassStudentCount2);
                wb.Worksheets[1].Cells[rowIdx, 8].PutValue(_SchoolClassTypeCountDict[School].IepClassStudentCount2);
                wb.Worksheets[1].Cells[rowIdx, 9].PutValue(_SchoolClassTypeCountDict[School].NormalClassStudentCount3);
                wb.Worksheets[1].Cells[rowIdx, 10].PutValue(_SchoolClassTypeCountDict[School].SportClassStudentCount3);
                wb.Worksheets[1].Cells[rowIdx, 11].PutValue(_SchoolClassTypeCountDict[School].SkillClassStudentCount3);
                wb.Worksheets[1].Cells[rowIdx, 12].PutValue(_SchoolClassTypeCountDict[School].IepClassStudentCount3);

                rowIdx++;
            }


            int _rowIdx = 1;
            foreach (var classitem in _SchoolClassTypeStudentCountList)
            {
                wb.Worksheets[2].Cells[_rowIdx, 0].PutValue(classitem.SchoolName);
                wb.Worksheets[2].Cells[_rowIdx, 1].PutValue(classitem.ClassName);
                wb.Worksheets[2].Cells[_rowIdx, 2].PutValue(classitem.ClassStudentCount);
                wb.Worksheets[2].Cells[_rowIdx, 3].PutValue(classitem.GradeYear);

                /// 是否為 班級分類: 普通班
                wb.Worksheets[2].Cells[_rowIdx, 4].PutValue(classitem.NormalClass ? "O" : "");
                /// 是否為 班級分類: 體育班
                wb.Worksheets[2].Cells[_rowIdx, 5].PutValue(classitem.SportClass ? "O" : "");
                /// 是否為 班級分類: 美術班
                wb.Worksheets[2].Cells[_rowIdx, 6].PutValue(classitem.ArtClass ? "O" : "");
                /// 是否為 班級分類: 音樂班
                wb.Worksheets[2].Cells[_rowIdx, 7].PutValue(classitem.MusicClass ? "O" : "");
                /// 是否為 班級分類: 舞蹈班
                wb.Worksheets[2].Cells[_rowIdx, 8].PutValue(classitem.DanceClass ? "O" : "");
                /// 是否為 班級分類: 資優班
                wb.Worksheets[2].Cells[_rowIdx, 9].PutValue(classitem.GiftedClass ? "O" : "");
                /// 是否為 班級分類: 資源班
                wb.Worksheets[2].Cells[_rowIdx, 10].PutValue(classitem.ResourceClass ? "O" : "");
                /// 是否為 班級分類: 特教班
                wb.Worksheets[2].Cells[_rowIdx, 11].PutValue(classitem.IepClass ? "O" : "");
                /// 是否為 班級分類: 技藝專班
                wb.Worksheets[2].Cells[_rowIdx, 12].PutValue(classitem.SkillClass ? "O" : "");
                /// 是否為 班級分類: 機構式非學校自學班
                wb.Worksheets[2].Cells[_rowIdx, 13].PutValue(classitem.NoSchoolClass ? "O" : "");


                _rowIdx++;
            }

            Utility.ExprotXls("各校班級人數統計", wb); 
            #endregion
            
        }

        void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int cc = 5;
            _bgWorker.ReportProgress(1);
            // 取得學校 UDT 並加入 Dict
            _SchoolClassTypeStudentCountList.Clear();
            _SchoolClassTypeCountDict.Clear();
            Dictionary<string, string> SelectSchoolDict = new Dictionary<string, string>();
            foreach(string key in _SchoolDict.Keys)
            {
                if (_SelectSchoolList.Contains(key))
                    SelectSchoolDict.Add(key, _SchoolDict[key]);
            }
            
            _bgWorker.ReportProgress(5);

            foreach (string ScTitle in SelectSchoolDict.Keys)
            {
                string ScDSNS = SelectSchoolDict[ScTitle];

                //string ScDSNS = "test.kh.edu.tw";  //開發測試使用之DSNS
                
                try
                {
                    Connection con = new Connection();

                    //取得局端登入後Greening發的Passport，並登入指定的Contract
                    con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "ischool.kh.central_office.user", FISCA.Authentication.DSAServices.PassportToken);

                    //取得該Contract所發的Passport
                    Envelope Response = con.SendRequest("DS.Base.GetPassportToken", new Envelope());
                    PassportToken Passport = new PassportToken(Response.Body);

                    //TODO：拿此Passport登入各校
                    Connection conSchool = new Connection();
                    conSchool.Connect(ScDSNS, "ischool.kh.central_office", Passport);

                    Response = conSchool.SendRequest("_.GetClassStudentCount", new Envelope());
                    
                    XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString)); 
                    
                    foreach (XElement elmClass in elmResponse.Elements("Class"))
                    {
                        string ClassName = elmClass.ElementText("ClassName");
                        
                        //// 班級人數
                        //string StudentCount = elmClass.ElementText("StudentCount");

                        // 編班人數
                        //string ClassStudentCount = elmClass.ElementText("ClassStudentCount");

                        //// 班級人數 
                        string ClassStudentCount = elmClass.ElementText("StudentCount");

                        //2017/1/5 穎驊註解， 雖然看起來很像，但是 "編班人數" 不等於 "班級人數"
                        //編班人數比較像是 星海魔獸 中的人口概念， 有"人口負載力"的感覺
                        //若該班 有特殊學生，該特殊生狀況需要多照顧，其一人 就可能占住 兩份"編班人數"

                        //年級
                        string GradeYear = elmClass.ElementText("GradeYear");

                        bool NormalClass    = Convert.ToBoolean(elmClass.ElementText("NormalClass") != "" ? elmClass.ElementText("NormalClass"): "false") ;
                        bool SportClass     = Convert.ToBoolean(elmClass.ElementText("SportClass") != "" ? elmClass.ElementText("SportClass") : "false");
                        bool ArtClass       = Convert.ToBoolean(elmClass.ElementText("ArtClass") != "" ? elmClass.ElementText("ArtClass") : "false");
                        bool MusicClass     = Convert.ToBoolean(elmClass.ElementText("MusicClass") != "" ? elmClass.ElementText("MusicClass") : "false");
                        bool DanceClass     = Convert.ToBoolean(elmClass.ElementText("DanceClass") != "" ? elmClass.ElementText("DanceClass") : "false");
                        bool GiftedClass    = Convert.ToBoolean(elmClass.ElementText("GiftedClass") != "" ? elmClass.ElementText("GiftedClass") : "false");
                        bool ResourceClass  = Convert.ToBoolean(elmClass.ElementText("ResourceClass") != "" ? elmClass.ElementText("ResourceClass") : "false");
                        bool IepClass       = Convert.ToBoolean(elmClass.ElementText("IepClass") != "" ? elmClass.ElementText("IepClass") : "false");
                        bool SkillClass     = Convert.ToBoolean(elmClass.ElementText("SkillClass") != "" ? elmClass.ElementText("SkillClass") : "false");
                        bool NoSchoolClass  = Convert.ToBoolean(elmClass.ElementText("NoSchoolClass") != "" ? elmClass.ElementText("NoSchoolClass") : "false");

                        int cot= 0;
                        if (GradeYear!="")
                        {
                            int.TryParse(ClassStudentCount, out cot);
                            SchoolClassTypeStudentCount scts = new SchoolClassTypeStudentCount();
                            
                            scts.SchoolName = ScTitle;                            
                            scts.ClassName = ClassName;                            
                            scts.ClassStudentCount = cot;
                            scts.GradeYear = GradeYear;
                                
                            scts.NormalClass = NormalClass;
                            scts.SportClass = SportClass;
                            scts.ArtClass = ArtClass;
                            scts.MusicClass = MusicClass;
                            scts.DanceClass = DanceClass;
                            scts.GiftedClass = GiftedClass;
                            scts.ResourceClass = ResourceClass;
                            scts.IepClass = IepClass;
                            scts.SkillClass = SkillClass;
                            scts.NoSchoolClass = NoSchoolClass;
                                                            
                            _SchoolClassTypeStudentCountList.Add(scts);                            
                        }
                    }
                   
                    cc++;
                    if (cc > 99)
                        cc = 99;
                    
                    _bgWorker.ReportProgress(cc);
                }
                catch (Exception ex)
                {

                }
            }

            foreach(var classitem in _SchoolClassTypeStudentCountList)            
            {
                if (!_SchoolClassTypeCountDict.ContainsKey(classitem.SchoolName))
                {
                    SchoolClassFinalData SCFD = new SchoolClassFinalData();

                    SCFD.SchoolName = classitem.SchoolName;

                    #region 一年級班級整理
                    if (classitem.GradeYear == "1" || classitem.GradeYear == "7")
                    {
                        //一年級普通班
                        if (classitem.NormalClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount1++;
                            SCFD.NormalClassStudentCount1 += classitem.ClassStudentCount;
                        }
                        //一年級體育班
                        if (classitem.SportClass)
                        {
                            //最終分類為體育班
                            SCFD.SportClassCount1++;
                            SCFD.SportClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級美術班
                        if (classitem.ArtClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount1++;
                            SCFD.SkillClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級音樂班
                        if (classitem.MusicClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount1++;
                            SCFD.SkillClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級舞蹈班
                        if (classitem.DanceClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount1++;
                            SCFD.SkillClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級資優班
                        if (classitem.GiftedClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount1++;
                            SCFD.NormalClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級資源班
                        if (classitem.ResourceClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount1++;
                            SCFD.NormalClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級特教班
                        if (classitem.IepClass)
                        {
                            //最終分類為特教班
                            SCFD.IepClassCount1++;
                            SCFD.IepClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級技藝專班
                        if (classitem.SkillClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount1++;
                            SCFD.SkillClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級機構式非學校自學班
                        if (classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount1++;
                            SCFD.NormalClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //假如都沒有設定， 當作普通班
                        if (!classitem.NormalClass && !classitem.SportClass && !classitem.ArtClass && !classitem.MusicClass && !classitem.DanceClass && !classitem.GiftedClass && !classitem.ResourceClass && !classitem.IepClass && !classitem.SkillClass && !classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount1++;
                            SCFD.NormalClassStudentCount1 += classitem.ClassStudentCount;
                        }
                    } 
                    #endregion
                    #region 二年級班級整理
                    if (classitem.GradeYear == "2" || classitem.GradeYear == "8")
                    {
                        //二年級普通班
                        if (classitem.NormalClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount2++;
                            SCFD.NormalClassStudentCount2 += classitem.ClassStudentCount;
                        }
                        //二年級體育班
                        if (classitem.SportClass)
                        {
                            //最終分類為體育班
                            SCFD.SportClassCount2++;
                            SCFD.SportClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級美術班
                        if (classitem.ArtClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount2++;
                            SCFD.SkillClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級音樂班
                        if (classitem.MusicClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount2++;
                            SCFD.SkillClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級舞蹈班
                        if (classitem.DanceClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount2++;
                            SCFD.SkillClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級資優班
                        if (classitem.GiftedClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount2++;
                            SCFD.NormalClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級資源班
                        if (classitem.ResourceClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount2++;
                            SCFD.NormalClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級特教班
                        if (classitem.IepClass)
                        {
                            //最終分類為特教班
                            SCFD.IepClassCount2++;
                            SCFD.IepClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級技藝專班
                        if (classitem.SkillClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount2++;
                            SCFD.SkillClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級機構式非學校自學班
                        if (classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount2++;
                            SCFD.NormalClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //假如都沒有設定， 當作普通班
                        if (!classitem.NormalClass && !classitem.SportClass && !classitem.ArtClass && !classitem.MusicClass && !classitem.DanceClass && !classitem.GiftedClass && !classitem.ResourceClass && !classitem.IepClass && !classitem.SkillClass && !classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount2++;
                            SCFD.NormalClassStudentCount2 += classitem.ClassStudentCount;
                        }
                    } 
                    #endregion
                    #region 三年級班級整理
                    if (classitem.GradeYear == "3" || classitem.GradeYear == "9")
                    {
                        //三年級普通班
                        if (classitem.NormalClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount3++;
                            SCFD.NormalClassStudentCount3 += classitem.ClassStudentCount;
                        }
                        //三年級體育班
                        if (classitem.SportClass)
                        {
                            //最終分類為體育班
                            SCFD.SportClassCount3++;
                            SCFD.SportClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級美術班
                        if (classitem.ArtClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount3++;
                            SCFD.SkillClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級音樂班
                        if (classitem.MusicClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount3++;
                            SCFD.SkillClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級舞蹈班
                        if (classitem.DanceClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount3++;
                            SCFD.SkillClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級資優班
                        if (classitem.GiftedClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount3++;
                            SCFD.NormalClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級資源班
                        if (classitem.ResourceClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount3++;
                            SCFD.NormalClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級特教班
                        if (classitem.IepClass)
                        {
                            //最終分類為特教班
                            SCFD.IepClassCount3++;
                            SCFD.IepClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級技藝專班
                        if (classitem.SkillClass)
                        {
                            //最終分類為藝才班
                            SCFD.SkillClassCount3++;
                            SCFD.SkillClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級機構式非學校自學班
                        if (classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount3++;
                            SCFD.NormalClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //假如都沒有設定， 當作普通班
                        if (!classitem.NormalClass && !classitem.SportClass && !classitem.ArtClass && !classitem.MusicClass && !classitem.DanceClass && !classitem.GiftedClass && !classitem.ResourceClass && !classitem.IepClass && !classitem.SkillClass && !classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            SCFD.NormalClassCount3++;
                            SCFD.NormalClassStudentCount3 += classitem.ClassStudentCount;
                        }
                    } 
                    #endregion
                           
                    _SchoolClassTypeCountDict.Add(classitem.SchoolName, SCFD);
                }
                else 
                {
                    _SchoolClassTypeCountDict[classitem.SchoolName].SchoolName = classitem.SchoolName;

                    #region 一年級班級整理
                    if (classitem.GradeYear == "1" || classitem.GradeYear == "7")
                    {
                        //一年級普通班
                        if (classitem.NormalClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount1 += classitem.ClassStudentCount;
                        }
                        //一年級體育班
                        if (classitem.SportClass)
                        {
                            //最終分類為體育班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SportClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SportClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級美術班
                        if (classitem.ArtClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級音樂班
                        if (classitem.MusicClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級舞蹈班
                        if (classitem.DanceClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級資優班
                        if (classitem.GiftedClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級資源班
                        if (classitem.ResourceClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級特教班
                        if (classitem.IepClass)
                        {
                            //最終分類為特教班
                            _SchoolClassTypeCountDict[classitem.SchoolName].IepClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].IepClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級技藝專班
                        if (classitem.SkillClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount1 += classitem.ClassStudentCount;

                        }
                        //一年級機構式非學校自學班
                        if (classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount1 += classitem.ClassStudentCount;

                        }

                        //假如都沒有設定， 當作普通班
                        if (!classitem.NormalClass && !classitem.SportClass && !classitem.ArtClass && !classitem.MusicClass && !classitem.DanceClass && !classitem.GiftedClass && !classitem.ResourceClass && !classitem.IepClass && !classitem.SkillClass && !classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount1++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount1 += classitem.ClassStudentCount;
                        }

                    } 
                    #endregion
                    #region 二年級班級整理
                    if (classitem.GradeYear == "2" || classitem.GradeYear == "8")
                    {
                        //二年級普通班
                        if (classitem.NormalClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount2 += classitem.ClassStudentCount;
                        }
                        //二年級體育班
                        if (classitem.SportClass)
                        {
                            //最終分類為體育班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SportClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SportClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級美術班
                        if (classitem.ArtClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級音樂班
                        if (classitem.MusicClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級舞蹈班
                        if (classitem.DanceClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級資優班
                        if (classitem.GiftedClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級資源班
                        if (classitem.ResourceClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級特教班
                        if (classitem.IepClass)
                        {
                            //最終分類為特教班
                            _SchoolClassTypeCountDict[classitem.SchoolName].IepClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].IepClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級技藝專班
                        if (classitem.SkillClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //二年級機構式非學校自學班
                        if (classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount2 += classitem.ClassStudentCount;

                        }
                        //假如都沒有設定， 當作普通班
                        if (!classitem.NormalClass && !classitem.SportClass && !classitem.ArtClass && !classitem.MusicClass && !classitem.DanceClass && !classitem.GiftedClass && !classitem.ResourceClass && !classitem.IepClass && !classitem.SkillClass && !classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount2++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount2 += classitem.ClassStudentCount;
                        }
                    } 
                    #endregion
                    #region 三年級班級整理
                    if (classitem.GradeYear == "3" || classitem.GradeYear == "9")
                    {
                        //三年級普通班
                        if (classitem.NormalClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount3 += classitem.ClassStudentCount;
                        }
                        //三年級體育班
                        if (classitem.SportClass)
                        {
                            //最終分類為體育班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SportClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SportClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級美術班
                        if (classitem.ArtClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級音樂班
                        if (classitem.MusicClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級舞蹈班
                        if (classitem.DanceClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級資優班
                        if (classitem.GiftedClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級資源班
                        if (classitem.ResourceClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級特教班
                        if (classitem.IepClass)
                        {
                            //最終分類為特教班
                            _SchoolClassTypeCountDict[classitem.SchoolName].IepClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].IepClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級技藝專班
                        if (classitem.SkillClass)
                        {
                            //最終分類為藝才班
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].SkillClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //三年級機構式非學校自學班
                        if (classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount3 += classitem.ClassStudentCount;

                        }
                        //假如都沒有設定， 當作普通班
                        if (!classitem.NormalClass && !classitem.SportClass && !classitem.ArtClass && !classitem.MusicClass && !classitem.DanceClass && !classitem.GiftedClass && !classitem.ResourceClass && !classitem.IepClass && !classitem.SkillClass && !classitem.NoSchoolClass)
                        {
                            //最終分類為普通班
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassCount3++;
                            _SchoolClassTypeCountDict[classitem.SchoolName].NormalClassStudentCount3 += classitem.ClassStudentCount;
                        }
                    } 
                    #endregion
                                                                                
                }                        
            }
            _bgWorker.ReportProgress(100);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSchoolClassCount_Load(object sender, EventArgs e)
        {
            
            // 取得學校並放入選單
            AccessHelper accHepler = new AccessHelper();
            _SchoolDict.Clear();
            List<School> SchoolList = accHepler.Select<School>();
            SchoolList = (from data in SchoolList orderby data.Title ascending select data).ToList();

            foreach (School sc in SchoolList)
            {
                if (!_SchoolDict.ContainsKey(sc.Title))
                    _SchoolDict.Add(sc.Title, sc.DSNS);
            }

            List<string> ssc = cd["SelectSchool"].Split(',').ToList();
            if (ssc == null)
                ssc = new List<string>();

            foreach(string sc in _SchoolDict.Keys)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Name = sc;
                lvi.Text = sc;
                if (ssc.Contains(lvi.Text))
                    lvi.Checked = true;
                else
                    lvi.Checked = false;
                lvSchool.Items.Add(lvi);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            } else
            {
                if(lvSchool.CheckedItems.Count>0)
                {
                    _SelectSchoolList.Clear();
                    foreach(ListViewItem lvi in lvSchool.CheckedItems)
                        _SelectSchoolList.Add(lvi.Text);

                    btnQuery.Enabled = false;
                    
                    cd["SelectSchool"] = string.Join(",", _SelectSchoolList.ToArray());
                    cd.Save();
                    _bgWorker.RunWorkerAsync();
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請勾選學校.");
                }                
            }
        }

        private void chkSchool_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvSchool.Items)
                lvi.Checked = chkSchool.Checked;
        }
    }
}
