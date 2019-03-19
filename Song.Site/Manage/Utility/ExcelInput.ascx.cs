using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Xml;
using System.Collections.Generic;
using System.Data.OleDb;
using WeiSha.Common;


namespace Song.Site.Manage.Utility
{
    public partial class ExcelInput : System.Web.UI.UserControl
    {
        #region ���ԣ��¼�
        public event EventHandler Input;
        //�ĵ��ϴ������ʱ���·��
        private string _tempPathConfig = "Temp";
        //Excel�����ַ���
        string connStr = "Provider=Microsoft.Ace.OleDb.12.0;data source={0};Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
        /// <summary>
        /// ����ģ�������
        /// </summary>
        public string TemplateName
        {
            get
            {
                object obj = ViewState["TemplateName"];
                return (obj == null) ? "" : obj.ToString();
            }
            set
            {
                ViewState["TemplateName"] = value;
            }
        }
        /// <summary>
        /// ����ģ���·��
        /// </summary>
        public string TemplatePath
        {
            get
            {
                object obj = ViewState["TemplatePath"];
                return (obj == null) ? "" : obj.ToString();
            }
            set
            {
                ViewState["TemplatePath"] = value;
            }
        }
        /// <summary>
        /// ��¼Excel���������ݿ��ֶζ�Ӧ��ϵ�������ļ����ļ���
        /// </summary>
        public string Config
        {
            get
            {
                object obj = ViewState["Config"];
                return (obj == null) ? "" : obj.ToString();
            }
            set
            {
                ViewState["Config"] = value;
            }
        }
        public List<string> Keys
        {
            get
            {
                return GetKeys();
            }
        }
        private DataTable _sheetData;
        /// <summary>
        /// ��ǰ��Ҫ����Ĺ����������ݼ�
        /// </summary>
        public DataTable SheetDataTable
        {
            get { return _sheetData; }
            set { _sheetData = value; }
        }
        private List<DataRow> _errorDataRow = new List<DataRow>();
        /// <summary>
        /// ����ʧ�ܵ����ݼ�
        /// </summary>
        public List<DataRow> ErrorDataRows
        {
            get { return _errorDataRow; }
        }
        private Dictionary<String, String> _dataRelation;
        /// <summary>
        /// Excel���������ݿ��ֶζ�Ӧ��ϵ��ǰ��ΪExcel����(keyֵ��������Ϊ���ݿ��ֶ�����valueֵ��
        /// </summary>
        public Dictionary<String, String> DataRelation
        {
            get { return _dataRelation; }
            set { _dataRelation = value; }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //����ģ��
                linkDataTmp.NavigateUrl = this.ResolveUrl(this.TemplatePath);
                linkDataTmp.Text = linkDataTmp.Text.Replace("{0}", this.TemplateName);
            }
        }

        /// <summary>
        /// ��ӵ���ʧ�ܵ�����
        /// </summary>
        /// <param name="dr"></param>
        public void AddError(DataRow dr)
        {
            _errorDataRow.Add(dr);
        }
        /// <summary>
        /// ��һ�����ϴ������ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuLoad.PostedFile.FileName != "")
                {
                    fuLoad.UpPath = _tempPathConfig;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = false;
                    fuLoad.SaveAs();
                    ViewState["dataFilePath"] = fuLoad.File.Server.FileFullName;
                    //������
                    DataTable table = this.GetSheets(fuLoad.File.Server.FileFullName);
                    dlWorkBook.DataSource = table;
                    dlWorkBook.DataBind();
                    //״̬                    
                    ltSheetCount.Text = table.Rows.Count.ToString();
                    //
                    lbState.Text = "���ڲ����ĵ� ��" + fuLoad.File.Client.FileName + "��";
                    btnNext1.Visible = true;
                    lbFile2.Text = fuLoad.File.Client.FileName;
                    //
                    fdPanel1.Visible = false;
                    fdPanel2.Visible = true;
                    lbError2.Text = "";
                    //���ֻ��һ����������ֱ��������һ��
                    if (table.Rows.Count == 1)
                    {
                        foreach (DataListItem dli in dlWorkBook.Items)
                        {
                            Button lb = (Button)dli.FindControl("btnWorkBook");
                            btnSheet_Click(lb, null);
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                lbError1.Text = ex.Message;
            }
        }
        /// <summary>
        /// �ڶ�����������ѡ��İ�ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSheet_Click(object sender, EventArgs e)
        {
            lbFile3.Text = lbFile2.Text;
            //��ǰ�����Ĺ�����
            string sheetText = ((Button)sender).Text;
            lbSheet3.Text = "��" + sheetText.Substring(0, sheetText.IndexOf("��")) + "����" + sheetText.Substring(sheetText.IndexOf("��") + 1);
            //Ҫ������Excel�ļ����빤������
            string xlsFile = ViewState["dataFilePath"].ToString();
            //������������
            int sheetIndex = Convert.ToInt32(((Button)sender).CommandArgument);
            ViewState["sheetIndex"] = sheetIndex;
            try
            {
                //DataTable dtRows = SheetToDatatable(xlsFile, sheetIndex);
                //if (dtRows.Rows.Count < 1) throw new Exception("��ǰ������û������");
                //��ȡ����������
                DataTable dtColumn = this.GetSheetColumn(xlsFile, sheetIndex);
                dlColumn.DataSource = dtColumn;
                dlColumn.DataBind();
                //Ԥ�õ������ֶεĶ�Ӧ��ϵ
                DataTable dtConfig = getConfig();
                //�Զ�ʶ��excel���ֶ������ݿ����ַ�ƥ��
                for (int i = 0; i < dlColumn.Items.Count; i++)
                {
                    DataListItem dli = dlColumn.Items[i];
                    //����
                    Label lb = (Label)dli.FindControl("lbColumn");
                    //�󶨶�Ӧ��ϵ������
                    DropDownList ddl = (DropDownList)dli.FindControl("ddlColumnForField");
                    ddl.DataSource = dtConfig;
                    ddl.DataTextField = "Column";
                    ddl.DataValueField = "Field";
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("", ""));
                    //�Զ����ö�Ӧ��ϵ
                    setDataList(lb.Text, ddl);
                    if (i > 30) break;
                }
                fdPanel2.Visible = false;
                fdPanel3.Visible = true;
                lbError3.Text = "";
            }
            catch (Exception ex)
            {
                lbError2.Text = ex.Message;
            }
        }
        /// <summary>
        /// ���������������ݵİ�ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInput_Click(object sender, EventArgs e)
        {
            fdPanel5.Visible = false;
            btnOutpt.Visible = false;
            try
            {
                //ʵ�ʵ������ֶεĹ�ϵ
                this._dataRelation = getColumnForField();
                //������������
                string file = ViewState["dataFilePath"].ToString();
                int sheetIndex = Convert.ToInt32(ViewState["sheetIndex"].ToString());
                this._sheetData = this.SheetToDatatable(file, sheetIndex);
                //*****************
                //ִ�е������ݵ��¼�
                if (Input != null)
                    this.Input(sender, e);
                //
                lbFile4.Text = lbFile3.Text;
                lbSheet4.Text = lbSheet3.Text;
                fdPanel3.Visible = false;
                fdPanel4.Visible = true;
                //�������Ĵ���
                lbErrorCount.Text = ErrorDataRows.Count.ToString();
                lbSuccCount.Text = (SheetDataTable.Rows.Count - ErrorDataRows.Count).ToString();
                if (ErrorDataRows.Count > 0)
                {
                    fdPanel5.Visible = true;
                    btnOutpt.Visible = true;
                    //���������
                    DataTable dtErr = SheetDataTable.Clone();
                    foreach (DataRow dr in ErrorDataRows)
                    {
                        dtErr.ImportRow(dr);
                    }
                    gvError.DataSource = dtErr;
                    gvError.DataBind();
                }
            }
            catch (Exception ex)
            {
                lbError3.Text = ex.Message;
            }
        }

        #region ����Excel�ķ���
        /// <summary>
        /// ͨ��Excel�ĵ���������Ӧ�Ĵ������
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <returns></returns>
        private IWorkbook createWorkbook(string xlsFile)
        {
            //��������������
            IWorkbook workbook = null;
            using (FileStream file = new FileStream(xlsFile, FileMode.Open, FileAccess.Read))
            {               
                //������չ���ж�excel�汾
                string ext = xlsFile.Substring(xlsFile.LastIndexOf(".") + 1);
                //WorkbookFactory.Create(file);
                if (ext.ToLower() == "xls") workbook = new HSSFWorkbook(file);
                if (ext.ToLower() == "xlsx") workbook = new XSSFWorkbook(file);
            }
            return workbook;
        }
        /// <summary>
        /// ��Excel�ж�ȡһ��������������Datatable����
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        private DataTable SheetToDatatable(string xlsFile, int sheetIndex)
        {
            DataTable dt = new DataTable();
            //��������������
            IWorkbook workbook = createWorkbook(xlsFile);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            try
            {
                rows.MoveNext();
                //����Datatable�ṹ
                HSSFRow firsRow = (HSSFRow)rows.Current;
                for (int i = 0; i < firsRow.LastCellNum; i++)
                {
                    ICell cell = firsRow.GetCell(i);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    dt.Columns.Add(new DataColumn(cell.ToString(), getColumnType(cell.ToString())));
                }
                //���빤����������
                while (rows.MoveNext())
                {
                    HSSFRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row.GetCell(i);
                        if (cell == null) continue;
                        string value = cell.ToString();
                        //��ȡExcel��ʽ�����ݸ�ʽ��ȡ��������
                        switch (dt.Columns[i].DataType.FullName)
                        {
                            case "System.DateTime": //��������                                   
                                if (DateUtil.IsValidExcelDate(cell.NumericCellValue))
                                {
                                    try
                                    {
                                        value = cell.DateCellValue.ToString();
                                    }
                                    catch
                                    {
                                        value = cell.ToString();
                                    }
                                }
                                else
                                {
                                    value = cell.NumericCellValue.ToString();
                                }
                                break;

                            default:
                                value = cell.ToString();
                                break;
                        }
                        dr[i] = WeiSha.Common.Param.Method.ConvertToAnyValue.Get(value).ChangeType(dt.Columns[i].DataType);

                    }
                    dt.Rows.Add(dr);
                }
            }
            catch
            {
                return dt;
            }
            return dt;
        }
        /// <summary>
        /// ��ȡ�е���������
        /// </summary>
        /// <param name="colname"></param>
        /// <returns></returns>
        private System.Type getColumnType(string colname)
        {
            DataTable dtConfing = getConfig();
            System.Type type=null;
            foreach (DataRow dr in dtConfing.Rows)
            {
                if (colname.ToLower().Trim() == dr["Column"].ToString().ToLower().Trim())
                {
                    if (dr["DataType"].ToString().ToLower().Trim() == "date") type = Type.GetType("System.DateTime");
                }
            }
            if (type == null) type = Type.GetType("System.String");
            return type;
        }
        /// <summary>
        /// ��ȡ�ĵ��е����й�����
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <returns></returns>
        private DataTable GetSheets(string xlsFile)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Name"));
            dt.Columns.Add(new DataColumn("Count"));
            //��������������
            IWorkbook workbook = createWorkbook(xlsFile);
            int sheetNum = workbook.NumberOfSheets;
            for (int i = 0; i < sheetNum; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Name"] = workbook.GetSheetAt(i).SheetName;
                dr["Count"] = workbook.GetSheetAt(i).LastRowNum;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// ��ȡ���������б�����һ�еı���
        /// </summary>
        /// <param name="xlsFile"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        private DataTable GetSheetColumn(string xlsFile, int sheetIndex)
        {
            DataTable dtSch = new DataTable("SheetStructure");
            dtSch.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));
            //��������������
            IWorkbook workbook = createWorkbook(xlsFile);   
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            rows.MoveNext();
            //����Datatable�ṹ
            IRow firsRow = (IRow)rows.Current;
            for (int i = 0; i < firsRow.LastCellNum; i++)
            {
                DataRow dr = dtSch.NewRow();
                ICell cell = firsRow.GetCell(i);
                dr["Name"] = cell == null ? "(null)" + i : cell.ToString();
                dtSch.Rows.Add(dr);
            }
            return dtSch;
        }
        #endregion

        #region �����ֶζԱȵķ���
        /// <summary>
        /// ͨ���ֶΣ���ȡ��Ӧ������
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string GetColumnForField(string field)
        {
            Dictionary<String, String> dic = this._dataRelation;
            if (dic.Count < 1) return "";
            foreach (KeyValuePair<String, String> kvp in dic)
            {
                if (kvp.Value == field) return kvp.Key;
            }
            return "";
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeys()
        {
            List<string> keys = new List<string>();
            //�����ļ���·��
            string path = App.Get["ExcelInputConfig"].VirtualPath;
            string config = this.Server.MapPath(path + this.Config);
            if (!System.IO.File.Exists(config)) return keys;
            //����
            XmlDocument resXml = new XmlDocument();
            resXml.Load(config);
            //��ȡ����
            XmlNode ele = resXml.LastChild;
            if (ele.Attributes["Keys"] == null) return keys;
            string[] keyscol = ele.Attributes["Keys"].Value.Split(',');
            //ƥ�������������
            XmlNodeList nodes = resXml.GetElementsByTagName("item");
            //           
            foreach (string k in keyscol)
            {
                foreach (XmlNode n in nodes)
                {
                    string column = ((XmlElement)n).Attributes["Column"].Value;
                    string field = ((XmlElement)n).Attributes["Field"].Value;
                    if (k.Trim().ToLower() == column.Trim().ToLower())
                    {
                        keys.Add(field.Trim());
                    }
                }
            }
            return keys;
        }
        /// <summary>
        /// ͨ����������ȡ��Ӧ���ֶ�
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public string GetFieldForColumn(string column)
        {
            Dictionary<String, String> dic = this._dataRelation;
            if (dic.Count < 1) return "";
            foreach (KeyValuePair<String, String> kvp in dic)
            {
                if (kvp.Key == column) return kvp.Value;
            }
            return "";
        }
        /// <summary>
        /// ��ȡ�������ֶ����Ķ�Ӧ��ϵ������
        /// </summary>
        private DataTable getConfig()
        {
            //�����
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Column", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Field", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("DataType", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Format", Type.GetType("System.String")));
            //�����ļ���·��
            string path = App.Get["ExcelInputConfig"].VirtualPath;
            string config = this.Server.MapPath(path + this.Config);
            if (!System.IO.File.Exists(config)) return dt;
            //����
            XmlDocument resXml = new XmlDocument();
            resXml.Load(config);
            XmlNodeList nodes = resXml.GetElementsByTagName("item");
            foreach (XmlNode n in nodes)
            {
                XmlElement el = (XmlElement)n;
                DataRow dr = dt.NewRow();
                dr["Column"] = el.Attributes["Column"].Value;
                dr["Field"] = el.Attributes["Field"].Value;
                dr["DataType"] = el.Attributes["DataType"] != null ? el.Attributes["DataType"].Value : null;
                dr["Format"] = el.Attributes["Format"] != null ? el.Attributes["Format"].Value : null;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// ��ȡʵ�ʵ������ֶεĹ�ϵ,Dictionary<String, String> ǰ��Ϊ����������Ϊ�ֶ���
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, String> getColumnForField()
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            for (int i = 0; i < dlColumn.Items.Count; i++)
            {
                DataListItem dli = dlColumn.Items[i];
                //��������Ӧ���ֶ�
                Label lb = (Label)dli.FindControl("lbColumn");
                DropDownList ddl = (DropDownList)dli.FindControl("ddlColumnForField");
                if (!dic.ContainsKey(lb.Text))
                    dic.Add(lb.Text, ddl.SelectedValue);
            }
            return dic;
        }
        /// <summary>
        /// ��������ѡ���е��ĸ��Ϊѡ��״̬
        /// </summary>
        /// <param name="label">�ֶ����ƣ���Ӧ���ݿ�˵����</param>
        /// <param name="ddl"></param>
        private void setDataList(string label, DropDownList ddl)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (label == ddl.Items[i].Text)
                {
                    ddl.Items[i].Selected = true;
                    return;
                }

            }
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (isExts(label, ddl.Items[i].Text))
                {
                    ddl.Items[i].Selected = true;
                    break;
                }

            }
        }
        /// <summary>
        /// �ж�ǰ���ַ����Ƿ�����ں���
        /// </summary>
        /// <param name="ea"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool isExts(string ea, string str)
        {
            bool isExt = false;
            if (ea.Trim() == str.Trim()) return true;
            if (str.IndexOf(ea) > -1) isExt = true;
            if (ea.Length > 2)
            {
                for (int i = 0; i <= (ea.Length - 2); i++)
                {
                    string t = ea.Substring(i, 2);
                    if (str.IndexOf(t) > -1)
                    {
                        isExt = true;
                        break;
                    }
                }
            }
            return isExt;
        }
        #endregion

        #region ������ť
        protected void btnBack2_Click(object sender, EventArgs e)
        {
            fdPanel1.Visible = true;
            fdPanel2.Visible = false;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            fdPanel2.Visible = true;
            fdPanel3.Visible = false;
        }
        /// <summary>
        /// �������������Ĺ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack4_Click(object sender, EventArgs e)
        {
            fdPanel2.Visible = true;
            fdPanel4.Visible = false;
            lbError2.Text = "";
            fdPanel5.Visible = false;
        }
        /// <summary>
        /// ��������Excel�ĵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack5_Click(object sender, EventArgs e)
        {
            fdPanel1.Visible = true;
            fdPanel4.Visible = false;
            lbState.Text = "�ȴ��ϴ�����";
            btnNext1.Visible = false;
            lbError1.Text = "";
            fdPanel5.Visible = false;
        }


        protected void btnNext1_Click(object sender, EventArgs e)
        {
            fdPanel1.Visible = false;
            fdPanel2.Visible = true;
        }
        #endregion

        #region ������������
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOutpt_Click(object sender, EventArgs e)
        {
            //����Excel����
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //��������������
            ISheet sheet = hssfworkbook.CreateSheet(this.TemplateName);
            //sheet.DefaultColumnWidth = 30;
            //���������ж���
            IRow rowHead = sheet.CreateRow(0);
            //���ɱ�ͷ
            for (int i = 0; i < gvError.HeaderRow.Cells.Count; i++)
            {
                string txt = gvError.HeaderRow.Cells[i].Text;
                txt = txt.Replace("&nbsp;", "");
                rowHead.CreateCell(i).SetCellValue(txt);
            }
            //����������
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            for (int i = 0; i < gvError.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                GridViewRow gvr = gvError.Rows[i];
                for (int j = 0; j < gvr.Cells.Count; j++)
                {
                    string txt = gvr.Cells[j].Text;
                    if (string.IsNullOrEmpty(txt)) continue;
                    txt = txt.Replace("&nbsp;", "");
                    txt = txt.Replace("\n", "");
                    txt = txt.Replace("\r", "");
                    ICell cell = row.CreateCell(j);
                    cell.SetCellValue(txt.Trim());
                    cell.CellStyle = style_size;
                    if (txt.Length > 20)
                    {
                        cell.Sheet.SetColumnWidth(j, 100 * 256);
                    }
                }

            }
            //�����ļ�
            string filePath = Upload.Get["Temp"].Physics + WeiSha.Common.Server.LegalName(this.TemplateName + "-�������-" + DateTime.Now.ToLongDateString()) + ".xls";
            FileStream file = new FileStream(filePath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
            }
        }
        #endregion
    }
}