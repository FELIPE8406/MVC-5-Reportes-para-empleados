<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="iTextSharp.text" %>
<%@ Import Namespace="iTextSharp.text.pdf" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="PortalEmpleados.Util" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Comprobante de pago</title>
</head>
<body>
    <script runat="server">
            protected void Page_Init(object sender, EventArgs e)
            {
                ReportViewer1.ServerReport.ReportServerCredentials = ViewBag.credentials;
            }
            void Page_Load(object sender, EventArgs e) {

                if (!IsPostBack)
                {
                    ReportViewer1.ServerReport.ReportServerCredentials = ViewBag.credentials;
                    ReportViewer1.ProcessingMode = ProcessingMode.Remote;
                    ReportViewer1.ShowParameterPrompts = true;

                    Uri urlSrvr = new Uri(ViewBag.report.urlServer);
                    ReportViewer1.ServerReport.ReportServerUrl = urlSrvr;
                    ReportViewer1.ServerReport.ReportPath = ViewBag.report.ReportPath;

                    List<ReportParameter> ListParameters = new List<ReportParameter>();

                    ListParameters.Add(new ReportParameter("FecIni", ViewBag.fechaComprobante, false));
                    ListParameters.Add(new ReportParameter("FecFin", ViewBag.fechaComprobante, false));
                    ListParameters.Add(new ReportParameter("IndFec", "1", false));
                    ListParameters.Add(new ReportParameter("Bds", ViewBag.Bds, false));
                    ListParameters.Add(new ReportParameter("CodConv", "%", false));
                    <%--ListParameters.Add(new ReportParameter("codcia", "%", false));--%>
<%--                    ListParameters.Add(new ReportParameter("CodSuc", "%", false));
                    ListParameters.Add(new ReportParameter("codccoconv", "%", false));
                    ListParameters.Add(new ReportParameter("CodCco", "%", false));
                    ListParameters.Add(new ReportParameter("codcla1", "%", false));
                    ListParameters.Add(new ReportParameter("codcla2", "%", false));
                    ListParameters.Add(new ReportParameter("codcla3", "%", false));--%>
                    ListParameters.Add(new ReportParameter("CodEmp", ViewBag.Cedula, false));
                    ListParameters.Add(new ReportParameter("TipLiq", ViewBag.tipliq, false));
                    ListParameters.Add(new ReportParameter("Origen", "H", false));

                    ReportViewer1.ServerReport.SetParameters(ListParameters);
                    ReportViewer1.AsyncRendering = false;
                    ReportViewer1.SizeToReportContent = true;

                    if (ViewBag.enviarcorreo)
                    {
                        Warning[] warnings;
                        string[] streamids;
                        string mimeType;
                        string encoding;
                        string filenameExtension;
                        string filePath;


                        filePath=string.Format("//sqlmision/Temp/{0}.pdf",ViewBag.Cedula);

                        byte[] bytes = ReportViewer1.ServerReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);

                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                        }

                        EnviarReporte sendReport = new EnviarReporte();
                        sendReport.enviarComprobantesDePago(Session["nombre"].ToString(), ViewBag.fechaComprobante, 
                            ViewBag.correo, string.Format(ConfigurationManager.AppSettings["rutaTemp"].ToString()+"/{0}.pdf",ViewBag.Cedula), ViewBag.Cedula,Session["Database"].ToString());
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
                        //FileStream Nfs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                        //Nfs.Write(bytes, 0, bytes.Length);
                        //Document doc = new Document();
                        //PdfWriter Pdfwriter = PdfWriter.GetInstance(doc, Nfs);
                        //Pdfwriter.SetEncryption(PdfWriter.STRENGTH40BITS, "12345", null, 0);
                        //doc.Open();
                        //doc.Close();


                    }
                    else
                    {
                        ReportViewer1.ServerReport.Refresh();
                    }

                }
            }

        protected void ReportViewer_OnLoad(object sender, EventArgs e)
        {
            System.Reflection.FieldInfo info;
            
            foreach (RenderingExtension ext in ReportViewer1.ServerReport.ListRenderingExtensions())
             {

                info = ext.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                info.SetValue(ext, (ext.Name=="PDF" ? true : false));

            }       
        }

    </script>

    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Remote" Width="80%" Height="80%"
            ZoomMode="Percent" KeepSessionAlive="true" ViewStateMode="Enabled" OnLoad="ReportViewer_OnLoad">
            <ServerReport />
        </rsweb:ReportViewer>
    </form>

</body>
</html>
