using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Datos;
using Entidades;
using Newtonsoft.Json;
using PortalEmpleados.Models;
using PortalEmpleados.Security;
using System.Data.SqlClient;


namespace PortalEmpleados.Controllers
{
    [SessionSecurity]
    public class ReportsController : Controller
    {
        private PortalEmpleadosContext db = new PortalEmpleadosContext();
        private HelpersController hc = new HelpersController();

        [SessionSecurity]
        [VerifyAccountRule]
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo Index en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }
        }

        [SessionSecurity]
        [VerifyAccountRule]
        public ActionResult List()
        {
            try
            {
                DA_Reports daReports = new DA_Reports();
                ReportsViewModels model = new ReportsViewModels();
                model.reports = daReports.rs_listReports(null);
                return View(model);
            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo List en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }
        }

        [SessionSecurity]
        [VerifyAccountRule]
        public ActionResult Parameters(string cod_report)
        {
            try
            {
                DA_Reports daReports = new DA_Reports();
                ReportParamsViewModels model = new ReportParamsViewModels();
                model.parameters = daReports.rs_listReportParams(cod_report, Session["Database"].ToString());
                return View(model);
            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo Parameters en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }
        }

        [SessionSecurity]
        [VerifyAccountRule]
        public ActionResult comprobantesPago()
        {
            try
            {
                DA_Reports daReports = new DA_Reports();
                List<string> fechasComprobantes = new List<string>();
                fechasComprobantes = daReports.rs_listFechasComprobantes(Session["idUsr"].ToString(), "0", Session["Database"].ToString());
                ViewBag.fechasComprobantes = fechasComprobantes;
                TempData["rtrnIdx"] = "1";
                return View();
            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo comprobantesPago en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }

        }

        [HttpPost]
        [SessionSecurity]
        public ActionResult getFechasComprobantesPago(string tipliq)
        {
            try
            {
                var Result = new DataResponseAjax { Result = 0, Msg = "", Json = "" };
                DA_Reports daReports = new DA_Reports();
                List<string> fechasComprobantes = daReports.rs_listFechasComprobantes(Session["idUsr"].ToString(), tipliq, Session["Database"].ToString());
                if (fechasComprobantes == null)
                {
                    Result.Result = 1; Result.Msg = "No hay comprobantes disponibles";
                }
                else
                {
                    Result.Json = JsonConvert.SerializeObject(fechasComprobantes);
                }
                TempData["rtrnIdx"] = "1";
                return Json(Result);
            }
            catch (Exception ex)
            {
                var Result = new DataResponseAjax { Result = 0, Msg = "", Json = "" };
                Result.Result = 1; Result.Msg = "No hay comprobantes disponibles";
                hc.SaveLogs(User.Identity.Name, "Error en el metodo getFechasComprobantesPago en el controlador ReportsController. Exception: " + ex.Message);
                return Json(Result);
            }

        }

        [SessionSecurity]
        [VerifyAccountRule]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReportComprobantesPago(string fecha, bool enviarcorreo, string correo, string tipliq)
        {
            try
            {
                DA_Reports daReports = new DA_Reports();
                List<Ent_Reports> reportList = new List<Ent_Reports>();

                reportList = daReports.rs_listReports("NOMU204ME");

                Ent_Reports report = new Ent_Reports();

                report.codigo = reportList[0].codigo;
                report.nombre = reportList[0].nombre;
                report.ruta = reportList[0].ruta;
                report.actionCtrl = reportList[0].actionCtrl;
                report.urlServer = reportList[0].urlServer;
                report.ReportPath = reportList[0].ReportPath;

                ViewBag.ReportPath = reportList[0].ReportPath;
                ViewBag.report = report;
                ViewBag.enviarcorreo = enviarcorreo;
                ViewBag.correo = correo;

                switch (tipliq)
                {
                    case "0":
                        ViewBag.tipliq = "01";
                        break;
                    case "1":
                        ViewBag.tipliq = "02";
                        break;
                    case "2":
                        ViewBag.tipliq = "04";
                        break;
                    case "3":
                        ViewBag.tipliq = "03";
                        break;
                    case "4":
                        ViewBag.tipliq = "05";
                        break;
                    case "5":
                        ViewBag.tipliq = "07";
                        break;
                }

                DA_Login usr = new DA_Login();
                ViewBag.fechaComprobante = fecha;
                ViewBag.Cedula = usr.getCedula(Session["idUsr"].ToString());
                ViewBag.Bds = Session["Database"].ToString();

                if (enviarcorreo)
                {
                    return View("~/Views/Reports/ReportComprobantesPago.aspx");
                }
                else
                {
                    TempData["rtrnIdx"] = "1";
                    return View("~/Views/Reports/ReportComprobantesPago.cshtml");
                }
            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo ReportComprobantesPago en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }
        }

        [SessionSecurity]
        [VerifyAccountRule]
        public ActionResult ReportCertificadosLaborales(certificadosLaboralesViewModels model)
        {
            try
            {
                DA_Reports daReports = new DA_Reports();
                List<Ent_Reports> reportList = new List<Ent_Reports>();

                reportList = daReports.rs_listReports("NOMU9001");

                Ent_Reports report = new Ent_Reports();

                report.codigo = reportList[0].codigo;
                report.nombre = reportList[0].nombre;
                report.ruta = reportList[0].ruta;
                report.actionCtrl = reportList[0].actionCtrl;
                report.urlServer = reportList[0].urlServer;
                report.ReportPath = reportList[0].ReportPath;

                ViewBag.report = report;

                DA_Login usr = new DA_Login();
                model.Cedula = usr.getCedula(Session["idUsr"].ToString());
                ViewBag.Cedula = model.Cedula;
                ViewBag.Bds = Session["Database"].ToString();
                ViewBag.Sucursal = model.Sucursal;
                TempData["rtrnIdx"] = "1";
                return View();
            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo ReportCertificadosLaborales en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }

        }

        [SessionSecurity]
        [VerifyAccountRule]
        public ActionResult ReportCertIngYRet()
        {
            try
            {
                DA_Reports daReports = new DA_Reports();
                List<Ent_Reports> reportList = new List<Ent_Reports>();

                reportList = daReports.rs_listReports("NOM921_17");

                Ent_Reports report = new Ent_Reports();

                report.codigo = reportList[0].codigo;
                report.nombre = reportList[0].nombre;
                report.ruta = reportList[0].ruta;
                report.actionCtrl = reportList[0].actionCtrl;
                report.urlServer = reportList[0].urlServer;
                report.ReportPath = reportList[0].ReportPath;

                ViewBag.report = report;

                DA_Login usr = new DA_Login();

                int ano = System.Convert.ToInt32(ConfigurationManager.AppSettings["certIngYRet"]);

                ViewBag.Cedula = usr.getCedula(Session["idUsr"].ToString());
                ViewBag.FechaIni = (new System.DateTime(DateTime.Now.Year - 1, 1, 1)).ToShortDateString();
                ViewBag.FechaFin = (new System.DateTime(DateTime.Now.Year - 1, 12, 31)).ToShortDateString();
                ViewBag.fecExp = DateTime.Now.ToShortDateString();

                TempData["rtrnIdx"] = "1";
                return View();
            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo ReportCertIngYRet en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }

        }

        #region Certificado y reporte de Seguridad Social Empleado.
        //Certidficado y reporte de Seguridad Social.
        [SessionSecurity]
        [VerifyAccountRule]
        public ActionResult ReportCertiArus()
        {
            try
            {
                string cod_emp = Session["cedula"].ToString();
                string tip_ide = tipdoc(cod_emp);

                DA_Reports daReports = new DA_Reports();
                List<Ent_Reports> reportList = new List<Ent_Reports>();

                reportList = daReports.rs_listReports("NOMARUS");

                Ent_Reports report = new Ent_Reports();

                report.codigo = reportList[0].codigo;
                report.nombre = reportList[0].nombre;
                report.ruta = reportList[0].ruta;
                report.actionCtrl = reportList[0].actionCtrl;
                report.urlServer = reportList[0].urlServer;
                report.ReportPath = reportList[0].ReportPath;

                ViewBag.report = report;

                DA_Login usr = new DA_Login();

                ViewBag.tip_ide = tip_ide;
                ViewBag.Cedula = GetTipDocEmp(tip_ide) + cod_emp;
                ViewBag.Bds = Session["Database"].ToString();
                TempData["rtrnIdx"] = "1";
                return View();

            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo ReportCertiArus en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }

        }
        #endregion

        [SessionSecurity]
        [VerifyAccountRule]

        public ActionResult executeReport(string id)
        {
            try
            {
                DA_Reports daReports = new DA_Reports();
                List<Ent_Reports> reportList = new List<Ent_Reports>();

                reportList = daReports.rs_listReports(id);

                Ent_Reports report = new Ent_Reports();

                report.codigo = reportList[0].codigo;
                report.nombre = reportList[0].nombre;
                report.ruta = reportList[0].ruta;
                report.actionCtrl = reportList[0].actionCtrl;
                report.urlServer = reportList[0].urlServer;
                report.ReportPath = reportList[0].ReportPath;

                ViewBag.report = report;
                ViewBag.token = Session["idUsr"].ToString();
                return View();
            }
            catch (Exception ex)
            {
                hc.SaveLogs(User.Identity.Name, "Error en el metodo ReportCertIngYRet en el controlador ReportsController. Exception: " + ex.Message);
                return View("Error");
            }
        }
        #region Tipo de documento para asociar a la cedula "ARUS"
        //Tipo de documento para asociar a la cedula 
        [SessionSecurity]
        [VerifyAccountRule]
        public ActionResult ViewExample()
        {
            return View();
        }

        private string tipdoc(string cod_emp)
        {
            string response = "";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PortalEmpleados"].ToString()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand($"SELECT tip_ide FROM[PortalEmpleadosBD].[dbo].[tbl_usuarios] WHERE[empleado] = '{cod_emp}'", con);
                SqlDataReader myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    response = myreader.GetString(0);
                }
            }
            return response;
        }

        private string GetTipDocEmp(string tip_doc)
        {
            string response = "";
            switch (tip_doc)
            {
                case "01":
                    response = "C";
                    break;
                case "02":
                    response = "E";
                    break;
                case "03":
                    response = "I";
                    break;
                case "05":
                    response = "P";
                    break;
                case "06":
                    response = "R";
                    break;
            }
            return response;
        }
        #endregion
    }
}